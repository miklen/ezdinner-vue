<template>
  <Content :split="isFamilySelected">
    <v-row>
      <v-col class="text-center">
        <h1>Welcome</h1>
        <span>
          Get started by creating a family. Add family members to participate in
          planning. And begin tracking and planning your meals.
        </span>
      </v-col>
    </v-row>
    <v-row v-if="isFamilySelected">
      <v-col cols="12" xl="6">
        <v-card>
          <v-card-title>Dinner tonight</v-card-title>
          <v-card-text v-if="dinners[0] && dinners[0].menu.length">
            <v-list>
              <v-list-item
                v-for="menuItem in dinners[0].menu"
                :key="menuItem.dishId"
              >
                <v-list-item-title>{{ menuItem.dishName }}</v-list-item-title>
                <v-list-item-action style="display: inline">
                  <v-btn icon @click="routeTo(menuItem.dishId)">
                    <v-icon>mdi-information-outline</v-icon>
                  </v-btn>
                </v-list-item-action>
              </v-list-item>
            </v-list>
          </v-card-text>
          <v-card-text v-else>
            Nothing planned for tonight! Go to
            <router-link to="plan">Plan</router-link> and get organized!
          </v-card-text>
        </v-card>
      </v-col>

      <v-col cols="12" xl="6">
        <v-card>
          <v-card-title>Up for tomorrow</v-card-title>
          <v-card-text v-if="dinners[1] && dinners[1].menu.length">
            <v-list>
              <v-list-item
                v-for="menuItem in dinners[1].menu"
                :key="menuItem.dishId"
              >
                <v-list-item-title>{{ menuItem.dishName }}</v-list-item-title>
                <v-list-item-action style="display: inline">
                  <v-btn icon @click="routeTo(menuItem.dishId)">
                    <v-icon>mdi-information-outline</v-icon>
                  </v-btn>
                </v-list-item-action>
              </v-list-item>
            </v-list>
          </v-card-text>
          <v-card-text v-else>
            Nothing planned for tomorrow! Go to
            <router-link to="plan">Plan</router-link> and get organized!
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
    <template #support>
      <TopDishes />
    </template>
  </Content>
</template>

<script lang="ts">
import { DateTime } from 'luxon'
import Vue from 'vue'
import Content from '~/components/Content.vue'
import TopDishes from '~/components/Plan/TopDishes.vue'
import { Dinner } from '~/types/Dinner'
export default Vue.extend({
  components: {
    Content,
    TopDishes,
  },

  data() {
    return {
      dinners: [] as Dinner[],
    }
  },

  async fetch() {
    await this.init()
  },
  head: {
    title: 'Home',
  },

  computed: {
    isFamilySelected() {
      return !!this.$accessor.activeFamilyId
    },
  },

  watch: {
    '$accessor.activeFamilyId'() {},
  },
  methods: {
    async init() {
      if (!this.isFamilySelected) return
      await this.$accessor.dishes.populateDishes()
      const today = DateTime.now()
      const dinners = await this.$repositories.dinners.getRange(
        this.$accessor.activeFamilyId,
        today,
        today.plus({ days: 1 }),
      )

      // client-side join
      // TODO: Create query API that is specialized for this request
      // TODO: This is copy/pasted from store - refactor to keep DRY
      this.dinners = dinners.map((dinner: any) => {
        dinner.date = DateTime.fromISO(dinner.date)
        dinner.menu.map((item: any) => {
          item.dishName =
            this.$accessor.dishes.dishMap[item.dishId] ?? 'Dish not available'
          // TODO: add receipeName
          return item
        })
        return dinner
      })
    },
    routeTo(id: string) {
      this.$router.push({ name: 'dishes-id', params: { id } })
    },
  },
})
</script>
