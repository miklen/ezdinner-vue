import { NuxtAxiosInstance } from '@nuxtjs/axios'
import { FamilySelect } from '~/types/FamilySelect'
import { Family } from '~/types/Family'

export default class FamilyRepository {
  $axios: NuxtAxiosInstance

  constructor($axios: NuxtAxiosInstance) {
    this.$axios = $axios
  }

  async all() {
    return (await this.$axios.get(`/api/families/`)).data as Family[]
  }

  async get(familyId: string) {
    return (await this.$axios.get(`/api/families/${familyId}`)).data as Family
  }

  async familySelectors() {
    return (await this.$axios.get(`/api/families/select`))
      .data as FamilySelect[]
  }

  async inviteFamilyMember(familyId: string, email: string) {
    const result = await this.$axios.post(`api/family/${familyId}/member`, {
      email,
    })
    // 200 OK means user exists and has been invited and returns true
    // 204 No Content means user was not found and has not been invited and returns false
    return result.status === 200
  }

  async createFamily(familyName: string) {
    const result = await this.$axios.post('api/families', {
      name: familyName,
    })
    return result.status === 200 || result.status === 204
  }
}
