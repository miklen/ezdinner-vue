<template>
  <span>
    <v-card rounded="lg">
      <v-card-title>{{ dish.name }}</v-card-title>
      <!-- <v-card-subtitle>Recipes</v-card-subtitle>
            <v-card-text v-if="!dish.recipes || !dish.recipes.length"
              >No recipes yet</v-card-text
            > -->
      <v-list>
        <v-list-item>
          <v-list-item-content>
            <v-list-item-subtitle
              >Times for dinner: {{ getTimesUsed() }}</v-list-item-subtitle
            >
          </v-list-item-content>
        </v-list-item>
        <v-list-item>
          <v-list-item-content>
            <v-list-item-subtitle
              >Last time used: {{ getLastUsed() }}</v-list-item-subtitle
            >
          </v-list-item-content>
        </v-list-item>
      </v-list>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn text color="grey darken-1" @click="confirmDialog = true"
          ><v-icon>mdi-trash-can</v-icon></v-btn
        >
      </v-card-actions>
    </v-card>

    <v-dialog v-model="confirmDialog" width="400">
      <v-card>
        <v-card-title>Delete?</v-card-title>
        <v-card-text
          >Are you sure you want to delete <strong>{{ dish.name }}</strong
          >?</v-card-text
        >
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="grey darken-1" @click="confirmDialog = false"
            >Cancel</v-btn
          >
          <v-btn color="error" @click="doDelete()">Delete</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </span>
</template>

<script lang="ts">
import Vue, { PropType } from 'vue'
import { Dish, DishStats } from '~/types/Dish'
export default Vue.extend({
  props: {
    dish: {
      type: Object as PropType<Dish>,
      required: true,
    },
    dishStats: {
      type: Object as PropType<DishStats>,
      required: true,
    },
  },

  data() {
    return {
      confirmDialog: false,
    }
  },

  methods: {
    async doDelete() {
      await this.$repositories.dishes.delete(
        this.$accessor.activeFamilyId,
        this.dish.id,
      )
      this.confirmDialog = false
      this.$accessor.dishes.populateDishes()
    },

    getLastUsed() {
      return this.dishStats?.lastUsed.toLocaleString() ?? 'Never used'
    },
    getTimesUsed() {
      return this.dishStats?.timesUsed ?? 0
    },
  },
})
</script>
