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

        // Конструктор с подключением к Redis
        public RedisGameRepository(string connectionString = "localhost:6379,abortConnect=false,connectTimeout=5000")
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _db = _redis.GetDatabase();
        }

        public void CreateGame(GameState gameState)
        {
            // Формируем ключ вида "game:123e4567-e89b-12d3-a456-426614174000"
            var key = $"game:data:{gameState.Id}";

            // Сериализуем объект в JSON
            var json = JsonSerializer.Serialize(gameState);

            // Сохраняем в Redis с бессрочным сроком жизни
            _db.StringSet(key, json);
        }

        public GameState GetGame(Guid gameId)
        {
            var key = $"game:data:{gameId}";

            // Получаем данные из Redis
            var json = _db.StringGet(key);

            if (json.IsNull) return null;

            // Десериализуем обратно в объект
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

            // Исправляем шаблон ключей (game:* вместо games:*)
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
