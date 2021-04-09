<template>
  <v-container>
    <v-row>
      <v-col class="text-center">
        <h1>Your heavy rotation</h1>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-simple-table>
          <template #default>
            <thead>
              <tr>
                <th class="text-left">Meal</th>
                <th class="text-left">Times</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in topMeals" :key="item.name">
                <td>{{ item.name }}</td>
                <td>{{ item.times }}</td>
              </tr>
            </tbody>
          </template>
        </v-simple-table>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
  data() {
    return {
      topMeals: [
        { name: 'McDonalds', times: 231 },
        { name: 'Steak with fries', times: 123 },
        { name: 'Pasta ala cabonara', times: 51 },
        { name: 'Lasagne', times: 10 },
        { name: 'Tortillias', times: 5 },
        { name: 'Fish', times: 3 },
        { name: 'Tacos', times: 2 },
        { name: 'Nachos', times: 1 },
        { name: 'Pancakes', times: 1 },
        { name: 'Nudles', times: 1 },
      ] as { name: string; times: number }[],
    }
  },

  methods: {},
})
</script>
