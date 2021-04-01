<template>
  <v-app id="inspire">
    <Topbar />

    <v-main class="grey lighten-3">
      <v-container>
        <v-row>
          <v-col cols="2">
            <v-sheet rounded="lg" elevation="1">
              <v-list color="transparent  ">
                <v-list-item
                  v-show="showMenu(item)"
                  v-for="(item, i) in items"
                  :key="i"
                  :to="item.to"
                  router
                  exact
                >
                  <v-list-item-action>
                    <v-icon>{{ item.icon }}</v-icon>
                  </v-list-item-action>
                  <v-list-item-content>
                    <v-list-item-title v-text="item.title" />
                  </v-list-item-content>
                </v-list-item>

                <v-divider v-if="$msal.isAuthenticated" class="my-2"></v-divider>
                <v-list-item v-if="$msal.isAuthenticated" @click="clickSignOut">
                  <v-list-item-action>
                    <v-icon>mdi-logout</v-icon>
                  </v-list-item-action>
                  <v-list-item-title>Sign out</v-list-item-title>
                </v-list-item>
              </v-list>
            </v-sheet>
          </v-col>

          <v-col>
            <v-sheet min-height="70vh" rounded="lg" elevation="1">
              <v-container>
                <Nuxt />
              </v-container>
            </v-sheet>
          </v-col>
        </v-row>
      </v-container>
    </v-main>
  </v-app>
</template>

<script lang="ts">
import Vue from 'vue'
import Topbar from '../components/Topbar.vue'
import { MsalPlugin } from '@/plugins/msal'

export default Vue.extend({
  components: {
    Topbar
  },
  methods: {
    clickSignOut() {
      this.$msal.signOut()
    },
    showMenu(item: any) {
      if (!item.requireAuth) return true
      return this.$msal.isAuthenticated
    },
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
          requireAuth: true,
        },
        {
          icon: 'mdi-silverware-fork-knife',
          title: 'Plan',
          to: '/plan',
          requireAuth: true,
        },
      ],
    }
  },
})
</script>
