using ThreeChess.Models;

namespace ThreeChess.Interfaces
{
    public interface IGameRepository
    {
        void CreateGame(GameState gameState);
        GameState GetGame(Guid gameId);
        void UpdateGame(GameState gameState);
        IEnumerable<GameState> GetAllGames();
    }
}
