using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Scoreboard.Cards;
using Scoreboard.Cards.VictoryPoints;
using Shouldly;
using Sprache;

namespace Scoreboard.Tests.VictoryPoints;

public class VpAlgorithmTests
{
    private static IFixture CreateFixture()
    {
        var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        // Ensure cards always have non-null names and types
        fixture.Customize<Card>(c => c
            .With(x => x.Name, () => fixture.Create<string>())
            .With(x => x.Types, () => ["Treasure"]));

        return fixture;
    }

    [Fact]
    public void CountAll_ShouldReturn_TotalCards()
    {
        var fixture = CreateFixture();
        var deck = new PlayerDeck
        {
            Cards = [.. fixture.CreateMany<Card>(5)]
        };

        var result = VpAlgorithmParser.Evaluate("count(*)", deck);
        result.ShouldBe(5);
    }

    [Fact]
    public void CountByType_ShouldReturn_OnlyMatchingTypes()
    {
        var fixture = CreateFixture();
        var actionCards = fixture.Build<Card>()
            .With(c => c.Types, ["Action"])
            .CreateMany(3);

        var otherCards = fixture.Build<Card>()
            .With(c => c.Types, ["Treasure"])
            .CreateMany(2);

        var deck = new PlayerDeck
        {
            Cards = [.. actionCards, .. otherCards]
        };

        var result = VpAlgorithmParser.Evaluate("count(type:Action)", deck);
        result.ShouldBe(3);
    }

    [Fact]
    public void CountByName_ShouldReturn_MatchingCardNames()
    {
        var fixture = CreateFixture();
        var duchies = fixture.Build<Card>()
            .With(c => c.Name, "Duchy")
            .With(c => c.Types, ["Victory"])
            .CreateMany(2);

        var provinces = fixture.Build<Card>()
            .With(c => c.Name, "Province")
            .With(c => c.Types, ["Victory"])
            .CreateMany(1);

        var deck = new PlayerDeck
        {
            Cards = [.. duchies, .. provinces]
        };

        var result = VpAlgorithmParser.Evaluate("count(name:Duchy)", deck);
        result.ShouldBe(2);
    }

    [Fact]
    public void Evaluate_ShouldRoundDown()
    {
        var fixture = CreateFixture();
        var actions = fixture.Build<Card>()
            .With(c => c.Types, ["Action"])
            .CreateMany(4); // count / 3 = 1.33 => floor = 1

        var deck = new PlayerDeck
        {
            Cards = [.. actions]
        };

        //count(type: Action)
        var result = VpAlgorithmParser.Evaluate("count(type:Action) / 3", deck);
        result.ShouldBe(1);
    }

    [Fact]
    public void CountDistinctNames_ShouldReturnUniqueCardNames()
    {
        var deck = new PlayerDeck
        {
            Cards =
            [
                new("Village", "", new Dictionary<string, int>(), ["Action"]),
                new("Village", "", new Dictionary<string, int>(), ["Action"]),
                new("Smithy", "", new Dictionary<string, int>(), ["Action"]),
                new("Gold", "", new Dictionary<string, int>(), ["Treasure"])
            ]
        };

        var result = VpAlgorithmParser.Evaluate("count(distinct:name)", deck);
        result.ShouldBe(3);
    }

    [Fact]
    public void DeckWithNoCards_CountShouldBeZero()
    {
        var deck = new PlayerDeck { Cards = [] };
        var result = VpAlgorithmParser.Evaluate("count(*)", deck);
        result.ShouldBe(0);
    }


    [Fact]
    public void NegativeInteger_ShouldWork()
    {
        var result = VpAlgorithmParser.Evaluate("-1", new());
        result.ShouldBe(-1);
    }
}