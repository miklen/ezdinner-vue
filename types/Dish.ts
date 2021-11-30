import { DateTime } from 'luxon'
import { Tag } from './Tag'

export declare class DinnerDate {
  date: string
  daysSinceLast: number
}

export declare class Dish {
  name: string
  id: string
  url: string
  tags: Tag[]
  notes: string
  rating: number
  dates: DinnerDate[]
}

export declare class DishStats {
  dishId: string
  lastUsed: DateTime | undefined
  timesUsed: number
}
