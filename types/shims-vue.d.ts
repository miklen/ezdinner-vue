import Repositories from '../repository/repositories'
import { accessorType } from '~/store'

declare module 'vue/types/vue' {
  interface Vue {
    $accessor: typeof accessorType
    $vuetify: any
    $repositories: Repositories
  }
}

declare module '@nuxt/types' {
  interface NuxtAppOptions {
    $accessor: typeof accessorType
  }
}

declare module 'vuex/types/index' {
  // eslint-disable-next-line @typescript-eslint/no-unused-vars,no-unused-vars
  interface Store<S> {
    $repositories: Repositories
  }
}
