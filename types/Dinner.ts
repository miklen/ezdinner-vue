import { DateTime } from 'luxon'
import { Tag } from './Tag'

export declare class MenuItem {
  dishId: string
  receipeId?: string
  ordering: number
}

export declare class Dinner {
  description: string
  date: DateTime
  menu: MenuItem[]
  tags: Tag[]
  isPlanned: boolean
}
