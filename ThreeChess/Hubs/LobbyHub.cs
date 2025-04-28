using System.Collections.Concurrent;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using ThreeChess.Data;
using ThreeChess.DTOs;
using ThreeChess.Enums;
using ThreeChess.Interfaces;
using ThreeChess.Models;
using ThreeChess.Services;

namespace ThreeChess.Hubs
{
    public class LobbyHub : Hub
    {
        private readonly LobbyManager _lobbyManager;
        private readonly ILobbyWaitingService _lobbyWaitingService;
        private readonly UserManager<AppUser> _userManager;

        public LobbyHub(
            LobbyManager lobbyManager,
            ILobbyWaitingService lobbyWaitingService,
            UserManager<AppUser> userManager)
        {
            _lobbyManager = lobbyManager;
            _lobbyWaitingService = lobbyWaitingService;
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
                    Nickname = user?.UserName ?? "Аноним"
                });
            }

            return dto;
        }

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

            await Groups.AddToGroupAsync(Context.ConnectionId, $"lobby-{lobbyId}");

            var lobby = _lobbyManager.GetLobby(lobbyId);
            var dto = await ConvertToDto(lobby);

            if (lobby.PlayerIds.Count == 3)
            {
                _lobbyWaitingService.StartCountdown(lobbyId);
            }

            await Clients.Group($"lobby-{lobbyId}")
                         .SendAsync("LobbyUpdated", dto);

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
                _lobbyWaitingService.CancelCountdown(lobbyId);
                await Clients.Group($"lobby-{lobbyId}").SendAsync("CancelCountdown");
            }

            var dto = await ConvertToDto(lobby);

            await Clients.Group($"lobby-{lobbyId}")
                         .SendAsync("LobbyUpdated", dto);

            await Clients.All.SendAsync("LobbiesUpdated", _lobbyManager.GetAllLobbies());

            return true;
        }

        public async Task<bool> SubscribeLobby(int lobbyId)
        {
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
                var lobby = _lobbyManager.GetLobby(lobbyId);
                var dto = await ConvertToDto(lobby);

                await Clients.Group($"lobby-{lobbyId}").SendAsync("LobbyUpdated", dto);
                await Clients.All.SendAsync("LobbiesUpdated", _lobbyManager.GetAllLobbies());
            }
        }
    }
}
