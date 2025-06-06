using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeChess.Models;
using ThreeChess.Services;

namespace ThreeChess.Tests
{
    public class GameRepositoryTests
    {
        private readonly GameRepository _repository = new GameRepository();

        [Fact]
        public void CreateGame_AddsGameToCollection()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var gameState = new GameState { Id = gameId };

            // Act
            _repository.CreateGame(gameState);

            // Assert
            var result = _repository.GetGame(gameId);
            Assert.NotNull(result);
            Assert.Equal(gameId, result.Id);
        }

        [Fact]
        public void GetGame_ReturnsNull_WhenGameNotExists()
        {
            // Act
            var result = _repository.GetGame(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAllGames_ReturnsAllCreatedGames()
        {
            // Arrange
            var game1 = new GameState { Id = Guid.NewGuid() };
            var game2 = new GameState { Id = Guid.NewGuid() };

            _repository.CreateGame(game1);
            _repository.CreateGame(game2);

            // Act
            var result = _repository.GetAllGames().ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(game1, result);
            Assert.Contains(game2, result);
        }
    }
}
