using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThreeChess.Services;

namespace ThreeChess.Controllers
{
    [Authorize]
    public class LobbyController : Controller
    {
        private readonly LobbyManager _lobbyManager;
        IWebHostEnvironment _env;

        public LobbyController(LobbyManager lobbyManager, IWebHostEnvironment env)
        {
            _lobbyManager = lobbyManager;
            _env = env;
        }

        [HttpGet("all-lobbies")]
        public IActionResult GetAllLobbies()
        {
            return Ok(_lobbyManager.GetAllLobbies());
        }


        public IActionResult Index()
        {
            var filePath = Path.Combine(_env.ContentRootPath, "HtmlPages/Lobby/lobbies.html");
            return PhysicalFile(filePath, "text/html");
        }

    }
}
