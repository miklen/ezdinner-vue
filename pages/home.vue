<template>
  <Content>
    <v-row>
      <v-col class="text-center">
        <h1>Welcome</h1>
        <span>
          Get started by creating a family. Add family members to participate in
          planning. And begin tracking and planning your meals.
        </span>
      </v-col>
    </v-row>
  </Content>
</template>

<script lang="ts">
import Vue from 'vue'
import Content from '~/components/Content.vue'
export default Vue.extend({
  components: {
    Content,
  },
  head: {
    title: 'Home',
  },
})
</script>
