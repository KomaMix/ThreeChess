using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ThreeChess.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        IWebHostEnvironment _env;
        public HomeController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            var filePath = Path.Combine(_env.ContentRootPath, "HtmlPages/Home/index.html");
            return PhysicalFile(filePath, "text/html");
        }
    }
}
