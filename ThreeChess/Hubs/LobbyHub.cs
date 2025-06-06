using System.Collections.Concurrent;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ILobbyManager _lobbyManager;
        private readonly ILobbyWaitingService _lobbyWaitingService;
        private readonly UserManager<AppUser> _userManager;

        public LobbyHub(
            ILobbyManager lobbyManager,
            ILobbyWaitingService lobbyWaitingService,
            UserManager<AppUser> userManager)
        {
            _lobbyManager = lobbyManager;
            _lobbyWaitingService = lobbyWaitingService;
            _userManager = userManager;
        }

        public async Task<bool> JoinLobby(Guid lobbyId)
        {
            var playerId = Guid.Parse(Context.UserIdentifier);

            if (_lobbyManager.PlayerExist(lobbyId, playerId))
            {
                return true;
            }

            if (!_lobbyManager.JoinLobby(lobbyId, playerId))
                return false;


            var lobby = _lobbyManager.GetLobby(lobbyId);

            CheckCountdown(lobby);

            await NotifyLobbyUpdated(lobbyId);

            await Clients.All.SendAsync("LobbyPlayerNumbersChanged", lobby.Id, lobby.PlayerIds.Count);

            return true;
        }

        void CheckCountdown(Lobby lobby)
        {
            if (lobby.PlayerIds.Count == 3)
            {
                _lobbyWaitingService.StartCountdown(lobby.Id);
            }
        }


        public async Task<bool> LeaveLobby(Guid lobbyId)
        {
            var playerId = Guid.Parse(Context.UserIdentifier);

            if (!_lobbyManager.PlayerExist(lobbyId, playerId))
            {
                return false;
            }

            if (!_lobbyManager.LeaveLobby(lobbyId, playerId))
                return false;


            var lobby = _lobbyManager.GetLobby(lobbyId);

            if (lobby.PlayerIds.Count == 2)
            {
                _lobbyWaitingService.CancelCountdown(lobbyId);
                await NotifyCancelCountdown(lobbyId);
            }

            await NotifyLobbyUpdated(lobbyId);

            await Clients.All.SendAsync("LobbyPlayerNumbersChanged", lobby.Id, lobby.PlayerIds.Count);

            return true;
        }



        public async Task HandleLeave(Guid playerId, Guid lobbyId)
        {
            if (_lobbyManager.LeaveLobby(lobbyId, playerId))
            {
                await NotifyLobbyUpdated(lobbyId);
                await Clients.All.SendAsync("LobbiesUpdated", _lobbyManager.GetAllLobbies());
            }
        }

        private async Task NotifyCancelCountdown(Guid lobbyId)
        {
            var lobby = _lobbyManager.GetLobby(lobbyId);

            var userIds = lobby.PlayerIds;

            foreach (var userId in userIds)
            {
                await Clients.User(userId.ToString()).SendAsync("CancelCountdown");
            }
        }

        private async Task NotifyLobbyUpdated(Guid lobbyId)
        {
            var lobby = _lobbyManager.GetLobby(lobbyId);
            var dto = await ConvertToDto(lobby);

            var userIds = lobby.PlayerIds;

            foreach (var userId in userIds)
            {
                await Clients.User(userId.ToString()).SendAsync("LobbyUpdated", dto);
            }
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
                var user = await _userManager.FindByIdAsync(userId.ToString());
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

        public async Task<LobbyDto> GetLobbyInfo(Guid lobbyId)
        {
            var playerId = Guid.Parse(Context.UserIdentifier);

            if (!_lobbyManager.PlayerExist(lobbyId, playerId))
            {
                return null;
            }

            var lobby = _lobbyManager.GetLobby(lobbyId);
            return await ConvertToDto(lobby);
        }
    }
}
