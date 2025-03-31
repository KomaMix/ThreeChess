using System.Drawing;

namespace ThreeChess.Models
{
    public class CellItem
    {
        public string Id { get; set; }
        public bool IsWhite { get; set; }
        public Polygon Polygon { get; set; }
    }
}
