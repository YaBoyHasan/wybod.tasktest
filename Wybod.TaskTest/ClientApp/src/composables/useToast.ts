import { ref } from 'vue'

export type ToastType = 'success' | 'error' | 'info'

export interface Toast {
    id: number
    message: string
    type: ToastType
}

const toasts = ref<Toast[]>([])
let nextId = 0

export function useToast() {
    const show = (message: string, type: ToastType = 'info', duration = 3000) => {
        const id = nextId++
        const toast: Toast = { id, message, type }

        toasts.value.push(toast)

        setTimeout(() => {
            remove(id)
        }, duration)

        return id
    }

    const success = (message: string, duration = 3000) => {
        return show(message, 'success', duration)
    }

    const error = (message: string, duration = 4000) => {
        return show(message, 'error', duration)
    }

    const info = (message: string, duration = 3000) => {
        return show(message, 'info', duration)
    }

    const remove = (id: number) => {
        const index = toasts.value.findIndex(t => t.id === id)
        if (index > -1) {
            toasts.value.splice(index, 1)
        }
    }

    return {
        toasts,
        show,
        success,
        error,
        info,
        remove
    }
}
