<template>
  <Content split>
    <v-row>
      <v-col class="text-center">
        <h1>Welcome</h1>
        <span>
          Get started by creating a family. Add family members to participate in
          planning. And begin tracking and planning your meals.
        </span>
      </v-col>
    </v-row>
    <template #support>
      <TopDishes />
    </template>
  </Content>
</template>

<script lang="ts">
import Vue from 'vue'
import Content from '~/components/Content.vue'
import TopDishes from '~/components/Plan/TopDishes.vue'
export default Vue.extend({
  components: {
    Content,
    TopDishes,
  },
  head: {
    title: 'Home',
  },
})
</script>
