using Microsoft.AspNetCore.Mvc;
using Scoreboard.Cards;
using Scoreboard.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddLogging();
builder.Services.RegisterDominionExpansionManager();

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy =>
    {
        var corsConfig = builder.Configuration
            .GetSection("Cors")
            .Get<CorsConfig>() ?? throw new InvalidOperationException("Cors section missing from appsettings");
        
        policy.WithOrigins(corsConfig.AllowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseStaticFiles();
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// gets a random kingdom
app.MapGet("/kingdom", (
    IDominionSetManager setsManager,
    ILogger<Program> logger
) =>
{
    var set = setsManager.GetSet(SetNames.BaseSet2nd);
    if (set == null)
    {
        logger.LogWarning("Set not found");
        return Results.NotFound("Set not found");
    }

    return Results.Ok(set.Cards.Shuffle().Take(10));
})
.WithName("GetKingdom");

// calculates victory points for a given deck
app.MapPost("/calculate-vp", (
    IDominionSetManager setsManager,
    ILogger<Program> logger,
    [FromBody] Dictionary<string, int> cardCountByName
) =>
{
    var sets = setsManager.GetAllSets().ToList();
    if (sets.Count == 0)
    {
        logger.LogWarning("No sets found");
        return Results.NotFound("No sets found");
    }

    List<CardAndCount> cardAndCounts = [];
    foreach (var (name, count) in cardCountByName)
    {
        var card = sets.SelectMany(s => s.Cards).FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (card == null)
        {
            logger.LogWarning("Card '{name}' not found in any set", name);
            return Results.NotFound($"Card '{name}' not found in any set");
        }
        cardAndCounts.Add(new(card, count));
    }

    var vp = 0;
    foreach (var (card, count) in cardAndCounts)
    {
        vp += card.GetVictoryPoints(cardAndCounts) * count;
    }

    return Results.Ok(new { VictoryPoints = vp });
})
.WithName("CalculateVictoryPoints");

app.Run();
