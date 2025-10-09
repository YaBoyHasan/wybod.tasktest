using Wybod.TaskTest.Data.Models;

namespace Wybod.TaskTest.Data;

public interface IDataContext
{
    IList<TaskItem> Tasks { get; }
}

public class DataContext : IDataContext
{
    public IList<TaskItem> Tasks { get; } = new List<TaskItem>()
    {
        new() { Id = Guid.NewGuid(), Title = "Research hiking trails", Description = "Find and compare local hiking trails for weekend outdoor activity", IsCompleted = true },
        new() { Id = Guid.NewGuid(), Title = "Plan team lunch", Description = "Coordinate restaurant reservations for Friday team gathering", IsCompleted = true },
        new() { Id = Guid.NewGuid(), Title = "Organize home office", Description = "Declutter desk and reorganize filing system for better productivity", IsCompleted = false },
        new() { Id = Guid.NewGuid(), Title = "Book dentist appointment", Description = "Schedule routine dental checkup for next month", IsCompleted = false },
        new() { Id = Guid.NewGuid(), Title = "Review monthly budget", Description = "Analyze spending patterns and adjust budget categories as needed", IsCompleted = false },
        new() { Id = Guid.NewGuid(), Title = "Learn basic photography", Description = "Complete online photography course to improve vacation photos", IsCompleted = false },
        new() { Id = Guid.NewGuid(), Title = "Update personal website", Description = "Refresh portfolio and add recent project examples", IsCompleted = false },
        new() { Id = Guid.NewGuid(), Title = "Prepare presentation slides", Description = "Create slides for upcoming community workshop on gardening", IsCompleted = false },
        new() { Id = Guid.NewGuid(), Title = "Research vacation destinations", Description = "Compare travel options for summer holiday planning", IsCompleted = false },
        new() { Id = Guid.NewGuid(), Title = "Clean out garage", Description = "Sort through storage items and donate unused equipment", IsCompleted = false },
        new() { Id = Guid.NewGuid(), Title = "Read industry newsletter", Description = "Catch up on monthly newsletter from professional association", IsCompleted = false },
        new() { Id = Guid.NewGuid(), Title = "Plan garden layout", Description = "Design vegetable garden arrangement for spring planting", IsCompleted = false },
        new() { Id = Guid.NewGuid(), Title = "Schedule car maintenance", Description = "Book oil change and tire rotation at local service center", IsCompleted = false },
        new() { Id = Guid.NewGuid(), Title = "Organize photo library", Description = "Sort and tag digital photos from recent events", IsCompleted = false },
        new() { Id = Guid.NewGuid(), Title = "Practice piano pieces", Description = "Rehearse new songs for upcoming recital performance", IsCompleted = false },
        new() { Id = Guid.NewGuid(), Title = "Update emergency contacts", Description = "Review and refresh contact information for family members", IsCompleted = false },
        new() { Id = Guid.NewGuid(), Title = "Research cooking classes", Description = "Find local culinary workshops for Italian cuisine", IsCompleted = false },
        new() { Id = Guid.NewGuid(), Title = "Backup computer files", Description = "Create backup of important documents to external drive", IsCompleted = false },
        new() { Id = Guid.NewGuid(), Title = "Plan charity event", Description = "Coordinate details for neighborhood food drive next month", IsCompleted = false },
        new() { Id = Guid.NewGuid(), Title = "Review insurance policies", Description = "Compare coverage options and renewal dates for home and auto", IsCompleted = false },
    };
}