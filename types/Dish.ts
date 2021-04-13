import { Tag } from './Tag'

export declare class Recipe {
  id: string
  name: string
  rl: string
  tags: Tag[]
  notes: string
}

export declare class Dish {
  id: string
  familyId: string
  recipes: Recipe[]
}
