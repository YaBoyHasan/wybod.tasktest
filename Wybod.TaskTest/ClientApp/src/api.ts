
export type TaskItem = {
    id: string; title: string; description?: string;
    isCompleted: boolean; createdAt: string; completedAt?: string | null;
};
const base = "/api/tasks";

export const getTasks = async (status?: "completed" | "pending") =>
    (await fetch(status ? `${base}?status=${status}` : base)).json();

export const getTask = async (id: string) =>
    (await fetch(`${base}/${id}`)).json();

export const createTask = async (title: string, description?: string) => {
    const r = await fetch(base, {
        method: "POST", headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ title, description })
    });
    return r.json();
};

export const updateTask = async (id: string, data: Partial<TaskItem>) =>
    fetch(`${base}/${id}`, {
        method: "PUT", headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });

export const deleteTask = async (id: string) =>
    fetch(`${base}/${id}`, { method: "DELETE" });
