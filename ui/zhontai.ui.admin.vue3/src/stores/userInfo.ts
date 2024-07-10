import { defineStore } from 'pinia'
import { AuthApi } from '/@/api/admin/Auth'
import { merge } from 'lodash-es'
import { Local } from '/@/utils/storage'
import { useThemeConfig } from '/@/stores/themeConfig'
import Watermark from '/@/utils/watermark'

export const adminTokenKey = 'admin-token'

/**
 * 用户信息
 * @methods setUserInfos 设置用户信息
 */
export const useUserInfo = defineStore('userInfo', {
  state: (): UserInfosState => ({
    userInfos: {
      token: Local.get(adminTokenKey) || '',
      userName: '',
      photo: '',
      time: 0,
      roles: [],
      authBtnList: [],
    },
  }),
  actions: {
    async setUserInfos() {
      const userInfos = <UserInfos>await this.getUserInfo().catch(() => {})
      merge(this.userInfos, userInfos)
    },
    setUserName(userName: string) {
      this.userInfos.userName = userName
    },
    setPhoto(photo: string) {
      this.userInfos.photo = photo
    },
    setToken(token: string) {
      this.userInfos.token = token
      Local.set(adminTokenKey, token)
    },
    getToken() {
      const token = Local.get(adminTokenKey)
      this.userInfos.token = token
      return token
    },
    removeToken() {
      this.userInfos.token = ''
      Local.remove(adminTokenKey)
    },
    clear() {
      this.userInfos.token = ''
      Local.remove(adminTokenKey)
      window.requests = []
      window.location.reload()
    },
    //查询用户信息
    async getUserInfo() {
      return new Promise((resolve, reject) => {
        Promise.all([new AuthApi().getUserProfile(), new AuthApi().getUserPermissions()])
          .then((res) => {
            if (res[0]?.success && res[1]?.success) {
              const user = res[0].data
              const userInfos = {
                userName: user?.nickName || user?.name,
                photo: user?.avatar ? user?.avatar : '',
                time: new Date().getTime(),
                roles: [],
                authBtnList: res[1].data?.permissions,
              }

              // 水印文案
              const storesThemeConfig = useThemeConfig()
              if (storesThemeConfig.themeConfig.isWatermark) {
                storesThemeConfig.themeConfig.watermarkText = user?.watermarkText || '中台Admin'
                Watermark.set(storesThemeConfig.themeConfig.watermarkText)
                Local.remove('themeConfig')
                Local.set('themeConfig', storesThemeConfig.themeConfig)
              } else {
                Watermark.del()
              }

              resolve(userInfos)
            } else {
              this.clear()
            }
          })
          .catch((err) => {
            reject(err)
          })
      })
    },
  },
})
