using ThreeChess.Models;

namespace ThreeChess.Interfaces
{
    public interface ILobbyManager
    {
        IEnumerable<Lobby> GetAllLobbies();
        Lobby GetLobby(int lobbyId);
        bool JoinLobby(int lobbyId, string playerId);
        bool LeaveLobby(int lobbyId, string playerId);
        bool PlayerExist(int lobbyId, string playerId);
        bool RemoveLobby(int lobbyId);
    }
}
