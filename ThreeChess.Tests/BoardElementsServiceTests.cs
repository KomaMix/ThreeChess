using ThreeChess.Enums;
using ThreeChess.Interfaces;
using ThreeChess.Models.CellElements;
using ThreeChess.Services;

namespace ThreeChess.Tests
{
    public class BoardElementsServiceTests
    {
        private readonly BoardElementsService _service = new BoardElementsService();

        [Theory]
        [InlineData(FigureColor.White)]
        [InlineData(FigureColor.Black)]
        [InlineData(FigureColor.Red)]
        public void CreateBoardCellsForColor_Returns96Cells(FigureColor color)
        {
            // Act
            var cells = _service.CreateBoardCellsForColor(color);

            // Assert
            Assert.Equal(96, cells.Count);
        }

        [Fact]
        public void CreateFigures_ReturnsCorrectTotalCount()
        {
            // Act
            var figures = _service.CreateFigures();

            // Assert
            Assert.Equal(48, figures.Count);
        }

        [Theory]
        [InlineData(FigureColor.White, 16)]
        [InlineData(FigureColor.Black, 16)]
        [InlineData(FigureColor.Red, 16)]
        public void CreateFigures_ReturnsCorrectCountForEachColor(FigureColor color, int expectedCount)
        {
            // Act
            var figures = _service.CreateFigures();

            // Assert
            var colorFigures = figures.Values.Where(f => f.FigureColor == color);
            Assert.Equal(expectedCount, colorFigures.Count());
        }

        [Theory]
        [InlineData("A8", FigureType.Rook, FigureColor.White)]
        [InlineData("D8", FigureType.King, FigureColor.White)]
        [InlineData("I8", FigureType.Queen, FigureColor.White)]
        [InlineData("L12", FigureType.Rook, FigureColor.Black)]
        [InlineData("I12", FigureType.King, FigureColor.Black)]
        [InlineData("E12", FigureType.Queen, FigureColor.Black)]
        [InlineData("A1", FigureType.Rook, FigureColor.Red)]
        [InlineData("E1", FigureType.King, FigureColor.Red)]
        [InlineData("D1", FigureType.Queen, FigureColor.Red)]
        public void CreateFigures_ReturnsCorrectFiguresAtPositions(
            string position,
            FigureType expectedType,
            FigureColor expectedColor)
        {
            // Act
            var figures = _service.CreateFigures();

            // Assert
            Assert.True(figures.ContainsKey(position));
            Assert.Equal(expectedType, figures[position].FigureType);
            Assert.Equal(expectedColor, figures[position].FigureColor);
        }

        [Fact]
        public void CreateBoardCellsForColor_ReturnsUniqueCellIds()
        {
            // Arrange
            var colors = new[] { FigureColor.White, FigureColor.Black, FigureColor.Red };

            foreach (var color in colors)
            {
                // Act
                var cells = _service.CreateBoardCellsForColor(color);
                var cellIds = cells.Select(c => c.Id).ToList();

                // Assert
                Assert.Equal(96, cellIds.Count);
                Assert.Equal(96, cellIds.Distinct().Count());
            }
        }

        [Fact]
        public void CreateBoardCellsForColor_AllCellsHaveValidPolygons()
        {
            // Arrange
            var colors = new[] { FigureColor.White, FigureColor.Black, FigureColor.Red };

            foreach (var color in colors)
            {
                // Act
                var cells = _service.CreateBoardCellsForColor(color);

                // Assert
                foreach (var cell in cells)
                {
                    Assert.NotNull(cell.Polygon);
                    Assert.NotNull(cell.Polygon.Points);
                    Assert.Equal(5, cell.Polygon.Points.Count); // 5 точек (замкнутый многоугольник)
                    Assert.NotNull(cell.Center);
                }
            }
        }

        [Fact]
        public void CreateBoardCellsForColor_CentersAreCalculatedCorrectly()
        {
            // Arrange
            var testCell = new CellItem
            {
                Polygon = new Polygon
                {
                    Points = new List<Point>
                {
                    new Point { X = 0, Y = 0 },
                    new Point { X = 2, Y = 0 },
                    new Point { X = 2, Y = 2 },
                    new Point { X = 0, Y = 2 },
                    new Point { X = 0, Y = 0 } // closed
                }
                }
            };

            // Act
            var center = _service.FindMidLinesIntersection(testCell.Polygon.Points);

            // Assert
            Assert.Equal(1, center.X);
            Assert.Equal(1, center.Y);
        }

        [Fact]
        public void RotatePoint_AppliesCorrectTransformation()
        {
            // Arrange
            var point = new Point { X = 1, Y = 0 };
            var rotate90 = 90; // градусов

            // Act
            var rotated = _service.RotatePoint(point, rotate90);

            // Assert
            Assert.Equal(0, rotated.X, precision: 5);
            Assert.Equal(1, rotated.Y, precision: 5);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(60)]
        [InlineData(120)]
        [InlineData(180)]
        [InlineData(240)]
        [InlineData(300)]
        public void CreateBoardCellsForColor_RotationPreservesCenterDistances(int rotation)
        {
            // Arrange
            var baseCells = _service.CreateBoardCellsForBlack();
            var testCell = baseCells.First();
            var originalCenter = testCell.Center;

            // Act
            var rotatedPoint = _service.RotatePoint(originalCenter, rotation);
            var distance = Math.Sqrt(rotatedPoint.X * rotatedPoint.X + rotatedPoint.Y * rotatedPoint.Y);
            var originalDistance = Math.Sqrt(originalCenter.X * originalCenter.X + originalCenter.Y * originalCenter.Y);

            // Assert
            Assert.Equal(originalDistance, distance, precision: 4);
        }

        
        [Fact]
        public void CreateFigures_HasCorrectPawnCounts()
        {
            // Act
            var figures = _service.CreateFigures();

            // Assert
            var whitePawns = figures.Values.Count(f =>
                f.FigureColor == FigureColor.White &&
                f.FigureType == FigureType.Pawn);

            var blackPawns = figures.Values.Count(f =>
                f.FigureColor == FigureColor.Black &&
                f.FigureType == FigureType.Pawn);

            var redPawns = figures.Values.Count(f =>
                f.FigureColor == FigureColor.Red &&
                f.FigureType == FigureType.Pawn);

            Assert.Equal(8, whitePawns);
            Assert.Equal(8, blackPawns);
            Assert.Equal(8, redPawns);
        }
    }
}