using Microsoft.AspNetCore.SignalR;
using ThreeChess.DTOs;
using ThreeChess.Enums;
using ThreeChess.Interfaces;
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
            }
            catch (Exception ex)
            {
                return;
            }


        }
    }
}
