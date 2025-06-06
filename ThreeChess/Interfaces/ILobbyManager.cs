using ThreeChess.Models;

namespace ThreeChess.Interfaces
{
    public interface ILobbyManager
    {
        IEnumerable<Lobby> GetAllLobbies();
        Lobby GetLobby(Guid lobbyId);
        bool JoinLobby(Guid lobbyId, Guid playerId);
        bool LeaveLobby(Guid lobbyId, Guid playerId);
        bool PlayerExist(Guid lobbyId, Guid playerId);
        bool RemoveLobby(Guid lobbyId);
    }
}
