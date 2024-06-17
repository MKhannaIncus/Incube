using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

[ApiController]
[Route("api/[controller]")]
public class TaskStatusController : ControllerBase
{
    private readonly BackgroundTaskService _backgroundTaskService;
    private readonly ILogger<TaskStatusController> _logger;

    public TaskStatusController(BackgroundTaskService backgroundTaskService, ILogger<TaskStatusController> logger)
    {
        _backgroundTaskService = backgroundTaskService;
        _logger = logger;
    }

    [HttpPost("start")]
    public IActionResult Start()
    {
        _logger.LogInformation("Starting background task service.");
        // In reality, you might need a way to restart the service
        return Ok("Background task service is controlled by the application lifecycle.");
    }

    [HttpPost("stop")]
    public IActionResult Stop()
    {
        _logger.LogInformation("Stopping background task service.");
        // There's no straightforward way to stop a BackgroundService without stopping the application
        return Ok("Background task service is controlled by the application lifecycle.");
    }

    [HttpGet("status")]
    public DateTimeOffset Status()
    {
        DateTimeOffset LastRun = _backgroundTaskService.GetLastRun();
            //Status = _backgroundTaskService.GetStatus()

        return LastRun;
    }

    //[HttpGet]
    //public IActionResult GetStatus()
    //{
    //    return Ok(new
    //    {
    //        status = _backgroundTaskService.GetStatus(),
    //        lastRun = _backgroundTaskService.GetLastRun()
    //    });
    //}
}
