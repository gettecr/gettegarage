using Microsoft.AspNetCore.Mvc;
using GetteGarage.Models;
using GetteGarage.Services;

[Route("api/[controller]")]
[ApiController]
public class FishingController : ControllerBase
{
    private readonly FishingLeaderboardService _service;

    public FishingController(FishingLeaderboardService service)
    {
        _service = service;
    }

    [HttpGet("top/{fishName}")]
    public IActionResult GetTopCatches(string fishName)
    {
        return Ok(_service.GetTopCatches(fishName));
    }

    [HttpGet("check-record/{fishName}/{size}")]
    public IActionResult CheckRecord(string fishName, double size)
    {
        return Ok(_service.IsWorldRecord(fishName, size));
    }

    [HttpPost]
    public IActionResult SubmitRecord([FromBody] FishingRecord record)
    {
        _service.AddRecord(record);
        return Ok();
    }
}