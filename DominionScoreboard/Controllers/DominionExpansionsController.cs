using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scoreboard.Cards;

namespace DominionScoreboard.Controllers;

[Route("api/expansions")]
public class DominionExpansionsController : ControllerBase
{
    private readonly IDominionSetManager _dominionSetManager;

    public DominionExpansionsController(IDominionSetManager dominionSetManager)
    {
        _dominionSetManager = dominionSetManager;
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult GetAvailableExpansions()
    {
        var allSets = _dominionSetManager.GetAllSets();
        return Ok(allSets);
    }
}