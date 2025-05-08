using Microsoft.AspNetCore.SignalR;
using ThreeChess.Enums;
using ThreeChess.Services;


namespace ThreeChess.Hubs
{
    public class MoveHub : Hub
    {
        private readonly LobbyManager _gameManager;
        private readonly GameRepository _gameRepository;

        public MoveHub(LobbyManager gameManager, GameRepository gameRepository)
        {
            _gameManager = gameManager;
            _gameRepository = gameRepository;
        }

        public async Task HandleMove(string startCellId, string endCellId)
        {
            var playerId = Context.UserIdentifier;


            await Clients.Others.SendAsync("handleMove", startCellId, endCellId);


        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
