import {
  getAccessorType,
  getterTree,
  mutationTree,
  actionTree,
} from 'typed-vuex'
import * as families from './families'
import * as dishes from './dishes'
import * as dinners from './dinners'

const localStoragePrefix = 'ezdinner'

// Typings:
// https://typed-vuex.roe.dev/

export const state = () => ({
  activeFamilyId: loadFromLocalStorage('activeFamilyId') as string,
})

export type RootState = ReturnType<typeof state>

export const getters = getterTree(state, {})

export const mutations = mutationTree(state, {
  updateFamilyId(state, familyId: string) {
    state.activeFamilyId = familyId
    saveInLocalStorage('activeFamilyId', familyId)
  },
})

export const actions = actionTree(
  { state, getters, mutations },
  {
    setActiveFamilyId({ commit }, familyId: string) {
      commit('updateFamilyId', familyId)
    },
  },
)

export const accessorType = getAccessorType({
  state,
  getters,
  mutations,
  actions,
  modules: {
    // The key (submodule) needs to match the Nuxt namespace (e.g. ~/store/submodule.ts)
    families,
    dishes,
    dinners,
  },
})

export const loadFromLocalStorage = (key: string) => {
  const value = window.localStorage.getItem(`${localStoragePrefix}:${key}`)
  if (!value) return value
  try {
    return JSON.parse(value)
  } catch (e) {
    return value
  }
}

export const saveInLocalStorage = (key: string, payload: any) => {
  window.localStorage.setItem(
    `${localStoragePrefix}:${key}`,
    JSON.stringify(payload),
  )
}
