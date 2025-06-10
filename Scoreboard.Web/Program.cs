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

app.MapGet("/base-cards",(
    IDominionSetManager setsManager,
    ILogger<Program> logger
) =>
{
    return Results.Ok(setsManager.GetBaseCards().ToList());
})
.WithName("GetBaseCards");

// gets a random kingdom
app.MapGet("/kingdom", (
    IDominionSetManager setsManager,
    ILogger<Program> logger
) =>
{
    var sets = setsManager.GetAllSets();
    if (sets?.Any() != true)
    {
        logger.LogWarning("No sets found");
        return Results.NotFound("No sets found ");
    }

    return Results.Ok(sets.SelectMany(x => x.Cards).Shuffle().Take(10));
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
            logger.LogWarning("Card '{name}' not found in any sets", name);
            return Results.NotFound($"Card '{name}' not found in any sets");
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
