import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import CardTitle from './CardTitle.vue'

describe('CardTitle', () => {
  it('renders slot content', () => {
    const wrapper = mount(CardTitle, {
      slots: {
        default: 'Title text'
      }
    })
    expect(wrapper.text()).toContain('Title text')
  })

  it('applies custom class', () => {
    const wrapper = mount(CardTitle, {
      props: {
        class: 'custom-title'
      }
    })
    expect(wrapper.classes()).toContain('custom-title')
  })

  it('renders as h3 element', () => {
    const wrapper = mount(CardTitle)
    expect(wrapper.element.tagName).toBe('H3')
  })

  it('has default title styling', () => {
    const wrapper = mount(CardTitle)
    expect(wrapper.classes()).toContain('text-2xl')
    expect(wrapper.classes()).toContain('font-semibold')
  })
})
