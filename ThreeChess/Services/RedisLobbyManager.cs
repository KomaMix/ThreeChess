using StackExchange.Redis;
using System.Text.Json;
using ThreeChess.Interfaces;
using ThreeChess.Models;

namespace ThreeChess.Services
{
    public class RedisLobbyManager : ILobbyManager
    {
        private readonly IDatabase _db;
        private readonly ConnectionMultiplexer _redis;
        private const string LobbyKeyPrefix = "lobby:";
        private const string PlayerLobbyKeyPrefix = "player:lobby:";
        private const string LobbyIdCounter = "lobby:id";

        public RedisLobbyManager(IConfiguration configuration)
        {
            _redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection"));
            _db = _redis.GetDatabase();
            InitializeLobbies();
        }

        private void InitializeLobbies()
        {
            if (!_db.KeyExists(LobbyIdCounter))
            {
                for (int i = 0; i < 10; i++)
                {
                    CreateLobby();
                }
            }
        }

        public IEnumerable<Lobby> GetAllLobbies()
        {
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var keys = server.Keys(pattern: $"{LobbyKeyPrefix}*");

            foreach (var key in keys)
            {
                var hash = _db.HashGetAll(key);
                yield return JsonSerializer.Deserialize<Lobby>(hash[0].Value);
            }
        }

        public bool JoinLobby(int lobbyId, string playerId)
        {
            var transaction = _db.CreateTransaction();

            // Проверка существования игрока в другом лобби
            transaction.AddCondition(Condition.KeyNotExists($"{PlayerLobbyKeyPrefix}{playerId}"));

            // Проверка свободных мест
            transaction.AddCondition(Condition.HashLengthLessThan($"{LobbyKeyPrefix}{lobbyId}", 3));

            // Добавление игрока
            transaction.HashSetAsync($"{LobbyKeyPrefix}{lobbyId}",
                new HashEntry[] { new(playerId, "") });

            transaction.StringSetAsync($"{PlayerLobbyKeyPrefix}{playerId}", lobbyId);

            return transaction.Execute();
        }

        public bool LeaveLobby(int lobbyId, string playerId)
        {
            var transaction = _db.CreateTransaction();

            transaction.AddCondition(Condition.KeyExists($"{PlayerLobbyKeyPrefix}{playerId}"));

            transaction.HashDeleteAsync($"{LobbyKeyPrefix}{lobbyId}", playerId);
            transaction.KeyDeleteAsync($"{PlayerLobbyKeyPrefix}{playerId}");

            return transaction.Execute();
        }

        public Lobby GetLobby(int lobbyId)
        {
            var hash = _db.HashGetAll($"{LobbyKeyPrefix}{lobbyId}");
            return hash.Length == 0
                ? null
                : JsonSerializer.Deserialize<Lobby>(hash[0].Value);
        }

        public bool PlayerExist(int lobbyId, string playerId)
        {
            return _db.KeyExists($"{PlayerLobbyKeyPrefix}{playerId}") &&
                   (int)_db.StringGet($"{PlayerLobbyKeyPrefix}{playerId}") == lobbyId;
        }

        public bool RemoveLobby(int lobbyId)
        {
            var players = _db.HashKeys($"{LobbyKeyPrefix}{lobbyId}");
            var transaction = _db.CreateTransaction();

            foreach (var player in players)
                transaction.KeyDeleteAsync($"{PlayerLobbyKeyPrefix}{player}");

            transaction.KeyDeleteAsync($"{LobbyKeyPrefix}{lobbyId}");
            return transaction.Execute();
        }

        private void CreateLobby()
        {
            var lobbyId = (int)_db.StringIncrement(LobbyIdCounter);
            var lobby = new Lobby
            {
                GameDuration = TimeSpan.FromMinutes(10)
            };

            _db.HashSet($"{LobbyKeyPrefix}{lobbyId}",
                new HashEntry[] { new("data", JsonSerializer.Serialize(lobby)) });
        }
    }
}
