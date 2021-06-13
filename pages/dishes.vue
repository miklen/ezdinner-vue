<template>
  <span>
    <v-row>
      <v-col cols="4">
        <v-text-field v-model="searchDish" label="Search dishes"></v-text-field>
      </v-col>
    </v-row>
    <v-row>
      <v-col
        v-for="dish in dishes"
        :key="dish.id"
        xs="12"
        sm="12"
        md="6"
        lg="4"
      >
        <v-card rounded="lg">
          <v-card-title>{{ dish.name }}</v-card-title>
          <!-- <v-card-subtitle>Recipes</v-card-subtitle>
          <v-card-text v-if="!dish.recipes || !dish.recipes.length"
            >No recipes yet</v-card-text
          > -->
          <!-- <v-list>
            <v-list-item>
              <v-list-item-content>
                <v-list-item-subtitle>Times for dinner: 1</v-list-item-subtitle>
              </v-list-item-content>
            </v-list-item>
            <v-list-item>
              <v-list-item-content>
                <v-list-item-subtitle
                  >Last time used: 2021-01-01</v-list-item-subtitle
                >
              </v-list-item-content>
            </v-list-item>
          </v-list> -->
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn
              v-bind="attrs"
              text
              color="grey darken-1"
              v-on="on"
              @click="openConfirmDialog(dish)"
              ><v-icon>mdi-trash-can</v-icon></v-btn
            >
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
    <v-dialog v-model="confirmDialog" width="400">
      <v-card>
        <v-card-title>Delete?</v-card-title>
        <v-card-text
          >Are you sure you want to delete
          <strong>{{ dishToDelete.name }}</strong></v-card-text
        >
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="grey darken-1" @click="closeConfirmDialog"
            >Cancel</v-btn
          >
          <v-btn color="error" @click="doDelete(dishToDelete.id)">Delete</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </span>
</template>

<script lang="ts">
import Vue from 'vue'
import { Dish } from '~/types/Dish'
export default Vue.extend({
  data() {
    return {
      confirmDialog: false,
      dishToDelete: {} as Dish,
      searchDish: '',
    }
  },
  computed: {
    dishes(): Dish[] {
      return [...this.$accessor.dishes.dishes]
        .sort((a, b) => a.name.localeCompare(b.name))
        .filter((dish) => dish.name.includes(this.searchDish))
    },
  },

  async created() {
    await this.$accessor.dishes.populateDishes()
  },

  methods: {
    openConfirmDialog(dishToDelete: Dish) {
      this.dishToDelete = dishToDelete
      this.confirmDialog = true
    },
    closeConfirmDialog() {
      this.confirmDialog = false
      this.dishToDelete = {} as Dish
    },
    async doDelete(id: string) {
      await this.$repositories.dishes.delete(this.$accessor.activeFamilyId, id)
      this.confirmDialog = false
      this.$accessor.dishes.populateDishes()
    },
  },
})
</script>
