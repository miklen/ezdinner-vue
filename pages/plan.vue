<template>
  <v-container style="max-width: 600px">
    <v-timeline dense clipped>
      <div v-for="meal in meals" :key="meal.date">
        <v-timeline-item v-if="meal.meal" class="mb-4" color="green" small>
          <template #opposite>
            <v-icon>mdi-pencil</v-icon>
          </template>

          <v-row justify="space-between">
            <v-col cols="6" v-text="meal.meal"></v-col>
            <v-col
              class="text-right"
              cols="5"
              v-text="meal.date.toFormat('EEEE')"
            ></v-col>
          </v-row>
        </v-timeline-item>

        <v-timeline-item v-else class="mb-4" color="grey" small>
          <template #opposite>
            <v-icon>mdi-pencil</v-icon>
          </template>

          <v-row justify="space-between">
            <v-col cols="6" v-text="meal.meal"></v-col>
            <v-col
              class="text-right"
              cols="5"
              v-text="meal.date.toFormat('EEEE')"
            ></v-col>
          </v-row>
        </v-timeline-item>

        <v-timeline-item
          v-if="meal.date.weekday === 1"
          class="mb-4"
          color="pink"
          small
        >
          <v-row justify="space-between">
            <v-col cols="7">Week {{ meal.date.weekNumber }}</v-col>
            <v-col cols="5" class="text-right">{{
              getWeekDatesString(meal.date)
            }}</v-col>
          </v-row>
        </v-timeline-item>
      </div>
    </v-timeline>
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue'
import { DateTime } from 'luxon'

export default Vue.extend({
  data: () => ({
    mealOptions: [
      'McDonalds',
      'Steak with fries',
      'Pasta ala cabonara',
      'Lasagne',
      'Tortillias',
      'Fish',
      '',
      'Tacos',
      'Nachos',
      'Pancakes',
      '',
      'Nudles',
    ],
  }),

  head: {
    title: 'Plan',
  },

  computed: {
    meals() {
      const meals = []
      for (let i = 0; i < 30; i++) {
        const meal = {
          date: DateTime.now().minus({ days: i - 3 }),
          meal: this.mealOptions[i % this.mealOptions.length],
          tags: ['At home'],
        }
        meals.push(meal)
      }
      return meals
    },
  },

  methods: {
    getWeekDatesString(startOfWeekDay: DateTime) {
      return `${startOfWeekDay.toLocaleString(
        DateTime.DATE_SHORT,
      )} - ${startOfWeekDay
        .plus({ days: 7 })
        .toLocaleString(DateTime.DATE_SHORT)}`
    },
  },
})
</script>
