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
        private readonly ILobbyManager _lobbyManager;
        private readonly IGameRepository _gameRepository;
        private readonly ConcurrentDictionary<int, Timer> _countdownTimers = new();
        private readonly IBoardElementsService _boardElementsService;
        private readonly TimeSpan dueTime = new TimeSpan(0, 0, 0, 3);
        private readonly TimeSpan period = new TimeSpan(1, 0, 0);

        public LobbyWaitingService(
            IHubContext<LobbyHub> hubContext,
            ILobbyManager lobbyManager,
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
            var timer = new Timer(async _ => await FinishCountdown(lobbyId), null, dueTime, period);
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
                    Dictionary<string, TimeSpan> playerGameTimes = new Dictionary<string, TimeSpan>();

                    for (int i = 0; i < 3; i++)
                    {
                        playerColors[lobby.PlayerIds[i]] = (FigureColor)i;
                        playerGameTimes[lobby.PlayerIds[i]] = lobby.GameDuration;
                    }

                    var game = new GameState
                    {
                        Id = Guid.NewGuid(),
                        ActivePlayerIds = lobby.PlayerIds.ToList(),
                        GameStatus = GameStatus.Wait,
                        CurrentTurnColor = FigureColor.White,
                        FiguresLocation = _boardElementsService.CreateFigures(),
                        PlayerColors = playerColors,
                        PlayerGameTimes = playerGameTimes
                    };


                    _gameRepository.CreateGame(game);


                    var userIds = lobby.PlayerIds;

                    foreach (var userId in userIds)
                    {
                        // To Do
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
