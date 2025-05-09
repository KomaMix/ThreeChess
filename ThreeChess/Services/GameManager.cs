using System.Security.Claims;
using ThreeChess.Enums;
using ThreeChess.Interfaces;

namespace ThreeChess.Services
{
    public class GameManager : IGameManager
    {
        private readonly IGameRepository _gameRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GameManager(IGameRepository gameRepository, IHttpContextAccessor httpContextAccessor)
        {
            _gameRepository = gameRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<bool> Move(Guid gameId, string startCellId, string endCellId)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirstValue(ClaimTypes.NameIdentifier);

            var game = _gameRepository.GetGame(gameId);

            if (game == null || userId == null)
            {
                return Task.FromResult(false);
            }

            if (game.PlayerColors[userId] != game.CurrentTurnColor)
            {
                return Task.FromResult(false);
            }

            if (game.FiguresLocation.ContainsKey(endCellId))
            {
                game.FiguresLocation.Remove(endCellId);
            }

            game.FiguresLocation[endCellId] = game.FiguresLocation[startCellId];
            game.FiguresLocation.Remove(startCellId);

            game.CurrentTurnColor = game.CurrentTurnColor.NextColor();

            return Task.FromResult(true);
        }
    }
}
