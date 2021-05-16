import { NuxtAxiosInstance } from '@nuxtjs/axios'
import DinnerRepository from './dinner-repository'
import DishesRepository from './dishes-repository'
import FamilyRepository from './family-repository'

export default class Repositories {
  dishes: DishesRepository
  dinners: DinnerRepository
  families: FamilyRepository

  constructor($axios: NuxtAxiosInstance) {
    this.dishes = new DishesRepository($axios)
    this.dinners = new DinnerRepository($axios)
    this.families = new FamilyRepository($axios)
  }
}
