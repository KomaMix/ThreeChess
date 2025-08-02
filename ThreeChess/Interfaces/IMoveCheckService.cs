using ThreeChess.DTOs;

namespace ThreeChess.Interfaces
{
    public interface IMoveCheckService
    {
        bool MoveCheck(MoveRequest moveRequest);
    }
}
