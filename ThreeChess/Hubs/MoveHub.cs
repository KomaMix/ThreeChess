using Microsoft.AspNetCore.SignalR;
using ThreeChess.Enums;
using ThreeChess.Interfaces;
using ThreeChess.Services;


namespace ThreeChess.Hubs
{
    public class MoveHub : Hub
    {
        private readonly LobbyManager _lobbyManager;
        private readonly IGameManager _gameManager;

        public MoveHub(LobbyManager lobbyManager, IGameManager gameManager)
        {
            _lobbyManager = lobbyManager;
            _gameManager = gameManager;
        }

        public async Task HandleMove(string startCellId, string endCellId, Guid gameId)
        {
            var playerId = Context.UserIdentifier;

            var result = _gameManager.Move(gameId, startCellId, endCellId);

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
