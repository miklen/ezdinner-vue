import { NuxtAxiosInstance } from '@nuxtjs/axios'
import { Dish } from '~/types/Dish'

export default class DishesRepository {
  $axios: NuxtAxiosInstance

  constructor($axios: NuxtAxiosInstance) {
    this.$axios = $axios
  }

  async all(activeFamilyId: string) {
    return (await this.$axios.$get(
      `/api/dishes/family/${activeFamilyId}`,
    )) as Dish[]
  }

  create(familyId: string, dishName: string) {
    return this.$axios.$post('api/dishes', {
      name: dishName,
      familyId,
    })
  }

  delete(familyId: string, dishId: string) {
    return this.$axios.$delete(`/api/dishes/family/${familyId}/id/${dishId}`)
  }
}
