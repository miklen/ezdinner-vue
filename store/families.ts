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
    async getFamilies({ commit }) {
      const result = await this.$axios.get('/api/families')
      console.log(result)
      commit('updateFamilies', result.data)
    },
  },
)
