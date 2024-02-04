<template>
	<view>
		<web-view v-if="src" :src="src"></web-view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				src: ''
			}
		},
		onLoad() {
			// #ifdef APP-NVUE
			const eventChannel = this.$scope.eventChannel
			// #endif
			
			// #ifndef APP-NVUE
			const eventChannel = this.getOpenerEventChannel()
			// #endif
			
			eventChannel.on('3d-preview', (data) => {
				this.src = data.src
				if(data.title){
					uni.setNavigationBarTitle({
						title: data.title
					})
				}
			})
		},
		methods: {
		}
	}
</script>

<style>

</style>
