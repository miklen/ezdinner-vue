<template>
  <span>
    <v-row>
      <v-col
        v-for="family in families"
        :key="family.id"
        cols="12"
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
              <v-list-item-subtitle>{{
                familyMember.name
              }}</v-list-item-subtitle>
            </v-list-item>
          </v-list>
          <v-card-actions
            ><v-btn
              text
              color="primary"
              @click="openInviteFamilyMemberDialog(family.id)"
              >Invite</v-btn
            ><v-btn
              text
              color="primary"
              @click="openAddFamilyMemberDialog(family.id)"
              >Create</v-btn
            ></v-card-actions
          >
        </v-card>
      </v-col>
      <v-col cols="12" sm="12" md="6" lg="4">
        <v-card rounded="lg">
          <v-card-title>Create family</v-card-title>
          <v-card-text
            >To begin planning you need to create a family. After you've created
            your family you can then invite other family members to participate
            in planning or create family members which are used to rate
            dishes</v-card-text
          ><v-card-text
            >You can participate in more than one family!</v-card-text
          >
          <v-card-actions
            ><v-btn text color="primary" @click="newFamilyDialog = true"
              ><v-icon>mdi-account-multiple-plus</v-icon></v-btn
            ></v-card-actions
          >
        </v-card>
        <!-- Refactor to component -->
      </v-col>
    </v-row>
    <v-dialog v-model="inviteFamilyMemberDialog" width="500">
      <v-card>
        <v-card-title class="text-h5"> Invite family member </v-card-title>
        <v-divider></v-divider>
        <v-card-text style="padding-top: 16px">
          Invite someone to join as a family member in your planning family!
          Users must have an account before they can be invited</v-card-text
        >
        <v-card-text>
          <v-text-field
            v-model="inviteFamilyMemberEmail"
            autofocus
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
          <v-btn text @click="inviteFamilyMemberDialog = false">Cancel</v-btn>
          <v-btn color="primary" text @click="inviteFamilyMember">
            Invite
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="newFamilyDialog" width="500">
      <v-card>
        <v-card-title class="text-h5"> New family </v-card-title>
        <v-divider></v-divider>
        <v-card-text style="padding-top: 16px"
          >Give your family a recognizable name.</v-card-text
        >
        <v-card-text>
          <v-text-field
            v-model="newFamilyName"
            autofocus
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
          <v-btn text @click="newFamilyDialog = false">Cancel</v-btn>
          <v-btn color="primary" text @click="createFamily">Create</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="addFamilyMemberWithoutAccountDialog" width="500">
      <v-card>
        <v-card-title class="text-h5"> Create family member </v-card-title>
        <v-divider></v-divider>
        <v-card-text style="padding-top: 16px"
          >When you want to be able to track every family member's opinion of
          dishes, without requiring them to have their own account (e.g.
          children)</v-card-text
        >
        <v-card-text>
          <v-text-field
            v-model="familyMemberName"
            autofocus
            placeholder="Family member name"
            @keyup.enter="addFamilyMember"
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
          <v-btn text @click="addFamilyMemberWithoutAccountDialog = false"
            >Cancel</v-btn
          >
          <v-btn color="primary" text @click="addFamilyMember">Create</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </span>
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
      addFamilyMemberWithoutAccountDialog: false,
      familyMemberName: '',
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

        this.inviteFamilyMemberDialog = false
        this.inviteFamilyMemberEmail = ''
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
    openAddFamilyMemberDialog(familyId: string) {
      this.inviteFamilyMemberFamilyId = familyId
      this.addFamilyMemberWithoutAccountDialog = true
    },

    async addFamilyMember() {
      await this.$repositories.families.createFamilyMember(
        this.inviteFamilyMemberFamilyId,
        this.familyMemberName,
      )
      this.addFamilyMemberWithoutAccountDialog = false
      this.familyMemberName = ''
      this.families = await this.$repositories.families.all()
    },
  },
})
</script>
