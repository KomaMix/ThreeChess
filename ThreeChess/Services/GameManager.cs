using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using ThreeChess.DTOs;
using ThreeChess.Enums;
using ThreeChess.Hubs;
using ThreeChess.Interfaces;
using ThreeChess.Models;

namespace ThreeChess.Services
{
    public class GameManager : IGameManager
    {
        private readonly IGameRepository _gameRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMoveHistoryService _moveHistory;
        private readonly IHubContext<MoveHub> _moveContext;

        public GameManager(
            IGameRepository gameRepository, 
            IHttpContextAccessor httpContextAccessor, 
            IMoveHistoryService moveHistory,
            IHubContext<MoveHub> moveContext)
        {
            _gameRepository = gameRepository;
            _httpContextAccessor = httpContextAccessor;
            _moveHistory = moveHistory;
            _moveContext = moveContext;
        }

        public async Task<MoveResponse> MoveHandle(MoveRequest moveRequest)
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
            _moveHistory.AddMove(moveRequest.GameId, new Move
            {
                StartCellId = moveRequest.StartCellId,
                EndCellId = moveRequest.EndCellId
            });

            MoveResponse moveResponse = new MoveResponse
            {
                StartCellId = moveRequest.StartCellId,
                EndCellId = moveRequest.EndCellId,
                GameId = game.Id,
                UserId = userId,
                PlayerGameTimes = game.PlayerGameTimes
            };

            foreach (var activeUserId in game.ActivePlayerIds)
            {
                await _moveContext.Clients.User(activeUserId).SendAsync("handleMove", moveResponse);
            }

            return moveResponse;
        }
    }
}
