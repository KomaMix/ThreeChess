using ThreeChess.Models;

namespace ThreeChess.Interfaces
{
    public interface ILobbyWaitingService
    {
        public void StartCountdown(Guid lobbyId);
        public void CancelCountdown(Guid lobbyId);
    }
}
