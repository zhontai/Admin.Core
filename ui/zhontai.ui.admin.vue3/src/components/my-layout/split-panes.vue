<template>
  <MyLayout>
    <splitpanes :horizontal="state.isMobile" class="default-theme" v-bind="$attrs">
      <slot></slot>
    </splitpanes>
  </MyLayout>
</template>

<script lang="ts" setup name="my-layout-split-panes">
import { reactive, onBeforeMount } from 'vue'
import { Splitpanes } from 'splitpanes'
import 'splitpanes/dist/splitpanes.css'
import mittBus from '/@/utils/mitt'
import MyLayout from './index.vue'

const state = reactive({
  isMobile: document.body.clientWidth < 1000,
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
:deep(.splitpanes.default-theme .splitpanes__splitter) {
  background-color: transparent;
  border-left-color: transparent;
}
:deep(.splitpanes.default-theme .splitpanes__pane) {
  background-color: transparent;
}

:deep(.splitpanes__pane) {
  transition: none;
}
</style>
