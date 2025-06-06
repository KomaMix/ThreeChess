using StackExchange.Redis;
using System.Collections.Generic;
using System.Text.Json;
using ThreeChess.Interfaces;
using ThreeChess.Models;

namespace ThreeChess.Services
{
    public class RedisLobbyManager : ILobbyManager
    {
        private readonly IDatabase _redisDb;
        private readonly ConnectionMultiplexer _redis;
        private const string LOBBY_KEY_PREFIX = "lobby:";
        private const string LOBBY_PLAYERS_PREFIX = "lobby:players:";
        private const string PLAYER_LOBBY_KEY_PREFIX = "player:lobby:";
        private const string LOBBY_ID_COUNTER = "counter:lobby:id";

        public RedisLobbyManager(IConfiguration configuration)
        {
            _redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection"));
            _redisDb = _redis.GetDatabase();
            InitializeLobbies();
        }

        private void InitializeLobbies()
        {
            // Проверяем, существует ли хотя бы одно лобби
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            bool lobbiesExist = server.Keys(pattern: $"{LOBBY_KEY_PREFIX}*").Any();

            var lobbyId = (int)_redisDb.StringGet(LOBBY_ID_COUNTER) + 1;

            // Если лобби нет и счетчик тоже отсутствует, инициализируем
            if (!lobbiesExist)
            {
                _redisDb.StringSet(LOBBY_ID_COUNTER, 0);
                for (int i = 0; i < 10; i++)
                {
                    CreateLobby(Guid.NewGuid());
                }
            }
        }

        public IEnumerable<Lobby> GetAllLobbies()
        {
            var server = _redis.GetServer(_redis.GetEndPoints().First());

            var sortedLobbyIds = server.Keys(pattern: $"{LOBBY_KEY_PREFIX}*")
                                      .Where(k => Guid.TryParse(k.ToString().Split(':')[1], out _))
                                      .Select(k => Guid.Parse(k.ToString().Split(':')[1]))
                                      .OrderBy(id => id);

            foreach (var lobbyId in sortedLobbyIds)
            {
                yield return GetLobby(lobbyId);
            }
        }

        public bool JoinLobby(Guid lobbyId, Guid playerId)
        {
            var transaction = _redisDb.CreateTransaction();

            // Проверка, что игрок не в другом лобби
            transaction.AddCondition(Condition.KeyNotExists($"{PLAYER_LOBBY_KEY_PREFIX}{playerId}"));
            // Проверка свободных мест (максимум 3 игрока)
            transaction.AddCondition(Condition.SetLengthLessThan($"{LOBBY_PLAYERS_PREFIX}{lobbyId}", 3));

            // Добавление игрока в набор игроков лобби
            transaction.SetAddAsync($"{LOBBY_PLAYERS_PREFIX}{lobbyId}", playerId.ToString());
            // Сохранение связи игрок -> лобби
            transaction.StringSetAsync($"{PLAYER_LOBBY_KEY_PREFIX}{playerId}", lobbyId.ToString());

            return transaction.Execute();
        }

        public bool LeaveLobby(Guid lobbyId, Guid playerId)
        {
            var transaction = _redisDb.CreateTransaction();

            transaction.AddCondition(Condition.KeyExists($"{PLAYER_LOBBY_KEY_PREFIX}{playerId}"));
            transaction.AddCondition(Condition.StringEqual($"{PLAYER_LOBBY_KEY_PREFIX}{playerId}", lobbyId.ToString()));

            transaction.SetRemoveAsync($"{LOBBY_PLAYERS_PREFIX}{lobbyId}", playerId.ToString());
            transaction.KeyDeleteAsync($"{PLAYER_LOBBY_KEY_PREFIX}{playerId}");

            return transaction.Execute();
        }

        public Lobby GetLobby(Guid lobbyId)
        {
            var data = _redisDb.HashGet($"{LOBBY_KEY_PREFIX}{lobbyId}", "data");
            if (data.IsNull) return null;

            var lobby = JsonSerializer.Deserialize<Lobby>(data);
            lobby.PlayerIds = _redisDb.SetMembers($"{LOBBY_PLAYERS_PREFIX}{lobbyId}")
                      .Select(x =>
                      {
                          Guid id;
                          return Guid.TryParse(x.ToString(), out id) ? id : Guid.Empty;
                      })
                      .Where(g => g != Guid.Empty)
                      .ToList();
            return lobby;
        }

        public bool PlayerExist(Guid lobbyId, Guid playerId)
        {
            return _redisDb.KeyExists($"{PLAYER_LOBBY_KEY_PREFIX}{playerId}")
                && _redisDb.StringGet($"{PLAYER_LOBBY_KEY_PREFIX}{playerId}").ToString() == lobbyId.ToString();
        }

        public bool RemoveLobby(Guid lobbyId)
        {
            var players = _redisDb.SetMembers($"{LOBBY_PLAYERS_PREFIX}{lobbyId}");
            var transaction = _redisDb.CreateTransaction();

            foreach (var player in players)
                transaction.KeyDeleteAsync($"{PLAYER_LOBBY_KEY_PREFIX}{player}");

            transaction.KeyDeleteAsync($"{LOBBY_KEY_PREFIX}{lobbyId}");
            transaction.KeyDeleteAsync($"{LOBBY_PLAYERS_PREFIX}{lobbyId}");
            return transaction.Execute();
        }

        private void CreateLobby(Guid lobbyId)
        {
            var lobby = new Lobby
            {
                Id = lobbyId,
                GameDuration = TimeSpan.FromMinutes(10),
            };

            // Сохраняем данные лобби в хэш
            _redisDb.HashSet($"{LOBBY_KEY_PREFIX}{lobbyId}",
                new HashEntry[] { new("data", JsonSerializer.Serialize(lobby)) });
        }
    }
}
