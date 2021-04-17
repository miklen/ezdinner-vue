<template>
  <v-card>
    <v-card-subtitle>
      {{ formatDate(dinner.date) }}
    </v-card-subtitle>
    <v-card-text>
      <v-row>
        <v-col>
          <v-row>
            <v-col>
              <v-autocomplete
                ref="dishSelector"
                v-model="selectedDish"
                :items="dishVariants"
                :search-input.sync="dishSearch"
                return-object
                item-text="dishName"
                item-value="dishId"
                dense
                filled
                rounded
                solo
                label="Select a dish or create one"
                @input="addDishToMenu($event.dishId, $event.receipeId)"
              >
                <template #item="{ item }">
                  <template v-if="!item.receipeId">
                    <v-list-item-content>{{
                      item.dishName
                    }}</v-list-item-content>
                  </template>
                  <template v-else>
                    <v-list-item-content>{{
                      item.dishName
                    }}</v-list-item-content>
                    <v-list-item-subtitle>
                      {{ item.receipeId }}</v-list-item-subtitle
                    >
                  </template>
                </template>

                <template #no-data>
                  <v-list-item v-show="!dishSearch">
                    <span class="subheading">Enter name of dish</span>
                  </v-list-item>
                  <v-list-item v-show="dishSearch" @click="createDish">
                    <span class="subheading">Create {{ dishSearch }}</span>
                  </v-list-item>
                </template>
              </v-autocomplete>
            </v-col>
          </v-row>
          <v-list>
            <v-list-item-content
              >Nothing planned yet. Add a dish to the menu</v-list-item-content
            >
          </v-list>
        </v-col>
      </v-row>

      <v-row>
        <v-col>
          <v-combobox
            v-model="tags"
            :items="availableTags"
            :search-input.sync="tagSearch"
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
                    No results matching "<strong>{{ tagSearch }}</strong
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
      tagSearch: '',
      tags: [] as Tag[],
      availableTags: [] as Tag[],
      dishSearch: '',
      selectedDish: '',
    }
  },
  computed: {
    dishVariants() {
      return this.$accessor.dishes.dishes
    },
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
    async createDish() {
      const element = this.$refs.dishSelector as HTMLElement
      element.blur()
      const result = await this.$axios.post('api/dishes', {
        name: this.dishSearch,
        familyId: this.$accessor.activeFamilyId,
      })
      this.dishSearch = ''
      // repopulate dishes to add the new dish to the list
      this.$accessor.dishes.populateDishes()
      const dishId = result.data as string
      this.addDishToMenu(dishId, null)
    },
    addDishToMenu(dishId: string, receipeId: string | null) {
      return this.$axios.put('api/dinner/menuitem', {
        date: this.dinner.date,
        dishId,
        receipeId,
      })
    },
  },
})
</script>
