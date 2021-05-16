<template>
  <v-list>
    <v-list-item
      v-for="family in families"
      :key="family.id"
      @click="$accessor.setActiveFamilyId(family.id)"
    >
      <v-list-item-action>
        <v-icon v-if="family.id === selectedFamily" color="yellow darken-3">
          mdi-star
        </v-icon>
      </v-list-item-action>
      <v-list-item-title>{{ family.name }}</v-list-item-title>
    </v-list-item>

    <v-list-item v-if="families.length === 0" to="/families">
      <v-list-item-title>Create or join a family</v-list-item-title>
    </v-list-item>
  </v-list>
</template>

<script lang="ts">
import Vue from 'vue'

export default Vue.extend({
  computed: {
    families() {
      return this.$accessor.families.familySelectors
    },
    selectedFamily() {
      return this.$accessor.activeFamilyId
    },
  },
})
</script>
