import { NuxtAxiosInstance } from '@nuxtjs/axios'
import { Dish } from '~/types/Dish'

export default class DishesRepository {
  $axios: NuxtAxiosInstance

  constructor($axios: NuxtAxiosInstance) {
    this.$axios = $axios
  }

  async all(activeFamilyId: string) {
    return (await this.$axios.get(`/api/dishes/family/${activeFamilyId}`))
      .data as Dish[]
  }
}
