using ThreeChess.DTOs;
using ThreeChess.Models;

namespace ThreeChess.Interfaces
{
    public interface IMoveHistoryService
    {
        void AddMove(Guid gameId, Move move);
        IEnumerable<Move> GetMoveHistory(Guid gameId, int count = 50);
    }
}
