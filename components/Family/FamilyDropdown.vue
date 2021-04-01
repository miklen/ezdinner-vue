<template>
  <v-select dense :items="families" item-text="name" return-object label="Family" solo single-line></v-select>
</template>

<script lang="ts">
import Vue from 'vue'
import { Family } from '@/types/Family'
export default Vue.extend({
  data(): {
    families: Family[]
  } {
    return {
      families: [],
    }
  },
  methods: {
    async getFamilies() {
      const result = await this.$axios.get('/api/GetFamilies')
      this.families = result.data
    },
  },
  mounted() {
    this.getFamilies()
  },
})
</script>