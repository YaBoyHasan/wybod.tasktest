<template>
    <form @submit.prevent="handleSubmit" class="space-y-4 bg-white p-6 rounded-lg shadow-sm border">
        <h2 class="text-xl font-bold text-gray-900">Create New Task</h2>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Title *</label>
                <input v-model="form.title"
                       type="text"
                       required
                       class="w-full border border-gray-300 px-3 py-2 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 transition" />
            </div>

            <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Due Date</label>
                <input v-model="form.dueDate"
                       type="datetime-local"
                       class="w-full border border-gray-300 px-3 py-2 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 transition" />
            </div>
        </div>

        <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Description</label>
            <textarea v-model="form.description"
                      rows="3"
                      class="w-full border border-gray-300 px-3 py-2 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 transition" />
        </div>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Priority</label>
                <select v-model="form.priority"
                        class="w-full border border-gray-300 px-3 py-2 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 transition">
                    <option :value="TaskPriority.Low">Low</option>
                    <option :value="TaskPriority.Medium">Medium</option>
                    <option :value="TaskPriority.High">High</option>
                </select>
            </div>

            <div>
                <label class="block text-sm font-medium text-gray-700 mb-1">Tags (comma separated)</label>
                <input v-model="tagsInput"
                       type="text"
                       placeholder="work, urgent, personal"
                       class="w-full border border-gray-300 px-3 py-2 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 transition" />
            </div>
        </div>

        <div class="flex gap-2">
            <button type="submit"
                    :disabled="busy || !form.title.trim()"
                    class="px-6 py-2 rounded bg-blue-600 text-white hover:bg-blue-700 disabled:bg-gray-400 transition active:scale-95">
                Create Task
            </button>
            <button v-if="form.title || form.description"
                    type="button"
                    @click="resetForm"
                    class="px-4 py-2 rounded bg-gray-200 text-gray-700 hover:bg-gray-300 transition">
                Clear
            </button>
        </div>
    </form>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { TaskPriority } from '@/api'

const emit = defineEmits<{
    create: [data: {
        title: string;
        description?: string;
        dueDate?: string;
        priority: TaskPriority;
        tags: string[];
    }]
}>()

defineProps<{
    busy: boolean
}>()

const form = ref({
    title: '',
    description: '',
    dueDate: '',
    priority: TaskPriority.Medium
})

const tagsInput = ref('')

const handleSubmit = () => {
    if (!form.value.title.trim()) {
        return
    }

    const tags = tagsInput.value
        .split(',')
        .map(t => t.trim())
        .filter(t => t.length > 0)

    emit('create', {
        title: form.value.title.trim(),
        description: form.value.description.trim() || undefined,
        dueDate: form.value.dueDate || undefined,
        priority: form.value.priority,
        tags
    })

    resetForm()
}

const resetForm = () => {
    form.value = {
        title: '',
        description: '',
        dueDate: '',
        priority: TaskPriority.Medium
    }
    tagsInput.value = ''
}
</script>
