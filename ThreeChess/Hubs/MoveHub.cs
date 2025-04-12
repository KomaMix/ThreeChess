using Microsoft.AspNetCore.SignalR;
using ThreeChess.Enums;
using ThreeChess.Interfaces;

namespace ThreeChess.Hubs
{
    public class MoveHub : Hub
    {
        private readonly IMoveHandlerService _moveHandler;
        public MoveHub(IMoveHandlerService moveHandler)
        {
            _moveHandler = moveHandler;
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
