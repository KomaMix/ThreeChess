using ThreeChess.Services;

namespace ThreeChess.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void CheckCellCount()
        {
            BoardElementsService boardCreateService = new BoardElementsService();

            var cells = boardCreateService.CreateBoardCellsForRed();

            Assert.Equal(96, cells.Count);
        }
    }
}