<template>
	<view class="app-setting">
		<view v-if="isLogin" class="u-flex-column app-setting__item">
			<text class="app-setting__item__title">账号</text>
			<u-cell-group customStyle="background-color:#FFF;">
				<u-cell
					icon="lock"
					title="修改密码"
					isLink
					@click="onToChangePassword"
				></u-cell>
			</u-cell-group>
		</view>
		
		<view class="u-flex-column app-setting__item">
			<text class="app-setting__item__title">支持</text>
			<u-cell-group customStyle="background-color:#FFF;">
				<u-cell
					icon="trash"
					title="清除缓存"
					:value="currentSize"
					isLink
					@click="() => showClearCache = true"
				></u-cell>
			</u-cell-group>
		</view>
		
		<view class="u-flex-column app-setting__item" style="margin-top: 10px;">
			<u-cell-group customStyle="background-color:#FFF;">
				<u-cell
					title="关于中台Admin"
					isLink
					url="/pages/about/about"
				></u-cell>
			</u-cell-group>
		</view>
		
		<view v-if="isLogin" class="app-setting__btn-logout" @click="() => showLogout=true">退出登录</view>
		<u-modal
			title="确定退出登录吗？" 
			:show="showLogout" 
			:zoom="false" 
			showCancelButton
			closeOnClickOverlay
			confirmText="确定"
			@confirm="onLogout" 
			@close="() => showLogout = false"
			@cancel="() => showLogout=false"
		>
		</u-modal>
		<u-modal
			title="是否清除缓存？" 
			:show="showClearCache" 
			:zoom="false" 
			showCancelButton
			closeOnClickOverlay
			confirmText="确定"
			asyncClose
			@confirm="onClearCache" 
			@close="() => showClearCache = false"
			@cancel="() => showClearCache=false"
		>
		</u-modal>
	</view>
</template>

<script>
	import { mapState, mapGetters, mapActions } from 'vuex'
	import { fileSizeFormat } from '@/libs/utils/util.js'
	import { showToast } from '@/libs/utils/util.js'
	
	export default {
		name: 'page-setting-setting',
		data() {
			return {
				currentSize: '',
				showLogout: false,
				showClearCache: false,
			}
		},
		computed: {
			...mapGetters('account', ['isLogin'])
		},
		onLoad(){
			this.formatCacheSize()
		},
		methods: {
			...mapActions('account', {
				logout: 'logout'
			}),
			formatCacheSize(){
				try {
					const res = uni.getStorageInfoSync()
					this.currentSize = fileSizeFormat(res.currentSize*1024)
				} catch (e) {
				}
			},
			onLogout(){
				this.showLogout = false
				this.logout()
				uni.reLaunch({
					url: '/pages/tabbar/my/my'
				})
			},
			onClearCache(){
				try {
					const res = uni.getStorageInfoSync();
					const excludes = ['app_token', 'app_mobile', 'app_user_info', 'app_quotation_bom_list']
					res.keys?.forEach(key => {
						if(excludes.every(a => a != key)){
							uni.removeStorageSync(key)
						}
					})
					this.formatCacheSize()
					this.showClearCache = false
				} catch (e) {
				}
			},
			onToChangePassword(){
				uni.navigateTo({
					url: '/pages/account/change-password',
					events: {
						save: (data) => {
							uni.$u.sleep(0).then(() => {
								showToast('修改密码成功！')
							})
						}
					}
				})
			}
		}
	}
</script>

<!-- 添加界面背景色 -->
<style lang="scss">
	page {
		background-color: $u-bg-color;
	}
</style>
<style lang="scss" scoped>
	.app-setting{
		&__item {
			&__title {
				color: $u-tips-color;
				background-color: $u-bg-color;
				padding: 10px 15px;
				font-size: 14px;
			
				&__slot-title {
					color: $u-primary;
					font-size: 14px;
				}
			}
		}
		
		&__btn-logout{
			background: #FFF;
			color: #f56c6c;
			letter-spacing: 1px;
			padding: 10px 15px;
			text-align: center;
			margin-top: 20px;
			font-size: 15px;
		}
	}
</style>