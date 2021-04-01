<template>
  <v-app-bar app color="white" elevation="1">
    <v-container class="py-0 fill-height">
      <v-toolbar-title v-text="title" />
      <div class="families-select">
        <FamilyDropdown v-if="$msal.isAuthenticated" />
      </div>
      <v-btn v-for="link in links" :key="link" text>{{ link }}</v-btn>

      <v-spacer></v-spacer>
      <v-btn v-if="!$msal.isAuthenticated" @click="clickSignIn">Sign in</v-btn>
      <v-avatar v-else color="primary" size="36">{{ getInitials() }}</v-avatar>
    </v-container>
  </v-app-bar>
</template>

<script lang="ts">
import Vue from 'vue'
import FamilyDropdown from '@/components/Family/FamilyDropdown.vue'

export default Vue.extend({
  components: {
    FamilyDropdown,
  },
  data(): {
    title: string
    links: string[]
  } {
    return {
      title: 'Easy Dinner Planner',
      links: [],
    }
  },
  methods: {
    getInitials(): string {
      return (
        (this.$msal.getFirstName()?.toUpperCase()[0] ?? '') +
        (this.$msal.getLastName()?.toUpperCase()[0] ?? '')
      )
    },
    clickSignIn() {
      this.$msal.signIn()
    },
  },
})
</script>

<style lang="scss">
.families-select {
  width: 150px;
  margin-right: 20px;
  margin-left: 20px;

  .v-select {
    .v-input__control {
      .v-text-field__details {
        display: none;
      }
    }
  }
}
</style>
