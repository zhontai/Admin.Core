<template>
	<view class="app-home">
		<!-- #ifndef H5 -->
		<u-navbar
			:autoBack="false"
			:placeholder="true"
		>
			<view slot="left">中台Admin</view>
		</u-navbar>
		<!-- #endif -->
		
		<!-- 顶部banner -->
		<view class="u-flex-column u-flex-items-center">
			<u--image
				src="/static/img/long.png" 
				mode="aspectFill" 
				width="750rpx"
			></u--image>
		</view>
		 
		
		<!-- 功能菜单 -->
		<!-- <view class="app-home__grid-box">
			<u-grid
				:border="false"
				col="4"
			>
				<u-grid-item
					v-for="(item,index) in menuList"
					:key="index"
					class="app-home__grid-box__item"
					@click="$u.route(item.route)"
				>
					<u-icon
						:customStyle="{paddingTop:20+'rpx'}"
						:name="item.icon"
						:size="22"
					></u-icon>
					<text class="app-home__grid-box__item__text">{{item.title}}</text>
				</u-grid-item>
			</u-grid>
		</view> -->
		
		<!-- 自定义底部导航栏 -->
		<my-tabbar></my-tabbar>
	</view>
</template>

<script>
import myTabbar from '@/components/my-tabbar/my-tabbar.vue'

export default {
	components: { myTabbar },
	data() {
		return {
			
		}
	},
	computed: {
		
	},
	onLoad() {
		
	},
	filters: {
		
	},
	methods: {
		
	}
};
</script>

<style lang="scss">
	.app-home{
		&__grid-box{
			padding: 10px;
			background-color: #FFFFFF;
			&__item{
				&__text{
					margin-top:5px;
					font-size: 12px;
				}
			}
		}
		
		.type-box{
			padding-left:40rpx;
			padding-right: 40rpx;
			.type{
				flex: 1;
				margin-right: 40rpx;
				margin-bottom: 40rpx;
				border-radius: 2px;
				padding:40rpx 20rpx;
				border:1px solid #ceccca;
				
				.title{
					font-size: 30rpx;
					color: #474747;
					margin-bottom: 26rpx;
				}
				.desc{
					font-size: 16rpx;
					color: #7F7F7F;
					margin-bottom: 2px;
				}
			}
		}
	}
</style>