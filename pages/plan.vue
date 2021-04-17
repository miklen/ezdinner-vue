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
            <div v-for="(dinner, index) in dinners" :key="index">
              <!-- Everytime a new week starts -->
              <v-timeline-item
                v-if="dinner.date.weekday === 1"
                class="mb-4"
                color="pink"
                small
                hide-dot
              >
                <v-row justify="space-between">
                  <v-col cols="7">Week {{ dinner.date.weekNumber }}</v-col>
                  <v-col cols="5" class="text-right">{{
                    getWeekDatesString(dinner.date)
                  }}</v-col>
                </v-row>
              </v-timeline-item>

              <PlannedDinner :dinner="dinner" />
            </div>
          </v-timeline>
        </v-col>
      </v-row>
    </v-col>
    <v-col cols="5">
      <TopDishes />
    </v-col>
  </v-row>
</template>

<script lang="ts">
import Vue from 'vue'
import { DateTime } from 'luxon'
import TopDishes from '~/components/Plan/TopDishes.vue'
import PlannedDinner from '~/components/Plan/PlannedDinner.vue'
import { Dinner } from '~/types/Dinner'

export default Vue.extend({
  components: {
    TopDishes,
    PlannedDinner,
  },
  data: () => ({
    dinnerOptions: [
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
    dinners(): Dinner[] {
      const dinners: Dinner[] = []
      for (let i = 0; i < 30; i++) {
        const dinner: Dinner = {
          date: DateTime.now().minus({ days: i - 3 }),
          description: this.dinnerOptions[i % this.dinnerOptions.length],
          tags: [
            {
              value: i % 2 ? 'At home' : 'Guests',
              color: i % 2 ? 'blue' : 'pink',
            },
          ],
          menu: [],
        }
        dinners.push(dinner)
      }
      return dinners.reverse()
    },
    activeFamilyId(): string {
      return this.$accessor.activeFamilyId
    },
  },

  watch: {
    activeFamilyId(newValue: string) {
      if (!newValue) return
      this.init()
    },
  },

  created() {
    this.init()
  },

  methods: {
    init() {
      // dishes are used by child components
      if (!this.$accessor.activeFamilyId) return
      this.$accessor.dishes.populateDishes()
    },
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
