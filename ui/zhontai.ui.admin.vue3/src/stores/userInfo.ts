import { defineStore } from 'pinia'
import { AuthApi } from '/@/api/admin/Auth'
import { merge } from 'lodash-es'
import { Local } from '/@/utils/storage'
// import { Session } from '/@/utils/storage'

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
      const userInfos = <UserInfos>await this.getApiUserInfo().catch(() => {})
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
    async getApiUserInfo() {
      return new Promise((resolve, reject) => {
        new AuthApi()
          .getUserPermissions()
          .then((res) => {
            if (res?.success) {
              const user = res.data?.user
              const userInfos = {
                userName: user?.nickName || user?.name,
                photo: user?.avatar ? user?.avatar : '',
                time: new Date().getTime(),
                roles: [],
                authBtnList: res.data?.permissions,
              }
              // Session.set('userInfo', userInfos)
              resolve(userInfos)
            } else {
              this.clear()
            }
          })
          .catch((err) => {
            reject(err)
          })

        /*
        setTimeout(() => {
          // 模拟数据，请求接口时，记得删除多余代码及对应依赖的引入
          const userName = Cookies.get('userName')
          // 模拟数据
          let defaultRoles: Array<string> = []
          let defaultAuthBtnList: Array<string> = []
          // admin 页面权限标识，对应路由 meta.roles，用于控制路由的显示/隐藏
          let adminRoles: Array<string> = ['admin']
          // admin 按钮权限标识
          let adminAuthBtnList: Array<string> = ['btn.add', 'btn.del', 'btn.edit', 'btn.link']
          // test 页面权限标识，对应路由 meta.roles，用于控制路由的显示/隐藏
          let testRoles: Array<string> = ['common']
          // test 按钮权限标识
          let testAuthBtnList: Array<string> = ['btn.add', 'btn.link']
          // 不同用户模拟不同的用户权限
          if (userName === 'admin') {
            defaultRoles = adminRoles
            defaultAuthBtnList = adminAuthBtnList
          } else {
            defaultRoles = testRoles
            defaultAuthBtnList = testAuthBtnList
          }
          // 用户信息模拟数据
          const userInfos = {
            userName: userName,
            photo:
              userName === 'admin'
                ? 'https://img2.baidu.com/it/u=1978192862,2048448374&fm=253&fmt=auto&app=138&f=JPEG?w=504&h=500'
                : 'https://img2.baidu.com/it/u=2370931438,70387529&fm=253&fmt=auto&app=138&f=JPEG?w=500&h=500',
            time: new Date().getTime(),
            roles: defaultRoles,
            authBtnList: defaultAuthBtnList,
          }
          resolve(userInfos)
        }, 0)
        */
      })
    },
  },
})
