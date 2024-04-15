// 帮助文档 https://www.quanzhan.co/luch-request/guide/3.x
import store from 'store'
import Request from 'uview-ui/libs/luch-request'
import { apiUrl } from './config.js'
import { setConfig, requestConfig, responseOk, responseError } from './config-http.js'
import { api as apiAccount } from '@/api/api-account.js'
import { showToast } from '../../libs/utils/util.js'

// 创建请求实例
const http = new Request()

/* config 为默认全局配置*/
http.setConfig((config) => {
	setConfig(config)
	config.baseURL = apiUrl // 根域名
	return config
})

// 请求拦截
http.interceptors.request.use((config) => { // 可使用async await 做异步操作
	return requestConfig(config)
}, config => { // 可使用async await 做异步操作
	return Promise.reject(config)
})

// 储存过期的token
let expireTokenList = []

// 响应拦截
http.interceptors.response.use(async(response) => { // 可使用async await 做异步操作
	if(response.statusCode === 401){
		if(response.config.custom?._retry){
			const lastPage = uni.$u.page()
			showToast('您还没有登录', function(){
				//判断当前界面是否已是登录界面
				const currentPage = uni.$u.page()
				const loginPage = '/pages/account/login'
				if(lastPage === currentPage && currentPage !== loginPage){
					uni.navigateTo({
						url: loginPage
					})
				}
			},1000)
		}else{
			let accessToken = (response.config.header['Authorization']||'').replace('Bearer ','')
			expireTokenList.push(accessToken)
			let account = store.state.account
			debugger
			if (expireTokenList.includes(account.token)){
				await apiAccount.refreshToken({ token: account?.token })
				//重试
				try {
					response.config.custom = response.config.custom || {}
					//防止死循环
					response.config.custom._retry = true
					return http.middleware(response.config)
				} catch (e) {
					// 重新获取数据可能因为网络原因获取失败了
					return Promise.reject(e)
				}
			} else {
				return http.middleware(response.config)
			}
		}
	}
	return responseOk(response)
}, (response) => {
	return responseError(response)
})

export default http