<template>
  <v-app-bar app color="white" elevation="1">
    <v-container class="py-0 fill-height">
      <v-toolbar-title v-text="title" />

      <v-btn v-for="link in links" :key="link" text>{{ link }}</v-btn>

      <v-spacer></v-spacer>
      <div class="families-select">
        <v-select dense :items="families" label="Family" solo single-line></v-select>
      </div>
      <v-avatar color="primary" size="36">MN</v-avatar>
    </v-container>
  </v-app-bar>
</template>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
  data(): {
    title: string
    links: string[]
    families: string[]
  } {
    return {
      title: 'Easy Dinner Planner',
      links: ['Dashboard', 'Report'],
      families: ['Nygaard', 'Bengtson'],
    }
  },
})
</script>

<style lang="scss">
.families-select {
  width: 150px;
  margin-right: 20px;

  .v-select {
    .v-input__control {
      .v-text-field__details {
        display: none;
      }
    }
  }
}
</style>>
