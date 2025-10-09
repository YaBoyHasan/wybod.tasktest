import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import Card from './Card.vue'

describe('Card', () => {
  it('renders slot content', () => {
    const wrapper = mount(Card, {
      slots: {
        default: 'Test content'
      }
    })
    expect(wrapper.text()).toContain('Test content')
  })

  it('applies custom class', () => {
    const wrapper = mount(Card, {
      props: {
        class: 'custom-class'
      }
    })
    expect(wrapper.classes()).toContain('custom-class')
  })

  it('has default card styling', () => {
    const wrapper = mount(Card)
    expect(wrapper.classes()).toContain('rounded-lg')
    expect(wrapper.classes()).toContain('border')
  })
})
