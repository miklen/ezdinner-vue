<template>
  <v-app id="ezdinner">
    <TopbarLarge v-if="!$vuetify.breakpoint.smAndDown" />
    <TopbarSmall v-else :links="links" />

    <v-main class="grey lighten-3">
      <v-container :fluid="$vuetify.breakpoint.mdOnly">
        <v-row>
          <v-col v-if="!$vuetify.breakpoint.smAndDown" cols="2">
            <v-list nav color="transparent">
              <v-list-item-group color="primary">
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
              </v-list-item-group>
            </v-list>
          </v-col>

          <v-col>
            <Nuxt />
          </v-col>
        </v-row>
      </v-container>
    </v-main>
    <v-overlay :value="loading">
      <v-progress-circular
        :size="50"
        color="primary"
        indeterminate
      ></v-progress-circular
    ></v-overlay>
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
      loading: true,
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
          title: 'Dishes',
          to: '/dishes',
        },
        {
          icon: 'mdi-calendar-edit',
          title: 'Plan',
          to: '/plan',
        },
      ],
    }
  },
  async mounted() {
    await this.getFamilySelectors()
    this.loading = false
  },
  methods: {
    clickSignOut() {
      this.$msal.signOut()
    },
    showMenu(item: any) {
      if (!item.requireAuth) return true
      return this.$msal.isAuthenticated
    },
    getFamilySelectors() {
      return this.$accessor.families.getFamilySelectors()
    },
  },
})
</script>
