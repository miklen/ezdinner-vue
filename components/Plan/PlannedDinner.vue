<template>
  <div>
    <v-timeline-item class="mb-4" :color="getDotColor(dinner)" small>
      <v-row
        v-show="!selected"
        style="cursor: pointer"
        justify="space-between"
        :class="getTimelineTextStyle(dinner)"
        @click="$emit('dinner:clicked')"
      >
        <v-col cols="6"
          >{{ getTitle(dinner) }}
          <v-chip
            v-for="tag in dinner.tags"
            :key="tag.value"
            outlined
            x-small
            :color="tag.color || 'primary'"
            >{{ tag.value }}</v-chip
          ></v-col
        >
        <v-col class="text-right" cols="5">
          {{ formatDay(dinner.date) }}
          <span class="text-caption">{{ formatDate(dinner.date) }}</span>
        </v-col>
      </v-row>

      <PlannedMealDetails
        v-show="selected"
        :dinner="dinner"
        @dinner:close="$emit('dinner:close')"
        @dinner:menuupdated="menuUpdated"
      />
    </v-timeline-item>
  </div>
</template>

<script lang="ts">
import { DateTime } from 'luxon'
import Vue, { PropType } from 'vue'
import { Dinner } from '~/types/Dinner'
import PlannedMealDetails from '~/components/Plan/PlannedDinnerDetails.vue'

export default Vue.extend({
  components: {
    PlannedMealDetails,
  },
  props: {
    dinner: {
      type: Object as PropType<Dinner>,
      required: true,
    },
    selected: {
      type: Boolean,
      required: true,
    },
  },
  data() {
    return {
      expanded: false,
      search: '',
    }
  },

  computed: {},
  methods: {
    getTimelineTextStyle(dinner: Dinner) {
      if (dinner.date.hasSame(DateTime.local(), 'day'))
        return 'font-weight-bold'
      return ''
    },
    formatDate(date: DateTime) {
      return date.toFormat('D')
    },
    formatDay(date: DateTime) {
      return date.toFormat('EEEE')
    },
    getDotColor(dinner: Dinner) {
      return dinner.isPlanned ? 'green' : 'grey'
    },
    getTitle(dinner: Dinner) {
      if (!dinner) return 'Dinner not found - weird.'
      if (!dinner.isPlanned) return this.getUnplannedTitle(dinner.date)
      return this.getPlannedTitle(dinner)
    },
    getPlannedTitle(dinner: Dinner) {
      return dinner.menu.map((item) => item.dishName).join(', ')
    },
    getUnplannedTitle(date: DateTime) {
      // when unplanned dinners are in the past we're tracking
      if (date < DateTime.now()) return 'Track dinner'
      // if in the future we're planning
      return 'Plan dinner'
    },
    menuUpdated(event: any) {
      this.$emit('dinner:menuupdated', event)
    },
  },
})
</script>

<style lang="scss" scoped>
.planning-card {
  margin-bottom: 20px;
}
</style>
