<template>
  <v-app-bar app color="white" elevation="1">
    <v-avatar>
      <v-img src="/android-chrome-192x192.png"></v-img>
    </v-avatar>

    <v-tabs centered class="ml-n4" color="grey darken-1">
      <v-tab v-for="link in links" :key="link.to" :to="link.to" router exact>
        {{ link.title }}
      </v-tab>
    </v-tabs>

    <v-spacer></v-spacer>

    <TopbarProfile />
  </v-app-bar>
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
  data(): {
    title: string
  } {
    return {
      title: 'Dinner Planner',
    }
  },
})
</script>
