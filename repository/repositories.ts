import { NuxtAxiosInstance } from '@nuxtjs/axios'
import DinnerRepository from './dinner-repository'
import DishesRepository from './dishes-repository'

export default class Repositories {
  dishes: DishesRepository
  dinners: DinnerRepository

  constructor($axios: NuxtAxiosInstance) {
    this.dishes = new DishesRepository($axios)
    this.dinners = new DinnerRepository($axios)
  }
}
