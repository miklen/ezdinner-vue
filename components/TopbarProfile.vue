<template>
  <v-menu v-model="menu" offset-y left>
    <template #activator="{ on, attrs }">
      <v-btn
        v-if="!$msal.isAuthenticated"
        color="primary"
        large
        @click="clickSignIn"
      >
        <span class="text--darken-1 font-weight-bold"> SIGN IN </span>
      </v-btn>

      <v-avatar
        v-else
        class="white--text"
        color="primary"
        size="36"
        v-bind="attrs"
        v-on="on"
        >{{ getInitials() }}</v-avatar
      >
    </template>

    <v-card>
      <v-list>
        <v-list-item>
          <v-list-item-avatar size="30" color="primary">
            <v-icon color="white">mdi-account</v-icon>
          </v-list-item-avatar>
          <v-list-item-content>
            <v-list-item-title>{{ getFullName() }}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>

      <v-divider></v-divider>

      <v-list>
        <v-list-item v-for="(item, i) in items" :key="i" @click="item.action">
          <v-list-item-icon>
            <v-icon v-text="item.icon"></v-icon>
          </v-list-item-icon>

          <v-list-item-content>
            <v-list-item-title v-text="item.text"></v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-card>
  </v-menu>
</template>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
  data: () => ({
    menu: false as boolean,
  }),
  computed: {
    items(): { icon: string; text: string; action: () => void }[] {
      return [
        {
          icon: 'mdi-logout',
          text: 'Sign out',
          action: this.clickSignOut,
        },
      ]
    },
  },
  methods: {
    getInitials(): string {
      return (
        (this.$msal.getFirstName()?.toUpperCase()[0] ?? '') +
        (this.$msal.getLastName()?.toUpperCase()[0] ?? '')
      )
    },
    getFullName(): string {
      return `${this.$msal.getFirstName()} ${this.$msal.getLastName()}`
    },
    clickSignIn() {
      this.$msal.signIn()
    },
    clickSignOut() {
      this.$msal.signOut()
    },
  },
})
</script>
