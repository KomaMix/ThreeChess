using System.Security.Claims;
using ThreeChess.DTOs;
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

        public Task<MoveResponse> MoveHandle(MoveRequest moveRequest)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirstValue(ClaimTypes.NameIdentifier);

            var game = _gameRepository.GetGame(moveRequest.GameId);

            if (game == null || userId == null)
            {
                throw new Exception("game not finding");
            }

            if (game.PlayerColors[userId] != game.CurrentTurnColor)
            {
                throw new Exception("turn error");
            }

            if (game.FiguresLocation.ContainsKey(moveRequest.EndCellId))
            {
                game.FiguresLocation.Remove(moveRequest.EndCellId);
            }

            if (game.GameStatus == GameStatus.Wait)
            {
                game.GameStatus = GameStatus.InProgress;

                game.LastMoveTime = DateTime.UtcNow;
            }

            game.FiguresLocation[moveRequest.EndCellId] = game.FiguresLocation[moveRequest.StartCellId];
            game.FiguresLocation.Remove(moveRequest.StartCellId);

            game.CurrentTurnColor = game.CurrentTurnColor.NextColor();

            game.PlayerGameTimes[userId] -= (DateTime.UtcNow - game.LastMoveTime);

            game.LastMoveTime = DateTime.UtcNow;

            _gameRepository.UpdateGame(game);

            return Task.FromResult(new MoveResponse
            {
                StartCellId = moveRequest.StartCellId,
                EndCellId = moveRequest.EndCellId,
                GameId = game.Id,
                UserId = userId,
                PlayerGameTimes = game.PlayerGameTimes
            });
        }
    }
}
