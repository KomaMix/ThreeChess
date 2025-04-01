using ThreeChess.Enums;

namespace ThreeChess.Interfaces
{
    public interface IMoveHandlerService
    {
        public Task<bool> ValidateAndApplyMoveAsync(
            string gameSessionId,
            string playerId,
            string startCellId,
            string endCellId);

        public Task<bool> ReplacePawnAsync(
            string gameSessionId,
            string playerId,
            string startCellId,
            string endCellId,
            FigureType figureType);
        
    }
}
