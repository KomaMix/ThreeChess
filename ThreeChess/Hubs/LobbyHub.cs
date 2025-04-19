using Microsoft.AspNetCore.SignalR;
using ThreeChess.Models;
using ThreeChess.Services;

namespace ThreeChess.Hubs
{
    public class LobbyHub : Hub
    {
        private readonly LobbyManager _lobbyManager;

        public LobbyHub(LobbyManager lobbyManager)
        {
            _lobbyManager = lobbyManager;
        }

        public async Task JoinLobby(int lobbyId)
        {
            var playerId = Context.UserIdentifier;
            if (_lobbyManager.JoinLobby(lobbyId, playerId))
            {
                await Clients.All.SendAsync("LobbiesUpdated", _lobbyManager.GetAllLobbies());
            }
        }

        public async Task LeaveLobby(int lobbyId)
        {
            var playerId = Context.UserIdentifier;
            if (_lobbyManager.LeaveLobby(lobbyId, playerId))
            {
                await Clients.All.SendAsync("LobbiesUpdated", _lobbyManager.GetAllLobbies());
            }
        }

        public IEnumerable<Lobby> GetAllLobbies() 
        { 
            return _lobbyManager.GetAllLobbies(); 
        }
    }
}
