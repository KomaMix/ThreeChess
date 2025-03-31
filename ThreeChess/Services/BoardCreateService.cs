using ThreeChess.Models;

namespace ThreeChess.Services
{
    public class BoardCreateService
    {
        public List<CellItem> CreateBoardCells()
        {
            var rightCells = GetUpperLeftTriangleCells();

            rightCells = rightCells.Concat(GetTriangleCells(
                    new char[] { 'D', 'C', 'B', 'A' },
                    new int[] { 5, 6, 7, 8 },
                    -60, true, false)).ToList();

            rightCells = rightCells.Concat(GetTriangleCells(
                    new char[] { 'D', 'C', 'B', 'A' },
                    new int[] { 4, 3, 2, 1 },
                    -120, false, true)).ToList();

            rightCells = rightCells.Concat(GetTriangleCells(
                    new char[] { 'E', 'F', 'G', 'H' },
                    new int[] { 4, 3, 2, 1 },
                    -180, true, false)).ToList();

            rightCells = rightCells.Concat(GetTriangleCells(
                    new char[] { 'E', 'F', 'G', 'H' },
                    new int[] { 9, 10, 11, 12 },
                    -240, false, true)).ToList();

            rightCells = rightCells.Concat(GetTriangleCells(
                    new char[] { 'I', 'J', 'K', 'L' },
                    new int[] { 9, 10, 11, 12 },
                    -300, true, false)).ToList();


            return rightCells;
        }


        private List<CellItem> GetTriangleCells(char[] alphs, int[] nums, int rotate, bool isMainNums, bool isFirstWhite)
        {
            var rightCells = GetUpperLeftTriangleCells();

            var resultCells = new List<CellItem>();

            int cnt;
            if (isFirstWhite)
            {
                cnt = 0;
            } else
            {
                cnt = 1;
            }

            for (int i = 0; i < rightCells.Count; i++)
            {
                resultCells.Add(new CellItem
                {
                    Id = alphs[isMainNums ? i % 4 : i / 4].ToString()
                    + nums[!isMainNums ? i % 4 : i / 4].ToString(),
                    Polygon = new Polygon
                    {
                        Points = rightCells[i].Polygon.Points.Select(p => new Point
                        {
                            X = p.X * Math.Cos(DegreesToRadians(rotate)) - p.Y * Math.Sin(DegreesToRadians(rotate)),
                            Y = p.X * Math.Sin(DegreesToRadians(rotate)) + p.Y * Math.Cos(DegreesToRadians(rotate)),
                        }).ToList()
                    },
                    IsWhite = cnt % 2 == 0
                });


                cnt++;
                if (i % 4 == 3)
                {
                    cnt++;
                }
            }


            return resultCells;
        }

        private List<CellItem> GetUpperLeftTriangleCells()
        {
            List<CellItem> cellItems = new List<CellItem>();

            Point centerPoint = new Point { X = 0, Y = 0 };

            Point rightUpPoint = new Point
            {
                X = Math.Cos(DegreesToRadians(30)) * Math.Cos(DegreesToRadians(30)),
                Y = Math.Sin(DegreesToRadians(30) * Math.Cos(DegreesToRadians(30)))
            };
            Point rightBottomPoint = new Point { X = rightUpPoint.X, Y = -rightUpPoint.Y };

            Point rightPoint = new Point { X = 1, Y = 0 };

            Point[] rightUpLine = GetBetweenPoints(centerPoint, rightUpPoint, 5);
            Point[] bottomRightLine = GetBetweenPoints(rightBottomPoint, rightPoint, 5);

            char[] alphs = { 'I', 'J', 'K', 'L' };
            int[] nums = { 5, 6, 7, 8 };
            int cnt = 0;
            
            for (int i = 0; i < 4; i++)
            {
                Point[] line1 = GetBetweenPoints(rightUpLine[i], bottomRightLine[i], 5);
                Point[] line2 = GetBetweenPoints(rightUpLine[i + 1], bottomRightLine[i + 1], 5);

                for (int j = 0; j < 4; j++)
                {
                    List<Point> points = new List<Point>()
                    {
                        line1[j],
                        line1[j + 1],
                        line2[j + 1],
                        line2[j],
                        line1[j]
                    };

                    cellItems.Add(new CellItem
                    {
                        Id = alphs[i].ToString() + nums[j].ToString(),
                        IsWhite = cnt % 2 == 0,
                        Polygon = new Polygon
                        {
                            Points = points
                        }
                    });

                    cnt++;
                    if (j == 3)
                    {
                        cnt += 1;
                    }
                }
            }


            return cellItems;
        }

        private double DegreesToRadians(double degrees)
        {
            return Math.PI * degrees / 180.0;
        }

        private Point[] GetBetweenPoints(Point p1, Point p2, int countPoints)
        {
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;

            return Enumerable.Range(0, countPoints)
                .Select(i => new Point
                {
                    X = p1.X + i * dx / (countPoints - 1),
                    Y = p1.Y + i * dy / (countPoints - 1)
                }).ToArray();
        }
    }
}
