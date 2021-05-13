<template>
  <v-row>
    <v-col>
      <v-sheet min-height="30vh" rounded="lg" elevation="1">
        <v-container>
          <slot></slot>
        </v-container>
      </v-sheet>
    </v-col>

    <v-col v-if="split" sm="12" md="4">
      <v-sheet min-height="10vh" rounded="lg" elevation="1">
        <v-container>
          <slot name="support"></slot>
        </v-container>
      </v-sheet>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
  props: {
    split: Boolean,
  },
})
</script>
