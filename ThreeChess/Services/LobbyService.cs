using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using ThreeChess.Enums;
using ThreeChess.Hubs;
using ThreeChess.Models;

namespace ThreeChess.Services
{
    public class LobbyService
    {
        private readonly IHubContext<LobbyHub> _hubContext;
        private readonly LobbyManager _lobbyManager;
        private readonly GameManager _gameManager;
        private readonly ConcurrentDictionary<int, Timer> _countdownTimers = new();

        public LobbyService(
            IHubContext<LobbyHub> hubContext,
            LobbyManager lobbyManager,
            GameManager gameManager)
        {
            _hubContext = hubContext;
            _lobbyManager = lobbyManager;
            _gameManager = gameManager;
        }

        public void StartCountdown(int lobbyId)
        {
            var timer = new Timer(async _ => await FinishCountdown(lobbyId), null, 10000, Timeout.Infinite);

            if (_countdownTimers.TryAdd(lobbyId, timer))
            {
                _hubContext.Clients.Group($"lobby-{lobbyId}").SendAsync("StartCountdown");
            }
        }

        private async Task FinishCountdown(int lobbyId)
        {
            if (_countdownTimers.TryRemove(lobbyId, out var timer))
            {
                timer.Dispose();
                var lobby = _lobbyManager.GetLobby(lobbyId);
                if (lobby?.PlayerIds.Count == 3)
                {
                    var game = new GameState
                    {
                        Id = Guid.NewGuid(),
                        ActivePlayerIds = lobby.PlayerIds.ToList(),
                        GameStatus = GameStatus.WaitingToStart
                    };

                    _gameManager.CreateGame(game);
                    _lobbyManager.RemoveLobby(lobbyId);

                    await _hubContext.Clients.Group($"lobby-{lobbyId}")
                        .SendAsync("GameStarted", game.Id);
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
