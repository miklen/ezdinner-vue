<template>
  <span>
    <v-card rounded="lg">
      <v-card-title v-show="!editNameMode" @click="routeTo(dish.id)"
        ><v-row style="overflow: hidden">
          <v-col style="word-break: normal; cursor: pointer">{{ name }}</v-col>
          <v-col class="text-right" cols="4" sm="3" lg="5" xl="3">
            <v-icon @click.stop="enableEditNameMode()">mdi-pencil</v-icon>
            <v-icon @click.stop="moveDialog = true">mdi-transfer-right</v-icon>
            <v-icon @click.stop="confirmDialog = true"
              >mdi-trash-can</v-icon
            ></v-col
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
            <v-icon @click.stop="doUpdateName()">mdi-check</v-icon>
            <v-icon @click.stop="editNameMode = false">mdi-close</v-icon></v-col
          >
        </v-row></v-card-title
      >
      <v-card-subtitle
        ><v-row>
          <v-rating
            color="primary"
            half-increments
            empty-icon="mdi-heart-outline"
            full-icon="mdi-heart"
            half-icon="mdi-heart-half-full"
            length="5"
            size="20"
            :value="dish.rating"
            readonly
          ></v-rating> </v-row
      ></v-card-subtitle>

      <v-card-text>
        <v-row>
          <v-col v-if="dishStats.timesUsed > 0">
            You've had {{ name }} for dinner
            {{ getTimesUsedText() }}
          </v-col>
        </v-row>
        <v-row>
          <v-col v-if="dishStats.lastUsed"
            >Last was {{ getDaysAgo() }} days ago on {{ getLastUsed() }}</v-col
          >
          <v-col v-else>Never been planned for dinner</v-col>
        </v-row>
      </v-card-text>
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
          <v-btn text @click="confirmDialog = false">Cancel</v-btn>
          <v-btn text color="error" @click="doDelete()">Delete</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="moveDialog" width="400">
      <v-card>
        <v-card-title
          >Move {{ dishStats.timesUsed }} dinner occurrences?</v-card-title
        >
        <v-card-text
          >Move all tracked dinner occurences of {{ dish.name }} to be tracked
          as
          <v-autocomplete
            v-model="moveToDish"
            :items="dishItems"
            item-text="name"
            return-object
            style="width: 50%; display: inline-block; margin-left: 10px"
          ></v-autocomplete
        ></v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn text @click="moveDialog = false">Cancel</v-btn>
          <v-btn text color="error" @click="doMove()">Move</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </span>
</template>

<script lang="ts">
import { DateTime } from 'luxon'
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
      moveDialog: false,
      moveToDish: {} as Dish,
      rating: 0,
    }
  },

  computed: {
    dishItems(): Dish[] {
      return this.$accessor.dishes.dishes
        .filter((w) => w.id !== this.dish.id)
        .sort((a, b) => a.name.localeCompare(b.name))
    },
  },

  watch: {
    'dish.name'() {
      this.name = this.dish.name
    },
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

    async doMove() {
      await this.$repositories.dinners.moveDinnerDishes(
        this.$accessor.activeFamilyId,
        this.dish.id,
        this.moveToDish.id,
      )
      this.moveDialog = false
      this.$emit('menuitem:moved')
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
      if (!this.dishStats.lastUsed) return 'Never used'
      return this.dishStats.lastUsed.toLocaleString(DateTime.DATE_FULL)
    },

    getDaysAgo() {
      if (!this.dishStats.lastUsed) return 0
      return Math.floor(
        DateTime.now().diff(this.dishStats.lastUsed, 'days').toObject()?.days ||
          0,
      )
    },

    getTimesUsedText() {
      const result = `${this.dishStats.timesUsed} time`
      return this.dishStats.timesUsed === 1 ? result : result + 's'
    },

    routeTo(id: string) {
      this.$router.push({ name: 'dishes-id', params: { id } })
    },
  },
})
</script>
