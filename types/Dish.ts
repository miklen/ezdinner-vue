import { Tag } from './Tag'

export declare class DinnerDate {
  date: string
  daysSinceLast: number
}

export declare class DishStats {
  dishId: string
  lastUsed: string | undefined
  timesUsed: number
}

export declare class Rating {
  familyMemberId: string
  rating: number
}

export declare class Dish {
  name: string
  id: string
  url: string
  tags: Tag[]
  notes: string
  rating: number
  dates: DinnerDate[]
  dishStats: DishStats
  ratings: Rating[]
}

export declare class DishSelector {
  name: string
  id: string
  url: string
  tags: Tag[]
  rating: number
}
