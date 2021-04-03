import {
  getAccessorType,
  getterTree,
  mutationTree,
  actionTree,
} from 'typed-vuex'
import * as families from './families'

export const state = () => ({})

export type RootState = ReturnType<typeof state>

export const getters = getterTree(state, {
  // Type-checked
  // email: state => (state.emails.length ? state.emails[0] : ''),
  // NOT type-checked
  // aDependentGetter: (_state, getters) => getters.email,
})

export const mutations = mutationTree(state, {
  //   setEmail(state, newValue: string) {
  //     state.email = newValue
  //   },
  //   initialiseStore() {
  //     console.log('initialised')
  //   },
})

export const actions = actionTree(
  { state, getters, mutations },
  {
    //     async resetEmail({ commit, dispatch, getters, state }) {
    //       // Typed
    //       commit('initialiseStore')
    //       let a = getters.email
    //       let b = state._email
    //       // Not typed
    //       dispatch('resetEmail')
    //       // Typed
    //       this.app.$accessor.resetEmail()
  },
)

export const accessorType = getAccessorType({
  // state,
  getters,
  mutations,
  actions,
  modules: {
    // The key (submodule) needs to match the Nuxt namespace (e.g. ~/store/submodule.ts)
    families,
  },
})
