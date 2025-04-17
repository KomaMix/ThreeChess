using Microsoft.AspNetCore.SignalR;
using ThreeChess.Enums;
using ThreeChess.Services;


namespace ThreeChess.Hubs
{
    public class MoveHub : Hub
    {
        private readonly GameManager _gameManager;

        public MoveHub(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public async Task HandleMove(string startCellId, string endCellId)
        {
            var playerId = Context.UserIdentifier;


            await Clients.Others.SendAsync("handleMove", startCellId, endCellId);


        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
