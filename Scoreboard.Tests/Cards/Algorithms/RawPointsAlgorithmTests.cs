using Moq;
using Scoreboard.Cards;
using Scoreboard.Cards.Algorithms;

namespace Scoreboard.Tests.Cards.Algorithms;

public class RawPointsAlgorithmTests
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(3, 3)]
    [InlineData(6, 6)]
    [InlineData(-1, -1)]
    public void Test(int numberOfPoints, int expected)
    {
        // arrange
        var playerCards = new Mock<ICollection<ICard>>().Object;
        var rawPointsAlgorithm = new RawPointsAlgorithm(numberOfPoints);

        // act
        var points = rawPointsAlgorithm.GetVictoryPoints(playerCards);
        
        // assert
        Assert.Equal(expected, points);
    }
}