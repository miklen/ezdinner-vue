<template>
  <v-app dark>
    <v-navigation-drawer v-model="drawer" :clipped="clipped" fixed app>
      <v-list>
        <v-list-item v-show="showMenu(item)" v-for="(item, i) in items" :key="i" :to="item.to" router exact>
          <v-list-item-action>
            <v-icon>{{ item.icon }}</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title v-text="item.title" />
          </v-list-item-content>
        </v-list-item>
        <v-list-item v-if="!$msal.isAuthenticated" @click="clickSignIn">
          <v-list-item-action>
            <v-icon>mdi-login</v-icon>
          </v-list-item-action>
          <v-list-item-title>Sign in</v-list-item-title>
        </v-list-item>
        <v-list-item v-else @click="clickSignOut">
          <v-list-item-action>
            <v-icon>mdi-logout</v-icon>
          </v-list-item-action>
          <v-list-item-title>Sign out</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>
    <v-app-bar :clipped-left="clipped" fixed app>
      <v-toolbar-title v-text="title" />
      <v-spacer />
      <v-avatar color="primary" size="36">MN</v-avatar>
    </v-app-bar>
    <v-main>
      <v-container>
        <nuxt />
      </v-container>
    </v-main>
    <v-footer :absolute="!fixed" app>
      <span>&copy; {{ new Date().getFullYear() }}</span>
    </v-footer>
  </v-app>
</template>

<script lang="ts">
import { defineComponent } from '@vue/composition-api'
import { MsalPlugin } from '@/plugins/msal'

export default defineComponent({
  methods: {
    clickSignIn() {
      this.$msal.signIn()
    },
    clickSignOut() {
      this.$msal.signOut()
    },
    showMenu(item: any) {
      if (!item.requireAuth) return true
      return this.$msal.isAuthenticated
    }
  },
  data() {
    return {
      clipped: true,
      drawer: true,
      fixed: true,
      items: [
        {
          icon: 'mdi-apps',
          title: 'Welcome',
          to: '/',
        },
        {
          icon: 'mdi-chart-bubble',
          title: 'Inspire',
          to: '/inspire',
        },
        {
          icon: 'mdi-account-group',
          title: 'Families',
          to: '/families',
          requireAuth: true
        }
      ],
      title: 'Easy Dinner Planner',
    }
  },
})
</script>
