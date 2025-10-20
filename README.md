# 🧩 Task Management Application

A fully functional **Task Management App** built with **ASP.NET Core (C#)** and **Vue.js 3 + TypeScript**.  
It provides a complete workflow for creating, organizing, and managing tasks with advanced filtering, search, and smooth UX.

---

## 🚀 Features Overview

### ✅ Core Features
#### 📝 Task Creation & Management
- Create tasks with **title**, **description**, **due date**, **priority**, and **tags**
- Toggle task **complete/incomplete** instantly
- Delete tasks with confirmation prompt
- Visual feedback using **toast notifications** for all actions

#### 🔍 Task Filtering & Search
- Filter by **status:** All, Completed, Pending, Overdue  
- Filter by **priority:** All, High, Medium, Low  
- Real-time **search** across title, description, and tags  
- **Sort by:** Latest (default), Title, Priority, Due Date  
- One-click **clear filters**

#### 📄 Task Details Page
- Dedicated **detail view** for each task  
- Displays all task metadata (dates, priority, tags, overdue status)  
- Quick actions: toggle completion or delete  
- Accessible by clicking any task card

---

## 🌟 Stretch Features
### 🧠 Enhanced Task Functionality
- **Due dates** with red highlighting for overdue tasks  
- **Priority levels** (Low, Medium, High) with color coding  
- **Tags/categories** for better organization  
- **Computed overdue status** for incomplete overdue tasks

### 💎 User Experience Enhancements
- **Toast notifications** for success/error feedback  
- **Smooth animations & transitions** throughout  
- **Loading spinners** for async states  
- **Hover effects** on interactive elements  
- **Keyboard shortcuts**
  - `Ctrl + N` → Focus on new task input  
  - `Ctrl + F` or `/` → Focus on search  
- Active filter highlighting  
- Live **task count display**  
- Fully **responsive design**

---

## 🧱 Architecture & Code Quality

### 🔧 Backend (C# / ASP.NET Core)
- `Controller → Service → Repository` clean separation  
- Central **TaskService** for business logic  
- **Error handling** & validation  
- Search, filter, and sorting support  
- New `PATCH` endpoint for toggling task completion  
- **Enhanced Task model:**
  - `Priority` enum  
  - `Tags`, `DueDate`, `IsOverdue` (computed property)

### 🧩 Frontend (Vue.js 3 + TypeScript)
- **Modular, component-based** architecture  
  - `TaskCard` — reusable card component  
  - `TaskCreateForm` — dedicated form component  
  - `TaskFilters` — comprehensive filtering controls  
  - `ToastContainer` — global notification system  
- **Custom composables**
  - `useToast` — notification management  
  - `useKeyboardShortcuts` — global keyboard bindings  
- Organized, **type-safe**, and clean structure  
- Styled with **Tailwind CSS**

---

## 🧪 Testing

### ✅ Coverage Summary
- **35 tests total**
  - **Backend:** 20+ unit tests for `TaskService` & `TaskRepository`
  - **Frontend:** 15+ component tests for UI and logic
- **All tests passing**
- Covers edge cases, error handling, and UI interactions

---

## 🎨 Design & UX

- Modern, clean interface using **blue/green/gray** palette  
- Consistent **spacing & typography**  
- Clear **visual hierarchy**  
- Accessible contrast ratios  
- Smooth **micro-interactions** & transitions  
- **Card-based layout** for easy task scanning  
- Friendly **empty states** and **loading indicators**

---

## ⚙️ Technical Stack

| Layer | Technology |
|-------|-------------|
| Frontend | Vue.js 3 + TypeScript |
| Styling | Tailwind CSS |
| Build Tool | Vite |
| Backend | ASP.NET Core (C#) |
| Architecture | RESTful API + Component-based Frontend |
| Tests | xUnit / Vue Test Utils |
| Deployment | Ready for production build |

---

## 🏁 Getting Started

### 1️⃣ Backend
```bash
cd Wybod.TaskTest
dotnet run