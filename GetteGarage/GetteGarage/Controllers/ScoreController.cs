using Microsoft.AspNetCore.Mvc;
using GetteGarage.Models;
using GetteGarage.Services;

[Route("api/[controller]")]
[ApiController]
public class ScoreController : ControllerBase
{
    private readonly HighScoreService _service;

    public ScoreController(HighScoreService service)
    {
        _service = service;
    }

    [HttpGet("{gameName}")]
    public IActionResult GetScores(string gameName)
    {
        return Ok(_service.GetTopScores(gameName));
    }

    [HttpPost]
    public IActionResult PostScore([FromBody] GameScore score)
    {
        _service.AddScore(score);
        return Ok();
    }
}