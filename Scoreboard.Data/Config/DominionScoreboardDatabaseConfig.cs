namespace Scoreboard.Data.Config;

public class DominionScoreboardDatabaseConfig
{
    public const string DominionScoreboardDatabase = nameof(DominionScoreboardDatabase);
    
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string GamesCollectionName { get; set; } = null!;
}