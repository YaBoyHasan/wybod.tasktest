using Wybod.TaskTest.Data.Models;
using Wybod.TaskTest.Data.Repositories;

namespace Wybod.TaskTest.Services;

public interface ITaskService
{
    IEnumerable<TaskItem> GetAllTasks(string? status = null, string? searchTerm = null);
    TaskItem? GetTaskById(Guid id);
    TaskItem CreateTask(string title, string? description = null, DateTime? dueDate = null, TaskPriority priority = TaskPriority.Medium, List<string>? tags = null);
    bool UpdateTask(Guid id, TaskItem task);
    bool ToggleTaskCompletion(Guid id);
    bool DeleteTask(Guid id);
    IEnumerable<TaskItem> GetOverdueTasks();
    IEnumerable<TaskItem> SearchTasks(string searchTerm);
    IEnumerable<TaskItem> GetTasksByPriority(TaskPriority priority);
    IEnumerable<TaskItem> GetTasksByTag(string tag);
}

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public IEnumerable<TaskItem> GetAllTasks(string? status = null, string? searchTerm = null)
    {
        var tasks = _taskRepository.GetAll();

        if (!string.IsNullOrWhiteSpace(status))
        {
            tasks = status.ToLower() switch
            {
                "completed" => tasks.Where(t => t.IsCompleted),
                "pending" => tasks.Where(t => !t.IsCompleted),
                "overdue" => tasks.Where(t => t.IsOverdue),
                _ => tasks
            };
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var search = searchTerm.ToLower();
            tasks = tasks.Where(t =>
                t.Title.ToLower().Contains(search) ||
                (t.Description?.ToLower().Contains(search) ?? false) ||
                t.Tags.Any(tag => tag.ToLower().Contains(search))
            );
        }

        return tasks;
    }

    public TaskItem? GetTaskById(Guid id)
    {
        return _taskRepository.GetById(id);
    }

    public TaskItem CreateTask(string title, string? description = null, DateTime? dueDate = null, TaskPriority priority = TaskPriority.Medium, List<string>? tags = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        var task = new TaskItem
        {
            Title = title.Trim(),
            Description = description?.Trim() ?? string.Empty,
            DueDate = dueDate,
            Priority = priority,
            Tags = tags ?? new List<string>(),
            IsCompleted = false
        };

        return _taskRepository.Create(task);
    }

    public bool UpdateTask(Guid id, TaskItem task)
    {
        if (string.IsNullOrWhiteSpace(task.Title))
            throw new ArgumentException("Title cannot be empty", nameof(task.Title));

        return _taskRepository.Update(id, task);
    }

    public bool ToggleTaskCompletion(Guid id)
    {
        var task = _taskRepository.GetById(id);
        if (task == null) return false;

        task.IsCompleted = !task.IsCompleted;
        task.CompletedAt = task.IsCompleted ? DateTime.UtcNow : null;

        return _taskRepository.Update(id, task);
    }

    public bool DeleteTask(Guid id)
    {
        return _taskRepository.Delete(id);
    }

    public IEnumerable<TaskItem> GetOverdueTasks()
    {
        return _taskRepository.GetAll().Where(t => t.IsOverdue);
    }

    public IEnumerable<TaskItem> SearchTasks(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return Enumerable.Empty<TaskItem>();

        var search = searchTerm.ToLower();
        return _taskRepository.GetAll().Where(t =>
            t.Title.ToLower().Contains(search) ||
            (t.Description?.ToLower().Contains(search) ?? false) ||
            t.Tags.Any(tag => tag.ToLower().Contains(search))
        );
    }

    public IEnumerable<TaskItem> GetTasksByPriority(TaskPriority priority)
    {
        return _taskRepository.GetAll().Where(t => t.Priority == priority);
    }

    public IEnumerable<TaskItem> GetTasksByTag(string tag)
    {
        if (string.IsNullOrWhiteSpace(tag))
            return Enumerable.Empty<TaskItem>();

        return _taskRepository.GetAll().Where(t =>
            t.Tags.Any(t => t.Equals(tag, StringComparison.OrdinalIgnoreCase))
        );
    }
}