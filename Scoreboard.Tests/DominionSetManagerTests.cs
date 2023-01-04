using Scoreboard.Cards;

namespace Scoreboard.Tests;

public class DominionSetManagerTests
{
    [Theory]
    [InlineData("../../../../Scoreboard.Cards/Expansions/base-cards.yaml")]
    [InlineData("../../../../Scoreboard.Cards/Expansions/base-set-2.yaml")]
    public void CanLoadCardsWithoutException(string cardsPath)
    {
        var expansionManager = new DominionSetsManager();

        var cardsYml = File.ReadAllText(cardsPath);
        var cards = expansionManager.GetExpansion(cardsYml);
        
        Assert.NotEmpty(cards);
    }
}