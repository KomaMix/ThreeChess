using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThreeChess.Services;

namespace ThreeChess.Controllers
{
    [Authorize]
    public class LobbyController : Controller
    {
        private readonly GameManager _gameManager;
        IWebHostEnvironment _env;

        public LobbyController(GameManager gameManager, IWebHostEnvironment env)
        {
            _gameManager = gameManager;
            _env = env;
        }

        [HttpGet("all-lobbies")]
        public IActionResult GetAllLobbies()
        {
            return Ok(_gameManager.GetAllLobbies());
        }


        public IActionResult Index()
        {
            var filePath = Path.Combine(_env.ContentRootPath, "HtmlPages/Lobby/lobbies.html");
            return PhysicalFile(filePath, "text/html");
        }

    }
}
