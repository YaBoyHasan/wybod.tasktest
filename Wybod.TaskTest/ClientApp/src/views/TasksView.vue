<template>
  <div>
    <div v-if="loading" class="text-center py-8 text-lg text-gray-600">
      Loading tasks...
    </div>

    <div v-else-if="error" class="text-center py-8 text-lg text-red-600">
      {{ error }}
    </div>

    <div v-else>
      <div v-if="tasks.length === 0" class="text-center py-8 text-lg text-gray-500">
        No tasks yet
      </div>

      <div v-else class="space-y-4">
        <Card v-for="task in tasks" :key="task.id">
          <CardHeader>
            <div class="flex justify-between items-center">
              <CardTitle>{{ task.title }}</CardTitle>
              <span
                :class="[
                  'px-3 py-1 rounded-full text-xs font-bold',
                  task.isCompleted ? 'bg-green-500 text-white' : 'bg-amber-500 text-white'
                ]"
              >
                {{ task.isCompleted ? 'Completed' : 'Pending' }}
              </span>
            </div>
            <CardDescription>{{ task.description }}</CardDescription>
          </CardHeader>
          <CardContent>
            <div class="flex gap-5 text-xs text-gray-500">
              <small>Created: {{ formatDate(task.createdAt) }}</small>
              <small v-if="task.completedAt">Completed: {{ formatDate(task.completedAt) }}</small>
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import Card from '@/components/ui/Card.vue'
import CardHeader from '@/components/ui/CardHeader.vue'
import CardTitle from '@/components/ui/CardTitle.vue'
import CardDescription from '@/components/ui/CardDescription.vue'
import CardContent from '@/components/ui/CardContent.vue'

interface TaskItem {
  id: string
  title: string
  description: string
  isCompleted: boolean
  createdAt: string
  completedAt?: string
}

const tasks = ref<TaskItem[]>([])
const loading = ref(true)
const error = ref<string | null>(null)

const fetchTasks = async () => {
  try {
    const response = await fetch('/api/tasks')
    if (!response.ok) {
      throw new Error('Failed to fetch tasks')
    }
    tasks.value = await response.json()
  } catch (err) {
    error.value = (err as Error).message
  } finally {
    loading.value = false
  }
}

const formatDate = (dateString: string): string => {
  return new Date(dateString).toLocaleString()
}

onMounted(() => {
  fetchTasks()
})
</script>
