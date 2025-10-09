import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import CardDescription from './CardDescription.vue'

describe('CardDescription', () => {
  it('renders slot content', () => {
    const wrapper = mount(CardDescription, {
      slots: {
        default: 'Description text'
      }
    })
    expect(wrapper.text()).toContain('Description text')
  })

  it('applies custom class', () => {
    const wrapper = mount(CardDescription, {
      props: {
        class: 'custom-description'
      }
    })
    expect(wrapper.classes()).toContain('custom-description')
  })

  it('renders as p element', () => {
    const wrapper = mount(CardDescription)
    expect(wrapper.element.tagName).toBe('P')
  })

  it('has default description styling', () => {
    const wrapper = mount(CardDescription)
    expect(wrapper.classes()).toContain('text-sm')
    expect(wrapper.classes()).toContain('text-muted-foreground')
  })
})
