<template>
  <span>
    <v-app-bar app color="white" elevation="1">
      <v-avatar>
        <v-img src="/android-chrome-192x192.png"></v-img>
      </v-avatar>

      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>

      <v-spacer></v-spacer>

      <TopbarProfile />
    </v-app-bar>

    <v-navigation-drawer v-model="drawer" fixed temporary>
      <v-list-item>
        <v-list-item-content>
          <v-list-item-title class="title"
            ><v-avatar>
              <v-img src="/android-chrome-192x192.png"></v-img>
            </v-avatar>
            {{ title }}
          </v-list-item-title>
        </v-list-item-content>
        <v-list-item-action @click="drawer = !drawer">
          <v-icon>mdi-chevron-left</v-icon>
        </v-list-item-action>
      </v-list-item>

      <v-divider></v-divider>

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
    </v-navigation-drawer>
  </span>
</template>

<script lang="ts">
import Vue from 'vue'
import TopbarProfile from './TopbarProfile.vue'

export default Vue.extend({
  components: {
    TopbarProfile,
  },
  props: {
    links: {
      type: Array,
      required: true,
    },
  },
  data() {
    return {
      title: 'Dinner Planner',
      drawer: false,
    }
  },
})
</script>
