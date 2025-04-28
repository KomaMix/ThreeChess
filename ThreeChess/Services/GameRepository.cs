using System.Collections.Concurrent;
using ThreeChess.Interfaces;
using ThreeChess.Models;

namespace ThreeChess.Services
{
    public class GameRepository : IGameRepository
    {
        private readonly ConcurrentDictionary<Guid, GameState> _games = new();

        public void CreateGame(GameState gameState)
        {
            _games.TryAdd(gameState.Id, gameState);
        }

        public GameState GetGame(Guid gameId)
        {
            return _games.TryGetValue(gameId, out var game) ? game : null;
        }
    }
}
