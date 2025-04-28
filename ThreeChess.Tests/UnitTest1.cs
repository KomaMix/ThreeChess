using ThreeChess.Enums;
using ThreeChess.Interfaces;
using ThreeChess.Services;

namespace ThreeChess.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void CheckCellCount()
        {
            IBoardElementsService boardCreateService = new BoardElementsService();

            var cells = boardCreateService.CreateBoardCellsForColor(FigureColor.Red);

            Assert.Equal(96, cells.Count);
        }
    }
}