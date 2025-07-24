import { defineStore } from 'pinia'
import { AuthApi } from '/@/api/admin/Auth'
import { merge, isObject } from 'lodash-es'
import { Local } from '/@/utils/storage'
import { useThemeConfig } from '/@/stores/themeConfig'
import Watermark from '/@/utils/watermark'
import { TokenInfo } from '/@/api/admin/data-contracts'

export const adminTokenKey = 'admin-token'
export const adminTokenInfoKey = 'admin-token-info'

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
      if (userInfos && Object.keys(userInfos).length > 0) {
        merge(this.userInfos, userInfos)
      }
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
    setTokenInfo(tokenInfo: TokenInfo | undefined) {
      this.userInfos.token = tokenInfo?.accessToken as string
      Local.set(adminTokenInfoKey, tokenInfo)
    },
    getToken() {
      const tokenInfo = this.getTokenInfo()
      this.userInfos.token = tokenInfo?.accessToken as string
      return tokenInfo?.accessToken
    },
    getTokenInfo() {
      const tokenInfo = Local.get(adminTokenInfoKey) as TokenInfo
      return tokenInfo
    },
    removeTokenInfo() {
      this.userInfos.token = ''
      Local.remove(adminTokenInfoKey)
    },
    clear() {
      this.removeTokenInfo()
      window.requests = []
      window.location.reload()
    },
    //查询用户信息
    async getUserInfo() {
      try {
        const [profileResponse, permissionsResponse] = await Promise.allSettled([new AuthApi().getUserProfile(), new AuthApi().getUserPermissions()])

        const profile = profileResponse.status === 'fulfilled' ? profileResponse.value : null
        const permissions = permissionsResponse.status === 'fulfilled' ? permissionsResponse.value : null

        if (!profile?.success || !permissions?.success) {
          this.clear()
          return {}
        }

        const user = profile.data
        const userInfos = {
          userName: user?.nickName || user?.name,
          photo: user?.avatar ? user?.avatar : '',
          time: new Date().getTime(),
          roles: [],
          authBtnList: permissions.data?.permissions,
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

        return userInfos
      } catch (err) {
        console.error('获取用户信息失败:', err)
        throw err
      }
    },
  },
})
