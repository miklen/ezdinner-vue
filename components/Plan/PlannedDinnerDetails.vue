<template>
  <v-card>
    <v-card-subtitle @click="$emit('dinner:close')">
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
                item-text="name"
                item-value="id"
                outlined
                label="Add dish to menu"
                placeholder="Start typing to search"
                @input="addDishToMenu($event.id, $event.recipeId)"
                @keyup.enter.native="createDish"
              >
                <template #item="{ item }">
                  <template v-if="!item.recipeId">
                    <v-list-item-content>{{ item.name }}</v-list-item-content>
                  </template>
                  <template v-else>
                    <v-list-item-content>{{ item.name }}</v-list-item-content>
                    <v-list-item-subtitle>
                      {{ item.recipeId }}</v-list-item-subtitle
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
            <v-list-item v-for="menuItem in dinner.menu" :key="menuItem.dishId">
              <v-list-item-title>{{ menuItem.dishName }}</v-list-item-title>
              <v-list-item-subtitle>{{
                menuItem.recipeName
              }}</v-list-item-subtitle>
              <v-list-item-action>
                <v-btn
                  icon
                  @click="
                    removeDishFromMenu(menuItem.dishId, menuItem.recipeId)
                  "
                >
                  <v-icon>mdi-close-circle-outline</v-icon>
                </v-btn>
              </v-list-item-action>
            </v-list-item>
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
      const element = this.$refs.dishSelector as HTMLElement
      element.blur()
      const dishId = await this.$repositories.dishes.create(
        this.$accessor.activeFamilyId,
        this.dishSearch,
      )
      this.dishSearch = ''
      // repopulate dishes to add the new dish to the list
      this.$accessor.dishes.populateDishes()
      this.addDishToMenu(dishId, null)
    },
    async addDishToMenu(dishId: string, recipeId: string | null) {
      await this.$repositories.dinners.addDishToMenu(
        this.$accessor.activeFamilyId,
        this.dinner.date,
        dishId,
        recipeId,
      )
      const dish = this.dishVariants.find((d) => d.id === dishId)
      this.$emit('dinner:menuupdated', {
        date: this.dinner.date,
        dishId,
        recipeId,
        dishName: dish?.name,
        receipeName: dish?.recipeName,
      })
      this.selectedDish = ''
    },
    async removeDishFromMenu(dishId: string, recipeId: string | null) {
      await this.$repositories.dinners.removeDishFromMenu(
        this.$accessor.activeFamilyId,
        this.dinner.date,
        dishId,
        recipeId,
      )
      this.$emit('dinner:menuupdated', {
        date: this.dinner.date,
        dishId,
        recipeId,
        dishName: '',
        receipeName: '',
      })
    },
  },
})
</script>
