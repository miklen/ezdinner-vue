<template>
  <v-row>
    <v-col
      v-for="family in families"
      :key="family.id"
      xs="12"
      sm="12"
      md="6"
      lg="4"
    >
      <v-card>
        <v-card-title>{{ family.name }}</v-card-title>
        <v-card-subtitle>Family members</v-card-subtitle>
        <v-card-text>
          <v-row v-for="familyMember in familyMembers" :key="familyMember">
            <v-col class="col-1">
              <v-icon>mdi-account</v-icon>
            </v-col>
            <v-col>{{ familyMember }}</v-col>
          </v-row>
        </v-card-text>

        <v-dialog v-model="inviteFamilyMemberDialog" width="500">
          <template #activator="{ on, attrs }">
            <v-card-actions
              ><v-btn
                text
                color="primary"
                v-bind="attrs"
                v-on="on"
                @click="openInviteFamilyMemberDialog(family.id)"
                >Invite family member</v-btn
              ></v-card-actions
            >
          </template>

          <v-card>
            <v-card-title class="headline grey lighten-2">
              Invite family member
            </v-card-title>
            <v-card-text>
              <v-text-field
                v-model="inviteFamilyMemberEmail"
                placeholder="Family member email address"
              ></v-text-field>
              <v-alert
                v-model="alert"
                dismissible
                color="error"
                border="left"
                elevation="2"
                colored-border
                icon="mdi-alert-decagram"
              >
                An error occured
              </v-alert>
            </v-card-text>

            <v-divider></v-divider>

            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="primary" text @click="inviteFamilyMember">
                Invite
              </v-btn>
              <v-btn
                color="primary"
                text
                @click="inviteFamilyMemberDialog = false"
              >
                Cancel
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-card>
    </v-col>
    <v-col xs="12" sm="12" md="6" lg="4">
      <v-card>
        <v-card-title>Create family</v-card-title>

        <!-- Refactor to component -->
        <v-dialog v-model="newFamilyDialog" width="500">
          <template #activator="{ on, attrs }">
            <v-card-actions
              ><v-btn text color="primary" v-bind="attrs" v-on="on"
                ><v-icon>mdi-account-multiple-plus</v-icon></v-btn
              ></v-card-actions
            >
          </template>

          <v-card>
            <v-card-title class="headline grey lighten-2">
              New family
            </v-card-title>
            <v-card-text>
              <v-text-field
                v-model="newFamilyName"
                placeholder="Family name"
              ></v-text-field>
              <v-alert
                v-model="alert"
                dismissible
                color="error"
                border="left"
                elevation="2"
                colored-border
                icon="mdi-alert-decagram"
              >
                An error occured
              </v-alert>
            </v-card-text>

            <v-divider></v-divider>

            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="primary" text @click="createFamily"> Create </v-btn>
              <v-btn color="primary" text @click="newFamilyDialog = false">
                Cancel
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-card>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import Vue from 'vue'
import { Family } from '~/types/Family'

export default Vue.extend({
  data() {
    return {
      inviteFamilyMemberFamilyId: '',
      inviteFamilyMemberDialog: false,
      newFamilyDialog: false,
      newFamilyName: '',
      alert: false,
    }
  },

  head: {
    title: 'Family',
  },

  computed: {
    families(): Family[] {
      return this.$accessor.families.families
    },
    familyMembers(): string[] {
      return ['Mikkel Nygaard', 'Linda Nygaard']
    },
  },
  methods: {
    openInviteFamilyMemberDialog(familyId: string) {
      this.inviteFamilyMemberFamilyId = familyId
      this.inviteFamilyMemberDialog = true
    },
    async inviteFamilyMember() {},
    async createFamily() {
      this.alert = false
      const result = await this.$axios.post('api/families', {
        name: this.newFamilyName,
      })
      if (result.status === 200 || result.status === 204) {
        this.$accessor.families.getFamilies()
        this.newFamilyName = ''
        this.newFamilyDialog = false
        return
      }
      this.alert = true
    },
  },
})
</script>
