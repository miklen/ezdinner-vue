<template>
  <v-row>
    <v-col cols="7">
      <v-row>
        <v-col class="text-center">
          <h1>Dinner plan</h1>
        </v-col>
      </v-row>
      <v-row>
        <v-col>
          <v-timeline dense>
            <div v-for="(meal, index) in meals" :key="index">
              <!-- Everytime a new week starts -->
              <v-timeline-item
                v-if="meal.date.weekday === 1"
                class="mb-4"
                color="pink"
                small
                hide-dot
              >
                <v-row justify="space-between">
                  <v-col cols="7">Week {{ meal.date.weekNumber }}</v-col>
                  <v-col cols="5" class="text-right">{{
                    getWeekDatesString(meal.date)
                  }}</v-col>
                </v-row>
              </v-timeline-item>

              <PlannedMeal :meal="meal" />
            </div>
          </v-timeline>
        </v-col>
      </v-row>
    </v-col>
    <v-col cols="5">
      <TopMeals />
    </v-col>
  </v-row>
</template>

<script lang="ts">
import Vue from 'vue'
import { DateTime } from 'luxon'
import TopMeals from '~/components/Plan/TopMeals.vue'
import PlannedMeal from '~/components/Plan/PlannedMeal.vue'
import { Meal } from '~/types/Meal'

export default Vue.extend({
  components: {
    TopMeals,
    PlannedMeal,
  },
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
    meals(): Meal[] {
      const meals: Meal[] = []
      for (let i = 0; i < 30; i++) {
        const meal: Meal = {
          date: DateTime.now().minus({ days: i - 3 }),
          name: this.mealOptions[i % this.mealOptions.length],
          tags: ['At home'],
        }
        meals.push(meal)
      }
      return meals.reverse()
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
