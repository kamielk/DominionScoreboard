using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Dominion.Data;

/// <summary>
/// A single dominion player
/// </summary>
public class Player
{
    public Player()
    {
    }

    public Player(string name, Dictionary<string, int> cards)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Cards = cards ?? throw new ArgumentNullException(nameof(cards));
    }
    
    [BsonRepresentation(BsonType.String)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// number of cards by name
    /// </summary>
    public Dictionary<string, int> Cards { get; set; } = new Dictionary<string, int>();
}