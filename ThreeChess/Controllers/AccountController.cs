using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ThreeChess.Data;
using ThreeChess.Models.Account;

namespace ThreeChess.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        IWebHostEnvironment _env;

        public AccountController(IWebHostEnvironment env, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            _env = env;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            var filePath = Path.Combine(_env.ContentRootPath, "HtmlPages/Auth/login.html");
            return PhysicalFile(filePath, "text/html");
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // При успешном входе cookie будут установлены
                    return Ok(new { message = "Вход выполнен успешно", user = user.UserName });
                }
            }

            return Unauthorized(new { message = "Error login/password" });
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            var filePath = Path.Combine(_env.ContentRootPath, "HtmlPages/Auth/register.html");
            return PhysicalFile(filePath, "text/html");
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new AppUser { UserName = registerDto.Username, Email = registerDto.Email };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok(new { message = "Регистрация и вход выполнены успешно", user = user.UserName });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Вы успешно вышли" });
        }
    }
}
