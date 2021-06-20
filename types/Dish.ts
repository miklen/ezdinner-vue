import { DateTime } from 'luxon'
import { Tag } from './Tag'

export declare class Recipe {
  id: string
  name: string
  rl: string
  tags: Tag[]
  notes: string
}

export declare class Dish {
  name: string
  id: string
  recipeId: string
  recipeName: string
}

export declare class DishStats {
  dishId: string
  lastUsed: DateTime | undefined
  timesUsed: number
}
