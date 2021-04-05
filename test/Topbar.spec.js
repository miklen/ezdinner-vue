import { mount } from '@vue/test-utils'
import Topbar from '@/components/Topbar.vue'

describe('Topbar', () => {
  test('is a Vue instance', () => {
    const wrapper = mount(Topbar)
    expect(wrapper.vm).toBeTruthy()
  })
})
