<template>
	<view style="padding:60rpx;">
		<view class="u-flex-column u-flex-items-center" style="margin-top:40rpx;">
			<u--image src="/static/img/logo.png" bgColor="#FFFFFF" width="128rpx" height="128rpx"></u--image>
			<text style="font-weight: 500;font-size: 36rpx;margin-top:60rpx;">中台Admin</text>
			<text style="font-weight: 500;font-size: 36rpx;margin-top:10rpx;">当前版本号 1.0.0</text>
		</view>
		<view class="u-flex-column app-setting__item" style="margin-top: 100rpx;">
			<u-cell-group customStyle="background-color:#FFF;">
				<!-- <u-cell
					title="功能介绍"
					isLink
					url="/pages/about/version"
				></u-cell> -->
				<!-- #ifdef MP -->
				<u-cell
					title="检查新版本"
					isLink
					@click="onCheckForUpdate"
				></u-cell>
				<u-toast ref="toast"></u-toast>
				<u-modal
					title="更新提示"
					content="发现新版本，立即更新？" 
					:show="showUpdate" 
					:zoom="false" 
					showCancelButton
					closeOnClickOverlay
					confirmText="确定"
					@confirm="onSureUpdate" 
					@close="() => showUpdate = false"
					@cancel="() => showUpdate=false"
				>
				</u-modal>
				<!-- #endif -->
			</u-cell-group>
		</view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				showUpdate: false
			}
		},
		methods: {
			// #ifdef MP
			showToast(message) {
				this.$refs.toast.show({
					type: 'default',
					position: 'bottom',
					message: message
				})
			},
			onSureUpdate(){
				this.showUpdate = false
				const updateManager = uni.getUpdateManager()
				updateManager.applyUpdate()
				
			},
			onCheckForUpdate(){
				uni.showLoading({
					mask: true,
					title: '检查中...'
				});
				const updateManager = uni.getUpdateManager()
				updateManager.onCheckForUpdate(checkRes => {
					uni.hideLoading()
					if(checkRes.hasUpdate){
						updateManager.onUpdateReady(res => {
							this.showUpdate = true
						})
					}else{
						this.showToast('无更新')
					}
				})
				
			}
			// #endif
		}
	}
</script>

<style lang="scss" scoped>

</style>
