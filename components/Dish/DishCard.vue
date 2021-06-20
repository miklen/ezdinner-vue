<template>
  <span>
    <v-card rounded="lg">
      <v-card-title v-show="!editNameMode"
        ><v-row>
          <v-col>{{ name }}</v-col>
          <v-col cols="2">
            <v-icon @click="enableEditNameMode()">mdi-pencil</v-icon>
            <v-icon @click="confirmDialog = true">mdi-trash-can</v-icon></v-col
          >
        </v-row></v-card-title
      >
      <v-card-title v-show="editNameMode"
        ><v-row>
          <v-col
            ><v-text-field
              v-model="newName"
              autofocus
              dense
              @keyup.enter="doUpdateName()"
              @keyup.esc="editNameMode = false"
            ></v-text-field
          ></v-col>
          <v-col cols="2">
            <v-icon @click="doUpdateName()">mdi-check</v-icon>
            <v-icon @click="editNameMode = false">mdi-close</v-icon></v-col
          >
        </v-row></v-card-title
      >
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
      required: false,
      default(): DishStats {
        return { dishId: this.dish.id, lastUsed: undefined, timesUsed: 0 }
      },
    },
  },

  data() {
    return {
      confirmDialog: false,
      editNameMode: false,
      newName: '',
      name: '',
    }
  },

  mounted() {
    this.name = this.dish.name
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

    async doUpdateName() {
      // no need to fetch all dishes again just make it look like it's updated
      this.name = this.newName
      this.editNameMode = false
      try {
        await this.$repositories.dishes.updateName(this.dish.id, this.newName)
        await this.$accessor.dishes.updateDish({ dishId: this.dish.id })
      } catch (e) {
        // TODO: show error message
        this.name = this.dish.name
      }
    },

    enableEditNameMode() {
      this.newName = this.name
      this.editNameMode = true
    },

    getLastUsed() {
      return this.dishStats.lastUsed?.toLocaleString() ?? 'Never used'
    },
    getTimesUsed() {
      return this.dishStats.timesUsed
    },
  },
})
</script>
