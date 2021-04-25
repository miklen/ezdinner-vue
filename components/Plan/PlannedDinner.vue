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
          v-text="dinner.date.toFormat('EEEE')"
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
        <v-col cols="6">Click to plan</v-col>
        <v-col
          class="text-right"
          cols="5"
          v-text="dinner.date.toFormat('EEEE')"
        ></v-col>
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
import Vue, { PropType } from 'vue'
import { Dinner } from '~/types/Dinner'
import PlannedMealDetails from '~/components/Plan/PlannedDinnerDetails.vue'
import { DateTime } from 'luxon'

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

  computed: {
    dishMap(): { [key: string]: string } {
      return this.$accessor.dishes.dishes.reduce(
        (prev, current) => (prev[current.id] = current.name),
        {} as any,
      )
    },
  },
  methods: {
    getPlannedTitle(dinner: Dinner) {
      if (!dinner) return 'Dinner not found'
      return dinner.menu.map(item => this.dishMap[item.dishId]).join()
    },
    getUnplannedTitle(date: DateTime) {
      // when unplanned dinners are in the past we're tracking
      if (date < DateTime.now()) return 'Track dinner'
      // if in the future we're planning
      return 'Plan dinner'
    }
  }
})
</script>

<style lang="scss" scoped>
.planning-card {
  margin-bottom: 20px;
}
</style>
