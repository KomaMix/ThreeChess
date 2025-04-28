using ThreeChess.Enums;
using ThreeChess.Models;
using ThreeChess.Models.CellElements;

namespace ThreeChess.Interfaces
{
    public interface IBoardElementsService
    {
        public Dictionary<string, FigureInfo> CreateFigures();
        public List<CellItem> CreateBoardCellsForColor(FigureColor color);
    }
}
