import { DateTime } from 'luxon'
import { Tag } from './Tag'

export declare class MenuItem {
  dishId: string
  dishName: string
  recipeId?: string
  recipeName?: string
  ordering: number
}

export declare class Dinner {
  description: string
  date: DateTime
  menu: MenuItem[]
  tags: Tag[]
  isPlanned: boolean
}
