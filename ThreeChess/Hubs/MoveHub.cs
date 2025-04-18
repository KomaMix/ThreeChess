using Microsoft.AspNetCore.SignalR;
using ThreeChess.Enums;
using ThreeChess.Services;


namespace ThreeChess.Hubs
{
    public class MoveHub : Hub
    {
        private readonly LobbyManager _gameManager;

        public MoveHub(LobbyManager gameManager)
        {
            _gameManager = gameManager;
        }

        public async Task HandleMove(string startCellId, string endCellId)
        {
            var playerId = Context.UserIdentifier;


            await Clients.Others.SendAsync("handleMove", startCellId, endCellId);


        }
    }
}
