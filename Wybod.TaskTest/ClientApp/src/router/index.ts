import { createRouter, createWebHistory } from 'vue-router'
import DefaultLayout from '@/layouts/DefaultLayout.vue'
import TasksView from '@/views/TasksView.vue'
import TaskDetailView from '@/views/TaskDetailView.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      component: DefaultLayout,
      children: [
        {
          path: '',
          name: 'tasks',
          component: TasksView
        },
        {
          path: '/task/:id',
          name: 'task-detail',
          component: TaskDetailView
        }
      ]
    }
  ]
})

export default router
