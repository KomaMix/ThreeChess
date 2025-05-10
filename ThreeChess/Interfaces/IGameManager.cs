using ThreeChess.DTOs;

namespace ThreeChess.Interfaces
{
    public interface IGameManager
    {
        Task<MoveResponse> MoveHandle(MoveRequest moveRequest);
    }
}
