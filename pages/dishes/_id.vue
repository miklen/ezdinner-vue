<template>
  <Content split>
    <v-card-title v-show="!editNameMode"
      ><v-row style="overflow: hidden">
        <v-col style="word-break: normal">{{ name }}</v-col>
        <v-col cols="3" md="1">
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
          @input="setRating($event)"
        ></v-rating> </v-row
    ></v-card-subtitle>

    <v-row>
      <v-col>
        <v-card>
          <v-card-title>Family rating</v-card-title>
          <v-card-text>
            <v-row v-for="familyMember in familyMembers" :key="familyMember"
              ><v-col cols="1">
                <v-avatar class="white--text" color="primary" size="36">{{
                  familyMember
                }}</v-avatar></v-col
              ><v-col>
                <v-rating
                  color="primary"
                  half-increments
                  empty-icon="mdi-heart-outline"
                  full-icon="mdi-heart"
                  half-icon="mdi-heart-half-full"
                  length="5"
                  size="20"
                  :value="dish.rating"
                  @input="setRating($event)"
                ></v-rating>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <v-row>
      <v-col>
        <v-card>
          <v-card-title
            >Recipe &amp; notes
            <v-spacer></v-spacer>
            <v-card-actions v-show="editNotesMode">
              <v-btn @click="disableEditNotesMode()">Cancel</v-btn>
              <v-btn color="primary">Save</v-btn>
            </v-card-actions>
            <v-card-actions v-show="!editNotesMode">
              <v-btn color="primary" @click="enableEditNotesMode()">Edit</v-btn>
            </v-card-actions></v-card-title
          >
          <v-card-text>
            <v-row>
              <v-col>
                <v-icon class="float-left" style="margin-right: 5px"
                  >mdi-link-variant</v-icon
                >
                <v-text-field
                  v-show="editNotesMode"
                  v-model="url"
                  autofocus
                  dense
                ></v-text-field>
                <a v-show="!editNotesMode" :href="url" target="_blank">{{
                  url
                }}</a>
              </v-col>
            </v-row>
            <v-row>
              <v-col v-if="editNotesMode">
                <textarea ref="textarea" v-model="notes"></textarea>
              </v-col>

              <v-col v-else>
                <!-- eslint-disable-next-line vue/no-v-html -->
                <span v-html="notesHtml"></span>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <v-dialog v-model="confirmDialog" width="400">
      <v-card>
        <v-card-title>Delete?</v-card-title>
        <v-card-text
          >Are you sure you want to delete <strong>{{ dish.name }}</strong
          >?</v-card-text
        >
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="confirmDialog = false">Cancel</v-btn>
          <v-btn color="error" @click="doDelete()">Delete</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="moveDialog" width="400">
      <v-card>
        <v-card-title
          >Move {{ getTimesUsed() }} dinner occurrences?</v-card-title
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
          <v-btn color="grey darken-1" @click="moveDialog = false"
            >Cancel</v-btn
          >
          <v-btn color="error" @click="doMove()">Move</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <template #support>
      <v-card-title>Dates</v-card-title>

      <v-simple-table>
        <template #default>
          <thead>
            <tr>
              <th class="text-left">Date</th>
              <th class="text-center">Days since last</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="date in dates" :key="date.date">
              <td>{{ formatDate(date.date) }}</td>
              <td class="text-center">{{ date.daysSinceLast }}</td>
            </tr>
          </tbody>
        </template>
      </v-simple-table>
    </template>
  </Content>
</template>

<script lang="ts">
import Vue from 'vue'
import EasyMDE from 'easymde'
import { marked } from 'marked'
import 'easymde/dist/easymde.min.css'
import { DateTime } from 'luxon'
import { Dish, DishStats, DinnerDate } from '~/types/Dish'

export default Vue.extend({
  data() {
    return {
      confirmDialog: false,
      editNameMode: false,
      newName: '',
      name: '',
      moveDialog: false,
      moveToDish: {} as Dish,
      rating: 0,
      dish: {} as Dish,
      dishStats: {} as DishStats,
      familyMembers: ['MN', 'LN'],
      url: '',
      editNotesMode: false,
      mde: null as EasyMDE | null,
      notes: '',
      notesHtml: '',
      dates: [] as DinnerDate[],
    }
  },

  async fetch() {
    this.dish = await this.$repositories.dishes.getFull(this.$route.params.id)
    this.name = this.dish.name
    this.notes = this.dish.notes
    this.notesHtml = marked.parse(this.dish.notes || '')
    this.url = this.dish.url
    this.dates = this.dish.dates
  },

  computed: {
    dishItems(): Dish[] {
      return this.$accessor.dishes.dishes
        .filter((w) => w.id !== this.dish.id)
        .sort((a, b) => a.name.localeCompare(b.name))
    },
  },

  mounted() {},

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

    async setRating(newRating: number) {
      await this.$repositories.dishes.setRating(this.dish.id, newRating)
      this.$emit('rating:updated')
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

    async saveNotes() {},

    enableEditNameMode() {
      this.newName = this.name
      this.editNameMode = true
    },

    enableEditNotesMode() {
      this.editNotesMode = true
      // workaround to prevent textarea not being defined when deeplinking/cold-loading the page
      const interval = setInterval(() => {
        if (!this.$refs.textarea) return
        this.mde = new EasyMDE({ element: this.$refs.textarea as HTMLElement })
        this.mde.codemirror.on('change', () => {
          this.notes = this.mde?.value()!
          this.notesHtml = marked.parse(this.notes)
        })
        this.notesHtml = marked.parse(this.notes)
        clearInterval(interval)
      }, 50)
    },

    disableEditNotesMode() {
      this.editNotesMode = false
      this.mde?.toTextArea()
      this.mde = null
    },

    getLastUsed() {
      return this.dishStats.lastUsed?.toLocaleString() ?? 'Never used'
    },
    getTimesUsed() {
      return this.dishStats.timesUsed
    },

    formatDate(date: string) {
      return DateTime.fromISO(date).toLocaleString(DateTime.DATE_FULL)
    },
  },
})
</script>
