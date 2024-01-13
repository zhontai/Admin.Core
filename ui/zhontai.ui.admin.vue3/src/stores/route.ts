import { defineStore } from 'pinia'

/**
 * 路由信息
 * @methods to 设置路由to数据
 */
export const useRoute = defineStore('route', {
  state: (): RouteState => ({
    to: {
      path: '',
      params: '',
    },
  }),
  actions: {
    async setRouteTo({ to }: RouteState) {
      this.to = to
    },
  },
})
