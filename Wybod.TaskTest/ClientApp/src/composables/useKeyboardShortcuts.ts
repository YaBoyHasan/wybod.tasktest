import { onMounted, onUnmounted } from 'vue'

export type ShortcutHandler = () => void

export interface ShortcutConfig {
    key: string
    ctrl?: boolean
    shift?: boolean
    alt?: boolean
    handler: ShortcutHandler
    description?: string
}

export function useKeyboardShortcuts(shortcuts: ShortcutConfig[]) {
    const handleKeyDown = (event: KeyboardEvent) => {
        for (const shortcut of shortcuts) {
            const keyMatch = event.key.toLowerCase() === shortcut.key.toLowerCase()
            const ctrlMatch = !!shortcut.ctrl === (event.ctrlKey || event.metaKey)
            const shiftMatch = !!shortcut.shift === event.shiftKey
            const altMatch = !!shortcut.alt === event.altKey

            if (keyMatch && ctrlMatch && shiftMatch && altMatch) {
                const target = event.target as HTMLElement
                const isInput = target.tagName === 'INPUT' || target.tagName === 'TEXTAREA'

                if (!isInput || (shortcut.ctrl || shortcut.alt)) {
                    event.preventDefault()
                    shortcut.handler()
                    return
                }
            }
        }
    }

    onMounted(() => {
        window.addEventListener('keydown', handleKeyDown)
    })

    onUnmounted(() => {
        window.removeEventListener('keydown', handleKeyDown)
    })

    return {
        shortcuts
    }
}
