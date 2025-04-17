using System.Collections.Concurrent;
using ThreeChess.Models;

namespace ThreeChess.Services
{
    public class GameManager
    {
        private readonly ConcurrentDictionary<int, Lobby> _lobbies = new();

        public GameManager()
        {
            _lobbies[1] = new Lobby();
            _lobbies[2] = new Lobby();
            _lobbies[3] = new Lobby();
        }

        
        public List<Lobby> GetAllLobbies()
        {
            return _lobbies.Values.ToList();
        }
    }



}
