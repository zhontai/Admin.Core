<template>
	<u-icon
		:name="iconName"
		:color="color"
		:size="size"
		:bold="bold"
		:index="index"
		:hoverClass="hoverClass"
		customPrefix="custom-icon custom-icon"
		:label="label"
		:labelPos="labelPos"
		:labelSize="labelSize"
		:labelColor="labelColor"
		:space="space"
		:imgMode="imgMode"
		:width="width"
		:height="height"
		:top="top"
		:stop="stop"
		:customStyle="customStyle"
		@click="clickHandler"
	></u-icon>
</template>

<script>
// 引入图标名称，已经对应的unicode
import icons from './icons.js'
import props from 'uview-ui/components/u-icon/props.js'

export default {
	data() {
		return {
		}
	},
	mixins: [props],
	computed:{
		iconName(){
			return icons[this.name]
		}
	},
	methods: {
		clickHandler(e) {
			this.$emit('click', this.index)
			// 是否阻止事件冒泡
			this.stop && this.preventEvent(e)
		}
	}
};
</script>
