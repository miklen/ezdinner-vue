<template>
  <span>
    <v-row>
      <v-col align-left class="text-center">
        <h1>Top {{ top }}</h1>
      </v-col>
      <v-col cols="3" xl="2"
        ><v-select v-model="top" :items="choices"></v-select
      ></v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-simple-table>
          <template #default>
            <thead>
              <tr>
                <th class="text-left">Dish</th>
                <th class="text-left">Times</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in topDishes" :key="item.name">
                <td>{{ item.name }}</td>
                <td>{{ item.times }}</td>
              </tr>
            </tbody>
          </template>
        </v-simple-table>
      </v-col>
    </v-row>
  </span>
</template>

<script lang="ts">
import Vue from 'vue'
import { DishStats } from '~/types/Dish'
export default Vue.extend({
  data() {
    return {
      stats: {} as { [key: string]: DishStats },
      top: 10,
      choices: [10, 25, 50],
    }
  },

  async fetch() {
    this.stats = await this.$repositories.dishes.allUsageStats(
      this.$accessor.activeFamilyId,
    )
    if (!this.$accessor.dishes.dishes.length) {
      this.$accessor.dishes.populateDishes()
    }
  },

  computed: {
    topDishes(): { name: string; times: number }[] {
      const topDishes = this.$accessor.dishes.dishes
        .map((d) => ({
          name: d.name,
          times: this.stats[d.id]?.timesUsed ?? 0,
        }))
        .sort((a, b) => b.times - a.times)
        .slice(0, Math.min(this.top, this.$accessor.dishes.dishes.length))
      return topDishes
    },
  },
})
</script>
