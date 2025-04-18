using Microsoft.AspNetCore.SignalR;
using ThreeChess.Models;
using ThreeChess.Services;

namespace ThreeChess.Hubs
{
    public class LobbyHub : Hub
    {
        private readonly LobbyManager _gameManager;

        public LobbyHub(LobbyManager gameManager)
        {
            _gameManager = gameManager;
        }

        public async Task JoinLobby(int lobbyId)
        {
            var playerId = Context.UserIdentifier;
            if (_gameManager.JoinLobby(lobbyId, playerId))
            {
                await Clients.All.SendAsync("LobbiesUpdated", _gameManager.GetAllLobbies());
            }
        }

        public IEnumerable<Lobby> GetAllLobbies() => _gameManager.GetAllLobbies();
    }
}
