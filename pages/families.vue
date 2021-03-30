<template>
  <v-row>
    <v-col v-for="family in families" :key="family.id" class="text-center">
        {{ family.name }}
    </v-col>
  </v-row>
</template>

<script lang="ts">
declare class Family {
  public id: string
  public name: string
}

import Vue from 'vue'
export default Vue.extend({
  middleware: ['auth'],
  data() : {
    families: Family[]
  } {
    return {
      families: []
    }
  },
  methods: {
      async getFamilies() {
          const result = await this.$axios.get('/api/GetFamilies')
          this.families = result.data
      }
  },
  mounted() {
      this.getFamilies()
  }
})
</script>