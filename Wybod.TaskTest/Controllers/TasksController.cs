using Microsoft.AspNetCore.Mvc;
using Wybod.TaskTest.Data.Models;
using Wybod.TaskTest.Data.Repositories;

namespace Wybod.TaskTest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _repository;
    public TasksController(ITaskRepository repository) => _repository = repository;

    // GET /api/tasks?status=completed|pending
    [HttpGet]
    public IActionResult GetTasks([FromQuery] string? status)
    {
        // Filter tasks based on status query parameter
        var items = _repository.GetAll();
        
        // Apply filtering
        items = status?.ToLower() switch
        {
            "completed" => items.Where(t => t.IsCompleted),
            "pending" => items.Where(t => !t.IsCompleted),
            _ => items
        };

        // Return the filtered list
        return Ok(items);
    }

    // GET /api/tasks/{id}
    [HttpGet("{id:guid}")]
    public IActionResult GetTask(Guid id)
        => _repository.GetById(id) is { } t ? Ok(t) : NotFound();

    // POST /api/tasks
    [HttpPost]
    public IActionResult CreateTask([FromBody] TaskItem dto)
    {
        // Basic validation
        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest("Title is required");

        // Create new task
        var created = _repository.Create(new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            IsCompleted = dto.IsCompleted
        });

        // Return 201 Created with location header (successful creation)
        return CreatedAtAction(nameof(GetTask), new { id = created.Id }, created);
    }

    // PUT /api/tasks/{id}
    [HttpPut("{id:guid}")]
    public IActionResult UpdateTask(Guid id, [FromBody] TaskItem dto)
    {
        // Basic validation
        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest("Title is required");

        // Update existing task
        var ok = _repository.Update(id, dto);

        // Return 204 No Content if successful, 404 Not Found if task doesn't exist
        return ok ? NoContent() : NotFound();
    }

    // DELETE /api/tasks/{id}
    [HttpDelete("{id:guid}")]
    public IActionResult DeleteTask(Guid id)
        => _repository.Delete(id) ? NoContent() : NotFound();
}
