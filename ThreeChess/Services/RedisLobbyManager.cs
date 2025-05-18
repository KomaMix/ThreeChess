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
        private const string LobbyPlayersPrefix = "lobby:players:";
        private const string PlayerLobbyKeyPrefix = "player:lobby:";
        private const string LobbyIdCounter = "counter:lobby:id";

        public RedisLobbyManager(IConfiguration configuration)
        {
            _redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection"));
            _db = _redis.GetDatabase();
            InitializeLobbies();
        }

        private void InitializeLobbies()
        {
            // Проверяем, существует ли хотя бы одно лобби
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            bool lobbiesExist = server.Keys(pattern: $"{LobbyKeyPrefix}*").Any();

            // Если лобби нет и счетчик тоже отсутствует, инициализируем
            if (!lobbiesExist && !_db.KeyExists(LobbyIdCounter))
            {
                _db.StringSet(LobbyIdCounter, 0);
                for (int i = 0; i < 10; i++)
                {
                    CreateLobby();
                }
            }
        }

        public IEnumerable<Lobby> GetAllLobbies()
        {
            var server = _redis.GetServer(_redis.GetEndPoints().First());

            var sortedLobbyIds = server.Keys(pattern: $"{LobbyKeyPrefix}*")
                                      .Where(k => int.TryParse(k.ToString().Split(':')[1], out _))
                                      .Select(k => int.Parse(k.ToString().Split(':')[1]))
                                      .OrderBy(id => id);

            foreach (var lobbyId in sortedLobbyIds)
            {
                yield return GetLobby(lobbyId);
            }
        }

        public bool JoinLobby(int lobbyId, string playerId)
        {
            var transaction = _db.CreateTransaction();

            // Проверка, что игрок не в другом лобби
            transaction.AddCondition(Condition.KeyNotExists($"{PlayerLobbyKeyPrefix}{playerId}"));
            // Проверка свободных мест (максимум 3 игрока)
            transaction.AddCondition(Condition.SetLengthLessThan($"{LobbyPlayersPrefix}{lobbyId}", 3));

            // Добавление игрока в набор игроков лобби
            transaction.SetAddAsync($"{LobbyPlayersPrefix}{lobbyId}", playerId);
            // Сохранение связи игрок -> лобби
            transaction.StringSetAsync($"{PlayerLobbyKeyPrefix}{playerId}", lobbyId);

            return transaction.Execute();
        }

        public bool LeaveLobby(int lobbyId, string playerId)
        {
            var transaction = _db.CreateTransaction();

            transaction.AddCondition(Condition.KeyExists($"{PlayerLobbyKeyPrefix}{playerId}"));
            transaction.AddCondition(Condition.StringEqual($"{PlayerLobbyKeyPrefix}{playerId}", lobbyId));

            transaction.SetRemoveAsync($"{LobbyPlayersPrefix}{lobbyId}", playerId);
            transaction.KeyDeleteAsync($"{PlayerLobbyKeyPrefix}{playerId}");

            return transaction.Execute();
        }

        public Lobby GetLobby(int lobbyId)
        {
            var data = _db.HashGet($"{LobbyKeyPrefix}{lobbyId}", "data");
            if (data.IsNull) return null;

            var lobby = JsonSerializer.Deserialize<Lobby>(data);
            lobby.PlayerIds = _db.SetMembers($"{LobbyPlayersPrefix}{lobbyId}").Select(x => x.ToString()).ToList();
            return lobby;
        }

        public bool PlayerExist(int lobbyId, string playerId)
        {
            return _db.KeyExists($"{PlayerLobbyKeyPrefix}{playerId}")
                && (int)_db.StringGet($"{PlayerLobbyKeyPrefix}{playerId}") == lobbyId;
        }

        public bool RemoveLobby(int lobbyId)
        {
            var players = _db.SetMembers($"{LobbyPlayersPrefix}{lobbyId}");
            var transaction = _db.CreateTransaction();

            foreach (var player in players)
                transaction.KeyDeleteAsync($"{PlayerLobbyKeyPrefix}{player}");

            transaction.KeyDeleteAsync($"{LobbyKeyPrefix}{lobbyId}");
            transaction.KeyDeleteAsync($"{LobbyPlayersPrefix}{lobbyId}");
            return transaction.Execute();
        }

        private void CreateLobby()
        {
            var lobbyId = (int)_db.StringIncrement(LobbyIdCounter);
            var lobby = new Lobby
            {
                Id = Lobby.NewId(),
                GameDuration = TimeSpan.FromMinutes(10),
            };

            // Сохраняем данные лобби в хэш
            _db.HashSet($"{LobbyKeyPrefix}{lobbyId}",
                new HashEntry[] { new("data", JsonSerializer.Serialize(lobby)) });
        }
    }
}
