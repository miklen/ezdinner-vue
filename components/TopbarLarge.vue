<template>
  <v-app-bar app color="white" elevation="1">
    <v-container :fluid="$vuetify.breakpoint.mdOnly" class="py-0 fill-height">
      <v-avatar>
        <v-img src="/android-chrome-192x192.png"></v-img>
      </v-avatar>
      <v-toolbar-title v-text="title" />

      <v-spacer></v-spacer>

      <TopbarProfile />
    </v-container>
  </v-app-bar>
</template>

<script lang="ts">
import Vue from 'vue'
import TopbarProfile from './TopbarProfile.vue'

export default Vue.extend({
  components: {
    TopbarProfile,
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
