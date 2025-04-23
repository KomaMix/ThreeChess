using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThreeChess.Models;
using ThreeChess.Services;

namespace ThreeChess.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class GameController : Controller
    {
        IWebHostEnvironment _env;
        private readonly GameManager _gameManager;
        private readonly BoardElementsService _boardCreateService;
        private readonly MoveLogicalElementsService _moveElementsService;

        public GameController(
            IWebHostEnvironment env,
            GameManager gameManager,
            BoardElementsService boardCreateService,
            MoveLogicalElementsService moveElementsService)
        {
            _env = env;
            _gameManager = gameManager;
            _boardCreateService = boardCreateService;
            _moveElementsService = moveElementsService;
        }


        [HttpGet("Play/{gameId:guid}")]
        public IActionResult Play(Guid gameId)
        {
            var filePath = Path.Combine(_env.ContentRootPath, "HtmlPages/Game/game.html");
            return PhysicalFile(filePath, "text/html");
        }

        [HttpGet("game-config")]
        public IActionResult GetGameConfig([FromQuery] Guid gameId)
        {
            var game = _gameManager.GetGame(gameId);
            if (game == null)
                return NotFound($"Game {gameId} not found.");

            // Определяем, каким цветом играет текущий пользователь
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!game.PlayerColors.TryGetValue(userId, out var controlledColor))
                return Forbid();

            var dto = new InitializingGameDto
            {
                GameId = game.Id,
                ControlledColor = controlledColor,
                CurrentTurnColor = game.CurrentTurnColor,
                CellsLocation = _boardCreateService.CreateBoardCellsForColor(controlledColor),
                FiguresLocation = game.FiguresLocation,
                Diagonals = _moveElementsService.GetDiagonals(),
                MainLines = _moveElementsService.GetMainLines(),
                SecondaryLines = _moveElementsService.GetSecondaryLines()
            };

            return Ok(dto);
        }
    }
}
