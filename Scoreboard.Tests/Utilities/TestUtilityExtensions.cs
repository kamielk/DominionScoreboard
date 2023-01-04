using System.Security.Cryptography;
using Scoreboard.Cards;

namespace Scoreboard.Tests.Utilities;

/// <summary>
/// Contains extension methods that are neat for testing
/// </summary>
internal static class TestUtilityExtensions
{
    /// <summary>
    /// Shuffles a list using a Fisher-Yates shuffle  
    /// inspiration: https://stackoverflow.com/questions/273313/randomize-a-listt
    /// </summary>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    public static void Shuffle<T>(this IList<T> list)
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        var n = list.Count;
        while (n > 1)
        {
            var box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (byte.MaxValue / n)));
            var k = (box[0] % n);
            n--;
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
    
    /// <summary>
    /// Repeats a <paramref name="card"/> a number of <paramref name="times"/>
    /// </summary>
    /// <param name="card"></param>
    public static IEnumerable<ICard> Repeat(this ICard card, int times)
    {
        for (var i = 0; i < times; i++)
        {
            yield return card;
        }
    }
}