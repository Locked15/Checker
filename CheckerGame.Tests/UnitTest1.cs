using System;
using System.Windows;
using Xunit;
using CheckerGame;

namespace CheckerGame.Tests
{
    public class CheckerGameTests
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(MainWindow.DirectionDefinition(new Position(3, 3), new Position(4, 4)).ToString(), Direction.RightUp.ToString());
        }
    }
}
