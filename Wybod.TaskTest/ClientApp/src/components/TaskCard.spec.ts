import { describe, it, expect, vi } from 'vitest'
import { mount } from '@vue/test-utils'
import TaskCard from './TaskCard.vue'
import { TaskPriority, type TaskItem } from '@/api'

describe('TaskCard', () => {
    const mockTask: TaskItem = {
        id: '123',
        title: 'Test Task',
        description: 'Test Description',
        isCompleted: false,
        createdAt: new Date().toISOString(),
        priority: TaskPriority.Medium,
        tags: ['test'],
        completedAt: null,
        dueDate: null
    }

    it('renders task title and description', () => {
        const wrapper = mount(TaskCard, {
            props: { task: mockTask, busy: false }
        })

        expect(wrapper.text()).toContain('Test Task')
        expect(wrapper.text()).toContain('Test Description')
    })

    it('shows pending status for incomplete task', () => {
        const wrapper = mount(TaskCard, {
            props: { task: mockTask, busy: false }
        })

        expect(wrapper.text()).toContain('Pending')
    })

    it('shows completed status for completed task', () => {
        const completedTask = { ...mockTask, isCompleted: true }
        const wrapper = mount(TaskCard, {
            props: { task: completedTask, busy: false }
        })

        expect(wrapper.text()).toContain('Completed')
    })

    it('emits toggle event when toggle button is clicked', async () => {
        const wrapper = mount(TaskCard, {
            props: { task: mockTask, busy: false }
        })

        const toggleButton = wrapper.findAll('button').find(b => b.text().includes('Complete'))
        await toggleButton?.trigger('click')

        expect(wrapper.emitted('toggle')).toBeTruthy()
    })

    it('emits delete event when delete button is clicked', async () => {
        const wrapper = mount(TaskCard, {
            props: { task: mockTask, busy: false }
        })

        const deleteButton = wrapper.findAll('button').find(b => b.text() === 'Delete')
        await deleteButton?.trigger('click')

        expect(wrapper.emitted('delete')).toBeTruthy()
    })

    it('emits click event when card is clicked', async () => {
        const wrapper = mount(TaskCard, {
            props: { task: mockTask, busy: false }
        })

        await wrapper.trigger('click')

        expect(wrapper.emitted('click')).toBeTruthy()
    })

    it('displays priority label', () => {
        const wrapper = mount(TaskCard, {
            props: { task: mockTask, busy: false }
        })

        expect(wrapper.text()).toContain('Medium')
    })

    it('displays high priority with correct styling', () => {
        const highPriorityTask = { ...mockTask, priority: TaskPriority.High }
        const wrapper = mount(TaskCard, {
            props: { task: highPriorityTask, busy: false }
        })

        expect(wrapper.text()).toContain('High')
    })

    it('displays tag count when tags exist', () => {
        const taskWithTags = { ...mockTask, tags: ['urgent', 'work', 'personal'] }
        const wrapper = mount(TaskCard, {
            props: { task: taskWithTags, busy: false }
        })

        expect(wrapper.text()).toContain('3 tags')
    })

    it('shows overdue indicator for overdue tasks', () => {
        const overdueTask = {
            ...mockTask,
            dueDate: new Date(Date.now() - 86400000).toISOString(),
            isOverdue: true
        }
        const wrapper = mount(TaskCard, {
            props: { task: overdueTask, busy: false }
        })

        expect(wrapper.html()).toContain('bg-red-100')
    })

    it('disables buttons when busy', () => {
        const wrapper = mount(TaskCard, {
            props: { task: mockTask, busy: true }
        })

        const buttons = wrapper.findAll('button')
        buttons.forEach(button => {
            expect(button.attributes('disabled')).toBeDefined()
        })
    })
})
