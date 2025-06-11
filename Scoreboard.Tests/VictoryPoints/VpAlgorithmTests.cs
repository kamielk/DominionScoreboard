using AutoFixture;
using AutoFixture.AutoNSubstitute;
using NSubstitute;
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
        ICollection<CardAndCount> cardAndCounts = [.. fixture.CreateMany<Card>(5).Select(c => new CardAndCount(c, 2))];
        var currentCard = cardAndCounts.First().Card;
        var result = VpAlgorithmParser.Evaluate("count(*)", cardAndCounts, currentCard);
        result.ShouldBe(10);
    }

    [Fact]
    public void CountByType_ShouldReturn_OnlyMatchingTypes()
    {
        var fixture = CreateFixture();
        var actionCard = fixture.Build<Card>()
            .With(c => c.Types, ["Action"])
            .Create();

        var actionCardAndCount = new CardAndCount(actionCard, 3);

        var otherCard = fixture.Build<Card>()
            .With(c => c.Types, ["Treasure"])
            .Create();
        var otherCardAndCount = new CardAndCount(otherCard, 2);

        var result = VpAlgorithmParser.Evaluate("count(type:Action)", [actionCardAndCount, otherCardAndCount], Arg.Any<ICard>());
        result.ShouldBe(3);
    }

    [Fact]
    public void CountByName_ShouldReturn_MatchingCardNames()
    {
        var fixture = CreateFixture();
        var duchy = fixture.Build<Card>()
            .With(c => c.Name, "Duchy")
            .With(c => c.Types, ["Distant Lands"])
            .Create();

        var duchyCardAndCount = new CardAndCount(duchy, 2);

        var province = fixture.Build<Card>()
            .With(c => c.Name, "Province")
            .With(c => c.Types, ["Victory"])
            .Create();

        var provincesCardAndCount = new CardAndCount(province, 3);

        var result = VpAlgorithmParser.Evaluate("count(name:Duchy)", [duchyCardAndCount, provincesCardAndCount], Arg.Any<ICard>());
        result.ShouldBe(2);
    }

    [Fact]
    public void Evaluate_ShouldRoundDown()
    {
        var fixture = CreateFixture();
        var actionCard = fixture.Build<Card>()
            .With(c => c.Types, ["Action"])
            .Create();
         CardAndCount cardAndCount = new(actionCard, 4); // count / 3 = 1.33 => floor = 1

        //count(type: Action)
        var result = VpAlgorithmParser.Evaluate("count(type:Action) / 3", [cardAndCount], Arg.Any<ICard>());
        result.ShouldBe(1);
    }

    [Fact]
    public void CountDistinctNames_ShouldReturnUniqueCardNames()
    {
        ICollection<ICard> cards =
        [
            new Card("Village", "", new Dictionary<string, int>(), ["Action"]),
            new Card("Village", "", new Dictionary<string, int>(), ["Action"]),
            new Card("Smithy", "", new Dictionary<string, int>(), ["Action"]),
            new Card("Gold", "", new Dictionary<string, int>(), ["Treasure"])
        ];

        var cardAndCounts = cards.Select(c => new CardAndCount(c, 1));

        var result = VpAlgorithmParser.Evaluate("count(distinct:name)", cardAndCounts, Arg.Any<ICard>());
        result.ShouldBe(3);
    }

    [Fact]
    public void DeckWithNoCards_CountShouldBeZero()
    {
        var result = VpAlgorithmParser.Evaluate("count(*)", [], Arg.Any<ICard>());
        result.ShouldBe(0);
    }


    [Fact]
    public void NegativeInteger_ShouldWork()
    {
        var result = VpAlgorithmParser.Evaluate("-1", [], Arg.Any<ICard>());
        result.ShouldBe(-1);
    }

    [Fact]
    public void ConditionalExpression_ShouldEvaluateCorrectly()
    {
        var fixture = CreateFixture();
        var actionCard = fixture.Build<Card>()
            .With(c => c.Types, ["Action"])
            .Create();
        var actionCardAndCount = new CardAndCount(actionCard, 3);
        var otherCardAndCount = new CardAndCount(fixture.Create<Card>(), 2);
        // e.g., "count(type:Action) > 2 ? 5 : 0"
        var result = VpAlgorithmParser.Evaluate("count(type:Action) > 2 ? 5 : 0", [actionCardAndCount, otherCardAndCount], Arg.Any<ICard>());
        result.ShouldBe(5);
    }
}