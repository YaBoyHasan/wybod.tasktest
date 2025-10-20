<template>
    <div class="fixed top-4 right-4 z-50 space-y-2 max-w-md">
        <transition-group name="toast">
            <div v-for="toast in toasts"
                 :key="toast.id"
                 :class="[
                     'px-6 py-4 rounded-lg shadow-lg text-white font-medium flex items-center gap-3',
                     'animate-slide-in',
                     toastClass(toast.type)
                 ]">
                <span class="text-xl">{{ toastIcon(toast.type) }}</span>
                <span class="flex-1">{{ toast.message }}</span>
                <button @click="remove(toast.id)"
                        class="text-white hover:text-gray-200 transition">
                    ✕
                </button>
            </div>
        </transition-group>
    </div>
</template>

<script setup lang="ts">
import { useToast, type ToastType } from '@/composables/useToast'

const { toasts, remove } = useToast()

const toastClass = (type: ToastType): string => {
    const classes = {
        success: 'bg-green-600',
        error: 'bg-red-600',
        info: 'bg-blue-600'
    }
    return classes[type]
}

const toastIcon = (type: ToastType): string => {
    const icons = {
        success: '✓',
        error: '✕',
        info: 'ℹ'
    }
    return icons[type]
}
</script>

<style scoped>
@keyframes slide-in {
    from {
        transform: translateX(100%);
        opacity: 0;
    }
    to {
        transform: translateX(0);
        opacity: 1;
    }
}

.animate-slide-in {
    animation: slide-in 0.3s ease-out;
}

.toast-enter-active {
    animation: slide-in 0.3s ease-out;
}

.toast-leave-active {
    transition: all 0.3s ease-in;
}

.toast-leave-to {
    transform: translateX(100%);
    opacity: 0;
}
</style>
