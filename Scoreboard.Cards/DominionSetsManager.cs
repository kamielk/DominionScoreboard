using System.Reflection;
using System.Text.Json;

namespace Scoreboard.Cards;

public class DominionSetManager : IDominionSetManager
{
    private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
    };

    private DominionSet[]? Sets = null;

    public IEnumerable<DominionSet> GetAllSets()
    {
        return Sets ??= [.. LoadAllSets()];
    }

    public DominionSet? GetSet(string name)
    {
        return GetAllSets().FirstOrDefault(set => set.Name == name);
    }

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
                    .Select(representation => new Card(representation.Name, setFile.Name, representation.Cost, representation.Types)) ?? Enumerable.Empty<ICard>()
            };
        }
    }
}