<template>
	<view>
		<u-tabbar
			:value="value"
			:fixed="true"
			:placeholder="true"
			:safeAreaInsetBottom="true"
			:zIndex="999"
		>
			<u-tabbar-item 
				v-for="(item,index) in list" 
				:key="index" 
				:text="item.text" 
				:icon="value === item.url ? item.iconFill : item.icon" 
				:name="item.url" 
				@click="click(item)"
			></u-tabbar-item>
		</u-tabbar>
	</view>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
	data() {
		const list  = [
			{
				text: '首页',
				icon: "home",
				iconFill: 'home-fill',
				url: "/pages/tabbar/home/home",
			},
			{
				text: '我的',
				icon: "account",
				iconFill: 'account-fill',
				url: "/pages/tabbar/my/my",
			},
		];
		//获取当前页面的路径
		let value = uni.$u.page()
		return {
			value: value,
			list: list
		}
	},
	computed: {
		...mapGetters('account', ['isLogin'])
	},
	created() {
		uni.hideTabBar()
	},
	methods: {
		//切换tab
		click(item) {
			if(item.auth && !this.isLogin){
				uni.navigateTo({
					url: '/pages/account/login',
					success: (res) => {
						res.eventChannel.emit('login', {
							route: {
								type: 'tab',
								url: item.url,
							}
						})
					}
				})
				return
			}
			
			uni.switchTab({
				url: item.url
			})
		}
	}
};
</script>
