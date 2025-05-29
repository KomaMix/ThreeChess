using System.Collections.Concurrent;
using ThreeChess.Interfaces;
using ThreeChess.Models;

namespace ThreeChess.Services
{
    public class GameRepository : IGameRepository
    {
        private readonly ConcurrentDictionary<Guid, GameState> _games = new ConcurrentDictionary<Guid, GameState>();

        public void CreateGame(GameState gameState)
        {
            _games.TryAdd(gameState.Id, gameState);
        }

        public IEnumerable<GameState> GetAllGames()
        {
            return _games.Values;
        }

        public GameState GetGame(Guid gameId)
        {
            return _games.TryGetValue(gameId, out var game) ? game : null;
        }

        public void UpdateGame(GameState gameState)
        {
            
        }
    }
}
