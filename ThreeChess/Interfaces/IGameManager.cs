namespace ThreeChess.Interfaces
{
    public interface IGameManager
    {
        Task<bool> Move(Guid gameId, string startCellId, string endCellId);
    }
}
