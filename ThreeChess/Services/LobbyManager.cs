using System.Collections.Concurrent;
using ThreeChess.Interfaces;
using ThreeChess.Models;

namespace ThreeChess.Services
{
    public class LobbyManager : ILobbyManager
    {
        private readonly List<Lobby> _lobbies = new();
        private readonly ConcurrentDictionary<Guid, Guid> _playerLobbyMap = new();

        public LobbyManager()
        {
            for (int i = 0; i < 100; i++)
            {
                CreateLobby();
            }
        }

        public IEnumerable<Lobby> GetAllLobbies() => _lobbies;

        public bool JoinLobby(Guid lobbyId, Guid playerId)
        {
            if (_playerLobbyMap.ContainsKey(playerId))
            {
                return false;
            }


            var lobby = _lobbies.FirstOrDefault(l => l.Id == lobbyId);
            if (lobby == null || lobby.PlayerIds.Count >= 3) return false;

            lobby.PlayerIds.Add(playerId);
            _playerLobbyMap[playerId] = lobbyId;

            return true;
        }

        public bool LeaveLobby(Guid lobbyId, Guid playerId)
        {
            if (!_playerLobbyMap.ContainsKey(playerId) || _playerLobbyMap[playerId] != lobbyId)
            {
                return false;
            }

            var lobby = _lobbies.FirstOrDefault(l => l.Id == lobbyId);

            if (lobby == null)
            {
                return false;
            }

            lobby.PlayerIds.Remove(playerId);
            _playerLobbyMap.Remove(playerId, out var a);

            return true;
        }

        private void CreateLobby()
        {
            _lobbies.Add(new Lobby
            {
                GameDuration = TimeSpan.FromMinutes(10)
            });
        }

        public Lobby GetLobby(Guid lobbyId)
        {
            return _lobbies.FirstOrDefault(l => l.Id == lobbyId);
        }

        public bool PlayerExist(Guid lobbyId, Guid playerId)
        {
            if (_playerLobbyMap.ContainsKey(playerId) && _playerLobbyMap[playerId] == lobbyId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveLobby(Guid lobbyId)
        {
            var lobby = _lobbies.FirstOrDefault(l => l.Id == lobbyId);
            if (lobby == null) return false;

            foreach (var playerId in lobby.PlayerIds)
                _playerLobbyMap.Remove(playerId, out var a);

            return _lobbies.Remove(lobby);

        }
    }



}
