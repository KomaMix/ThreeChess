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
        public readonly IHubContext<LobbyHub> _hubContext;
        public readonly ILobbyManager _lobbyManager;
        public readonly IGameRepository _gameRepository;
        public readonly ConcurrentDictionary<Guid, Timer> _countdownTimers = new();
        public readonly IBoardElementsService _boardElementsService;
        public readonly TimeSpan dueTime = new TimeSpan(0, 0, 0, 3);
        public readonly TimeSpan period = new TimeSpan(1, 0, 0);

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

        public void StartCountdown(Guid lobbyId)
        {
            var timer = new Timer(async _ => await FinishCountdown(lobbyId), null, dueTime, period);
            _countdownTimers[lobbyId] = timer;
        }

        public async Task FinishCountdown(Guid lobbyId)
        {
            if (_countdownTimers.TryRemove(lobbyId, out var timer))
            {
                timer.Dispose();
                var lobby = _lobbyManager.GetLobby(lobbyId);

                

                if (lobby?.PlayerIds.Count == 3)
                {
                    Dictionary<Guid, FigureColor> playerColors = new Dictionary<Guid, FigureColor>();
                    Dictionary<Guid, TimeSpan> playerGameTimes = new Dictionary<Guid, TimeSpan>();

                    for (int i = 0; i < 3; i++)
                    {
                        playerColors[lobby.PlayerIds[i]] = (FigureColor)i;
                        playerGameTimes[lobby.PlayerIds[i]] = lobby.GameDuration;
                    }

                    var game = new GameState
                    {
                        Id = Guid.NewGuid(),
                        ActivePlayerIds = lobby.PlayerIds,
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
                        await _hubContext.Clients.User(userId.ToString()).SendAsync("GameStarted", game.Id);
                    }

                    _lobbyManager.RemoveLobby(lobbyId);
                }
            }
        }

        public void CancelCountdown(Guid lobbyId)
        {
            if (_countdownTimers.TryRemove(lobbyId, out var timer))
            {
                timer.Dispose();
            }
        }
    }
}
