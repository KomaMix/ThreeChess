using Microsoft.AspNetCore.SignalR;
using ThreeChess.DTOs;
using ThreeChess.Enums;
using ThreeChess.Interfaces;
using ThreeChess.Models;
using ThreeChess.Services;


namespace ThreeChess.Hubs
{
    public class MoveHub : Hub
    {
        private readonly ILobbyManager _lobbyManager;
        private readonly IGameManager _gameManager;

        public MoveHub(ILobbyManager lobbyManager, IGameManager gameManager)
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

                await Clients.Group(moveResponse.GameId.ToString()).SendAsync("handleMove", moveResponse);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var gameId = httpContext.Request.Query["gameId"];

            if (!string.IsNullOrEmpty(gameId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, gameId);

            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = Context.GetHttpContext();
            var gameId = httpContext.Request.Query["gameId"];

            if (!string.IsNullOrEmpty(gameId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
