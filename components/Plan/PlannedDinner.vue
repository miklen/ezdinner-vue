<template>
  <div>
    <v-timeline-item v-if="dinner.description" class="mb-4" color="green" small>
      <v-row
        v-show="!expanded"
        justify="space-between"
        @click="expanded = !expanded"
      >
        <v-col cols="6"
          >{{ dinner.description }}
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
    </v-timeline-item>
  </div>
</template>

<script lang="ts">
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
      dinnerName: '',
      search: '',
    }
  },
  created() {
    this.dinnerName = this.dinner.description
  },
})
</script>

<style lang="scss" scoped>
.planning-card {
  margin-bottom: 20px;
}
</style>
