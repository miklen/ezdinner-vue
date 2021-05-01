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
                  <v-col cols="5" class="text-right text-caption">{{
                    formatWeekDatesString(dinner.date)
                  }}</v-col>
                </v-row>
              </v-timeline-item>

              <PlannedDinner
                :dinner="dinner"
                @dinner:menuupdated="menuUpdated"
              />
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

export default Vue.extend({
  components: {
    TopDishes,
    PlannedDinner,
  },
  data: () => ({}),

  head: {
    title: 'Plan',
  },

  computed: {
    activeFamilyId(): string {
      return this.$accessor.activeFamilyId
    },
    dinners() {
      return this.$accessor.dinners.dinners
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
    populateDinners(): Promise<void> {
      const to = DateTime.now().plus({ week: 1 })
      const from = to.minus({ month: 1 })
      return this.$accessor.dinners.populateDinner({ from, to })
    },
    formatWeekDatesString(startOfWeekDay: DateTime) {
      return `${startOfWeekDay.toLocaleString(
        DateTime.DATE_SHORT,
      )} - ${startOfWeekDay
        .plus({ days: 7 })
        .toLocaleString(DateTime.DATE_SHORT)}`
    },
    /**
     * Dinner's menu updated event handler. Currently just repopulates
     * the entire set. Will be changed to just refresh the changed dinner later.
     */
    menuUpdated() {
      this.populateDinners()
    },
  },
})
</script>
