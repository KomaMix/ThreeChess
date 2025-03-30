using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ThreeChess.Models;

namespace ThreeChess.Controllers
{
    [Route("api/")]
    [ApiController]
    public class BoardValuesController : ControllerBase
    {

        [HttpGet("cells")]
        public IActionResult Get()
        {
            

            var rightCells = GetCellItems();


            AddRatated(60, rightCells);
            AddRatated(120, rightCells);
            AddRatated(180, rightCells);
            AddRatated(240, rightCells);
            AddRatated(300, rightCells);


            return Ok(rightCells);
        }

        private void AddRatated(int rotate, List<CellItem> cells)
        {
            List<CellItem> rightUpCells = new List<CellItem>();
            foreach (var cell in cells.Take(16))
            {

                rightUpCells.Add(new CellItem
                {
                    Id = cell.Id + '1',
                    Polygon = new Polygon
                    {
                        Points = cell.Polygon.Points.Select(p => new Point
                        {
                            X = p.X * Math.Cos(DegreesToRadians(rotate)) - p.Y * Math.Sin(DegreesToRadians(rotate)),
                            Y = p.X * Math.Sin(DegreesToRadians(rotate)) + p.Y * Math.Cos(DegreesToRadians(rotate)),
                        }).ToList()
                    }
                });
            }

            cells.AddRange(rightUpCells);
        }

        private List<CellItem> GetCellItems()
        {
            List<CellItem> cellItems = new List<CellItem>();

            Point centerPoint = new Point { X = 0, Y = 0 };

            Point rightUpPoint = new Point { X = Math.Cos(DegreesToRadians(30)) * Math.Cos(DegreesToRadians(30)), 
                Y = Math.Sin(DegreesToRadians(30) * Math.Cos(DegreesToRadians(30)))
            };
            Point rightBottomPoint = new Point { X = rightUpPoint.X, Y = -rightUpPoint.Y };
            
            Point rightPoint = new Point { X = 1, Y = 0 };

            Point[] rightUpLine = GetBetweenPoints(centerPoint, rightUpPoint, 5);
            Point[] bottomRightLine = GetBetweenPoints(rightBottomPoint, rightPoint, 5);

            char[] alphs = { 'I', 'C', 'K', 'J' };
            int[] nums = { 5, 6, 7, 8 };

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
                        Id = alphs[j].ToString() + nums[i].ToString(),
                        Polygon = new Polygon
                        {
                            Points = points
                        }
                    });
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
