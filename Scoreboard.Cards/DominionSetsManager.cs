using System.Reflection;
using System.Text.Json;
using Scoreboard.Cards.VictoryPoints;

namespace Scoreboard.Cards;

public class DominionSetManager : IDominionSetManager
{
    private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
    };

    private readonly IEnumerable<ICard> _baseCards;
    private readonly DominionSet[] _sets;

    public DominionSetManager()
    {
        ICollection<DominionSet> sets = [.. LoadAllSets()];
        _baseCards = sets.FirstOrDefault(set => set.Name == SetNames.Base)?.Cards ?? [];
        _sets = [.. sets.Where(set => set.Name != SetNames.Base)];
    }

    public IEnumerable<DominionSet> GetAllSets() => _sets;

    public IEnumerable<ICard> GetBaseCards() => _baseCards;

    public DominionSet? GetSet(string name) => _sets.FirstOrDefault(set => set.Name == name);

    private IEnumerable<DominionSet> LoadAllSets()
    {
        // get all json files in Sets
        var assembly = Assembly.GetExecutingAssembly();
        var setPaths = assembly.GetManifestResourceNames();

        // deserialize each set file and return as a DominionSet
        foreach (var path in setPaths)
        {
            using var stream = assembly.GetManifestResourceStream(path);
            using var reader = new StreamReader(stream!);
            var json = reader.ReadToEnd();

            var setFile = JsonSerializer.Deserialize<DominionSetFile>(json, _serializerOptions);
            if (setFile == null)
            {
                continue; // skip if deserialization failed
            }

            yield return new DominionSet
            {
                Name = setFile.Name,
                Cards = setFile.Cards?
                    .Select(representation => new Card(representation.Name,
                        setFile.Name,
                        representation.Cost,
                        representation.Types,
                        representation.VpAlg is null
                            ? null 
                            : (IEnumerable<CardAndCount> deck) => VpAlgorithmParser.Evaluate(representation.VpAlg, deck)))
                        ?? Enumerable.Empty<ICard>()
            };
        }
    }
}