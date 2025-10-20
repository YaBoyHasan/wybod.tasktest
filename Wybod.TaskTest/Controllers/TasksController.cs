using Microsoft.AspNetCore.Mvc;
using Wybod.TaskTest.Data.Models;
using Wybod.TaskTest.Services;

namespace Wybod.TaskTest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    public TasksController(ITaskService taskService) => _taskService = taskService;

    [HttpGet]
    public IActionResult GetTasks([FromQuery] string? status, [FromQuery] string? search, [FromQuery] string? priority, [FromQuery] string? sortBy)
    {
        var tasks = _taskService.GetAllTasks(status, search);

        if (!string.IsNullOrWhiteSpace(priority) && Enum.TryParse<TaskPriority>(priority, true, out var parsedPriority))
        {
            tasks = tasks.Where(t => t.Priority == parsedPriority);
        }

        tasks = sortBy?.ToLower() switch
        {
            "title" => tasks.OrderBy(t => t.Title),
            "priority" => tasks.OrderByDescending(t => t.Priority).ThenBy(t => t.Title),
            "duedate" => tasks.OrderBy(t => t.DueDate ?? DateTime.MaxValue),
            _ => tasks.OrderByDescending(t => t.CreatedAt)
        };

        return Ok(tasks);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetTask(Guid id)
        => _taskService.GetTaskById(id) is { } t ? Ok(t) : NotFound();

    [HttpPost]
    public IActionResult CreateTask([FromBody] TaskItem dto)
    {
        try
        {
            var created = _taskService.CreateTask(
                dto.Title,
                dto.Description,
                dto.DueDate,
                dto.Priority,
                dto.Tags
            );
            return CreatedAtAction(nameof(GetTask), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateTask(Guid id, [FromBody] TaskItem dto)
    {
        try
        {
            var success = _taskService.UpdateTask(id, dto);
            return success ? NoContent() : NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id:guid}/toggle")]
    public IActionResult ToggleTask(Guid id)
    {
        var success = _taskService.ToggleTaskCompletion(id);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteTask(Guid id)
        => _taskService.DeleteTask(id) ? NoContent() : NotFound();
}
