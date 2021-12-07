import { NuxtAxiosInstance } from '@nuxtjs/axios'
import { DateTime } from 'luxon'
import { Dish, DishStats } from '~/types/Dish'

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

  /**
   * Gets a dishs
   * @param dishId
   * @returns dishes available
   */
  async get(dishId: string) {
    return (await this.$axios.$get(`api/dishes/${dishId}`)) as Dish
  }

  /**
   * Gets a dishs with full information
   * @param dishId
   * @returns dishes available
   */
  async getFull(dishId: string, familyId: string) {
    return (await this.$axios.$get(
      `api/dishes/${dishId}/full/family/${familyId}`,
    )) as Dish
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

  async allUsageStats(
    activeFamilyId: string,
  ): Promise<{ [key: string]: DishStats }> {
    const result = await this.$axios.$get(
      `api/dishes/stats/family/${activeFamilyId}`,
    )
    Object.keys(result).forEach((i: any) => {
      result[i].lastUsed = DateTime.fromISO(result[i].lastUsed)
    })
    return result
  }

  updateName(dishId: string, newName: string) {
    return this.$axios.$put(`api/dishes/${dishId}/name/`, { name: newName })
  }

  updateRating(dishId: string, newRating: number, familyMemberId: string) {
    return this.$axios.$put(`api/dishes/${dishId}/rating`, {
      rating: newRating,
      familyMemberId,
    })
  }

  updateNotes(dishId: string, notes: string, url: string) {
    return this.$axios.$put(`api/dishes/${dishId}/notes`, {
      notes,
      url,
    })
  }
}
