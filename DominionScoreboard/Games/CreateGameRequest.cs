using Newtonsoft.Json;

namespace DominionScoreboard.Games;

public class CreateGameRequest
{
    [JsonProperty("players")]
    public IEnumerable<Player> Players { get; set; } = null!;

    public class Player
    {
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// number of cards by name
        /// </summary>
        /// <example>
        /// { "estate", 1 }
        /// </example>
        [JsonProperty("cards")]
        public Dictionary<string,int> Cards { get; set; } = null!;
    }
}