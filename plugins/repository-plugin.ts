import { NuxtApp } from '@nuxt/types/app'
import Repositories from '../repository/repositories'

export default ({ app }: { app: NuxtApp }, inject: any) => {
  inject('repositories', new Repositories(app.$axios))
}
