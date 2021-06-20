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

    async updateDish({ commit, state }, { dishId }: { dishId: string }) {
      const dishRecipes = await this.$repositories.dishes.get(dishId)
      const dishes = [...state.dishes]
      for (const dish of dishRecipes) {
        const index = dishes.findIndex(
          (f) => f.id === dish.id && f.recipeId === dish.recipeId,
        )
        if (index === -1) {
          dishes.push(dish)
        } else {
          dishes[index] = dish
        }
      }

      commit('updateDishes', dishes)
    },
  },
)
