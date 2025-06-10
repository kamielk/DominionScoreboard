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
        ICollection<CardAndCount> cardAndCounts = [.. fixture.CreateMany<Card>(5).Select(c => new CardAndCount(c, 2))];
        var result = VpAlgorithmParser.Evaluate("count(*)", cardAndCounts);
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

        var result = VpAlgorithmParser.Evaluate("count(type:Action)", [actionCardAndCount, otherCardAndCount]);
        result.ShouldBe(3);
    }

    [Fact]
    public void CountByName_ShouldReturn_MatchingCardNames()
    {
        var fixture = CreateFixture();
        var duchy = fixture.Build<Card>()
            .With(c => c.Name, "Duchy")
            .With(c => c.Types, ["Victory"])
            .Create();

        var duchyCardAndCount = new CardAndCount(duchy, 2);

        var province = fixture.Build<Card>()
            .With(c => c.Name, "Province")
            .With(c => c.Types, ["Victory"])
            .Create();

        var provincesCardAndCount = new CardAndCount(province, 1);


        var result = VpAlgorithmParser.Evaluate("count(name:Duchy)", [duchyCardAndCount, provincesCardAndCount]);
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
        var result = VpAlgorithmParser.Evaluate("count(type:Action) / 3", [cardAndCount]);
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

        var result = VpAlgorithmParser.Evaluate("count(distinct:name)", cardAndCounts);
        result.ShouldBe(3);
    }

    [Fact]
    public void DeckWithNoCards_CountShouldBeZero()
    {
        var result = VpAlgorithmParser.Evaluate("count(*)", []);
        result.ShouldBe(0);
    }


    [Fact]
    public void NegativeInteger_ShouldWork()
    {
        var result = VpAlgorithmParser.Evaluate("-1", []);
        result.ShouldBe(-1);
    }
}