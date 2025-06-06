using Scoreboard.Cards;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddLogging();
builder.Services.RegisterDominionExpansionManager();

var app = builder.Build();

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

app.Run();
