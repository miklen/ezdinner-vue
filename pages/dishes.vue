<template>
  <span>
    <v-row>
      <v-col cols="4">
        <v-text-field v-model="searchDish" label="Search dishes"></v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <v-col
        v-for="dish in dishes"
        :key="dish.id"
        xs="12"
        sm="12"
        md="6"
        lg="4"
      >
        <DishCard
          :dish="dish"
          :dish-stats="stats[dish.id]"
          @menuitem:moved="getStats()"
        ></DishCard>
      </v-col>
    </v-row>
  </span>
</template>

<script lang="ts">
import Vue from 'vue'
import { Dish, DishStats } from '~/types/Dish'
import DishCard from '~/components/Dish/DishCard.vue'
export default Vue.extend({
  components: {
    DishCard,
  },
  data() {
    return {
      searchDish: '',
      stats: {} as { [key: string]: DishStats },
    }
  },

  async fetch() {
    await this.init()
  },

  head: {
    title: 'Dishes',
  },

  computed: {
    dishes(): Dish[] {
      return [...this.$accessor.dishes.dishes]
        .sort((a, b) => a.name.localeCompare(b.name))
        .filter((dish) =>
          dish.name?.toLowerCase().includes(this.searchDish?.toLowerCase()),
        )
    },
    activeFamilyId() {
      return this.$accessor.activeFamilyId
    },
  },

  watch: {
    activeFamilyId(newValue: string) {
      if (!newValue) return
      this.stats = {}
      this.init()
    },
  },

  methods: {
    async init() {
      await this.$accessor.dishes.populateDishes()
      await this.getStats()
    },
    async getStats() {
      this.stats = await this.$repositories.dishes.allUsageStats(
        this.activeFamilyId,
      )
    },
  },
})
</script>
