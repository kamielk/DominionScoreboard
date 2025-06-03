using Dominion.Data;
using Dominion.Services;
using DominionScoreboard.Games;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace DominionScoreboard.Controllers;

[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly IGamesService _gamesService;
    private readonly IValidator<CreateGameRequest> _gameValidator;

    public GamesController(IGamesService gamesService, IValidator<CreateGameRequest> createGameRequestValidator)
    {
        _gamesService = gamesService ?? throw new ArgumentNullException(nameof(gamesService));
        _gameValidator = createGameRequestValidator ?? throw new ArgumentNullException(nameof(createGameRequestValidator));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllGames()
    {
        var games = await _gamesService.GetAsync();
        return Ok(games);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGame(string id)
    {
        var game = await _gamesService.GetAsync(id);
        if (game is null) return NotFound();
        return Ok(game);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGame([FromBody] CreateGameRequest createGameRequest)
    {
        await _gameValidator.ValidateAndThrowAsync(createGameRequest);

        var newId = ObjectId.GenerateNewId().ToString();
        var game = new Game
        {
            Id = newId,
            Players = createGameRequest.Players.Select(player => new Player(player.Name, player.Cards)),
            CreatedOn = DateTime.Now,
        };
        
        await _gamesService.CreateAsync(game);

        var createdGame = await _gamesService.GetAsync(newId);

        return CreatedAtAction(nameof(GetGame), new { id = newId }, createdGame);
    }
}