import { getterTree, mutationTree, actionTree } from 'typed-vuex'
import { Dish } from '@/types/Dish'

export const state = () => ({
  dishes: [] as Dish[],
  updating: null as null | Promise<Dish[]>,
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
  setUpdating(state, updating: null | Promise<Dish[]>) {
    state.updating = updating
  },
})

export const actions = actionTree(
  { state, getters, mutations },
  {
    async populateDishes({ commit, rootState, state }) {
      if (state.updating) return state.updating

      const resultPromise = this.$repositories.dishes.all(
        rootState.activeFamilyId,
      )
      commit('setUpdating', resultPromise)
      const result = await resultPromise
      commit('setUpdating', null)
      commit('updateDishes', result)
    },

    async updateDish({ commit, state }, { dishId }: { dishId: string }) {
      const dish = await this.$repositories.dishes.get(dishId)
      const dishes = [...state.dishes]
      const index = dishes.findIndex((f) => f.id === dish.id)
      if (index === -1) {
        dishes.push(dish)
      } else {
        dishes[index] = dish
      }

      commit('updateDishes', dishes)
    },
  },
)
