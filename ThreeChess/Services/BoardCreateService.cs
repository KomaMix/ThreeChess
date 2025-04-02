using ThreeChess.Models;

namespace ThreeChess.Services
{
    public class BoardCreateService
    {
        public Dictionary<string, string> CreateFigures()
        {
            Dictionary<string, string> figuresMap = new Dictionary<string, string>();
            
            figuresMap["A8"] = "/images/white_rook.svg";
            figuresMap["L8"] = "/images/white_rook.svg";

            figuresMap["B8"] = "/images/white_knight.svg";
            figuresMap["K8"] = "/images/white_knight.svg";

            figuresMap["C8"] = "/images/white_bishop.svg";
            figuresMap["J8"] = "/images/white_bishop.svg";

            figuresMap["D8"] = "/images/white_king.svg";
            figuresMap["I8"] = "/images/white_queen.svg";

            foreach (var c in "ABCDIJKL")
            {
                string id = c + "7";
                figuresMap[id] = "/images/white_pawn.svg";
            }


            figuresMap["L12"] = "/images/black_rook.svg";
            figuresMap["H12"] = "/images/black_rook.svg";

            figuresMap["G12"] = "/images/black_knight.svg";
            figuresMap["K12"] = "/images/black_knight.svg";

            figuresMap["J12"] = "/images/black_bishop.svg";
            figuresMap["F12"] = "/images/black_bishop.svg";

            figuresMap["I12"] = "/images/black_king.svg";
            figuresMap["E12"] = "/images/black_queen.svg";

            foreach (var c in "EFGHIJKL")
            {
                string id = c + "11";
                figuresMap[id] = "/images/black_pawn.svg";
            }


            figuresMap["A1"] = "/images/red_rook.svg";
            figuresMap["H1"] = "/images/red_rook.svg";

            figuresMap["B1"] = "/images/red_knight.svg";
            figuresMap["G1"] = "/images/red_knight.svg";

            figuresMap["C1"] = "/images/red_bishop.svg";
            figuresMap["F1"] = "/images/red_bishop.svg";

            figuresMap["D1"] = "/images/red_king.svg";
            figuresMap["E1"] = "/images/red_queen.svg";

            foreach (var c in "ABCDEFGH")
            {
                string id = c + "2";
                figuresMap[id] = "/images/red_pawn.svg";
            }


            return figuresMap;
        }

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

                AddedCenters(resultCells);


                cnt++;
                if (i % 4 == 3)
                {
                    cnt++;
                }
            }


            return resultCells;
        }

        private void AddedCenters(List<CellItem> cells)
        {
            foreach (var cell in cells)
            {
                cell.Center = FindMidLinesIntersection(cell.Polygon.Points);
            }
        }

        private Point FindMidLinesIntersection(List<Point> points)
        {
            Point mid1 = GetMidPoint(points[0], points[1]);
            Point mid2 = GetMidPoint(points[2], points[3]);
            Point mid3 = GetMidPoint(points[1], points[2]);
            Point mid4 = GetMidPoint(points[3], points[0]);

            Line line1 = new Line
            {
                Start = mid1,
                End = mid2,
            };

            Line line2 = new Line
            {
                Start = mid3,
                End = mid4,
            };

            return LineIntersection(line1 , line2); 
        }

        private class Line
        {
            public Point Start { get; set; }
            public Point End { get; set; }
        }

        private Point GetMidPoint(Point p1, Point p2)
        {
            return new Point
            {
                X = (p1.X + p2.X) / 2,
                Y = (p1.Y + p2.Y) / 2
            };
        }

        private Point LineIntersection(Line line1, Line line2)
        {
            double x1 = line1.Start.X, y1 = line1.Start.Y;
            double x2 = line1.End.X, y2 = line1.End.Y;
            double x3 = line2.Start.X, y3 = line2.Start.Y;
            double x4 = line2.End.X, y4 = line2.End.Y;

            double denominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            if (denominator == 0) throw new Exception("lines parallel");

            double px = ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) / denominator;
            double py = ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) / denominator;

            return new Point
            {
                X = px,
                Y = py
            };
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

            AddedCenters(cellItems);

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
