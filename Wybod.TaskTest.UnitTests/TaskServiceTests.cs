using Moq;
using Wybod.TaskTest.Data.Models;
using Wybod.TaskTest.Data.Repositories;
using Wybod.TaskTest.Services;

namespace Wybod.TaskTest.UnitTests;

public class TaskServiceTests
{
    [Fact]
    public void CreateTaskTest()
    {
        var taskRepository = new Mock<ITaskRepository>();
        var taskService = new TaskService(taskRepository.Object);

        taskRepository.Setup(tr => tr.Create(It.IsAny<TaskItem>()));
        
        taskService.CreateTask("Test", "Task description");
        
        taskRepository.Verify(tr => tr.Create(It.IsAny<TaskItem>()), Times.Once);
    }
}