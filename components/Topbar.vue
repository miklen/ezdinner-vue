<template>
  <v-app-bar app color="white" elevation="1">
    <v-container class="py-0 fill-height">
      <v-avatar>
        <v-img src="/android-chrome-192x192.png"></v-img>
      </v-avatar>
      <v-toolbar-title v-text="title" />
      <div :class="familiesDropdownClasses">
        <FamilyDropdown v-if="$msal.isAuthenticated" />
      </div>
      <v-btn v-for="link in links" :key="link" text>{{ link }}</v-btn>

      <v-spacer></v-spacer>
      <TopbarProfile />
    </v-container>
  </v-app-bar>
</template>

<script lang="ts">
import Vue from 'vue'
import FamilyDropdown from '@/components/Family/FamilyDropdown.vue'
import TopbarProfile from './TopbarProfile.vue'

export default Vue.extend({
  components: {
    FamilyDropdown,
    TopbarProfile,
  },
  data(): {
    title: string
    links: string[]
  } {
    return {
      title: 'Dinner Planner',
      links: [],
    }
  },
  computed: {
    familiesDropdownClasses(): string[] {
      const classes = ['families-select']
      if (this.$vuetify.breakpoint.mobile) classes.push('families-select-small')
      return classes
    },
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
.families-select-small {
  width: 60px !important;
}

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
