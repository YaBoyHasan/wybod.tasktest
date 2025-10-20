<template>
    <Card class="transition-all hover:-translate-y-1 hover:shadow-xl duration-200 cursor-pointer"
          @click="$emit('click')">
        <CardHeader>
            <div class="flex justify-between items-start gap-4">
                <div class="flex-1 min-w-0">
                    <CardTitle class="truncate">{{ task.title }}</CardTitle>
                    <CardDescription v-if="task.description" class="line-clamp-2">
                        {{ task.description }}
                    </CardDescription>
                </div>

                <span :class="[
                    'px-3 py-1 rounded-full text-xs font-bold shadow-sm whitespace-nowrap flex-shrink-0',
                    task.isCompleted ? 'bg-green-500 text-white' : 'bg-amber-500 text-white'
                ]">
                    {{ task.isCompleted ? 'Completed' : 'Pending' }}
                </span>
            </div>
        </CardHeader>

        <CardContent>
            <div class="space-y-3">
                <div class="flex flex-wrap gap-2">
                    <span :class="['px-2 py-1 rounded text-xs font-medium', priorityColor(task.priority)]">
                        {{ priorityLabel(task.priority) }}
                    </span>

                    <span v-if="task.dueDate"
                          :class="[
                              'px-2 py-1 rounded text-xs font-medium',
                              task.isOverdue ? 'bg-red-100 text-red-800' : 'bg-gray-100 text-gray-800'
                          ]">
                        Due: {{ formatDueDate(task.dueDate) }}
                    </span>

                    <span v-if="task.tags && task.tags.length > 0"
                          class="px-2 py-1 rounded text-xs bg-blue-50 text-blue-700">
                        {{ task.tags.length }} tag{{ task.tags.length > 1 ? 's' : '' }}
                    </span>
                </div>

                <div class="flex justify-between items-center pt-2 border-t">
                    <small class="text-xs text-gray-500">{{ formatDate(task.createdAt) }}</small>

                    <div class="flex gap-2" @click.stop>
                        <button @click="$emit('toggle')"
                                :disabled="busy"
                                class="text-xs px-3 py-1 rounded bg-blue-600 text-white hover:bg-blue-700 active:scale-95 transition-all disabled:bg-gray-400">
                            {{ task.isCompleted ? 'Reopen' : 'Complete' }}
                        </button>

                        <button @click="$emit('delete')"
                                :disabled="busy"
                                class="text-xs px-3 py-1 rounded bg-red-600 text-white hover:bg-red-700 active:scale-95 transition-all disabled:bg-gray-400">
                            Delete
                        </button>
                    </div>
                </div>
            </div>
        </CardContent>
    </Card>
</template>

<script setup lang="ts">
import { type TaskItem, TaskPriority } from '@/api'
import Card from '@/components/ui/Card.vue'
import CardHeader from '@/components/ui/CardHeader.vue'
import CardTitle from '@/components/ui/CardTitle.vue'
import CardDescription from '@/components/ui/CardDescription.vue'
import CardContent from '@/components/ui/CardContent.vue'

defineProps<{
    task: TaskItem
    busy: boolean
}>()

defineEmits<{
    click: []
    toggle: []
    delete: []
}>()

const formatDate = (dateString: string): string => {
    const date = new Date(dateString)
    return date.toLocaleDateString()
}

const formatDueDate = (dateString: string): string => {
    const date = new Date(dateString)
    const now = new Date()
    const diffTime = date.getTime() - now.getTime()
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24))

    if (diffDays < 0) return `${Math.abs(diffDays)}d ago`
    if (diffDays === 0) return 'Today'
    if (diffDays === 1) return 'Tomorrow'
    if (diffDays < 7) return `${diffDays}d`
    return date.toLocaleDateString()
}

const priorityLabel = (priority: TaskPriority): string => {
    return ['Low', 'Medium', 'High'][priority]
}

const priorityColor = (priority: TaskPriority): string => {
    const colors = [
        'bg-gray-200 text-gray-700',
        'bg-blue-200 text-blue-800',
        'bg-red-200 text-red-800'
    ]
    return colors[priority]
}
</script>
