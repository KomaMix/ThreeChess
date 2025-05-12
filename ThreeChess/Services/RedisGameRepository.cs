using StackExchange.Redis;
using System.Text.Json;
using ThreeChess.Interfaces;
using ThreeChess.Models;

namespace ThreeChess.Services
{
    public class RedisGameRepository : IGameRepository
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;


        public RedisGameRepository(IConfiguration configuration)
        {
            _redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection"));
            _db = _redis.GetDatabase();
        }

        public void CreateGame(GameState gameState)
        {
            var key = $"game:data:{gameState.Id}";

            var json = JsonSerializer.Serialize(gameState);

            _db.StringSet(key, json);
        }

        public GameState GetGame(Guid gameId)
        {
            var key = $"game:data:{gameId}";

            var json = _db.StringGet(key);

            if (json.IsNull) return null;

            return JsonSerializer.Deserialize<GameState>(json);
        }

        public void UpdateGame(GameState gameState)
        {
            CreateGame(gameState);
        }

        public IEnumerable<GameState> GetAllGames()
        {
            var endpoints = _redis.GetEndPoints();
            if (endpoints.Length == 0) yield break;

            var server = _redis.GetServer(endpoints.First());

            var keys = server.Keys(pattern: "game:data:*");

            foreach (var key in keys)
            {
                var json = _db.StringGet(key);
                if (!json.IsNull)
                {
                    yield return JsonSerializer.Deserialize<GameState>(json);
                }
            }
        }

        public void Dispose()
        {
            _redis?.Close();
            _redis?.Dispose();
        }
    }
}
