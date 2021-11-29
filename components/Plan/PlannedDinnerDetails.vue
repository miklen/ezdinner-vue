<template>
  <v-card>
    <v-card-subtitle style="cursor: pointer" @click="$emit('dinner:close')">
      <v-row>
        <v-col>{{ formatDate(dinner.date) }}</v-col>
        <v-col cols="1"><v-icon>mdi-close-circle-outline</v-icon></v-col>
      </v-row>
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
                item-text="name"
                item-value="id"
                outlined
                label="Add dish to menu"
                placeholder="Start typing to search"
                @input="addDishToMenu($event.id)"
                @keyup.enter.native="createDish"
              >
                <template #item="{ item }">
                  <v-list-item-content>{{ item.name }}</v-list-item-content>
                  <v-list-item-action>
                    <v-rating
                      dense
                      color="primary"
                      half-increments
                      empty-icon="mdi-heart-outline"
                      full-icon="mdi-heart"
                      half-icon="mdi-heart-half-full"
                      length="5"
                      size="20"
                      :value="item.rating"
                      readonly
                    ></v-rating>
                  </v-list-item-action>
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
            <v-list-item v-for="menuItem in dinner.menu" :key="menuItem.dishId">
              <v-list-item-title>{{ menuItem.dishName }}</v-list-item-title>
              <v-list-item-action>
                <v-btn icon @click="removeDishFromMenu(menuItem.dishId)">
                  <v-icon>mdi-close-circle-outline</v-icon>
                </v-btn>
              </v-list-item-action>
            </v-list-item>
          </v-list>
        </v-col>
      </v-row>

      <!-- <v-row>
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
      </v-row> -->
    </v-card-text>
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
  },
  methods: {
    formatDate(date: DateTime) {
      return date.toLocaleString(DateTime.DATE_HUGE)
    },
    async createDish() {
      const dishName = this.dishSearch
      this.dishSearch = ''
      const element = this.$refs.dishSelector as HTMLElement
      element.blur()
      const dishId = await this.$repositories.dishes.create(
        this.$accessor.activeFamilyId,
        dishName,
      )
      // repopulate dishes to add the new dish to the list
      await this.$accessor.dishes.updateDish({ dishId })
      await this.addDishToMenu(dishId)
    },
    async addDishToMenu(dishId: string) {
      await this.$repositories.dinners.addDishToMenu(
        this.$accessor.activeFamilyId,
        this.dinner.date,
        dishId,
      )
      const dish = this.dishVariants.find((d) => d.id === dishId)
      this.$emit('dinner:menuupdated', {
        date: this.dinner.date,
        dishId,
        dishName: dish?.name,
      })
      this.selectedDish = ''
    },
    async removeDishFromMenu(dishId: string) {
      await this.$repositories.dinners.removeDishFromMenu(
        this.$accessor.activeFamilyId,
        this.dinner.date,
        dishId,
      )
      this.$emit('dinner:menuupdated', {
        date: this.dinner.date,
        dishId,
        dishName: '',
      })
    },
  },
})
</script>
