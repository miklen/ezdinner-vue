import { NuxtApp } from '@nuxt/types/app'
import Vue from 'vue'
import { MsalPlugin, MsalPluginOptions } from './msal'

export default ({ app }: { app: NuxtApp }, inject: any) => {
  const options: MsalPluginOptions = {
    clientId: app.$config.clientId,
    loginAuthority: app.$config.loginAuthority,
    passwordAuthority: app.$config.passwordAuthority,
    knownAuthority: app.$config.knownAuthority,
    // this is required for the signin to work from any subroute
    redirectUri: app.$config.browserBaseURL,
  }

  const msal = new MsalPlugin()

  Vue.use(msal, options)

  // Vue.observable is required for isAuthenticated to be reactive in components
  inject('msal', Vue.observable(msal))
}
