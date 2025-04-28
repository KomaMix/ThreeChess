using ThreeChess.Models;

namespace ThreeChess.Interfaces
{
    public interface ILobbyWaitingService
    {
        public void StartCountdown(int lobbyId);
        public void CancelCountdown(int lobbyId);
    }
}
