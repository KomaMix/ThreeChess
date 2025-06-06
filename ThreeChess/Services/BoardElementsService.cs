using ThreeChess.Enums;
using ThreeChess.Interfaces;
using ThreeChess.Models;
using ThreeChess.Models.CellElements;

namespace ThreeChess.Services
{
    public class BoardElementsService : IBoardElementsService
    {
        public Dictionary<string, FigureInfo> CreateFigures()
        {
            Dictionary<string, FigureInfo> figuresMap = new Dictionary<string, FigureInfo>();

            figuresMap["A8"] = new FigureInfo {
                Path = "/images/white_rook.svg",
                FigureType = FigureType.Rook,
                FigureColor = FigureColor.White
            };

            figuresMap["L8"] = new FigureInfo
            {
                Path = "/images/white_rook.svg",
                FigureType = FigureType.Rook,
                FigureColor = FigureColor.White
            };

            figuresMap["B8"] = new FigureInfo
            {
                Path = "/images/white_knight.svg",
                FigureType = FigureType.Knight,
                FigureColor = FigureColor.White
            };

            figuresMap["K8"] = new FigureInfo
            {
                Path = "/images/white_knight.svg",
                FigureType = FigureType.Knight,
                FigureColor = FigureColor.White
            };

            figuresMap["C8"] = new FigureInfo
            {
                Path = "/images/white_bishop.svg",
                FigureType = FigureType.Bishop,
                FigureColor = FigureColor.White
            };

            figuresMap["J8"] = new FigureInfo
            {
                Path = "/images/white_bishop.svg",
                FigureType = FigureType.Bishop,
                FigureColor = FigureColor.White
            };

            figuresMap["D8"] = new FigureInfo
            {
                Path = "/images/white_king.svg",
                FigureType = FigureType.King,
                FigureColor = FigureColor.White
            };

            figuresMap["I8"] = new FigureInfo
            {
                Path = "/images/white_queen.svg",
                FigureType = FigureType.Queen,
                FigureColor = FigureColor.White
            };

            foreach (var c in "ABCDIJKL")
            {
                string id = c + "7";

                figuresMap[id] = new FigureInfo
                {
                    Path = "/images/white_pawn.svg",
                    FigureType = FigureType.Pawn,
                    FigureColor = FigureColor.White
                };
            }

            figuresMap["L12"] = new FigureInfo
            {
                Path = "/images/black_rook.svg",
                FigureType = FigureType.Rook,
                FigureColor = FigureColor.Black
            };

            figuresMap["H12"] = new FigureInfo
            {
                Path = "/images/black_rook.svg",
                FigureType = FigureType.Rook,
                FigureColor = FigureColor.Black
            };

            figuresMap["G12"] = new FigureInfo
            {
                Path = "/images/black_knight.svg",
                FigureType = FigureType.Knight,
                FigureColor = FigureColor.Black
            };

            figuresMap["K12"] = new FigureInfo
            {
                Path = "/images/black_knight.svg",
                FigureType = FigureType.Knight,
                FigureColor = FigureColor.Black
            };

            figuresMap["J12"] = new FigureInfo
            {
                Path = "/images/black_bishop.svg",
                FigureType = FigureType.Bishop,
                FigureColor = FigureColor.Black
            };

            figuresMap["F12"] = new FigureInfo
            {
                Path = "/images/black_bishop.svg",
                FigureType = FigureType.Bishop,
                FigureColor = FigureColor.Black
            };

            figuresMap["I12"] = new FigureInfo
            {
                Path = "/images/black_king.svg",
                FigureType = FigureType.King,
                FigureColor = FigureColor.Black
            };

            figuresMap["E12"] = new FigureInfo
            {
                Path = "/images/black_queen.svg",
                FigureType = FigureType.Queen,
                FigureColor = FigureColor.Black
            };

            foreach (var c in "EFGHIJKL")
            {
                string id = c + "11";
                figuresMap[id] = new FigureInfo
                {
                    Path = "/images/black_pawn.svg",
                    FigureType = FigureType.Pawn,
                    FigureColor = FigureColor.Black
                };
            }

            figuresMap["A1"] = new FigureInfo
            {
                Path = "/images/red_rook.svg",
                FigureType = FigureType.Rook,
                FigureColor = FigureColor.Red
            };

            figuresMap["H1"] = new FigureInfo
            {
                Path = "/images/red_rook.svg",
                FigureType = FigureType.Rook,
                FigureColor = FigureColor.Red
            };

            figuresMap["B1"] = new FigureInfo
            {
                Path = "/images/red_knight.svg",
                FigureType = FigureType.Knight,
                FigureColor = FigureColor.Red
            };

            figuresMap["G1"] = new FigureInfo
            {
                Path = "/images/red_knight.svg",
                FigureType = FigureType.Knight,
                FigureColor = FigureColor.Red
            };

            figuresMap["C1"] = new FigureInfo
            {
                Path = "/images/red_bishop.svg",
                FigureType = FigureType.Bishop,
                FigureColor = FigureColor.Red
            };

            figuresMap["F1"] = new FigureInfo
            {
                Path = "/images/red_bishop.svg",
                FigureType = FigureType.Bishop,
                FigureColor = FigureColor.Red
            };

            figuresMap["E1"] = new FigureInfo
            {
                Path = "/images/red_king.svg",
                FigureType = FigureType.King,
                FigureColor = FigureColor.Red
            };

            figuresMap["D1"] = new FigureInfo
            {
                Path = "/images/red_queen.svg",
                FigureType = FigureType.Queen,
                FigureColor = FigureColor.Red
            };

            foreach (var c in "ABCDEFGH")
            {
                string id = c + "2";
                figuresMap[id] = new FigureInfo
                {
                    Path = "/images/red_pawn.svg",
                    FigureType = FigureType.Pawn,
                    FigureColor = FigureColor.Red
                };
            }


            return figuresMap;
        }

        public List<CellItem> CreateBoardCellsForWhite()
        {
            var cells = CreateBoardCellsForBlack();

            int rotate = 120;

            foreach (var cell in cells)
            {
                for (int i = 0; i < cell.Polygon.Points.Count; i++)
                {
                    cell.Polygon.Points[i] = RotatePoint(cell.Polygon.Points[i], rotate);
                }

                cell.Center = RotatePoint(cell.Center, rotate);
            }


            return cells;
        }

        public List<CellItem> CreateBoardCellsForRed()
        {
            var cells = CreateBoardCellsForBlack();

            int rotate = 240;

            foreach (var cell in cells)
            {
                for (int i = 0; i < cell.Polygon.Points.Count; i++)
                {
                    cell.Polygon.Points[i] = RotatePoint(cell.Polygon.Points[i], rotate);
                }

                cell.Center = RotatePoint(cell.Center, rotate);
            }


            return cells;
        }

        public List<CellItem> CreateBoardCellsForBlack()
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

        public List<CellItem>  CreateBoardCellsForColor(FigureColor color)
        {
            switch (color)
            {
                case FigureColor.White:
                    return CreateBoardCellsForWhite();
                case FigureColor.Black:
                    return CreateBoardCellsForBlack();
                case FigureColor.Red:
                    return CreateBoardCellsForRed();
            }

            return CreateBoardCellsForWhite();
        }


        public List<CellItem> GetTriangleCells(char[] alphs, int[] nums, int rotate, bool isMainNums, bool isFirstWhite)
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
                        Points = rightCells[i].Polygon.Points
                            .Select(p => RotatePoint(p, rotate)).ToList()
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

        public Point RotatePoint(Point point, int rotate)
        {
            return new Point
            {
                X = point.X * Math.Cos(DegreesToRadians(rotate)) - point.Y * Math.Sin(DegreesToRadians(rotate)),
                Y = point.X * Math.Sin(DegreesToRadians(rotate)) + point.Y * Math.Cos(DegreesToRadians(rotate)),
            };
        }

        public double DegreesToRadians(double degrees)
        {
            return Math.PI * degrees / 180.0;
        }

        public void AddedCenters(List<CellItem> cells)
        {
            foreach (var cell in cells)
            {
                cell.Center = FindMidLinesIntersection(cell.Polygon.Points);
            }
        }

        public Point FindMidLinesIntersection(List<Point> points)
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

        public Point GetMidPoint(Point p1, Point p2)
        {
            return new Point
            {
                X = (p1.X + p2.X) / 2,
                Y = (p1.Y + p2.Y) / 2
            };
        }

        public Point LineIntersection(Line line1, Line line2)
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

        public List<CellItem> GetUpperLeftTriangleCells()
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

        public Point[] GetBetweenPoints(Point p1, Point p2, int countPoints)
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
