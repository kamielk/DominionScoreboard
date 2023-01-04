namespace Scoreboard.Cards.Algorithms;

/// <summary>
/// Gives a raw number of points (can be negative)
/// </summary>
public class RawPointsAlgorithm : IVpAlgorithm
{
    private readonly int _number;

    /// <summary>
    /// The amount of points this gives
    /// </summary>
    /// <param name="number"></param>
    public RawPointsAlgorithm(int number)
    {
        _number = number;
    }
    
    public string Name => VpAlgorithmNames.Raw;
    public int GetVictoryPoints(ICollection<ICard> playerCards)
    {
        return _number;
    }
}