using ThreeChess.Services;

namespace ThreeChess.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void CheckCellCount()
        {
            BoardCreateService boardCreateService = new BoardCreateService();

            var cells = boardCreateService.CreateBoardCellsForRed();

            Assert.Equal(96, cells.Count);
        }
    }
}