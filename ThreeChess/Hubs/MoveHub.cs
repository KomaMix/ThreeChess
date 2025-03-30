using Microsoft.AspNetCore.SignalR;

namespace ThreeChess.Hubs
{
    public class MoveHub : Hub
    {
        public async Task Send(string message)
        {
            var hubContext = Context;

            Console.WriteLine(message);

            await Task.CompletedTask;
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
