using ThreeChess.Models;

namespace ThreeChess.Services
{
    public class GameManager
    {
        private readonly List<GameState> _games = new();

        public IEnumerable<GameState> GetAllGames() => _games;


        public void CreateGame(GameState gameState)
        {
            _games.Add(gameState);
        }
    }
}
