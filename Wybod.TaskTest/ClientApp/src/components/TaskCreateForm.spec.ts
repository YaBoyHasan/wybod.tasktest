import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import TaskCreateForm from './TaskCreateForm.vue'
import { TaskPriority } from '@/api'

describe('TaskCreateForm', () => {
    it('renders form with all fields', () => {
        const wrapper = mount(TaskCreateForm, {
            props: { busy: false }
        })

        expect(wrapper.find('input[type="text"]').exists()).toBe(true)
        expect(wrapper.find('textarea').exists()).toBe(true)
        expect(wrapper.find('input[type="datetime-local"]').exists()).toBe(true)
        expect(wrapper.find('select').exists()).toBe(true)
    })

    it('emits create event with form data', async () => {
        const wrapper = mount(TaskCreateForm, {
            props: { busy: false }
        })

        await wrapper.find('input[type="text"]').setValue('New Task')
        await wrapper.find('textarea').setValue('Task Description')
        await wrapper.find('form').trigger('submit')

        expect(wrapper.emitted('create')).toBeTruthy()
        const emitted = wrapper.emitted('create') as any[]
        expect(emitted[0][0].title).toBe('New Task')
        expect(emitted[0][0].description).toBe('Task Description')
    })

    it('includes tags when provided', async () => {
        const wrapper = mount(TaskCreateForm, {
            props: { busy: false }
        })

        await wrapper.find('input[type="text"]').setValue('Task with tags')
        const tagInputs = wrapper.findAll('input[type="text"]')
        await tagInputs[1].setValue('urgent, work, important')
        await wrapper.find('form').trigger('submit')

        const emitted = wrapper.emitted('create') as any[]
        expect(emitted[0][0].tags).toEqual(['urgent', 'work', 'important'])
    })

    it('clears form after submission', async () => {
        const wrapper = mount(TaskCreateForm, {
            props: { busy: false }
        })

        const titleInput = wrapper.find('input[type="text"]')
        await titleInput.setValue('Test Task')
        await wrapper.find('form').trigger('submit')

        expect((titleInput.element as HTMLInputElement).value).toBe('')
    })

    it('does not submit when title is empty', async () => {
        const wrapper = mount(TaskCreateForm, {
            props: { busy: false }
        })

        await wrapper.find('form').trigger('submit')

        expect(wrapper.emitted('create')).toBeFalsy()
    })

    it('shows clear button when form has data', async () => {
        const wrapper = mount(TaskCreateForm, {
            props: { busy: false }
        })

        expect(wrapper.text()).not.toContain('Clear')

        await wrapper.find('input[type="text"]').setValue('Test')

        await wrapper.vm.$nextTick()
        expect(wrapper.text()).toContain('Clear')
    })

    it('sets default priority to Medium', async () => {
        const wrapper = mount(TaskCreateForm, {
            props: { busy: false }
        })

        await wrapper.find('input[type="text"]').setValue('Test')
        await wrapper.find('form').trigger('submit')

        const emitted = wrapper.emitted('create') as any[]
        expect(emitted[0][0].priority).toBe(TaskPriority.Medium)
    })
})
