using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Dominion.Data;

/// <summary>
/// A single Dominion game
/// </summary>
public class Game
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; init; }

    /// <summary>
    /// The time the game was created
    /// </summary>
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime CreatedOn { get; set; }
    
    public IEnumerable<Player> Players { get; set; }
}