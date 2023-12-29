<template>
  <div class="my-layout h100" :style="`position: ${state.isMobile ? 'relative' : 'absolute'}`">
    <splitpanes :horizontal="state.isMobile" class="default-theme">
      <slot></slot>
    </splitpanes>
  </div>
</template>

<script lang="ts" setup name="my-layout">
import { reactive, onBeforeMount } from 'vue'
import mittBus from '/@/utils/mitt'
import { Splitpanes } from 'splitpanes'
import 'splitpanes/dist/splitpanes.css'

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
