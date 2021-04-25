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
                    formatWeekDatesString(dinner.date)
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
import { Dinner, MenuItem } from '~/types/Dinner'

export default Vue.extend({
  components: {
    TopDishes,
    PlannedDinner,
  },
  data: () => ({
    dinners: [] as Dinner[],
  }),

  head: {
    title: 'Plan',
  },

  computed: {
    activeFamilyId(): string {
      return this.$accessor.activeFamilyId
    },
    dishMap(): { [key: string]: string } {
      return this.$accessor.dishes.dishes.reduce((prev, current) => {
        prev[current.id] = current.name
        return prev
      }, {} as { [key: string]: string })
    },
  },

  watch: {
    activeFamilyId(newValue: string) {
      if (!newValue) return
      this.init()
    },
  },

  async created() {
    await this.init()
  },

  methods: {
    async init() {
      // dishes are used by child components
      if (!this.$accessor.activeFamilyId) return
      await this.$accessor.dishes.populateDishes()
      await this.populateDinners()
    },

    async populateDinners() {
      const to = DateTime.now().plus({ week: 1 })
      const from = to.minus({ month: 1 })
      const result = await this.$axios.get(
        `api/dinners/family/${
          this.activeFamilyId
        }/dates/${from.toISODate()}/${to.toISODate()}`,
      )

      // runtime join - consider if this is done better elsewhere
      this.dinners = result.data.map((dinner: any) => {
        dinner.date = DateTime.fromISO(dinner.date)
        dinner.menu.map((item: any) => {
          item.dishName = this.dishMap[item.dishId]
          // TODO: add receipeName
          return item
        })
        return dinner
      })
    },
    formatWeekDatesString(startOfWeekDay: DateTime) {
      return `${startOfWeekDay.toLocaleString(
        DateTime.DATE_SHORT,
      )} - ${startOfWeekDay
        .plus({ days: 7 })
        .toLocaleString(DateTime.DATE_SHORT)}`
    },
  },
})
</script>
