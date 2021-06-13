import { NuxtAxiosInstance } from '@nuxtjs/axios'
import { DateTime } from 'luxon'

export default class DinnerRepository {
  $axios: NuxtAxiosInstance

  constructor($axios: NuxtAxiosInstance) {
    this.$axios = $axios
  }

  getRange(familyId: number, from: DateTime, to: DateTime) {
    return this.$axios.$get(
      `api/dinners/family/${familyId}/dates/${from.toISODate()}/${to.toISODate()}`,
    )
  }

  get(familyId: string, exactDate: DateTime) {
    return this.$axios.$get(
      `api/dinners/family/${familyId}/date/${exactDate.toISODate()}`,
    )
  }

  addDishToMenu(
    familyId: string,
    date: DateTime,
    dishId: string,
    recipeId: string | null,
  ) {
    return this.$axios.put('api/dinners/menuitem', {
      date: date.toISODate(),
      dishId,
      recipeId,
      familyId,
    })
  }

  removeDishFromMenu(
    familyId: string,
    date: DateTime,
    dishId: string,
    recipeId: string | null,
  ) {
    return this.$axios.put('api/dinners/menuitem/remove', {
      date: date.toISODate(),
      dishId,
      recipeId,
      familyId,
    })
  }
}
