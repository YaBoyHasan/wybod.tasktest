<template>
    <div class="p-4">
        <!-- Controls -->
        <div class="mb-6 flex flex-col gap-3">
            <form class="flex flex-wrap gap-2" @submit.prevent="onCreate">
                <input v-model="title"
                       class="border px-3 py-2 rounded w-56 focus:outline-none focus:ring-2 focus:ring-blue-500 transition"
                       placeholder="Title"
                       required />
                <input v-model="desc"
                       class="border px-3 py-2 rounded w-80 focus:outline-none focus:ring-2 focus:ring-blue-500 transition"
                       placeholder="Description (optional)" />
                <button :disabled="busy"
                        class="px-4 py-2 rounded bg-black text-white hover:bg-gray-800 active:scale-95 transition-all">
                    Add
                </button>
            </form>

            <div class="flex gap-2">
                <button :class="[
            'px-3 py-1 rounded transition-all duration-200',
            filter==='' ? 'bg-gray-900 text-white shadow' : 'bg-gray-200 hover:bg-gray-300'
          ]"
                        @click="setFilter('')">
                    All
                </button>
                <button :class="[
            'px-3 py-1 rounded transition-all duration-200',
            filter==='completed' ? 'bg-green-600 text-white shadow' : 'bg-gray-200 hover:bg-gray-300'
          ]"
                        @click="setFilter('completed')">
                    Completed
                </button>
                <button :class="[
            'px-3 py-1 rounded transition-all duration-200',
            filter==='pending' ? 'bg-amber-600 text-white shadow' : 'bg-gray-200 hover:bg-gray-300'
          ]"
                        @click="setFilter('pending')">
                    Pending
                </button>
            </div>

            <p v-if="error" class="text-red-600 text-sm animate-pulse">{{ error }}</p>
        </div>

        <!-- Loading / Error / Empty -->
        <div v-if="loading" class="text-center py-8 text-lg text-gray-600 animate-pulse">
            Loading tasks...
        </div>

        <div v-else-if="error" class="text-center py-8 text-lg text-red-600">
            {{ error }}
        </div>

        <div v-else>
            <div v-if="tasks.length === 0" class="text-center py-8 text-lg text-gray-500">
                No tasks yet
            </div>

            <!-- Task Cards -->
            <div v-else class="grid gap-4 md:grid-cols-2 xl:grid-cols-3">
                <Card v-for="task in tasks"
                      :key="task.id"
                      class="transition-transform hover:-translate-y-1 hover:shadow-lg duration-200">
                    <CardHeader>
                        <div class="flex justify-between items-start">
                            <div>
                                <CardTitle>{{ task.title }}</CardTitle>
                                <CardDescription>{{ task.description }}</CardDescription>
                            </div>

                            <div class="flex flex-col items-end gap-2">
                                <span :class="[
                    'px-3 py-1 rounded-full text-xs font-bold shadow-sm',
                    task.isCompleted ? 'bg-green-500 text-white' : 'bg-amber-500 text-white'
                  ]">
                                    {{ task.isCompleted ? 'Completed' : 'Pending' }}
                                </span>

                                <div class="flex gap-2">
                                    <button class="text-xs px-3 py-1 rounded bg-blue-600 text-white hover:bg-blue-700 active:scale-95 transition-all"
                                            :disabled="busy"
                                            @click="toggle(task)">
                                        {{ task.isCompleted ? 'Mark Pending' : 'Mark Done' }}
                                    </button>

                                    <button class="text-xs px-3 py-1 rounded bg-red-600 text-white hover:bg-red-700 active:scale-95 transition-all"
                                            :disabled="busy"
                                            @click="removeTask(task.id)">
                                        Delete
                                    </button>
                                </div>
                            </div>
                        </div>
                    </CardHeader>

                    <CardContent>
                        <div class="flex gap-5 text-xs text-gray-500">
                            <small>Created: {{ formatDate(task.createdAt) }}</small>
                            <small v-if="task.completedAt">Completed: {{ formatDate(task.completedAt!) }}</small>
                        </div>
                    </CardContent>
                </Card>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
    import { ref, onMounted } from 'vue'
    import { getTasks, createTask, updateTask, deleteTask, type TaskItem } from '@/api'

    import Card from '@/components/ui/Card.vue'
    import CardHeader from '@/components/ui/CardHeader.vue'
    import CardTitle from '@/components/ui/CardTitle.vue'
    import CardDescription from '@/components/ui/CardDescription.vue'
    import CardContent from '@/components/ui/CardContent.vue'

    const tasks = ref<TaskItem[]>([])
    const loading = ref(true)
    const busy = ref(false)
    const error = ref<string | null>(null)
    const filter = ref<'' | 'completed' | 'pending'>('')

    const title = ref('')
    const desc = ref('')

    const load = async () => {
        loading.value = true
        error.value = null
        try {
            tasks.value = await getTasks(filter.value || undefined)
        } catch (e: any) {
            error.value = e?.message || 'Failed to load'
        } finally {
            loading.value = false
        }
    }

    const setFilter = (f: '' | 'completed' | 'pending') => {
        filter.value = f
        load()
    }

    const onCreate = async () => {
        if (!title.value.trim()) return
        busy.value = true
        error.value = null
        try {
            await createTask(title.value.trim(), desc.value.trim() || undefined)
            title.value = ''
            desc.value = ''
            await load()
        } catch (e: any) {
            error.value = e?.message || 'Create failed'
        } finally {
            busy.value = false
        }
    }

    const toggle = async (t: TaskItem) => {
        busy.value = true
        error.value = null
        try {
            await updateTask(t.id, {
                ...t,
                isCompleted: !t.isCompleted,
                title: t.title,
                description: t.description
            })
            await load()
        } catch (e: any) {
            error.value = e?.message || 'Update failed'
        } finally {
            busy.value = false
        }
    }

    const removeTask = async (id: string) => {
        if (!confirm('Delete this task?')) return
        busy.value = true
        error.value = null
        try {
            await deleteTask(id)
            await load()
        } catch (e: any) {
            error.value = e?.message || 'Delete failed'
        } finally {
            busy.value = false
        }
    }

    const formatDate = (dateString: string): string =>
        new Date(dateString).toLocaleString()

    onMounted(load)
</script>

<style scoped>
    /* subtle entry animation for cards */
    @keyframes fadeUp {
        0% {
            opacity: 0;
            transform: translateY(8px);
        }

        100% {
            opacity: 1;
            transform: translateY(0);
        }
    }

    Card {
        animation: fadeUp 0.3s ease both;
    }
</style>
