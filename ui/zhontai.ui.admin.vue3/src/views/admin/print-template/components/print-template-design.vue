<template>
  <el-drawer v-model="state.visible" direction="ltr" destroy-on-close size="100%" @closed="onClosed">
    <template #header="{ titleId, titleClass }">
      <div class="my-flex my-flex-between mr20">
        <span :id="titleId" :class="titleClass">{{ title }}</span>
      </div>
    </template>

    <div>213</div>
  </el-drawer>
</template>

<script lang="ts" setup name="admin/print-template-form">
import { reactive, ref } from 'vue'
import eventBus from '/@/utils/mitt'

defineProps({
  title: {
    type: String,
    default: '',
  },
})

const formRef = ref()
const state = reactive({
  visible: false,
  sureLoading: false,
})

// 打开对话框
const open = async (row: any = {}) => {
  state.visible = true
}
// 关闭
const onClosed = () => {
  state.visible = false
}

defineExpose({
  open,
})
</script>
