namespace ThreeChess.Interfaces
{
    public interface IMoveLogicalElementsService
    {
        public List<List<string>> GetDiagonals();
        public List<List<string>> GetMainLines();
        public List<List<string>> GetSecondaryLines();
    }
}
