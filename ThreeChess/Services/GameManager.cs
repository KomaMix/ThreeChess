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
        private readonly IMoveCheckService _moveCheckService;

        public GameManager(
            IGameRepository gameRepository, 
            IHttpContextAccessor httpContextAccessor, 
            IMoveHistoryService moveHistory,
            IMoveCheckService moveCheckService)
        {
            _gameRepository = gameRepository;
            _httpContextAccessor = httpContextAccessor;
            _moveHistory = moveHistory;
            _moveCheckService = moveCheckService;
        }

        public async Task<MoveResponse> MoveHandle(MoveRequest moveRequest)
        {
            var curUserId = GetCurUserId();

            var game = _gameRepository.GetGame(moveRequest.GameId);

            if (game == null || curUserId == null)
            {
                throw new Exception("game not finding");
            }

            if (game.PlayerColors[curUserId] != game.CurrentTurnColor)
            {
                throw new Exception("turn error");
            }

            if (!_moveCheckService.MoveCheck(moveRequest))
            {
                throw new Exception("error move");
            }


            if (game.FiguresLocation.ContainsKey(moveRequest.EndCellId))
            {
                game.FiguresLocation.Remove(moveRequest.EndCellId);
            }

            UpdateGameStatus(game, curUserId);


            game.FiguresLocation[moveRequest.EndCellId] = game.FiguresLocation[moveRequest.StartCellId];
            game.FiguresLocation.Remove(moveRequest.StartCellId);

            game.CurrentTurnColor = game.CurrentTurnColor.NextColor();

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
                UserId = curUserId,
                PlayerGameTimes = game.PlayerGameTimes
            };

            return moveResponse;
        }

        void UpdateGameStatus(GameState game, Guid curUser)
        {
            if (game.GameStatus == GameStatus.Wait)
            {
                game.GameStatus = GameStatus.InProgress;

                game.LastMoveTime = DateTime.UtcNow;
            }
            else
            {
                game.PlayerGameTimes[curUser] -= (DateTime.UtcNow - game.LastMoveTime);
            }
        }

        private Guid GetCurUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = Guid.Parse(user?.FindFirstValue(ClaimTypes.NameIdentifier));

            return userId;
        }
    }
}
