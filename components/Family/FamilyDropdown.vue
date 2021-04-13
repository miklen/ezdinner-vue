<template>
  <v-select
    v-model="selectedFamily"
    dense
    :items="families"
    item-text="name"
    item-value="id"
    label="Family"
    solo
    single-line
  ></v-select>
</template>

<script lang="ts">
import Vue from 'vue'

export default Vue.extend({
  computed: {
    families() {
      return this.$accessor.families.families
    },
    selectedFamily: {
      get() {
        return this.$accessor.activeFamilyId
      },
      set(value: string) {
        this.$accessor.setActiveFamilyId(value)
      },
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
