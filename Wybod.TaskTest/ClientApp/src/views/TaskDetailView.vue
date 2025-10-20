<template>
    <div v-if="loading" class="flex justify-center items-center min-h-[400px]">
        <div class="text-lg text-gray-600 animate-pulse">Loading task...</div>
    </div>

    <div v-else-if="error" class="p-6">
        <div class="bg-red-50 border border-red-200 rounded-lg p-4 mb-4">
            <p class="text-red-800">{{ error }}</p>
        </div>
        <button @click="router.push('/')"
                class="px-4 py-2 bg-gray-800 text-white rounded hover:bg-gray-700 transition">
            Back to Tasks
        </button>
    </div>

    <div v-else-if="task" class="p-6 max-w-4xl mx-auto">
        <div class="mb-6 flex items-center justify-between">
            <button @click="router.push('/')"
                    class="flex items-center gap-2 text-gray-600 hover:text-gray-900 transition">
                <span class="text-xl">←</span>
                <span>Back to Tasks</span>
            </button>

            <div class="flex gap-2">
                <button @click="handleToggle"
                        :disabled="busy"
                        class="px-4 py-2 rounded transition active:scale-95"
                        :class="task.isCompleted
                            ? 'bg-amber-600 text-white hover:bg-amber-700'
                            : 'bg-green-600 text-white hover:bg-green-700'">
                    {{ task.isCompleted ? 'Mark Pending' : 'Mark Complete' }}
                </button>

                <button @click="handleDelete"
                        :disabled="busy"
                        class="px-4 py-2 rounded bg-red-600 text-white hover:bg-red-700 transition active:scale-95">
                    Delete
                </button>
            </div>
        </div>

        <Card>
            <CardHeader>
                <div class="flex items-start justify-between gap-4">
                    <div class="flex-1">
                        <CardTitle class="text-3xl mb-2">{{ task.title }}</CardTitle>
                        <CardDescription v-if="task.description" class="text-base">
                            {{ task.description }}
                        </CardDescription>
                    </div>

                    <span :class="[
                        'px-4 py-2 rounded-full text-sm font-bold shadow-sm whitespace-nowrap',
                        task.isCompleted ? 'bg-green-500 text-white' : 'bg-amber-500 text-white'
                    ]">
                        {{ task.isCompleted ? 'Completed' : 'Pending' }}
                    </span>
                </div>
            </CardHeader>

            <CardContent>
                <div class="space-y-6">
                    <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                        <div class="space-y-4">
                            <div>
                                <h3 class="text-sm font-semibold text-gray-600 uppercase mb-1">Priority</h3>
                                <span :class="[
                                    'inline-block px-3 py-1 rounded text-sm font-medium',
                                    priorityColor(task.priority)
                                ]">
                                    {{ priorityLabel(task.priority) }}
                                </span>
                            </div>

                            <div v-if="task.dueDate">
                                <h3 class="text-sm font-semibold text-gray-600 uppercase mb-1">Due Date</h3>
                                <div :class="[
                                    'text-base',
                                    task.isOverdue ? 'text-red-600 font-semibold' : 'text-gray-800'
                                ]">
                                    {{ formatDate(task.dueDate) }}
                                    <span v-if="task.isOverdue" class="ml-2 text-xs bg-red-100 text-red-800 px-2 py-1 rounded">
                                        OVERDUE
                                    </span>
                                </div>
                            </div>
                        </div>

                        <div class="space-y-4">
                            <div>
                                <h3 class="text-sm font-semibold text-gray-600 uppercase mb-1">Created</h3>
                                <p class="text-gray-800">{{ formatDate(task.createdAt) }}</p>
                            </div>

                            <div v-if="task.completedAt">
                                <h3 class="text-sm font-semibold text-gray-600 uppercase mb-1">Completed</h3>
                                <p class="text-gray-800">{{ formatDate(task.completedAt) }}</p>
                            </div>
                        </div>
                    </div>

                    <div v-if="task.tags && task.tags.length > 0">
                        <h3 class="text-sm font-semibold text-gray-600 uppercase mb-2">Tags</h3>
                        <div class="flex flex-wrap gap-2">
                            <span v-for="tag in task.tags"
                                  :key="tag"
                                  class="px-3 py-1 bg-blue-100 text-blue-800 rounded-full text-sm">
                                {{ tag }}
                            </span>
                        </div>
                    </div>
                </div>
            </CardContent>
        </Card>
    </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { getTask, toggleTask, deleteTask, type TaskItem, TaskPriority } from '@/api'
import Card from '@/components/ui/Card.vue'
import CardHeader from '@/components/ui/CardHeader.vue'
import CardTitle from '@/components/ui/CardTitle.vue'
import CardDescription from '@/components/ui/CardDescription.vue'
import CardContent from '@/components/ui/CardContent.vue'

const route = useRoute()
const router = useRouter()

const task = ref<TaskItem | null>(null)
const loading = ref(true)
const busy = ref(false)
const error = ref<string | null>(null)

const load = async () => {
    loading.value = true
    error.value = null
    try {
        const id = route.params.id as string
        task.value = await getTask(id)
    } catch (e: any) {
        error.value = e?.message || 'Failed to load task'
    } finally {
        loading.value = false
    }
}

const handleToggle = async () => {
    if (!task.value) return
    busy.value = true
    error.value = null
    try {
        await toggleTask(task.value.id)
        await load()
    } catch (e: any) {
        error.value = e?.message || 'Failed to update task'
    } finally {
        busy.value = false
    }
}

const handleDelete = async () => {
    if (!task.value || !confirm('Are you sure you want to delete this task?')) return
    busy.value = true
    error.value = null
    try {
        await deleteTask(task.value.id)
        router.push('/')
    } catch (e: any) {
        error.value = e?.message || 'Failed to delete task'
        busy.value = false
    }
}

const formatDate = (dateString: string): string =>
    new Date(dateString).toLocaleString()

const priorityLabel = (priority: TaskPriority): string => {
    return ['Low', 'Medium', 'High'][priority]
}

const priorityColor = (priority: TaskPriority): string => {
    const colors = [
        'bg-gray-200 text-gray-800',
        'bg-blue-200 text-blue-800',
        'bg-red-200 text-red-800'
    ]
    return colors[priority]
}

onMounted(load)
</script>
