using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ThreeChess.Data;
using ThreeChess.DTOs;
using ThreeChess.Models;
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


        [HttpGet("AllLobbiesPage")]
        public IActionResult GetAllLobbiesPage()
        {
            var filePath = Path.Combine(_env.ContentRootPath, "HtmlPages/Lobby/lobbies.html");
            return PhysicalFile(filePath, "text/html");
        }

        [HttpGet("{lobbyId}")]
        public IActionResult GetLobbyPage(int lobbyId)
        {
            var playerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!_lobbyManager.PlayerExist(lobbyId, playerId))
            {
                return NotFound();
            }

            var filePath = Path.Combine(_env.ContentRootPath, "HtmlPages/Lobby/lobby.html");
            return PhysicalFile(filePath, "text/html");
        }

    }
}
