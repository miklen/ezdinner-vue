<template>
  <div>
    <v-timeline-item v-if="dinner.isPlanned" class="mb-4" color="green" small>
      <v-row
        v-show="!expanded"
        justify="space-between"
        @click="expanded = !expanded"
      >
        <v-col cols="6"
          >{{ getPlannedTitle(dinner) }}
          <v-chip
            v-for="tag in dinner.tags"
            :key="tag.value"
            outlined
            x-small
            :color="tag.color || 'primary'"
            >{{ tag.value }}</v-chip
          ></v-col
        >
        <v-col
          class="text-right"
          cols="5"
          v-text="formatDay(dinner.date)"
        ></v-col>
      </v-row>

      <PlannedMealDetails
        v-show="expanded"
        :dinner="dinner"
        @cancel="expanded = false"
      />
    </v-timeline-item>
    <v-timeline-item v-else class="mb-4" color="grey" small>
      <v-row justify="space-between" @click="expanded = !expanded">
        <v-col cols="6">{{ getUnplannedTitle(dinner.date) }}</v-col>
        <v-col class="text-right" cols="5">
          <v-row>
            <v-col
              >{{ formatDay(dinner.date) }}
              <span class="text-caption">{{
                formatDate(dinner.date)
              }}</span></v-col
            >
          </v-row>
        </v-col>
      </v-row>

      <PlannedMealDetails
        v-show="expanded"
        :dinner="dinner"
        @cancel="expanded = false"
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
  },
  data() {
    return {
      expanded: false,
      search: '',
    }
  },

  computed: {},
  methods: {
    formatDate(date: DateTime) {
      return date.toFormat('D')
    },
    formatDay(date: DateTime) {
      return date.toFormat('EEEE')
    },
    getPlannedTitle(dinner: Dinner) {
      if (!dinner) return 'Dinner not found'
      return dinner.menu.map((item) => item.dishName).join()
    },
    getUnplannedTitle(date: DateTime) {
      // when unplanned dinners are in the past we're tracking
      if (date < DateTime.now()) return 'Track dinner'
      // if in the future we're planning
      return 'Plan dinner'
    },
  },
})
</script>

<style lang="scss" scoped>
.planning-card {
  margin-bottom: 20px;
}
</style>
