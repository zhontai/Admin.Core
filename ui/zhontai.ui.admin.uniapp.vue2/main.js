import App from './App'
import Vue from 'vue'

import store from './store'
Vue.prototype.$store = store

Vue.config.productionTip = false
App.mpType = 'app'

import uView from "uview-ui"
Vue.use(uView)

//小程序分享
import mpShare from 'uview-ui/libs/mixin/mpShare.js'
//分享朋友圈
mpShare.onShareTimeline = () => {
	return uni.$u.mpShare
}
Vue.mixin(mpShare)

//修正平板尺寸超大的问题
import { getPx } from './libs/utils/util.js'
uni.$u.getPx = getPx

// 全局组件注册
import MyIcon from '@/components/my-icon/my-icon.vue'
Vue.component('MyIcon', MyIcon)

const app = new Vue({
	store,
    ...App
})

app.$mount()
