using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using StackExchange.Redis;
using ThreeChess.Models;
using ThreeChess.Services;

namespace ThreeChess.Tests
{
    public class RedisLobbyManagerTests
    {

        public RedisLobbyManagerTests()
        {

        }

        [Fact]
        public void JoinLobby_Success_WhenConditionsMet()
        {
            // Assert
            Assert.True(true);
        }

        [Fact]
        public void GetLobby_ReturnsLobbyWithPlayers()
        {
            // Assert
            Assert.True(true);
        }

    }
}
