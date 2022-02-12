import { NuxtAxiosInstance } from '@nuxtjs/axios'
import { DateTime } from 'luxon'
import { Dinner } from '~/types/Dinner'

export default class DinnerRepository {
  $axios: NuxtAxiosInstance

  constructor($axios: NuxtAxiosInstance) {
    this.$axios = $axios
  }

  getRange(familyId: string, from: DateTime, to: DateTime): Promise<Dinner[]> {
    return this.$axios.$get(
      `api/dinners/family/${familyId}/dates/${from.toISODate()}/${to.toISODate()}`,
    )
  }

  get(familyId: string, exactDate: DateTime) {
    return this.$axios.$get(
      `api/dinners/family/${familyId}/date/${exactDate.toISODate()}`,
    )
  }

  addDishToMenu(familyId: string, date: DateTime, dishId: string) {
    return this.$axios.put('api/dinners/menuitem', {
      date: date.toISODate(),
      dishId,
      familyId,
    })
  }

  removeDishFromMenu(familyId: string, date: DateTime, dishId: string) {
    return this.$axios.put('api/dinners/menuitem/remove', {
      date: date.toISODate(),
      dishId,
      familyId,
    })
  }

  moveDinnerDishes(familyId: string, dishId: string, newDishId: string) {
    return this.$axios.put('api/dinners/menuitem/replace', {
      familyId,
      dishId,
      newDishId,
    })
  }
}
