using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using ThreeChess.Enums;
using ThreeChess.Hubs;
using ThreeChess.Interfaces;
using ThreeChess.Models;

namespace ThreeChess.Services
{
    public class LobbyWaitingService : ILobbyWaitingService
    {
        private readonly IHubContext<LobbyHub> _hubContext;
        private readonly LobbyManager _lobbyManager;
        private readonly IGameRepository _gameRepository;
        private readonly ConcurrentDictionary<int, Timer> _countdownTimers = new();
        private readonly IBoardElementsService _boardElementsService;

        public LobbyWaitingService(
            IHubContext<LobbyHub> hubContext,
            LobbyManager lobbyManager,
            IGameRepository gameRepository,
            IBoardElementsService boardElementsService)
        {
            _hubContext = hubContext;
            _lobbyManager = lobbyManager;
            _gameRepository = gameRepository;
            _boardElementsService = boardElementsService;
        }

        public void StartCountdown(int lobbyId)
        {
            var timer = new Timer(async _ => await FinishCountdown(lobbyId), null, 10000, Timeout.Infinite);
            _countdownTimers[lobbyId] = timer;
        }

        private async Task FinishCountdown(int lobbyId)
        {
            if (_countdownTimers.TryRemove(lobbyId, out var timer))
            {
                timer.Dispose();
                var lobby = _lobbyManager.GetLobby(lobbyId);

                

                if (lobby?.PlayerIds.Count == 3)
                {
                    Dictionary<string, FigureColor> playerColors = new Dictionary<string, FigureColor>();
                    playerColors[lobby.PlayerIds[0]] = FigureColor.White;
                    playerColors[lobby.PlayerIds[1]] = FigureColor.Black;
                    playerColors[lobby.PlayerIds[2]] = FigureColor.Red;

                    var game = new GameState
                    {
                        Id = Guid.NewGuid(),
                        ActivePlayerIds = lobby.PlayerIds.ToList(),
                        GameStatus = GameStatus.InProgress,
                        CurrentTurnColor = FigureColor.White,
                        FiguresLocation = _boardElementsService.CreateFigures(),
                        PlayerColors = playerColors
                    };
                    
                    _gameRepository.CreateGame(game);

                    var userIds = lobby.PlayerIds;

                    foreach (var userId in userIds)
                    {
                        await _hubContext.Clients.User(userId).SendAsync("GameStarted", game.Id);
                    }

                    _lobbyManager.RemoveLobby(lobbyId);
                }
            }
        }

        public void CancelCountdown(int lobbyId)
        {
            if (_countdownTimers.TryRemove(lobbyId, out var timer))
            {
                timer.Dispose();
            }
        }
    }
}
