using System.Collections.Concurrent;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using ThreeChess.Data;
using ThreeChess.DTOs;
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
        private readonly UserManager<AppUser> _userManager;

        public LobbyHub(
            LobbyManager lobbyManager, 
            GameManager gameManager,
            LobbyService lobbyService,
            UserManager<AppUser> userManager)
        {
            _lobbyManager = lobbyManager;
            _gameManager = gameManager;
            _lobbyService = lobbyService;
            _userManager = userManager;
        }

        private async Task<LobbyDto> ConvertToDto(Lobby lobby)
        {
            var dto = new LobbyDto
            {
                Id = lobby.Id,
                CreatedAt = lobby.CreatedAt,
                Players = new List<PlayerDto>()
            };

            foreach (var userId in lobby.PlayerIds)
            {
                var user = await _userManager.FindByIdAsync(userId);
                dto.Players.Add(new PlayerDto
                {
                    UserId = userId,
                    Nickname = user?.UserName ?? "Аноним" // Или ваше поле с ником
                });
            }

            return dto;
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
            var dto = await ConvertToDto(lobby);

            if (lobby.PlayerIds.Count == 3)
            {
                _lobbyService.StartCountdown(lobbyId);
            }

            await Clients.Group($"lobby-{lobbyId}")
                         .SendAsync("LobbyUpdated", dto);

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
            var dto = await ConvertToDto(lobby);
            await Clients.Caller.SendAsync("LobbyUpdated", dto);
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
