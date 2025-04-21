using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using ThreeChess.Enums;
using ThreeChess.Models;
using ThreeChess.Services;

namespace ThreeChess.Hubs
{
    public class LobbyHub : Hub
    {
        private readonly LobbyManager _lobbyManager;
        private readonly GameManager _gameManager;
        private readonly LobbyService _lobbyService;

        public LobbyHub(
            LobbyManager lobbyManager, 
            GameManager gameManager,
            LobbyService lobbyService)
        {
            _lobbyManager = lobbyManager;
            _gameManager = gameManager;
            _lobbyService = lobbyService;
        }

        // Сохраняем GetAllLobbies
        public IEnumerable<Lobby> GetAllLobbies()
        {
            return _lobbyManager.GetAllLobbies();
        }

        public async Task<bool> JoinLobby(int lobbyId)
        {
            var playerId = Context.UserIdentifier;

            if (_lobbyManager.PlayerExist(lobbyId, playerId))
            {
                return true;
            }

            if (!_lobbyManager.JoinLobby(lobbyId, playerId))
                return false;

            // Группа для конкретного лобби
            await Groups.AddToGroupAsync(Context.ConnectionId, $"lobby-{lobbyId}");

            // Уведомляем участников этого лобби
            var lobby = _lobbyManager.GetLobby(lobbyId);

            if (lobby.PlayerIds.Count == 3)
            {
                _lobbyService.StartCountdown(lobbyId);
            }

            await Clients.Group($"lobby-{lobbyId}")
                         .SendAsync("LobbyUpdated", lobby);

            // И уведомляем всех об изменении списка (для страницы AllLobbies)
            await Clients.All.SendAsync("LobbiesUpdated", _lobbyManager.GetAllLobbies());

            return true;
        }


        public async Task<bool> LeaveLobby(int lobbyId)
        {
            var playerId = Context.UserIdentifier;

            if (!_lobbyManager.PlayerExist(lobbyId, playerId))
            {
                return false;
            }

            if (!_lobbyManager.LeaveLobby(lobbyId, playerId))
                return false;

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"lobby-{lobbyId}");

            var lobby = _lobbyManager.GetLobby(lobbyId);

            if (lobby.PlayerIds.Count == 2)
            {
                _lobbyService.CancelCountdown(lobbyId);
                await Clients.Group($"lobby-{lobbyId}").SendAsync("CancelCountdown");
            }

            await Clients.Group($"lobby-{lobbyId}")
                         .SendAsync("LobbyUpdated", lobby);

            await Clients.All.SendAsync("LobbiesUpdated", _lobbyManager.GetAllLobbies());

            return true;
        }

        public async Task<bool> SubscribeLobby(int lobbyId)
        {
            // просто добавляем текущее соединение в группу
            await Groups.AddToGroupAsync(Context.ConnectionId, $"lobby-{lobbyId}");
            var lobby = _lobbyManager.GetLobby(lobbyId);
            await Clients.Caller.SendAsync("LobbyUpdated", lobby);
            return true;
        }

        public Lobby GetLobby(int lobbyId)
        {
            return _lobbyManager.GetLobby(lobbyId);
        }


        public async Task HandleLeave(string playerId, int lobbyId)
        {
            if (_lobbyManager.LeaveLobby(lobbyId, playerId))
            {
                await Clients.Group($"lobby-{lobbyId}").SendAsync("LobbyUpdated", _lobbyManager.GetLobby(lobbyId));
                await Clients.All.SendAsync("LobbiesUpdated", _lobbyManager.GetAllLobbies());
            }
        }
    }
}
