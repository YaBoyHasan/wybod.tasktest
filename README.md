# Task Manager - Take Home Test

Welcome! This take-home test is designed to assess your full-stack development skills using C# and Vue.js.

## Prerequisites

Before you begin, ensure you have the following installed:
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v18 or higher)
- Your preferred IDE (Visual Studio, VS Code, Rider, etc.)

## Getting Started

### 1. Clone the Repository
```bash
git clone <repository-url>
cd TakeHomeTest/Wybod.TaskTest
```

### 2. Install Frontend Dependencies
```bash
cd ClientApp
npm install
cd ..
```

### 3. Run the Application
```bash
dotnet run
```

The application will start and automatically launch the Vue.js development server. Navigate to `https://localhost:7067` in your browser.

### 4. Run Tests
```bash
# Backend tests (if you add any)
dotnet test

# Frontend tests
cd ClientApp
npm test
```

## Project Structure

```
Wybod.TaskTest/
├── Controllers/          # API controllers
├── Data/
│   ├── Models/          # Domain models
│   └── Repositories/    # Data access layer
├── ClientApp/           # Vue.js application
│   ├── src/
│   │   ├── components/  # Reusable components
│   │   ├── views/       # Page components
│   │   └── router/      # Route configuration
│   └── package.json
└── Program.cs           # Application entry point
```

## Your Task

Build a fully functional task management application. The foundation has been laid, but key features are missing.

### Core Features

1. **Task Creation**
   - Users should be able to create new tasks with a title and description
   - New tasks should appear in the task list

2. **Task Management**
   - Users should be able to mark tasks as complete or incomplete
   - Users should be able to delete tasks they no longer need

3. **Task Filtering**
   - Users should be able to filter the task list by status (All, Completed, Pending)
   - The UI should clearly show which filter is active

4. **Task Details Page**
   - Users should be able to view the full details of any task
   - Clicking on a task should navigate to a dedicated detail page
   - The detail page should show all task information

5. **Code Quality**
   - Refactor the codebase to follow best practices
   - Add appropriate test coverage for new features

### Stretch Features (Optional - Pick Any)

6. **Advanced Search & Filtering**
   - Search tasks by title or description
   - Sort tasks by date, title, or status
   - Combine multiple filters

7. **Enhanced Task Features**
   - Add due dates to tasks with overdue indicators
   - Add priority levels (High, Medium, Low)
   - Add categories or tags for better organization

8. **User Experience**
   - Add visual feedback for all actions (loading states, success/error messages)
   - Implement animations and transitions
   - Add keyboard shortcuts for common actions

## What we'd like to see

- **Commit Regularly**: We want to see your thought process. Commit early and often
- **Show Your Skills**: Feel free to 'overengineer' - this is your chance to demonstrate what you can do
- **Both Frontend and Backend**: Ensure you showcase skills in both areas
- **Time Estimate**: 2-3 hours

## Evaluation Criteria

We'll be looking at:
- Code quality, organisation, and readability
- Problem-solving approach
- UI design and implementation - feel free to use prebuilt components such as shadcn-vue
- Understanding of C# and Vue.js best practices
- Testing strategy
- User experience considerations
- Git commit history and messages

## Questions?

If you have any questions or run into issues, please reach out to us.

Good luck! We're excited to see what you build.