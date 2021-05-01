import { getterTree, mutationTree, actionTree } from 'typed-vuex'
import { Dinner } from '@/types/Dinner'
import { DateTime } from 'luxon'

export const state = () => ({
  dinners: [] as Dinner[],
})

export type dinnerState = ReturnType<typeof state>

export const getters = getterTree(state, {})

export const mutations = mutationTree(state, {
  updateDinner(state, dinner: Dinner[]) {
    state.dinners = dinner
  },
})

export const actions = actionTree(
  { state, getters, mutations },
  {
    async populateDinner(
      { commit, rootState },
      { from, to }: { from: DateTime; to: DateTime },
    ) {
      const result = await this.$repositories.dinners.getRange(
        rootState.activeFamilyId,
        from,
        to,
      )

      // runtime join - consider if this is done better elsewhere
      const transformedDinner: Dinner[] = result.map((dinner: any) => {
        dinner.date = DateTime.fromISO(dinner.date)
        dinner.menu.map((item: any) => {
          item.dishName = this.app.$accessor.dishes.dishMap[item.dishId]
          // TODO: add receipeName
          return item
        })
        return dinner
      })

      commit('updateDinner', transformedDinner)
    },
  },
)
