<template>
  <span>
    <v-row style="padding-bottom: 16px">
      <!-- main content start -->
      <v-col cols="12" md="8">
        <v-row>
          <v-col>
            <DishCard :dish="dish" :dish-stats="dish.dishStats" />
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
                  <v-btn color="primary" @click="doUpdateNotes()">Save</v-btn>
                </v-card-actions>
                <v-card-actions v-show="!editNotesMode">
                  <v-btn color="primary" @click="enableEditNotesMode()"
                    >Edit</v-btn
                  >
                </v-card-actions></v-card-title
              >
              <v-card-text v-show="url || editNotesMode">
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
              </v-card-text>
              <v-row class="notes-area">
                <v-col v-if="editNotesMode">
                  <textarea ref="textarea" v-model="notes"></textarea>
                </v-col>
                <v-col v-else>
                  <!-- eslint-disable-next-line vue/no-v-html -->
                  <span v-html="notesHtml"></span>
                </v-col>
              </v-row>
            </v-card>
          </v-col>
        </v-row>
      </v-col>
      <!-- main content end -->

      <!-- side panel start -->
      <v-col>
        <v-row>
          <v-col>
            <v-card>
              <v-card-title>Family rating</v-card-title>
              <v-card-text>
                <v-row
                  v-for="familyMember in familyMembers"
                  :key="familyMember.id"
                  ><v-col cols="1">
                    <v-avatar
                      class="white--text"
                      :color="familyMember.id === userId ? 'primary' : 'grey'"
                      size="40"
                      >{{ getInitials(familyMember.name) }}</v-avatar
                    ></v-col
                  ><v-col>
                    <v-rating
                      color="primary"
                      half-increments
                      empty-icon="mdi-heart-outline"
                      full-icon="mdi-heart"
                      half-icon="mdi-heart-half-full"
                      length="5"
                      size="25"
                      :value="ratings[familyMember.id]"
                      :readonly="familyMember.id !== userId"
                      @input="updateRating($event)"
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
              <v-card-title>Dates<v-spacer></v-spacer></v-card-title>
              <v-card-subtitle
                >You last had {{ dish.name }} {{ getDaysAgo() }} days
                ago</v-card-subtitle
              >
              <v-simple-table>
                <template #default>
                  <thead>
                    <tr>
                      <th class="text-left">Date</th>
                      <th class="text-center">Days since previous</th>
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
            </v-card>
          </v-col>
        </v-row>
      </v-col>
      <!-- side panel end -->
    </v-row>
  </span>
</template>

<script lang="ts">
import Vue from 'vue'
import EasyMDE from 'easymde'
// ignoring due to an import error that says marked should be default imported. It only works like this though
// @ts-ignore
import { marked } from 'marked'
import 'easymde/dist/easymde.min.css'
import { DateTime } from 'luxon'
import DishCard from '~/components/Dish/DishCard.vue'
import { Dish, DinnerDate } from '~/types/Dish'
import { FamilyMember } from '~/types/Family'

export default Vue.extend({
  components: {
    DishCard,
  },
  data() {
    return {
      rating: 0,
      dish: {} as Dish,
      url: '',
      editNotesMode: false,
      mde: null as EasyMDE | null,
      notes: '',
      notesHtml: '',
      dates: [] as DinnerDate[],
    }
  },

  async fetch() {
    await this.init()
  },

  computed: {
    dishItems(): Dish[] {
      return this.$accessor.dishes.dishes
        .filter((w) => w.id !== this.dish.id)
        .sort((a, b) => a.name.localeCompare(b.name))
    },
    familyMembers(): FamilyMember[] {
      return this.$accessor.families.activeFamily?.familyMembers ?? []
    },
    userId() {
      return this.$msal.getObjectId()
    },
    ratings(): { [key: string]: number } {
      return Object.fromEntries(
        this.dish?.ratings?.map((e) => [e.familyMemberId, e.rating]) ?? [],
      )
    },
  },

  mounted() {},

  methods: {
    async init() {
      this.dish = await this.$repositories.dishes.getFull(this.$route.params.id)
      this.notes = this.dish.notes
      this.notesHtml = marked.parse(this.dish.notes || '')
      this.url = this.dish.url
      this.dates = this.dish.dates
    },

    async updateRating(newRating: number) {
      await this.$repositories.dishes.updateRating(this.dish.id, newRating)
      this.$emit('rating:updated')
      this.init()
    },

    async doUpdateNotes() {
      try {
        await this.$repositories.dishes.updateNotes(
          this.dish.id,
          this.notes,
          this.url,
        )
        this.disableEditNotesMode()
      } catch (e) {
        // TODO replace with pretty component
        alert('An error occured')
      }
    },

    enableEditNotesMode() {
      this.editNotesMode = true
      // workaround to prevent textarea not being defined when deeplinking/cold-loading the page
      const interval = setInterval(() => {
        if (!this.$refs.textarea) return
        this.mde = new EasyMDE({
          element: this.$refs.textarea as HTMLElement,
          spellChecker: false,
        })
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

    formatDate(date: string) {
      return DateTime.fromISO(date).toLocaleString(DateTime.DATE_FULL)
    },

    getDaysAgo() {
      if (!this.dish?.dishStats?.lastUsed) return 0
      return Math.floor(
        DateTime.now()
          .diff(DateTime.fromISO(this.dish.dishStats.lastUsed), 'days')
          .toObject()?.days || 0,
      )
    },

    getInitials(name: string) {
      const parts = name.split(' ')
      if (parts.length === 0) return '?'
      if (parts.length === 1) return parts[0][0].toUpperCase()
      return (
        parts[0][0].toUpperCase() + parts[parts.length - 1][0].toUpperCase()
      )
    },
  },
})
</script>

<style lang="scss">
.notes-area {
  padding: 16px;
}
</style>
