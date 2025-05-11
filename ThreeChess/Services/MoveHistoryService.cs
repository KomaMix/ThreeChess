using StackExchange.Redis;
using ThreeChess.Interfaces;
using ThreeChess.Models;

namespace ThreeChess.Services
{
    

    public class MoveHistoryService : IMoveHistoryService
    {
        private readonly IDatabase _redis;

        public MoveHistoryService(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }

        public void AddMove(Guid gameId, Move move)
        {
            var streamKey = $"game:moves:{gameId}";
            var fields = new NameValueEntry[]
            {
                new("start", move.StartCellId),
                new("end", move.EndCellId)
            };

            _redis.StreamAdd(streamKey, fields);
        }

        public IEnumerable<Move> GetMoveHistory(Guid gameId, int count = 50)
        {
            var streamKey = $"game:moves:{gameId}";
            var entries = _redis.StreamRange(streamKey, minId: "-", maxId: "+", count: count);

            return entries.Select(entry => new Move
            {
                StartCellId = entry["start"],
                EndCellId = entry["end"]
            });
        }
    }
}
