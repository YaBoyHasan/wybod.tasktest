<template>
    <div class="p-6 max-w-7xl mx-auto">
        <div class="mb-8">
            <h1 class="text-4xl font-bold text-gray-900 mb-2">Task Manager</h1>
            <p class="text-gray-600">Organize and track your tasks efficiently</p>
        </div>

        <div class="space-y-6">
            <TaskCreateForm :busy="busy" @create="handleCreate" />

            <TaskFiltersComponent :filters="filters" @change="handleFilterChange" />

            <div v-if="loading" class="text-center py-16">
                <div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
                <p class="mt-4 text-gray-600">Loading tasks...</p>
            </div>

            <div v-else-if="tasks.length === 0" class="text-center py-16">
                <div class="text-6xl mb-4">📋</div>
                <h3 class="text-xl font-semibold text-gray-700 mb-2">No tasks found</h3>
                <p class="text-gray-500">
                    {{ filters.search || filters.status || filters.priority !== undefined
                        ? 'Try adjusting your filters'
                        : 'Create your first task to get started'
                    }}
                </p>
            </div>

            <div v-else>
                <div class="mb-4 text-sm text-gray-600">
                    Showing {{ tasks.length }} task{{ tasks.length !== 1 ? 's' : '' }}
                </div>

                <div class="grid gap-4 md:grid-cols-2 xl:grid-cols-3">
                    <TaskCard v-for="task in tasks"
                              :key="task.id"
                              :task="task"
                              :busy="busy"
                              @click="handleViewDetails(task.id)"
                              @toggle="handleToggle(task)"
                              @delete="handleDelete(task.id)" />
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { getTasks, createTask, toggleTask, deleteTask, type TaskItem, type TaskFilters } from '@/api'
import { useToast } from '@/composables/useToast'
import { useKeyboardShortcuts } from '@/composables/useKeyboardShortcuts'
import TaskCreateForm from '@/components/TaskCreateForm.vue'
import TaskFiltersComponent from '@/components/TaskFilters.vue'
import TaskCard from '@/components/TaskCard.vue'

const router = useRouter()
const { success, error: showError } = useToast()

const tasks = ref<TaskItem[]>([])
const loading = ref(true)
const busy = ref(false)
const filters = ref<TaskFilters>({})

const load = async () => {
    loading.value = true
    try {
        tasks.value = await getTasks(filters.value)
    } catch (e: any) {
        showError(e?.message || 'Failed to load tasks')
    } finally {
        loading.value = false
    }
}

const handleCreate = async (data: any) => {
    busy.value = true
    try {
        await createTask(data.title, data.description, data.dueDate, data.priority, data.tags)
        success('Task created successfully!')
        await load()
    } catch (e: any) {
        showError(e?.message || 'Failed to create task')
    } finally {
        busy.value = false
    }
}

const handleToggle = async (task: TaskItem) => {
    busy.value = true
    try {
        await toggleTask(task.id)
        success(task.isCompleted ? 'Task marked as pending' : 'Task completed!')
        await load()
    } catch (e: any) {
        showError(e?.message || 'Failed to update task')
    } finally {
        busy.value = false
    }
}

const handleDelete = async (id: string) => {
    if (!confirm('Are you sure you want to delete this task?')) return

    busy.value = true
    try {
        await deleteTask(id)
        success('Task deleted successfully')
        await load()
    } catch (e: any) {
        showError(e?.message || 'Failed to delete task')
    } finally {
        busy.value = false
    }
}

const handleViewDetails = (id: string) => {
    router.push(`/task/${id}`)
}

const handleFilterChange = (newFilters: TaskFilters) => {
    filters.value = newFilters
    load()
}

useKeyboardShortcuts([
    {
        key: 'n',
        ctrl: true,
        handler: () => {
            const titleInput = document.querySelector('input[type="text"]') as HTMLInputElement
            titleInput?.focus()
        },
        description: 'Focus on new task input'
    },
    {
        key: 'f',
        ctrl: true,
        handler: () => {
            const searchInput = document.querySelector('input[placeholder*="Search"]') as HTMLInputElement
            searchInput?.focus()
        },
        description: 'Focus on search'
    },
    {
        key: '/',
        handler: () => {
            const searchInput = document.querySelector('input[placeholder*="Search"]') as HTMLInputElement
            searchInput?.focus()
        },
        description: 'Focus on search'
    }
])

onMounted(load)
</script>
