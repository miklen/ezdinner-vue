import { NuxtAxiosInstance } from '@nuxtjs/axios/types'
import { MsalPlugin } from './msal'

export default ({ app, $axios, $msal } : { app: any, $axios: any, $msal: MsalPlugin }) => {
    app.$axios.onRequest(async (config: any) => {
        if (!$msal.isAuthenticated) return config
        const token = await $msal.acquireToken()
        config.headers.common['Authorization'] = `Bearer ${token}`
        return config
    })
}