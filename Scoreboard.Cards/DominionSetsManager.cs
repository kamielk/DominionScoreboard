using Scoreboard.Cards.Algorithms;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Scoreboard.Cards;

public class DominionSetsManager : IDominionSetsManager
{
    public IEnumerable<ICard> GetExpansion(string dominionSetYml)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var expansion = deserializer.Deserialize<DominionSetFile>(dominionSetYml);

        if (expansion is null)
        {
            throw new ArgumentException("failed to deserialize given dominion set yml");
        }

        return expansion.Cards.Select(card => ToCard(card, expansion.Name)).ToList();
    }

    /// <summary>
    /// Creates a <see cref="ICard"/> from a <see cref="CardRepresentation"/>
    /// </summary>
    /// <param name="representation"><seealso cref="CardRepresentation"/></param>
    /// <param name="expansionName">name of the expansion</param>
    /// <returns></returns>
    private ICard ToCard(CardRepresentation representation, string expansionName)
    {
        return new Card
        {
            Name = representation.Name,
            Expansion = expansionName,
            Types = representation.Types,
            Cost = representation.Cost,
            Algorithms = GetVpAlgorithms(representation)
        };
    }

    private IEnumerable<IVpAlgorithm> GetVpAlgorithms(CardRepresentation card)
    {
        var algorithms = new List<IVpAlgorithm>();

        if (card.VictoryPoints != 0)
        {
            algorithms.Add(new RawPointsAlgorithm(card.VictoryPoints));
        }

        if (card.VpAlgorithm is null) return algorithms;

        var algorithmName = card.VpAlgorithm.Name;
        switch (algorithmName)
        {
            case VpAlgorithmNames.Raw:
                algorithms.Add(new RawPointsAlgorithm(card.VictoryPoints));
                break;
            case VpAlgorithmNames.Count:
                algorithms.Add(new CountAlgorithm(card.VpAlgorithm.Modifier, _ => true));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(algorithmName),
                    $"unexpected algorithm name '{algorithmName}'");
        }

        return algorithms;
    }
}