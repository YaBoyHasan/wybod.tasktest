<template>
    <div class="space-y-4 bg-white p-6 rounded-lg shadow-sm border">
        <div class="flex flex-wrap items-center gap-4">
            <div class="flex-1 min-w-[250px]">
                <input v-model="localSearch"
                       type="text"
                       placeholder="Search tasks by title, description, or tags..."
                       class="w-full border border-gray-300 px-4 py-2 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 transition"
                       @input="debouncedSearch" />
            </div>

            <div>
                <select v-model="localFilters.sortBy"
                        @change="emitChange"
                        class="border border-gray-300 px-3 py-2 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 transition">
                    <option value="">Sort: Latest</option>
                    <option value="title">Sort: Title</option>
                    <option value="priority">Sort: Priority</option>
                    <option value="duedate">Sort: Due Date</option>
                </select>
            </div>
        </div>

        <div class="flex flex-wrap gap-2">
            <div class="flex gap-2">
                <button :class="statusButtonClass('')"
                        @click="setStatus('')">
                    All
                </button>
                <button :class="statusButtonClass('pending')"
                        @click="setStatus('pending')">
                    Pending
                </button>
                <button :class="statusButtonClass('completed')"
                        @click="setStatus('completed')">
                    Completed
                </button>
                <button :class="statusButtonClass('overdue')"
                        @click="setStatus('overdue')">
                    Overdue
                </button>
            </div>

            <div class="h-6 w-px bg-gray-300"></div>

            <div class="flex gap-2">
                <button :class="priorityButtonClass(undefined)"
                        @click="setPriority(undefined)">
                    All Priority
                </button>
                <button :class="priorityButtonClass(TaskPriority.High)"
                        @click="setPriority(TaskPriority.High)">
                    High
                </button>
                <button :class="priorityButtonClass(TaskPriority.Medium)"
                        @click="setPriority(TaskPriority.Medium)">
                    Medium
                </button>
                <button :class="priorityButtonClass(TaskPriority.Low)"
                        @click="setPriority(TaskPriority.Low)">
                    Low
                </button>
            </div>

            <button v-if="hasActiveFilters"
                    @click="clearFilters"
                    class="ml-auto px-3 py-1 text-sm rounded bg-gray-200 text-gray-700 hover:bg-gray-300 transition">
                Clear Filters
            </button>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, computed, watch } from 'vue'
import { TaskPriority, type TaskFilters } from '@/api'

const props = defineProps<{
    filters: TaskFilters
}>()

const emit = defineEmits<{
    change: [filters: TaskFilters]
}>()

const localFilters = ref<TaskFilters>({ ...props.filters })
const localSearch = ref(props.filters.search || '')

let debounceTimer: number | null = null

const debouncedSearch = () => {
    if (debounceTimer) clearTimeout(debounceTimer)
    debounceTimer = window.setTimeout(() => {
        localFilters.value.search = localSearch.value || undefined
        emitChange()
    }, 300)
}

const emitChange = () => {
    emit('change', { ...localFilters.value })
}

const setStatus = (status: '' | 'completed' | 'pending' | 'overdue') => {
    localFilters.value.status = status || undefined
    emitChange()
}

const setPriority = (priority: TaskPriority | undefined) => {
    localFilters.value.priority = priority
    emitChange()
}

const clearFilters = () => {
    localFilters.value = {}
    localSearch.value = ''
    emitChange()
}

const hasActiveFilters = computed(() => {
    return !!(localFilters.value.status || localFilters.value.search || localFilters.value.priority !== undefined)
})

const statusButtonClass = (status: '' | 'completed' | 'pending' | 'overdue') => {
    const isActive = (localFilters.value.status || '') === status
    const baseClass = 'px-3 py-1 text-sm rounded transition-all duration-200'

    if (isActive) {
        const activeColors = {
            '': 'bg-gray-900 text-white shadow',
            'pending': 'bg-amber-600 text-white shadow',
            'completed': 'bg-green-600 text-white shadow',
            'overdue': 'bg-red-600 text-white shadow'
        }
        return `${baseClass} ${activeColors[status]}`
    }

    return `${baseClass} bg-gray-200 hover:bg-gray-300`
}

const priorityButtonClass = (priority: TaskPriority | undefined) => {
    const isActive = localFilters.value.priority === priority
    const baseClass = 'px-3 py-1 text-sm rounded transition-all duration-200'

    if (isActive) {
        if (priority === undefined) return `${baseClass} bg-gray-900 text-white shadow`
        const colors = ['bg-gray-600', 'bg-blue-600', 'bg-red-600']
        return `${baseClass} ${colors[priority]} text-white shadow`
    }

    return `${baseClass} bg-gray-200 hover:bg-gray-300`
}

watch(() => props.filters, (newFilters) => {
    localFilters.value = { ...newFilters }
    localSearch.value = newFilters.search || ''
}, { deep: true })
</script>
