import { DateTime } from 'luxon'
import { Tag } from './Tag'

export declare class Dish {
  name: string
  id: string
  url: string
  tags: Tag[]
  notes: string
}

export declare class DishStats {
  dishId: string
  lastUsed: DateTime | undefined
  timesUsed: number
}
