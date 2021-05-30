<template>
  <Content split>
    <v-row>
      <v-col class="text-center">
        <h1>Dinner plan</h1>
      </v-col>
      <v-col>
        <v-menu
          v-model="showDatePicker"
          :close-on-content-click="false"
          :nudge-right="40"
          transition="scale-transition"
          offset-y
          min-width="auto"
        >
          <template #activator="{ on, attrs }">
            <v-text-field
              v-model="dateRangeText"
              label="Select date range"
              prepend-icon="mdi-calendar"
              readonly
              v-bind="attrs"
              v-on="on"
            ></v-text-field>
          </template>
          <v-date-picker v-model="dateRange" show-week range></v-date-picker>
        </v-menu>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-timeline dense>
          <div v-for="(dinner, index) in dinners" :key="index">
            <PlannedDinner
              :dinner="dinner"
              :selected="isDinnerDateSelected(dinner, selectedDinnerDate)"
              @dinner:clicked="selectedDinnerDate = dinner.date"
              @dinner:menuupdated="menuUpdated"
              @dinner:close="selectedDinnerDate = null"
            />

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
          </div>
        </v-timeline>
      </v-col>
    </v-row>
    <template #support>
      <TopDishes />
    </template>
  </Content>
</template>

<script lang="ts">
import Vue from 'vue'
import { DateTime } from 'luxon'
import Content from '~/components/Content.vue'
import TopDishes from '~/components/Plan/TopDishes.vue'
import PlannedDinner from '~/components/Plan/PlannedDinner.vue'
import { Dinner } from '~/types/Dinner'

export default Vue.extend({
  components: {
    Content,
    TopDishes,
    PlannedDinner,
  },
  data: () => ({
    showDatePicker: false,
    dateRange: [] as string[],
    selectedDinnerDate: null as DateTime | null,
  }),

  head: {
    title: 'Plan',
  },

  computed: {
    dateRangeText() {
      return this.dateRange
        .map((d) => DateTime.fromISO(d).toLocaleString(DateTime.DATE_SHORT))
        .join(' ~ ')
    },
    activeFamilyId(): string {
      return this.$accessor.activeFamilyId
    },
    dinners() {
      return [...this.$accessor.dinners.dinners].reverse()
    },
  },

  watch: {
    activeFamilyId(newValue: string) {
      if (!newValue) return
      this.init()
    },
    dateRange(newValue) {
      if (newValue.length !== 2) return
      this.populateDinners()
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
      const to = DateTime.now().plus({ week: 1 })
      const from = to.minus({ month: 1 })
      // setting dateRange triggers the watcher which populates dinners
      this.dateRange = [from.toISODate(), to.toISODate()]
    },
    isDinnerDateSelected(dinner: Dinner, selectedDate: DateTime | null) {
      if (!dinner?.date || !selectedDate) return false
      return dinner.date.equals(selectedDate)
    },
    populateDinners(): Promise<void> {
      const from = DateTime.fromISO(this.dateRange[0])
      const to = DateTime.fromISO(this.dateRange[1])
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
