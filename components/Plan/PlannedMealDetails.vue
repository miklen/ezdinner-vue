<template>
  <v-timeline-item v-show="expanded" class="mb-4" hide-dot>
    <v-card>
      <v-card-text>
        <v-row>
          <v-col>
            <v-text-field
              v-model="mealName"
              label="What's for dinner?"
            ></v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <v-combobox
              v-model="tags"
              :items="items"
              :search-input.sync="search"
              hide-selected
              hint="What makes this day stand out?"
              label="Add some tags"
              multiple
              persistent-hint
              small-chips
              deletable-chips
            >
              <template #no-data>
                <v-list-item>
                  <v-list-item-content>
                    <v-list-item-title>
                      No results matching "<strong>{{ search }}</strong
                      >". Press <kbd>enter</kbd> to create a new one
                    </v-list-item-title>
                  </v-list-item-content>
                </v-list-item>
              </template>
            </v-combobox>
          </v-col>
        </v-row>
      </v-card-text>
      <v-card-actions><v-btn>SAVE</v-btn><v-btn>CANCEL</v-btn></v-card-actions>
    </v-card>
  </v-timeline-item>
</template>

<script lang="ts">
import Vue, { PropType } from 'vue'
import { Meal } from '~/types/Meal'

export default Vue.extend({
  props: {
    meal: {
      type: Object as PropType<Meal>,
      required: true,
    },
  },
  data() {
    return {
      mealName: '',
      search: '',
      tags: [] as string[],
    }
  },
  created() {
    this.tags = this.meal.tags
    this.mealName = this.meal.name
  },
})
</script>
