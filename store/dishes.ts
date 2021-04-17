import { getterTree, mutationTree, actionTree } from 'typed-vuex'
import { Dish } from '@/types/Dish'

export const state = () => ({
  dishes: [] as Dish[],
})

export type DishesState = ReturnType<typeof state>

export const getters = getterTree(state, {})

export const mutations = mutationTree(state, {
  updateDishes(state, dishes: Dish[]) {
    state.dishes = dishes
  },
})

export const actions = actionTree(
  { state, getters, mutations },
  {
    async populateDishes({ commit, rootState }) {
      const result = await this.$axios.get(
        `/api/dishes/family/${rootState.activeFamilyId}`,
      )
      commit('updateDishes', result.data)
    },
  },
)
