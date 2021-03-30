<template>
  <v-app id="inspire">
    <v-app-bar app color="white" flat>
      <v-container class="py-0 fill-height">
        <v-toolbar-title v-text="title" />

        <v-btn v-for="link in links" :key="link" text>{{ link }}</v-btn>

        <v-spacer></v-spacer>

        <v-avatar color="primary" size="36">MN</v-avatar>
      </v-container>
    </v-app-bar>

    <v-main class="grey lighten-3">
      <v-container>
        <v-row>
          <v-col cols="2">
            <v-sheet rounded="lg">
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

                <v-divider class="my-2"></v-divider>

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
            </v-sheet>
          </v-col>

          <v-col>
            <v-sheet min-height="70vh" rounded="lg">
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
      ],
      title: 'Easy Dinner Planner',
      links: ['Dashboard', 'Report'],
    }
  },
})
</script>
