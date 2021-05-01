import { NuxtAxiosInstance } from '@nuxtjs/axios'
import { DateTime } from 'luxon'

export default class DinnerRepository {
  $axios: NuxtAxiosInstance

  constructor($axios: NuxtAxiosInstance) {
    this.$axios = $axios
  }

  async getRange(familyId: number, from: DateTime, to: DateTime) {
    const result = await this.$axios.get(
      `api/dinners/family/${familyId}/dates/${from.toISODate()}/${to.toISODate()}`,
    )
    return result.data
  }

  async get(familyId: string, exactDate: DateTime) {
    return (
      await this.$axios.get(
        `api/dinners/family/${familyId}/date/${exactDate.toISODate()}`,
      )
    ).data
  }

  addDishToMenu(
    familyId: string,
    date: DateTime,
    dishId: string,
    receipeId: string | null,
  ) {
    return this.$axios.put('api/dinners/menuitem', {
      date,
      dishId,
      receipeId,
      familyId,
    })
  }
}
