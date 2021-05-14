<template>
  <v-app id="ezdinner">
    <TopbarLarge v-if="!$vuetify.breakpoint.smAndDown" />
    <TopbarSmall v-else :links="links" />

    <v-main class="grey lighten-3">
      <v-container :fluid="$vuetify.breakpoint.mdOnly">
        <v-row>
          <v-col v-if="!$vuetify.breakpoint.smAndDown" cols="2">
            <v-sheet rounded="lg" elevation="1">
              <v-list color="transparent">
                <v-list-item
                  v-for="(item, i) in links"
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
              </v-list>
            </v-sheet>
          </v-col>

          <v-col>
            <Nuxt />
          </v-col>
        </v-row>
      </v-container>
    </v-main>
  </v-app>
</template>

<script lang="ts">
import Vue from 'vue'
import TopbarLarge from '~/components/TopbarLarge.vue'
import TopbarSmall from '~/components/TopbarSmall.vue'

export default Vue.extend({
  name: 'Default',
  components: {
    TopbarLarge,
    TopbarSmall,
  },
  middleware: ['auth'],
  data() {
    return {
      clipped: true,
      drawer: true,
      fixed: true,
      links: [
        {
          icon: 'mdi-home',
          title: 'Home',
          to: '/home',
        },
        {
          icon: 'mdi-account-group',
          title: 'Families',
          to: '/families',
        },
        {
          icon: 'mdi-silverware-fork-knife',
          title: 'Plan',
          to: '/plan',
        },
      ],
    }
  },
  mounted() {
    this.getFamilies()
  },
  methods: {
    clickSignOut() {
      this.$msal.signOut()
    },
    showMenu(item: any) {
      if (!item.requireAuth) return true
      return this.$msal.isAuthenticated
    },
    getFamilies() {
      return this.$accessor.families.getFamilies()
    },
  },
})
</script>
