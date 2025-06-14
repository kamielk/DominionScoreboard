using Scoreboard.Cards;

namespace Scoreboard.Cards;

/// <summary>
/// A single dominion card
/// </summary>
public interface ICard
{
    /// <summary>
    /// A card's name
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// In which expansion this card is included
    /// </summary>
    string Set { get; }
    
    /// <summary>
    /// A card's cost
    /// </summary>
    /// <example>
    /// { {"coin", 1}, {"potion", 1} }
    /// </example>
    IDictionary<string, int> Cost { get; }

    /// <summary>
    /// A card's types
    /// </summary>
    IEnumerable<string> Types { get; }

    /// <summary>
    /// Url where the image for this card is served
    /// </summary>
    string ImageUrl { get; }

    /// <summary>
    /// Gets the amount of victory points this card gives at the end of the game
    /// </summary>
    /// <param name="playerCards">the player's cards</param>
    int GetVictoryPoints(IEnumerable<CardAndCount> cardAndCounts);
}