using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThreeChess.Data;
using ThreeChess.DTOs;
using ThreeChess.Enums;
using ThreeChess.Interfaces;


namespace ThreeChess.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class GameController : Controller
    {
        private readonly IGameRepository _gameRepository;
        private readonly IBoardElementsService _boardCreateService;
        private readonly IMoveLogicalElementsService _moveElementsService;
        private readonly IMoveHistoryService _moveHistoryService;
        private readonly AppDbContext appDbContext;
        IWebHostEnvironment _env;

        public GameController(
            IWebHostEnvironment env,
            IGameRepository gameRepository,
            IBoardElementsService boardCreateService,
            IMoveLogicalElementsService moveElementsService,
            IMoveHistoryService moveHistoryService,
            AppDbContext appDbContext)
        {
            _env = env;
            _gameRepository = gameRepository;
            _boardCreateService = boardCreateService;
            _moveElementsService = moveElementsService;
            _moveHistoryService = moveHistoryService;
            this.appDbContext = appDbContext;
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
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (!game.PlayerColors.TryGetValue(userId, out var controlledColor))
                return Forbid();

            var activePlayersStr = game.ActivePlayerIds.Select(p => p.ToString());

            var dto = new InitializingGameDto
            {
                GameId = game.Id,
                UserId = userId,
                GameStatus = game.GameStatus,
                ControlledColor = controlledColor,
                CurrentTurnColor = game.CurrentTurnColor,
                CellsLocation = _boardCreateService.CreateBoardCellsForColor(controlledColor),
                FiguresLocation = game.FiguresLocation,
                ActivePlayerIds = game.ActivePlayerIds,
                Diagonals = _moveElementsService.GetDiagonals(),
                MainLines = _moveElementsService.GetMainLines(),
                SecondaryLines = _moveElementsService.GetSecondaryLines(),
                PlayerGameTimes = game.PlayerGameTimes,
                PlayerColors = game.PlayerColors,
                MoveHistory = _moveHistoryService.GetMoveHistory(gameId).ToList(),
                PlayerInfos = appDbContext.Users
                    .Where(u => activePlayersStr.Contains(u.Id))
                    .ToDictionary(u => Guid.Parse(u.Id), u => u)
            };

            if (game.GameStatus != GameStatus.Wait)
            {
                var currentTurnUser = game
                    .ActivePlayerIds
                    .FirstOrDefault(i => game.PlayerColors[i] == game.CurrentTurnColor);

                dto.PlayerGameTimes[currentTurnUser] -= DateTime.UtcNow - game.LastMoveTime;
            }

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
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var games = _gameRepository.GetAllGames().Take(1000).ToList();

            var filteredGames = games
                .Where(g => g.GameStatus == GameStatus.InProgress || g.GameStatus == GameStatus.Wait)
                .Where(g => g.ActivePlayerIds.Contains(userId))
                .Select(g => new GameListItemDto
                {
                    Id = g.Id,
                    CreatedAt = g.CreatedAt.ToString(),
                    PlayersCount = g.PlayerColors.Count,
                    Status = g.GameStatus.ToString(),
                    LastMoveTime = g.LastMoveTime.ToString(),
                    CurrentTurn = g.CurrentTurnColor.ToString()
                }).ToList();

            return Ok(filteredGames);
        }

        public class GameListItemDto
        {
            public Guid Id { get; set; }
            public string CreatedAt { get; set; } = DateTime.UtcNow.ToString();
            public int PlayersCount { get; set; }
            public string Status { get; set; }
            public string LastMoveTime { get; set; }
            public string CurrentTurn { get; set; }
        }
    }
}
