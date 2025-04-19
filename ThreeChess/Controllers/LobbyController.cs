using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThreeChess.Services;

namespace ThreeChess.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class LobbyController : Controller
    {
        private readonly LobbyManager _lobbyManager;
        IWebHostEnvironment _env;

        public LobbyController(LobbyManager lobbyManager, IWebHostEnvironment env)
        {
            _lobbyManager = lobbyManager;
            _env = env;
        }


        [HttpGet("AllLobbies")]
        public IActionResult GetAllLobbiesPage()
        {
            var filePath = Path.Combine(_env.ContentRootPath, "HtmlPages/Lobby/lobbies.html");
            return PhysicalFile(filePath, "text/html");
        }

    }
}
