<template>
	<view class="u-flex-column app-result">
		<!-- 自定义顶部导航栏 -->
		<u-navbar
			leftText="提交结果"
			:autoBack="false"
			:placeholder="true"
			border
			@leftClick="onBack"
		>
		</u-navbar>
		
		<view class="u-flex-column u-flex-items-center" style="padding: 100rpx">
			<view style="margin-bottom: 100rpx;">
				<u-icon 
					size="42"
					name="checkmark-circle-fill" 
					color="#67C23A" 
					label="提交成功" 
					labelSize="20"
					labelPos="bottom"
				></u-icon>
			</view>
		</view>
		
	</view>
</template>

<script>
	export default {
		name: 'page-result',
		data() {
			return {
				
			}
		},
		methods: {
			// 返回
			back(){
				uni.reLaunch({
					url: '/pages/tabbar/my/my'
				})
			},
			//点击返回
			onBack() {
				this.back()
			}
		}
	}
</script>

<!-- 添加界面背景色 -->
<style lang="scss">
	page {
		/* #ifndef H5 */
		height: 100%;
		/* #endif */
	}
</style>
<style lang="scss" scoped>
	.app-result{
		::v-deep .u-icon__label{
			margin-top: 10px !important;
		}
	}
</style>
