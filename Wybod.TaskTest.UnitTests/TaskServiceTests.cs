using Moq;
using Wybod.TaskTest.Data.Models;
using Wybod.TaskTest.Data.Repositories;
using Wybod.TaskTest.Services;

namespace Wybod.TaskTest.UnitTests;

public class TaskServiceTests
{
    private Mock<ITaskRepository> CreateMockRepository()
    {
        return new Mock<ITaskRepository>();
    }

    [Fact]
    public void CreateTask_ValidInput_CreatesTask()
    {
        var mockRepo = CreateMockRepository();
        var taskService = new TaskService(mockRepo.Object);
        var createdTask = new TaskItem { Id = Guid.NewGuid(), Title = "Test Task" };
        mockRepo.Setup(r => r.Create(It.IsAny<TaskItem>())).Returns(createdTask);

        var result = taskService.CreateTask("Test Task", "Description", null, TaskPriority.Medium);

        Assert.NotNull(result);
        mockRepo.Verify(r => r.Create(It.Is<TaskItem>(t =>
            t.Title == "Test Task" &&
            t.Description == "Description" &&
            t.Priority == TaskPriority.Medium
        )), Times.Once);
    }

    [Fact]
    public void CreateTask_EmptyTitle_ThrowsArgumentException()
    {
        var mockRepo = CreateMockRepository();
        var taskService = new TaskService(mockRepo.Object);

        Assert.Throws<ArgumentException>(() =>
            taskService.CreateTask("", "Description"));
    }

    [Fact]
    public void CreateTask_WithTags_CreateTaskWithTags()
    {
        var mockRepo = CreateMockRepository();
        var taskService = new TaskService(mockRepo.Object);
        var tags = new List<string> { "urgent", "work" };
        mockRepo.Setup(r => r.Create(It.IsAny<TaskItem>())).Returns(new TaskItem());

        taskService.CreateTask("Task", null, null, TaskPriority.High, tags);

        mockRepo.Verify(r => r.Create(It.Is<TaskItem>(t =>
            t.Tags.Count == 2 &&
            t.Tags.Contains("urgent") &&
            t.Tags.Contains("work")
        )), Times.Once);
    }

    [Fact]
    public void GetAllTasks_NoFilters_ReturnsAllTasks()
    {
        var mockRepo = CreateMockRepository();
        var tasks = new List<TaskItem>
        {
            new() { Title = "Task 1", IsCompleted = false },
            new() { Title = "Task 2", IsCompleted = true }
        };
        mockRepo.Setup(r => r.GetAll()).Returns(tasks.AsEnumerable());
        var taskService = new TaskService(mockRepo.Object);

        var result = taskService.GetAllTasks();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetAllTasks_StatusFilterCompleted_ReturnsOnlyCompletedTasks()
    {
        var mockRepo = CreateMockRepository();
        var tasks = new List<TaskItem>
        {
            new() { Title = "Task 1", IsCompleted = false },
            new() { Title = "Task 2", IsCompleted = true },
            new() { Title = "Task 3", IsCompleted = true }
        };
        mockRepo.Setup(r => r.GetAll()).Returns(tasks.AsEnumerable());
        var taskService = new TaskService(mockRepo.Object);

        var result = taskService.GetAllTasks("completed");

        Assert.Equal(2, result.Count());
        Assert.All(result, task => Assert.True(task.IsCompleted));
    }

    [Fact]
    public void GetAllTasks_StatusFilterPending_ReturnsOnlyPendingTasks()
    {
        var mockRepo = CreateMockRepository();
        var tasks = new List<TaskItem>
        {
            new() { Title = "Task 1", IsCompleted = false },
            new() { Title = "Task 2", IsCompleted = true },
            new() { Title = "Task 3", IsCompleted = false }
        };
        mockRepo.Setup(r => r.GetAll()).Returns(tasks.AsEnumerable());
        var taskService = new TaskService(mockRepo.Object);

        var result = taskService.GetAllTasks("pending");

        Assert.Equal(2, result.Count());
        Assert.All(result, task => Assert.False(task.IsCompleted));
    }

    [Fact]
    public void GetAllTasks_SearchByTitle_ReturnsMatchingTasks()
    {
        var mockRepo = CreateMockRepository();
        var tasks = new List<TaskItem>
        {
            new() { Title = "Buy groceries", Description = "Milk and bread" },
            new() { Title = "Clean house", Description = "Vacuum and dust" },
            new() { Title = "Buy new phone", Description = "Research models" }
        };
        mockRepo.Setup(r => r.GetAll()).Returns(tasks.AsEnumerable());
        var taskService = new TaskService(mockRepo.Object);

        var result = taskService.GetAllTasks(null, "buy");

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void SearchTasks_ByTag_ReturnsMatchingTasks()
    {
        var mockRepo = CreateMockRepository();
        var tasks = new List<TaskItem>
        {
            new() { Title = "Task 1", Tags = new List<string> { "urgent", "work" } },
            new() { Title = "Task 2", Tags = new List<string> { "personal" } },
            new() { Title = "Task 3", Tags = new List<string> { "urgent" } }
        };
        mockRepo.Setup(r => r.GetAll()).Returns(tasks.AsEnumerable());
        var taskService = new TaskService(mockRepo.Object);

        var result = taskService.SearchTasks("urgent");

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetTasksByPriority_High_ReturnsHighPriorityTasks()
    {
        var mockRepo = CreateMockRepository();
        var tasks = new List<TaskItem>
        {
            new() { Title = "Task 1", Priority = TaskPriority.High },
            new() { Title = "Task 2", Priority = TaskPriority.Low },
            new() { Title = "Task 3", Priority = TaskPriority.High }
        };
        mockRepo.Setup(r => r.GetAll()).Returns(tasks.AsEnumerable());
        var taskService = new TaskService(mockRepo.Object);

        var result = taskService.GetTasksByPriority(TaskPriority.High);

        Assert.Equal(2, result.Count());
        Assert.All(result, task => Assert.Equal(TaskPriority.High, task.Priority));
    }

    [Fact]
    public void ToggleTaskCompletion_PendingTask_MarksAsCompleted()
    {
        var mockRepo = CreateMockRepository();
        var task = new TaskItem { Id = Guid.NewGuid(), Title = "Task", IsCompleted = false };
        mockRepo.Setup(r => r.GetById(task.Id)).Returns(task);
        mockRepo.Setup(r => r.Update(task.Id, task)).Returns(true);
        var taskService = new TaskService(mockRepo.Object);

        var result = taskService.ToggleTaskCompletion(task.Id);

        Assert.True(result);
        Assert.True(task.IsCompleted);
        Assert.NotNull(task.CompletedAt);
    }

    [Fact]
    public void ToggleTaskCompletion_CompletedTask_MarksAsPending()
    {
        var mockRepo = CreateMockRepository();
        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = "Task",
            IsCompleted = true,
            CompletedAt = DateTime.UtcNow
        };
        mockRepo.Setup(r => r.GetById(task.Id)).Returns(task);
        mockRepo.Setup(r => r.Update(task.Id, task)).Returns(true);
        var taskService = new TaskService(mockRepo.Object);

        var result = taskService.ToggleTaskCompletion(task.Id);

        Assert.True(result);
        Assert.False(task.IsCompleted);
        Assert.Null(task.CompletedAt);
    }

    [Fact]
    public void UpdateTask_EmptyTitle_ThrowsArgumentException()
    {
        var mockRepo = CreateMockRepository();
        var taskService = new TaskService(mockRepo.Object);

        Assert.Throws<ArgumentException>(() =>
            taskService.UpdateTask(Guid.NewGuid(), new TaskItem { Title = "" }));
    }

    [Fact]
    public void DeleteTask_ExistingTask_ReturnsTrue()
    {
        var mockRepo = CreateMockRepository();
        var taskId = Guid.NewGuid();
        mockRepo.Setup(r => r.Delete(taskId)).Returns(true);
        var taskService = new TaskService(mockRepo.Object);

        var result = taskService.DeleteTask(taskId);

        Assert.True(result);
        mockRepo.Verify(r => r.Delete(taskId), Times.Once);
    }

    [Fact]
    public void GetOverdueTasks_ReturnsOnlyOverdueTasks()
    {
        var mockRepo = CreateMockRepository();
        var tasks = new List<TaskItem>
        {
            new() { Title = "Overdue", DueDate = DateTime.UtcNow.AddDays(-1), IsCompleted = false },
            new() { Title = "Future", DueDate = DateTime.UtcNow.AddDays(1), IsCompleted = false },
            new() { Title = "No due date", IsCompleted = false }
        };
        mockRepo.Setup(r => r.GetAll()).Returns(tasks.AsEnumerable());
        var taskService = new TaskService(mockRepo.Object);

        var result = taskService.GetOverdueTasks();

        Assert.Single(result);
        Assert.Equal("Overdue", result.First().Title);
    }
}