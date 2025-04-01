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


        public async Task HandleMove(string gameSessionId, string startCellId, string endCellId)
        {
            var playerId = Context.UserIdentifier;
            var success = await _moveHandler.ValidateAndApplyMoveAsync(
                gameSessionId,
                playerId,
                startCellId,
                endCellId);

            if (success)
            {
                await Clients.Group(gameSessionId).SendAsync("BoardStateUpdated", gameSessionId);
            }
            else
            {
                await Clients.Caller.SendAsync("MoveError", "Invalid move");
            }
        }

        public async Task HandlePawnReplacement(
            string gameSessionId,
            string startCellId,
            string endCellId,
            FigureType figureType)
        {
            var playerId = Context.UserIdentifier;
            var success = await _moveHandler.ReplacePawnAsync(
                gameSessionId,
                playerId,
                startCellId,
                endCellId,
                figureType);

            if (success)
            {
                await Clients.Group(gameSessionId).SendAsync("PawnReplaced", endCellId, figureType);
            }
            else
            {
                await Clients.Caller.SendAsync("ReplacementError", "Invalid pawn replacement");
            }
        }

        public async Task Send(string message)
        {
            var hubContext = Context;

            Console.WriteLine(message);

            await Clients.All.SendAsync("Receive", message);

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
