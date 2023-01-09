using Moq;
using Scoreboard.Cards;
using Scoreboard.Cards.Algorithms;
using Scoreboard.Tests.Utilities;

namespace Scoreboard.Tests.Cards.Algorithms;

public class CountAlgorithmTests
{
    [Theory]
    [InlineData(40, 0.1, 4)] // this is the same as having 1 Gardens
    public void WhenNoFilterIsPassedCountAlgorithmCountsCorrectly(int numberOfCards, double modifier, int expectedVictoryPoints)
    {
        // arrange
        var alg = new CountAlgorithm(modifier, card => true);
        var mockCard = new Mock<ICard>().Object;
        var deck = mockCard.Repeat(numberOfCards).ToList();

        // act
        var victoryPoints = alg.GetVictoryPoints(deck);

        // assert
        Assert.Equal(expectedVictoryPoints, victoryPoints);
    }
    
    /// <summary>
    /// This takes the King's Castle (worth 2vp per castle) as an example
    /// </summary>
    [Theory]
    [InlineData(1, 2)]
    [InlineData(5, 10)]
    public void WhenFilterIsPassedCountAlgorithmCountsCorrectly(int numberOfCastles, int expectedVictoryPoints)
    {
        // arrange
        var castleCountAlgorithm = new CountAlgorithm(2, card => card.Types.Contains("castle"));
        
        var castleMock = new Mock<ICard>();
        castleMock.Setup(card => card.Types).Returns(new[] { "castle" });
        
        var meaninglessCard = new Mock<ICard>().Object;
        var castle = castleMock.Object;

        
        var deck = meaninglessCard.Repeat(30).Concat(castle.Repeat(numberOfCastles)).ToList();
        deck.Shuffle();

        // act
        var victoryPoints = castleCountAlgorithm.GetVictoryPoints(deck.ToList());

        // assert
        Assert.Equal(expectedVictoryPoints, victoryPoints);
    }
}