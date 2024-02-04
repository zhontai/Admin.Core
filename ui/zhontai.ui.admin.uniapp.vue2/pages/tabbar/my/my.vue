<template>
	<view class="app-my">
		<!-- #ifndef H5 -->
		<u-navbar
			:autoBack="false"
			:placeholder="true"
		>
			<view slot="left">中台Admin</view>
		</u-navbar>
		<!-- #endif -->
		
		<view class="u-flex app-my__account">
			<view class="u-flex u-flex-items-center" style="flex:1;" @click="() => $u.route(navUrl)">
				<u-avatar size="50" :src="avatarUrl"></u-avatar>
				<view class="app-my__account__content">
					<template v-if="isLogin">
						<u--text v-if="nickName" :text="nickName" bold></u--text>
						<!-- #ifndef H5 -->
						<view v-else @click.stop="onGetUserInfo">
							<u--text text="点击授权微信头像" bold></u--text>
						</view>
						<!--  #endif -->
						<u-gap v-if="!isH5 || nickName" height="5"></u-gap>
						<u--text 
							:mode="isMobile?'phone':''" 
							:text="mobile" 
							:format="isMobile?'encrypt':''" 
							:size="isH5 && !nickName?15:12" 
							:bold="isH5 && !nickName?true:false"
						></u--text>
					</template>
					<u--text v-else text="点击登录/注册" bold></u--text>
				</view>
			</view>
			<view v-if="cellPhoneNo" style="display: flex; align-items: center; margin-right: 12px;">
				<u-icon
					@click="onPhoneCall(cellPhoneNo)"
					name="phone-fill" 
					color="#01a1ff" 
					size="28" 
					:label="fullname?fullname:' ' " 
					labelSize="12" 
					labelPos="bottom"
					labelColor="#909193"
				>
				</u-icon>
			</view>
		</view>
		
		<view class="app-my__cell-box">
			<u-cell-group :border="false">
				<u-cell
					icon="setting"
					iconStyle="margin-right:5px;"
				    title="设置"
				    isLink
				    url="/pages/setting/setting"
					:border="false"
				></u-cell>
			</u-cell-group>
		</view>
		
		<!-- 自定义底部导航栏 -->
		<my-tabbar></my-tabbar>
	</view>
</template>

<script>
	import myTabbar from '@/components/my-tabbar/my-tabbar.vue'
	// import myCustomerService from '@/components/my-customer-service/my-customer-service.vue'
	import { mapState, mapGetters, mapMutations } from 'vuex'
	
	export default {
		components: { myTabbar },
		data() {
			return {
				cellPhoneNo: '',
				fullname: ''
			}
		},
		computed: {
			...mapGetters('account', ['isLogin', 'nickName', 'avatarUrl', 'mobile', 'userId']),
			navUrl(){
				return this.isLogin ? '/pages/account/account' : '/pages/account/login'
			},
			isH5(){
				return uni.$u.platform === 'h5'
			},
			isMobile(){
				return uni.$u.test.mobile(this.mobile)
			}
		},
		methods: {
			...mapMutations('account', {
				setUserInfo: 'setUserInfo'
			}),
			...mapMutations('order', {
				setStatus: 'setStatus'
			}),
			authNav(routeData){
				if(!this.isLogin){
					routeData.type = routeData.type === 'tab' ? 'tab' : 'redirectTo'
					uni.navigateTo({
						url: '/pages/account/login',
						success: (res) => {
							res.eventChannel.emit('login', {
								route: routeData
							})
						}
					})
					return
				}
				uni.$u.route(routeData)
			},
			onGetUserInfo(){
				const me = this
				uni[uni.getUserProfile ? 'getUserProfile' : 'getUserInfo']({
					desc: '用于完善用户信息',
					success: function (infoRes) {
						console.log(infoRes.userInfo)
						me.setUserInfo(infoRes.userInfo)
					},
					fail: function (infoRes) {
						console.log(infoRes)
					}
				})
			},			
			onPhoneCall(phoneNumber){
				uni.makePhoneCall({
					phoneNumber: phoneNumber
				})
			},
		}
	}
</script>

<!-- 添加界面背景色 -->
<style lang="scss">
	page {
		/* #ifndef H5 */
		height: 100%;
		/* #endif */
		background-color: $app-bg-color;
	}
</style>
<style lang="scss" scoped>
	.app-my{
		&__menu-box{
			padding: 10px 15px;
			background-color: #FFFFFF;
			margin:10px;
			border-radius: 4px;
			&__item{
				padding-left: 10px;
			}
		}
		
		&__cell-box{
			background-color: #FFFFFF;
			margin:15px 10px 10px 10px;
			border-radius: 4px;
		}
		
		&__account{
			padding:20px;
			background-color: #FFFFFF;
			
			&__content{
				margin-left: 5px;
			}
		}
	}
</style>