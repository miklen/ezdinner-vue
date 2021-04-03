<template>
  <v-container>
    <v-row>
      <v-col v-for="family in families" :key="family.id">
        <v-card>
          <v-card-title>{{ family.name }}</v-card-title>
          <v-card-subtitle>Family members</v-card-subtitle>
          <v-card-text>
            <v-row v-for="familyMember in familyMembers" :key="familyMember">
              <v-col class="col-1">
                <v-icon>mdi-account</v-icon>
              </v-col>
              <v-col>{{ familyMember }}</v-col>
            </v-row>
          </v-card-text>
          <v-card-actions
            ><v-btn text color="primary"
              >Add family member</v-btn
            ></v-card-actions
          >
        </v-card>
      </v-col>
      <v-col>
        <v-card>
          <v-card-title>New family</v-card-title>
          <v-card-actions
            ><v-btn text color="primary"
              ><v-icon>mdi-account-multiple-plus</v-icon></v-btn
            ></v-card-actions
          >
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue'
import { Family } from '~/types/Family'
export default Vue.extend({
  middleware: ['auth'],
  computed: {
    families(): Family[] {
      return this.$accessor.families.families
    },
    familyMembers(): string[] {
      return ['Mikkel Nygaard', 'Linda Nygaard']
    },
  },
})
</script>
