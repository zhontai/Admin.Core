<template>
  <div>
    <el-dialog
      v-model="state.showDialog"
      destroy-on-close
      :title="title"
      draggable
      :close-on-click-modal="false"
      :close-on-press-escape="true"
      :width="width"
      style="max-width: 90%"
    >
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="onCancel" size="default">关 闭</el-button>
        </span>
      </template>
      <div ref="previewContainerRef"></div>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup>
import { reactive, ref, nextTick, computed } from 'vue'
import { hiprint } from 'vue-plugin-hiprint'

defineProps({
  title: {
    type: String,
    default: '',
  },
})

const state = reactive({
  showDialog: false,
  width: 0,
  printData: {},
  template: {} as any,
})

const width = computed(() => {
  return `${state.width + 20}mm`
})

const hiprintTemplate = ref()
const previewContainerRef = ref<HTMLElement | null>(null) // 引用容器

// 打开对话框
const open = async (template: any, printData: {}) => {
  state.template = template
  state.printData = printData
  hiprintTemplate.value = new hiprint.PrintTemplate({
    template: state.template,
  })
  state.width = hiprintTemplate.value.printPanels?.length > 0 ? hiprintTemplate.value.printPanels[0].width : 210
  const htmlElements = hiprintTemplate.value.getHtml(printData)

  state.showDialog = true

  await nextTick()

  // 清空容器
  if (previewContainerRef.value) {
    previewContainerRef.value.innerHTML = ''
  }

  // 将 HTML 元素对象插入到容器中
  if (htmlElements?.length > 0 && previewContainerRef.value) {
    htmlElements.each((index: number, element: HTMLElement) => {
      previewContainerRef.value?.appendChild(element)
    })
  }
}
// 取消
const onCancel = () => {
  state.showDialog = false
}

defineExpose({
  open,
})
</script>

<style scoped lang="scss">
:deep() {
  .el-dialog__body {
    overflow-x: auto;
  }
}
</style>
