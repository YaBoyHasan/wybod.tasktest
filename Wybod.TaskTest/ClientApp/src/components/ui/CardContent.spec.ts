import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import CardContent from './CardContent.vue'

describe('CardContent', () => {
  it('renders slot content', () => {
    const wrapper = mount(CardContent, {
      slots: {
        default: 'Content text'
      }
    })
    expect(wrapper.text()).toContain('Content text')
  })

  it('applies custom class', () => {
    const wrapper = mount(CardContent, {
      props: {
        class: 'custom-content'
      }
    })
    expect(wrapper.classes()).toContain('custom-content')
  })

  it('has default content styling', () => {
    const wrapper = mount(CardContent)
    expect(wrapper.classes()).toContain('p-6')
    expect(wrapper.classes()).toContain('pt-0')
  })
})
