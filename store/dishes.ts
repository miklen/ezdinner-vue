import { getterTree, mutationTree, actionTree } from 'typed-vuex'
import { Dish } from '@/types/Dish'

export const state = () => ({
  dishes: [] as Dish[],
})

export type DishesState = ReturnType<typeof state>

export const getters = getterTree(state, {
  dishMap(state): { [key: string]: string } {
    return state.dishes.reduce((prev, current) => {
      prev[current.id] = current.name
      return prev
    }, {} as { [key: string]: string })
  },
})

export const mutations = mutationTree(state, {
  updateDishes(state, dishes: Dish[]) {
    state.dishes = dishes
  },
})

export const actions = actionTree(
  { state, getters, mutations },
  {
    async populateDishes({ commit, rootState }) {
      const result = await this.$repositories.dishes.all(
        rootState.activeFamilyId,
      )
      commit('updateDishes', result)
    },
  },
)
