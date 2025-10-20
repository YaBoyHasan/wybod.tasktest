<template>
  <div class="min-h-screen bg-gray-50">
    <nav class="bg-white shadow-sm border-b">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="flex justify-between h-16">
          <div class="flex items-center">
            <router-link to="/" class="text-xl font-bold text-gray-900 hover:text-gray-700 transition">
              Task Manager
            </router-link>
          </div>
          <div class="flex items-center space-x-4">
            <span class="text-sm text-gray-500">Ctrl+N: New Task • /: Search</span>
          </div>
        </div>
      </div>
    </nav>
    <main>
      <router-view />
    </main>
    <ToastContainer />
  </div>
</template>

<script setup lang="ts">
import ToastContainer from '@/components/ToastContainer.vue'
</script>
