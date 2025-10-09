import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import CardHeader from './CardHeader.vue'

describe('CardHeader', () => {
  it('renders slot content', () => {
    const wrapper = mount(CardHeader, {
      slots: {
        default: 'Header content'
      }
    })
    expect(wrapper.text()).toContain('Header content')
  })

  it('applies custom class', () => {
    const wrapper = mount(CardHeader, {
      props: {
        class: 'custom-header'
      }
    })
    expect(wrapper.classes()).toContain('custom-header')
  })

  it('has default header styling', () => {
    const wrapper = mount(CardHeader)
    expect(wrapper.classes()).toContain('flex')
    expect(wrapper.classes()).toContain('flex-col')
    expect(wrapper.classes()).toContain('p-6')
  })
})
