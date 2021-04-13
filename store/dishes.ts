import { getterTree, mutationTree, actionTree } from 'typed-vuex'
import { Dish } from '@/types/Dish'

export const state = () => ({
  dishes: [] as Dish[],
})

export type DishesState = ReturnType<typeof state>

export const getters = getterTree(state, {})

export const mutations = mutationTree(state, {
  updateDishes(state, Dishes: Dish[]) {
    state.dishes = Dishes
  },
})

export const actions = actionTree(
  { state, getters, mutations },
  {
    async getDishes({ commit }) {
      const result = await this.$axios.get('/api/dishes/family/{}')
      commit('updateDishes', result.data)
    },
  },
)
