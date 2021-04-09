<template>
  <div>
    <v-timeline-item v-if="meal.name" class="mb-4" color="green" small>
      <v-row justify="space-between" @click="expanded = !expanded">
        <v-col cols="6"
          >{{ meal.name }}
          <v-chip
            v-for="tag in meal.tags"
            :key="tag"
            outlined
            x-small
            color="primary"
            >{{ tag }}</v-chip
          ></v-col
        >
        <v-col
          class="text-right"
          cols="5"
          v-text="meal.date.toFormat('EEEE')"
        ></v-col>
      </v-row>
    </v-timeline-item>
    <v-timeline-item v-else class="mb-4" color="grey" small>
      <v-row justify="space-between" @click="expanded = !expanded">
        <v-col cols="6">Click to plan</v-col>
        <v-col
          class="text-right"
          cols="5"
          v-text="meal.date.toFormat('EEEE')"
        ></v-col>
      </v-row>
    </v-timeline-item>
    <PlannedMealDetails v-show="expanded" :meal="meal" />
  </div>
</template>

<script lang="ts">
import Vue, { PropType } from 'vue'
import { Meal } from '~/types/Meal'
import PlannedMealDetails from '~/components/Plan/PlannedMealDetails.vue'

export default Vue.extend({
  components: {
    PlannedMealDetails,
  },
  props: {
    meal: {
      type: Object as PropType<Meal>,
      required: true,
    },
  },
  data() {
    return {
      expanded: false,
      mealName: '',
      search: '',
    }
  },
  created() {
    this.mealName = this.meal.name
  },
})
</script>

<style lang="scss" scoped>
.planning-card {
  margin-bottom: 20px;
}
</style>
