using Scoreboard.Cards;

namespace Scoreboard.Tests;

public class DominionSetManagerTests
{
    [Fact]
    public void CanLoadCardsWithoutException()
    {
        // arrange
        var setManager = new DominionSetManager();

        // act
        var allSets = setManager.GetAllSets();
        
        // assert
        Assert.NotEmpty(allSets);
    }
}