import {
  getAccessorType,
  getterTree,
  mutationTree,
  actionTree,
} from 'typed-vuex'
import * as families from './families'
import * as dishes from './dishes'

// Typings:
// https://typed-vuex.roe.dev/

export const state = () => ({
  activeFamilyId: (null as unknown) as string,
})

export type RootState = ReturnType<typeof state>

export const getters = getterTree(state, {})

export const mutations = mutationTree(state, {
  updateFamilyId(state, familyId: string) {
    state.activeFamilyId = familyId
  },
})

export const actions = actionTree(
  { state, getters, mutations },
  {
    setActiveFamilyId({ commit }, familyId: string) {
      commit('updateFamilyId', familyId)
    },
  },
)

export const accessorType = getAccessorType({
  state,
  getters,
  mutations,
  actions,
  modules: {
    // The key (submodule) needs to match the Nuxt namespace (e.g. ~/store/submodule.ts)
    families,
    dishes,
  },
})
