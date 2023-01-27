using System.Net.Mime;
using System.Reflection;
using System.Text.Json;
using Scoreboard.Cards.Algorithms;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Scoreboard.Cards;

public class DominionSetManager : IDominionSetManager
{
    public IEnumerable<DominionSet> GetAllSets()
    {
        // get all json files in Sets
        var assembly = Assembly.GetExecutingAssembly();
        var setPaths = assembly.GetManifestResourceNames();

        // deserialize each set file and return as a DominionSet
        foreach (var path in setPaths)
        {
            using var stream = assembly.GetManifestResourceStream(path);
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            var setFile = JsonSerializer.Deserialize<DominionSetFile>(json);
            
            yield return new DominionSet
            {
                Name = setFile.Name,
                Cards = setFile.Cards?.Select(representation => new Card
                {
                    Name = representation.Name,
                    Expansion = setFile.Name,
                    Types = representation.Types,
                    Cost = representation.Cost,
                    Algorithms = GetVpAlgorithms(representation)
                }) ?? Enumerable.Empty<ICard>()
            };
        }
    }

    public DominionSet? GetSet(string name)
    {
        return GetAllSets().FirstOrDefault(set => set.Name == name);
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