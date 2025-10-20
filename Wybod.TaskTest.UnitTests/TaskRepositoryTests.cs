using Moq;
using Wybod.TaskTest.Data;
using Wybod.TaskTest.Data.Models;
using Wybod.TaskTest.Data.Repositories;

namespace Wybod.TaskTest.UnitTests;

public class TaskRepositoryTests
{
    private IDataContext CreateMockDataContext()
    {
        var mockContext = new Mock<IDataContext>();
        mockContext.Setup(c => c.Tasks).Returns(new List<TaskItem>());
        return mockContext.Object;
    }

    [Fact]
    public void GetAll_ReturnsTasksOrderedByCreatedAtDescending()
    {
        var context = CreateMockDataContext();
        var task1 = new TaskItem { Id = Guid.NewGuid(), Title = "Task 1", CreatedAt = DateTime.UtcNow.AddDays(-2) };
        var task2 = new TaskItem { Id = Guid.NewGuid(), Title = "Task 2", CreatedAt = DateTime.UtcNow.AddDays(-1) };
        var task3 = new TaskItem { Id = Guid.NewGuid(), Title = "Task 3", CreatedAt = DateTime.UtcNow };
        context.Tasks.Add(task1);
        context.Tasks.Add(task2);
        context.Tasks.Add(task3);

        var repository = new TaskRepository(context);

        var result = repository.GetAll().ToList();

        Assert.Equal(3, result.Count);
        Assert.Equal(task3.Id, result[0].Id);
        Assert.Equal(task2.Id, result[1].Id);
        Assert.Equal(task1.Id, result[2].Id);
    }

    [Fact]
    public void GetById_ExistingId_ReturnsTask()
    {
        var context = CreateMockDataContext();
        var task = new TaskItem { Id = Guid.NewGuid(), Title = "Test Task" };
        context.Tasks.Add(task);

        var repository = new TaskRepository(context);

        var result = repository.GetById(task.Id);

        Assert.NotNull(result);
        Assert.Equal(task.Id, result.Id);
        Assert.Equal("Test Task", result.Title);
    }

    [Fact]
    public void GetById_NonExistingId_ReturnsNull()
    {
        var context = CreateMockDataContext();
        var repository = new TaskRepository(context);

        var result = repository.GetById(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public void Create_SetsIdAndCreatedAt()
    {
        var context = CreateMockDataContext();
        var repository = new TaskRepository(context);
        var task = new TaskItem { Title = "New Task", Description = "Description" };

        var result = repository.Create(task);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.True(result.CreatedAt <= DateTime.UtcNow);
        Assert.True(result.CreatedAt >= DateTime.UtcNow.AddSeconds(-1));
    }

    [Fact]
    public void Create_CompletedTask_SetsCompletedAt()
    {
        var context = CreateMockDataContext();
        var repository = new TaskRepository(context);
        var task = new TaskItem { Title = "Completed Task", IsCompleted = true };

        var result = repository.Create(task);

        Assert.NotNull(result.CompletedAt);
        Assert.True(result.CompletedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Update_ExistingTask_UpdatesProperties()
    {
        var context = CreateMockDataContext();
        var repository = new TaskRepository(context);
        var task = new TaskItem { Title = "Original", Description = "Original Desc", Priority = TaskPriority.Low };
        task = repository.Create(task);

        var updated = new TaskItem
        {
            Title = "Updated",
            Description = "Updated Desc",
            IsCompleted = false,
            Priority = TaskPriority.High
        };

        var result = repository.Update(task.Id, updated);

        Assert.True(result);
        Assert.Equal("Updated", task.Title);
        Assert.Equal("Updated Desc", task.Description);
        Assert.Equal(TaskPriority.High, task.Priority);
    }

    [Fact]
    public void Update_MarkAsCompleted_SetsCompletedAt()
    {
        var context = CreateMockDataContext();
        var repository = new TaskRepository(context);
        var task = repository.Create(new TaskItem { Title = "Task", IsCompleted = false });

        var updated = new TaskItem { Title = "Task", IsCompleted = true };
        repository.Update(task.Id, updated);

        Assert.True(task.IsCompleted);
        Assert.NotNull(task.CompletedAt);
    }

    [Fact]
    public void Update_MarkAsIncomplete_ClearsCompletedAt()
    {
        var context = CreateMockDataContext();
        var repository = new TaskRepository(context);
        var task = repository.Create(new TaskItem { Title = "Task", IsCompleted = true });

        var updated = new TaskItem { Title = "Task", IsCompleted = false };
        repository.Update(task.Id, updated);

        Assert.False(task.IsCompleted);
        Assert.Null(task.CompletedAt);
    }

    [Fact]
    public void Update_NonExistingTask_ReturnsFalse()
    {
        var context = CreateMockDataContext();
        var repository = new TaskRepository(context);

        var result = repository.Update(Guid.NewGuid(), new TaskItem { Title = "Test" });

        Assert.False(result);
    }

    [Fact]
    public void Delete_ExistingTask_RemovesAndReturnsTrue()
    {
        var context = CreateMockDataContext();
        var repository = new TaskRepository(context);
        var task = repository.Create(new TaskItem { Title = "To Delete" });

        var result = repository.Delete(task.Id);

        Assert.True(result);
        Assert.Empty(context.Tasks);
    }

    [Fact]
    public void Delete_NonExistingTask_ReturnsFalse()
    {
        var context = CreateMockDataContext();
        var repository = new TaskRepository(context);

        var result = repository.Delete(Guid.NewGuid());

        Assert.False(result);
    }
}
