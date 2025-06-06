using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Moq;
using ThreeChess.Enums;
using ThreeChess.Hubs;
using ThreeChess.Interfaces;
using ThreeChess.Models;
using ThreeChess.Services;

namespace ThreeChess.Tests
{
    public class LobbyWaitingServiceTests
    {
        private readonly Mock<IHubContext<LobbyHub>> _hubContextMock = new();
        private readonly Mock<ILobbyManager> _lobbyManagerMock = new();
        private readonly Mock<IGameRepository> _gameRepositoryMock = new();
        private readonly Mock<IBoardElementsService> _boardElementsServiceMock = new();
        private readonly LobbyWaitingService _service;

        public LobbyWaitingServiceTests()
        {
            _service = new LobbyWaitingService(
                _hubContextMock.Object,
                _lobbyManagerMock.Object,
                _gameRepositoryMock.Object,
                _boardElementsServiceMock.Object
            );
        }

        [Fact]
        public void StartCountdown_AddsTimerToDictionary()
        {
            // Arrange
            int lobbyId = 1;

            // Act
            _service.StartCountdown(lobbyId);

            // Assert
            Assert.Single(_service._countdownTimers);
            Assert.Contains(lobbyId, _service._countdownTimers.Keys);
        }

        [Fact]
        public async Task FinishCountdown_With2Players_DoesNotCreateGame()
        {
            // Arrange
            int lobbyId = 1;
            var lobby = new Lobby { PlayerIds = new List<string> { "p1", "p2" } };
            _lobbyManagerMock.Setup(x => x.GetLobby(lobbyId)).Returns(lobby);

            // Act
            await _service.FinishCountdown(lobbyId);

            // Assert
            _gameRepositoryMock.Verify(x => x.CreateGame(It.IsAny<GameState>()), Times.Never);
            _lobbyManagerMock.Verify(x => x.RemoveLobby(lobbyId), Times.Never);
        }
    }
}
