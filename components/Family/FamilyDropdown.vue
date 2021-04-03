<template>
  <v-select
    dense
    :items="families"
    item-text="name"
    return-object
    label="Family"
    solo
    single-line
  ></v-select>
</template>

<script lang="ts">
import Vue from 'vue'
import { Family } from '~/types/Family'

export default Vue.extend({
  computed: {
    families(): Family[] {
      return this.$accessor.families.families
    },
  },
  mounted() {
    this.getFamilies()
  },
  methods: {
    getFamilies(): Promise<void> {
      return this.$accessor.families.getFamilies()
    },
  },
})
</script>
