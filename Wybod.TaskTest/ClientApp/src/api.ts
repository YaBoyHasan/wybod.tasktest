export enum TaskPriority {
    Low = 0,
    Medium = 1,
    High = 2
}

export type TaskItem = {
    id: string;
    title: string;
    description?: string;
    isCompleted: boolean;
    createdAt: string;
    completedAt?: string | null;
    dueDate?: string | null;
    priority: TaskPriority;
    tags: string[];
    isOverdue?: boolean;
};

export type TaskFilters = {
    status?: "completed" | "pending" | "overdue";
    search?: string;
    priority?: TaskPriority;
    sortBy?: "title" | "priority" | "duedate" | "created";
};

const base = "/api/tasks";

export const getTasks = async (filters?: TaskFilters) => {
    const params = new URLSearchParams();
    if (filters?.status) params.append("status", filters.status);
    if (filters?.search) params.append("search", filters.search);
    if (filters?.priority !== undefined) params.append("priority", TaskPriority[filters.priority]);
    if (filters?.sortBy) params.append("sortBy", filters.sortBy);

    const url = params.toString() ? `${base}?${params}` : base;
    return (await fetch(url)).json();
};

export const getTask = async (id: string) =>
    (await fetch(`${base}/${id}`)).json();

export const createTask = async (
    title: string,
    description?: string,
    dueDate?: string | null,
    priority: TaskPriority = TaskPriority.Medium,
    tags: string[] = []
) => {
    const r = await fetch(base, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ title, description, dueDate, priority, tags })
    });
    return r.json();
};

export const updateTask = async (id: string, data: Partial<TaskItem>) =>
    fetch(`${base}/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });

export const toggleTask = async (id: string) =>
    fetch(`${base}/${id}/toggle`, { method: "PATCH" });

export const deleteTask = async (id: string) =>
    fetch(`${base}/${id}`, { method: "DELETE" });
