import { NuxtAxiosInstance } from '@nuxtjs/axios'
import { Family } from '~/types/Family'

export default class FamilyRepository {
  $axios: NuxtAxiosInstance

  constructor($axios: NuxtAxiosInstance) {
    this.$axios = $axios
  }

  async all() {
    return (await this.$axios.get(`/api/families/`)).data as Family[]
  }

  async inviteFamilyMember(familyId: string, email: string) {
    const result = await this.$axios.post(`api/family/${familyId}/member`, {
      email,
    })
    // 200 OK means user exists and has been invited and returns true
    // 204 No Content means user was not found and has not been invited and returns false
    return result.status === 200
  }
}
