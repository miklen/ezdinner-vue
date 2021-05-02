import { getterTree, mutationTree, actionTree } from 'typed-vuex'
import { Family } from '@/types/Family'

export const state = () => ({
  families: [] as Family[],
})

export type FamiliesState = ReturnType<typeof state>

export const getters = getterTree(state, {})

export const mutations = mutationTree(state, {
  updateFamilies(state, families: Family[]) {
    state.families = families
  },
})

export const actions = actionTree(
  { state, getters, mutations },
  {
    async getFamilies({ commit, state, rootState }) {
      const result = await this.$axios.get('/api/families')
      commit('updateFamilies', result.data)
      if (!rootState.activeFamilyId && state.families.length > 0) {
        this.app.$accessor.setActiveFamilyId(state.families[0].id)
      }
      // explicit return null to satisfy typings
      return null
    },
  },
)
