using ThreeChess.Models;

namespace ThreeChess.Interfaces
{
    public interface IGameRepository
    {
        public void CreateGame(GameState gameState);
        public GameState GetGame(Guid gameId);
    }
}
