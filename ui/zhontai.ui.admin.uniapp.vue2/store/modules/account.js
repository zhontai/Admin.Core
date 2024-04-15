import { api } from '@/api/api-account.js'

const auth = uni.getStorageSync("app_token")
export default {
	namespaced: true,
	state: {
		token: auth?.token || '',
		refreshToken: auth?.refreshToken || '',
		expireTime: auth?.expireTime || 0,
		openId: auth?.openId || '',
		userInfo: uni.getStorageSync("app_user_info") || null,
	},
	getters: {
		isLogin(state){
			return state.token ? true : false
		},
		nickName(state){
			return state.userInfo?.nickName || ''
		},
		userName(state){
			return state.userInfo?.userName || ''
		},
		userId(state){
			return state.userInfo?.userId || ''
		},
		mobile(state){
			let mobile = ''
			const userName = state.userInfo?.userName
			if(userName && uni.$u.test.mobile(userName)){
				mobile = userName
			}else{
				const phoneNumber = auth?.phoneNumber
				if(phoneNumber && uni.$u.test.mobile(phoneNumber))
				{
					mobile = phoneNumber
				}
			}
			return mobile
		},
		avatarUrl(state){
			return state.userInfo?.avatarUrl || ''
		}
	},
	mutations: {
		setAuth(state, data){
			state.token = data?.token || ''
			state.refreshToken = data?.refreshToken || ''
			state.expireTime = data?.expireTime || 0
			uni.setStorageSync('app_token', data)
		},
		updateAuth(state, data){
			state.token = data?.token || ''
			state.expireTime = data?.expireTime || 0
			auth.accessToken = state.token
			auth.expireTime = state.expireTime
			uni.setStorageSync('app_token', auth)
		},
		setUserInfo(state, data){
			state.userInfo = data
			uni.setStorageSync('app_user_info', data)
		},
		logout(state){
			uni.removeStorageSync('app_token')
			uni.removeStorageSync('app_mobile')
			uni.removeStorageSync('app_user_info')
			state.token = ''
			state.mobile = ''
			state.userInfo = null
		}
	},
	actions: {
		async login({ commit }, data){
			const res = await api.login(data)
			if(res?.success){
				commit('setAuth', res.data)
				commit('setUserInfo', {
					userName: data.mobile || '',
					nickName: data.mobile || ''
				})
			}
			return res
		},
		logout({ commit }, data){
			commit('logout')
		}
	}
}
