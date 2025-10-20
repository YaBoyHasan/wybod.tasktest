using Wybod.TaskTest.Data.Models;

namespace Wybod.TaskTest.Data.Repositories;

public interface ITaskRepository
{
    IEnumerable<TaskItem> GetAll();
    TaskItem? GetById(Guid id);
    TaskItem Create(TaskItem task);
    bool Update(Guid id, TaskItem task);
    bool Delete(Guid id);
}

public class TaskRepository : ITaskRepository
{
    private readonly IDataContext _dataContext;

    // Read data from IDataContext via Dependency Injection
    public TaskRepository(IDataContext dataContext) => _dataContext = dataContext;

    // Get All Tasks ordered by CreatedAt descending
    public IEnumerable<TaskItem> GetAll()
        => _dataContext.Tasks.OrderByDescending(t => t.CreatedAt);

    // Get Task by Id
    public TaskItem? GetById(Guid id)
        => _dataContext.Tasks.FirstOrDefault(t => t.Id == id);

    // Create a new Task
    public TaskItem Create(TaskItem task)
    {
        // Set Id and CreatedAt
        if (task.Id == Guid.Empty) task.Id = Guid.NewGuid();
        task.CreatedAt = DateTime.UtcNow;
        // Set CompletedAt if IsCompleted is true
        if (task.IsCompleted && task.CompletedAt is null)
            task.CompletedAt = DateTime.UtcNow;

        // Add to DataContext
        _dataContext.Tasks.Add(task);

        // Return the created task
        return task;
    }

    // Update an existing Task
    public bool Update(Guid id, TaskItem task)
    {
        // Find existing task
        var existing = GetById(id);

        // If Id not found, return false
        if (existing is null) return false;

        // Update properties
        var wasCompleted = existing.IsCompleted;
        existing.Title = task.Title;
        existing.Description = task.Description;
        existing.IsCompleted = task.IsCompleted;

        // Update CompletedAt based on IsCompleted change
        if (!wasCompleted && existing.IsCompleted)
            existing.CompletedAt = DateTime.UtcNow;

        // Clear CompletedAt if marked as not completed
        if (wasCompleted && !existing.IsCompleted)
            existing.CompletedAt = null;

        // Return success
        return true;
    }

    // Delete a Task by Id
    public bool Delete(Guid id)
    {
        // Find existing task by Id
        var existing = GetById(id);

        // If Id not found, return false
        if (existing is null) return false;

        // Remove from DataContext
        _dataContext.Tasks.Remove(existing);

        // Return success
        return true;
    }
}
