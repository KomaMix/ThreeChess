using ThreeChess.Enums;
using ThreeChess.Interfaces;

namespace ThreeChess.Services
{
    public class MoveHandlerService : IMoveHandlerService
    {
        public Task<bool> ReplacePawnAsync(string gameSessionId, string playerId, string startCellId, string endCellId, FigureType figureType)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateAndApplyMoveAsync(string gameSessionId, string playerId, string startCellId, string endCellId)
        {
            throw new NotImplementedException();
        }
    }
}
