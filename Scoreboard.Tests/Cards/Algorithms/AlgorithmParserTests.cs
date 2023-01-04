using Moq;
using Scoreboard.Cards;
using Scoreboard.Cards.Algorithms;

namespace Scoreboard.Tests.Cards.Algorithms;

// victoryPointsAlg:
//     name: count
//     modifier: 0.1

public class AlgorithmParserTests
{
    /// <summary>
    /// This loads the algorithm from an example card string
    /// </summary>
    [Fact]
    public void Foo()
    {
        var a = @"victoryPointsAlg:
        name: count
        modifier: 0.1";
        var card = new Mock<ICard>();
    }
}

