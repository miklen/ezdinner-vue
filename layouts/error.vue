<template>
  <v-container>
    <v-row>
      <v-col align="center">
        <v-avatar size="400" roun
          ><v-img :src="require('~/assets/images/error.jpg')"></v-img
        ></v-avatar>
      </v-col>
    </v-row>
    <v-row>
      <v-col align="center">
        <h1 v-if="error.statusCode === 404">{{ pageNotFound }}</h1>
        <h1 v-else>{{ otherError }}</h1>
        <NuxtLink to="/">Home page</NuxtLink>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
export default {
  layout: 'empty',
  props: {
    error: {
      type: Object,
      default: null,
    },
  },
  data() {
    return {
      pageNotFound: '404 Not Found',
      otherError: 'An error occurred',
    }
  },
  head() {
    const title =
      this.error.statusCode === 404 ? this.pageNotFound : this.otherError
    return {
      title,
    }
  },
}
</script>

<style scoped>
h1 {
  font-size: 20px;
}
</style>
