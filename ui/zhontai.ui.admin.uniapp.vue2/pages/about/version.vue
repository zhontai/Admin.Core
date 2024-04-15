<template>
	<view style="padding:30rpx;">
		<view class="u-flex-column app-setting__item">
			<u-cell-group :border="false" customStyle="background-color:#FFF;">
				<u-cell
					v-for="(item, index) in versionList"
					:key="item.version"
					:title="item.title || `中台Admin ${item.version} 主要更新`"
					:label="item.label"
					isLink
					:url="`/pages/about/version-function?v=${item.version}`"
				></u-cell>
			</u-cell-group>
		</view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				versionList: [
					{
						// title: '中台Admin 1.0.0 主要更新',
						label: '2024-2-4',
						version: '1.1.0'
					}
				]
			}
		},
		methods: {
			
		}
	}
</script>

