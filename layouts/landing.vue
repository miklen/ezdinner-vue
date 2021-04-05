<template>
  <v-app>
    <v-app-bar app color="white" height="100">
      <v-avatar>
        <v-img src="/android-chrome-192x192.png"></v-img>
      </v-avatar>
      <v-toolbar-title class="font-weight-black headline">
        Dinner Planner
      </v-toolbar-title>
      <v-spacer></v-spacer>
      <v-btn color="primary" large @click="signIn">
        <span class="text--darken-1 font-weight-bold"> SIGN IN </span>
      </v-btn>
    </v-app-bar>

    <v-main>
      <section id="hero">
        <v-row no-gutters>
          <v-img
            :min-height="'calc(100vh - ' + $vuetify.application.top + 'px)'"
            :src="require('~/assets/images/pexels-fauxels-3184183-l.jpg')"
          >
            <v-theme-provider dark>
              <v-container fill-height>
                <v-row
                  align="center"
                  class="white--text mx-auto"
                  justify="center"
                >
                  <v-col class="white--text text-center" cols="12" tag="h1">
                    <span
                      :class="[
                        $vuetify.breakpoint.smAndDown
                          ? 'display-1'
                          : 'display-2',
                      ]"
                      class="font-weight-light"
                    >
                      EASY
                    </span>

                    <br />

                    <span
                      :class="[
                        $vuetify.breakpoint.smAndDown
                          ? 'display-3'
                          : 'display-4',
                      ]"
                      class="font-weight-black"
                    >
                      DINNER PLANNER
                    </span>
                  </v-col>

                  <v-btn
                    class="align-self-end"
                    fab
                    outlined
                    @click="$vuetify.goTo('#about')"
                  >
                    <v-icon>mdi-chevron-double-down</v-icon>
                  </v-btn>
                </v-row>
              </v-container>
            </v-theme-provider>
          </v-img>
        </v-row>
      </section>

      <section id="about">
        <div class="py-12"></div>

        <v-container class="text-center">
          <h2 class="display-2 font-weight-bold mb-3">ABOUT</h2>

          <v-responsive class="mx-auto mb-8" width="56">
            <v-divider class="mb-1"></v-divider>

            <v-divider></v-divider>
          </v-responsive>

          <v-responsive
            class="mx-auto title font-weight-light mb-8"
            max-width="720"
          >
            Easy Dinner Planner makes it easy to track and plan your family
            dinner. You can track your top dishes, and get suggestions for what
            to put on the menu the coming week.
          </v-responsive>

          <div></div>

          <v-btn color="primary" large @click="signIn">
            <span class="text--darken-1 font-weight-bold"> GET STARTED </span>
          </v-btn>
        </v-container>

        <div class="py-12"></div>
      </section>

      <section id="features" class="grey lighten-3">
        <div class="py-12"></div>

        <v-container class="text-center">
          <h2 class="display-2 font-weight-bold mb-3">
            EASY DINNER PLANNER FEATURES
          </h2>

          <v-responsive class="mx-auto mb-12" width="56">
            <v-divider class="mb-1"></v-divider>

            <v-divider></v-divider>
          </v-responsive>

          <v-row>
            <v-col
              v-for="({ icon, title, text }, i) in features"
              :key="i"
              cols="12"
              md="4"
            >
              <v-card class="py-12 px-4" color="grey lighten-5" flat>
                <v-theme-provider dark>
                  <div>
                    <v-avatar color="primary" size="88">
                      <v-icon large v-text="icon"></v-icon>
                    </v-avatar>
                  </div>
                </v-theme-provider>

                <v-card-title
                  class="justify-center font-weight-black text-uppercase"
                  v-text="title"
                ></v-card-title>

                <v-card-text class="subtitle-1" v-text="text"> </v-card-text>
              </v-card>
            </v-col>
          </v-row>
        </v-container>

        <div class="py-12"></div>
      </section>

      <!-- <section id="stats">
        <v-parallax
          :height="$vuetify.breakpoint.smAndDown ? 700 : 500"
          src="https://images.unsplash.com/photo-1510915228340-29c85a43dcfe?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=1950&q=80"
        >
          <v-container fill-height>
            <v-row class="mx-auto">
              <v-col
                v-for="[value, title] of stats"
                :key="title"
                cols="12"
                md="3"
              >
                <div class="text-center">
                  <div
                    class="display-3 font-weight-black mb-4"
                    v-text="value"
                  ></div>

                  <div
                    class="title font-weight-regular text-uppercase"
                    v-text="title"
                  ></div>
                </div>
              </v-col>
            </v-row>
          </v-container>
        </v-parallax>
      </section>

      <section id="blog">
        <div class="py-12"></div>

        <v-container>
          <h2
            class="display-2 font-weight-bold mb-3 text-uppercase text-center"
          >
            Blog
          </h2>

          <v-responsive class="mx-auto mb-12" width="56">
            <v-divider class="mb-1"></v-divider>

            <v-divider></v-divider>
          </v-responsive>

          <v-row>
            <v-col
              v-for="({ src, text, title }, i) in articles"
              :key="i"
              cols="12"
              md="4"
            >
              <v-img
                :src="src"
                class="mb-4"
                height="275"
                max-width="100%"
              ></v-img>

              <h3
                class="font-weight-black mb-4 text-uppercase"
                v-text="title"
              ></h3>

              <div class="title font-weight-light mb-5" v-text="text"></div>

              <v-btn class="ml-n4 font-weight-black" text>
                Continue Reading
              </v-btn>
            </v-col>
          </v-row>
        </v-container>

        <div class="py-12"></div>
      </section>

      <v-sheet id="contact" color="#333333" dark tag="section" tile>
        <div class="py-12"></div>

        <v-container>
          <h2
            class="display-2 font-weight-bold mb-3 text-uppercase text-center"
          >
            Contact Me
          </h2>

          <v-responsive class="mx-auto mb-12" width="56">
            <v-divider class="mb-1"></v-divider>

            <v-divider></v-divider>
          </v-responsive>

          <v-theme-provider light>
            <v-row>
              <v-col cols="12">
                <v-text-field flat label="Name*" solo></v-text-field>
              </v-col>

              <v-col cols="12">
                <v-text-field flat label="Email*" solo></v-text-field>
              </v-col>

              <v-col cols="12">
                <v-text-field flat label="Subject*" solo></v-text-field>
              </v-col>

              <v-col cols="12">
                <v-textarea flat label="Message*" solo></v-textarea>
              </v-col>

              <v-col class="mx-auto" cols="auto">
                <v-btn color="accent" x-large> Submit </v-btn>
              </v-col>
            </v-row>
          </v-theme-provider>
        </v-container>

        <div class="py-12"></div>
      </v-sheet> -->
    </v-main>

    <v-footer class="justify-center" color="#292929" height="100">
      <div
        class="title font-weight-light grey--text text--lighten-1 text-center"
      >
        &copy; {{ new Date().getFullYear() }} — EzLifehacks
      </div>
    </v-footer>
  </v-app>
</template>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
  data() {
    return {
      features: [
        {
          icon: 'mdi-account-group-outline',
          title: 'Track',
          text: 'Keep track of what you had for dinner in the past',
        },
        {
          icon: 'mdi-update',
          title: 'Plan',
          text: 'Plan your meals for the coming week(s)',
        },
        {
          icon: 'mdi-shield-outline',
          title: 'Suggestions',
          text: 'Out of ideas? Get suggestions based on your previous meals.',
        },
      ],
      // articles: [
      //   {
      //     src:
      //       'https://images.unsplash.com/photo-1423784346385-c1d4dac9893a?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=1950&q=80',
      //     title: 'Mobile first & Responsive',
      //     text:
      //       'Phasellus lorem enim, luctus ut velit eget, convallis egestas eros. Sed ornare ligula eget tortor tempor, quis porta tellus dictum.',
      //   },
      //   {
      //     src:
      //       'https://images.unsplash.com/photo-1475938476802-32a7e851dad1?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=1950&q=80',
      //     title: 'Think outside the box',
      //     text:
      //       'Nam ut leo ipsum. Maecenas pretium aliquam feugiat. Aenean vel tempor est, vitae tincidunt risus. Sed sodales vestibulum nibh.',
      //   },
      //   {
      //     src:
      //       'https://images.unsplash.com/photo-1416339442236-8ceb164046f8?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=1892&q=80',
      //     title: 'Small changes, big difference',
      //     text:
      //       'Vestibulum in dictum velit, in rhoncus nibh. Maecenas neque libero, interdum a dignissim in, aliquet vitae lectus. Phasellus lorem enim, luctus ut velit eget.',
      //   },
      // ],
      // stats: [
      //   ['24k', 'Github Stars'],
      //   ['330+', 'Releases'],
      //   ['1m', 'Downloads/mo'],
      //   ['5m', 'Total Downloads'],
      // ],
    }
  },
  head: {
    title: 'Welcome',
  },
  computed: {
    isAuthenticated() {
      return this.$msal.isAuthenticated
    },
  },
  watch: {
    isAuthenticated() {
      if (this.isAuthenticated) this.$router.push('/home')
    },
  },
  created() {
    if (this.isAuthenticated) this.$router.push('/home')
  },
  methods: {
    signIn() {
      this.$msal.signIn()
    },
  },
})
</script>
