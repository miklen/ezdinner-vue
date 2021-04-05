<template>
  <v-row>
    <v-spacer></v-spacer>
    <v-col class="col-4">
      <v-card>
        <v-card-title>Welcome</v-card-title>
        <v-card-text
          >Get started by creating a family. Add family members to participate
          in planning. And begin tracking and planning your meals.
        </v-card-text>
      </v-card>
    </v-col>
    <v-spacer></v-spacer>
  </v-row>
</template>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
  head: {
    title: 'Home',
  },
})
</script>
