using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThreeChess.DTOs;
using ThreeChess.Enums;
using ThreeChess.Interfaces;


namespace ThreeChess.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class GameController : Controller
    {
        IWebHostEnvironment _env;
        private readonly IGameRepository _gameRepository;
        private readonly IBoardElementsService _boardCreateService;
        private readonly IMoveLogicalElementsService _moveElementsService;

        public GameController(
            IWebHostEnvironment env,
            IGameRepository gameRepository,
            IBoardElementsService boardCreateService,
            IMoveLogicalElementsService moveElementsService)
        {
            _env = env;
            _gameRepository = gameRepository;
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
            var game = _gameRepository.GetGame(gameId);
            if (game == null)
                return NotFound($"Game {gameId} not found.");

            // Определяем, каким цветом играет текущий пользователь
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!game.PlayerColors.TryGetValue(userId, out var controlledColor))
                return Forbid();

            var dto = new InitializingGameDto
            {
                GameId = game.Id,
                UserId = userId,
                ControlledColor = controlledColor,
                CurrentTurnColor = game.CurrentTurnColor,
                CellsLocation = _boardCreateService.CreateBoardCellsForColor(controlledColor),
                FiguresLocation = game.FiguresLocation,
                ActivePlayerIds = game.ActivePlayerIds,
                Diagonals = _moveElementsService.GetDiagonals(),
                MainLines = _moveElementsService.GetMainLines(),
                SecondaryLines = _moveElementsService.GetSecondaryLines(),
                PlayerGameTimes = game.PlayerGameTimes,
                PlayerColors = game.PlayerColors
            };

            return Ok(dto);
        }

        [HttpGet("ActiveGames")]
        public IActionResult ActiveGames()
        {
            var filePath = Path.Combine(_env.ContentRootPath, "HtmlPages/Game/active-games.html");
            return PhysicalFile(filePath, "text/html");
        }

        [HttpGet("active-games")]
        public IActionResult GetAllActiveGames()
        {
            var games = _gameRepository.GetAllGames().ToList();

            var filteredGames = games
                .Where(g => g.GameStatus == GameStatus.InProgress || g.GameStatus == GameStatus.Wait)
                .Select(g => new GameListItemDto
                {
                    Id = g.Id,
                    CreatedAt = g.CreatedAt,
                    PlayersCount = g.PlayerColors.Count,
                    Status = g.GameStatus.ToString(),
                    LastMoveTime = g.LastMoveTime,
                    CurrentTurn = g.CurrentTurnColor.ToString()
                }).ToList();

            return Ok(filteredGames);
        }

        public class GameListItemDto
        {
            public Guid Id { get; set; }
            public DateTime CreatedAt { get; set; }
            public int PlayersCount { get; set; }
            public string Status { get; set; }
            public DateTime LastMoveTime { get; set; }
            public string CurrentTurn { get; set; }
        }
    }
}
