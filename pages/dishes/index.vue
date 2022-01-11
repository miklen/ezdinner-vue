<template>
  <span>
    <v-row>
      <v-col cols="9" md="4">
        <v-text-field v-model="searchDish" label="Search dishes"></v-text-field>
      </v-col>
      <v-col class="text-right">
        <v-btn color="primary" fab @click="newDishDialog = true">
          <v-icon>mdi-plus</v-icon>
        </v-btn>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="4" sm="3" xl="2"
        ><v-select
          v-model="sorter"
          :items="sorting"
          label="Sort by"
          item-text="name"
          return-object
          dense
          solo
        ></v-select
      ></v-col>
    </v-row>
    <v-row>
      <v-col v-for="dish in dishes" :key="dish.id" cols="12" md="6" lg="4">
        <DishCard
          :dish="dish"
          :dish-stats="stats[dish.id]"
          @menuitem:moved="getStats()"
        ></DishCard>
      </v-col>
    </v-row>
    <v-dialog v-model="newDishDialog">
      <v-card>
        <v-card-title class="text-h5">Create dish</v-card-title>
        <v-divider></v-divider>
        <v-card-text>
          <v-text-field
            v-model="newDishName"
            label="Dish name"
            @keyup.enter="createDish"
          ></v-text-field>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn text @click="newDishDialog = false">Cancel</v-btn
          ><v-btn text color="primary" @click="createDish">Create</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
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
      sorter: {} as { name: string; sorter: (a: Dish, b: Dish) => number },
      newDishName: '',
      newDishDialog: false,
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
        .sort(this.sorter.sorter)
        .filter((dish) =>
          dish.name?.toLowerCase().includes(this.searchDish?.toLowerCase()),
        )
    },
    sorting(): { name: string; sorter: (a: Dish, b: Dish) => number }[] {
      return [
        {
          name: '⬆A-Z',
          sorter: (a, b) => a.name.localeCompare(b.name),
        },
        {
          name: '⬇Z-A',
          sorter: (a, b) => b.name.localeCompare(a.name),
        },
        {
          name: '⬆❤',
          sorter: (a, b) => b.rating - a.rating,
        },
        {
          name: '⬇❤',
          sorter: (a, b) => a.rating - b.rating,
        },
        {
          name: '⬆Times',
          sorter: (a, b) => {
            return (
              (this.stats[b.id]?.timesUsed ?? 0) -
              (this.stats[a.id]?.timesUsed ?? 0)
            )
          },
        },
        {
          name: '⬇Times',
          sorter: (a, b) => {
            return (
              (this.stats[a.id]?.timesUsed ?? 0) -
              (this.stats[b.id]?.timesUsed ?? 0)
            )
          },
        },
        {
          name: '⬆Date',
          sorter: (a, b) => {
            const aDate = this.stats[a.id]?.lastUsed?.toMillis() ?? Math.min()
            const bDate = this.stats[b.id]?.lastUsed?.toMillis() ?? Math.min()
            return aDate - bDate
          },
        },
        {
          name: '⬇Date',
          sorter: (a, b) => {
            const aDate = this.stats[a.id]?.lastUsed?.toMillis() ?? Math.max()
            const bDate = this.stats[b.id]?.lastUsed?.toMillis() ?? Math.max()
            return bDate - aDate
          },
        },
      ]
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

  created() {
    this.sorter = this.sorting[0]
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

    async createDish() {
      await this.$repositories.dishes.create(
        this.$accessor.activeFamilyId,
        this.newDishName,
      )
      this.init()
      this.newDishName = ''
      this.newDishDialog = false
    },
  },
})
</script>
