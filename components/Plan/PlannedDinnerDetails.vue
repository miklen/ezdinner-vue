<template>
  <v-card>
    <v-card-subtitle>
      {{ formatDate(dinner.date) }}
    </v-card-subtitle>
    <v-card-text>
      <v-row>
        <v-col> </v-col>
      </v-row>
      <v-row>
        <v-col>
          <v-combobox
            v-model="tags"
            :items="availableTags"
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
    <v-card-actions
      ><v-btn outlined color="primary">SAVE</v-btn
      ><v-btn outlined color="primary" @click="$emit('cancel')"
        >CANCEL</v-btn
      ></v-card-actions
    >
  </v-card>
</template>

<script lang="ts">
import Vue, { PropType } from 'vue'
import { DateTime } from 'luxon'
import { Tag } from '~/types/Tag'
import { Dinner } from '~/types/Dinner'

export default Vue.extend({
  props: {
    dinner: {
      type: Object as PropType<Dinner>,
      required: true,
    },
  },
  data() {
    return {
      dinnerName: '',
      search: '',
      tags: [] as Tag[],
      availableTags: [] as Tag[],
    }
  },
  created() {
    this.tags = this.dinner.tags
    this.availableTags = []
    this.dinnerName = this.dinner.description
  },
  methods: {
    formatDate(date: DateTime) {
      return date.toLocaleString(DateTime.DATE_HUGE)
    },
  },
})
</script>
