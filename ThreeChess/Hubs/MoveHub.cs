using Microsoft.AspNetCore.SignalR;
using ThreeChess.Enums;


namespace ThreeChess.Hubs
{
    public class MoveHub : Hub
    {


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
