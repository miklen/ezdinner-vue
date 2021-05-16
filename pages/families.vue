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
      <v-card rounded="lg">
        <v-card-title>{{ family.name }}</v-card-title>
        <v-card-subtitle>Family members</v-card-subtitle>

        <v-list>
          <v-list-item
            v-for="familyMember in family.familyMembers"
            :key="familyMember.id"
          >
            <v-list-item-avatar
              ><v-icon>mdi-account</v-icon></v-list-item-avatar
            >
            <v-list-item-subtitle>{{ familyMember.name }}</v-list-item-subtitle>
          </v-list-item>
        </v-list>

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
                @keyup.enter="inviteFamilyMember"
              ></v-text-field>
              <v-alert
                v-model="notFoundAlert"
                dismissible
                type="warning"
                border="left"
                elevation="2"
                colored-border
              >
                User not found
              </v-alert>
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
      <v-card rounded="lg">
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
                @keyup.enter="createFamily"
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
      notFoundAlert: false,
      inviteFamilyMemberEmail: '',
      inviteFamilyMemberFamilyId: '',
      inviteFamilyMemberDialog: false,
      newFamilyDialog: false,
      newFamilyName: '',
      alert: false,
      families: [] as Family[],
    }
  },

  head: {
    title: 'Family',
  },

  async created() {
    this.families = await this.$repositories.families.all()
  },

  methods: {
    openInviteFamilyMemberDialog(familyId: string) {
      this.inviteFamilyMemberFamilyId = familyId
      this.inviteFamilyMemberDialog = true
    },
    async inviteFamilyMember() {
      this.notFoundAlert = false
      this.alert = false
      try {
        const invited = await this.$repositories.families.inviteFamilyMember(
          this.inviteFamilyMemberFamilyId,
          this.inviteFamilyMemberEmail,
        )

        if (!invited) {
          this.notFoundAlert = true
          return
        }

        this.families = await this.$repositories.families.all()
      } catch (e) {
        this.alert = true
      }
    },
    async createFamily() {
      this.alert = false
      try {
        const result = await this.$repositories.families.createFamily(
          this.newFamilyName,
        )
        if (result) {
          this.$accessor.families.getFamilySelectors()
          this.families = await this.$repositories.families.all()
          this.newFamilyName = ''
          this.newFamilyDialog = false
          return
        }
        this.alert = true
      } catch (e) {
        this.alert = true
      }
    },
  },
})
</script>
