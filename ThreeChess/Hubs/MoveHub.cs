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

            MoveResponse moveResponse;

            try
            {
                moveResponse = await _gameManager.MoveHandle(moveRequest);
            }
            catch (Exception ex)
            {
                return;
            }

            await Clients.Others.SendAsync("handleMove", moveResponse);



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
