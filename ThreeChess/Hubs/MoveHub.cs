using Microsoft.AspNetCore.SignalR;
using ThreeChess.DTOs;
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

        public async Task HandleMove(MoveRequest moveRequest)
        {
            var playerId = Context.UserIdentifier;

            var result = await _gameManager.Move(moveRequest.GameId, moveRequest.StartCellId, moveRequest.EndCellId);

            if (result)
            {
                await Clients.Others.SendAsync("handleMove", new MoveResponse
                {
                    GameId = moveRequest.GameId,
                    StartCellId = moveRequest.StartCellId,
                    EndCellId = moveRequest.EndCellId
                });
            }


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
