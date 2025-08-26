<template>
  <MyLayout>
    <el-splitter :layout="layout" lazy v-bind="$attrs">
      <slot></slot>
    </el-splitter>
  </MyLayout>
</template>

<script lang="ts" setup name="my-layout-split-panes">
import { reactive, onBeforeMount, computed } from 'vue'
import mittBus from '/@/utils/mitt'
import MyLayout from './index.vue'

const state = reactive({
  isMobile: document.body.clientWidth < 1000,
})

const layout = computed(() => {
  return state.isMobile ? 'vertical' : 'horizontal'
})

// 页面加载前
onBeforeMount(() => {
  // 监听窗口大小改变时(适配移动端)
  mittBus.on('layoutMobileResize', (res: LayoutMobileResize) => {
    // 判断是否是手机端
    state.isMobile = res.clientWidth < 1000
  })
})
</script>

<style scoped lang="scss">
:deep() {
  .el-splitter-bar {
    width: 6px !important;
  }
  .el-splitter-bar__dragger-horizontal:before {
    background-color: transparent;
  }
  .el-splitter__vertical {
    display: unset;
  }
}
</style>
