import { getterTree, mutationTree, actionTree } from 'typed-vuex'
import { Family } from '~/types/Family'
import { FamilySelect } from '~/types/FamilySelect'

export const state = () => ({
  familySelectors: [] as FamilySelect[],
  activeFamily: {} as Family,
})

export type familySelectorsState = ReturnType<typeof state>

export const getters = getterTree(state, {})

export const mutations = mutationTree(state, {
  updateFamilySelectors(state, familySelectors: FamilySelect[]) {
    state.familySelectors = familySelectors
  },
  updateActiveFamily(state, family: Family) {
    state.activeFamily = family
  },
})

export const actions = actionTree(
  { state, getters, mutations },
  {
    async getFamilySelectors({ commit, state, rootState }) {
      const result = await this.$repositories.families.familySelectors()
      commit('updateFamilySelectors', result)
      if (!rootState.activeFamilyId && state.familySelectors.length > 0) {
        this.app.$accessor.setActiveFamilyId(state.familySelectors[0].id)
      }
      // explicit return null to satisfy typings
      return null
    },
    async getActiveFamily({ commit, rootState }) {
      const result = await this.$repositories.families.get(
        rootState.activeFamilyId,
      )
      commit('updateActiveFamily', result)
      // explicit return null to satisfy typings
      return null
    },
  },
)
