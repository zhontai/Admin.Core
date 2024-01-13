<template>
  <!-- <el-dialog
    title="Cron表达式生成器"
    v-model="state.showDialog"
    append-to-body
    destroy-on-close
    draggable
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    v-bind="$attrs"
  >
    <MyCron ref="crontabRef" :expression="expression"></MyCron>

    <template #footer>
      <span class="dialog-footer">
        <el-button @click="onCancel" size="default">取 消</el-button>
        <el-button type="warning" @click="onReset">重置</el-button>
        <el-button type="primary" @click="onSure">确定</el-button>
      </span>
    </template>
  </el-dialog> -->

  <el-drawer v-model="state.showDialog" :title="title" direction="rtl" destroy-on-close size="620">
    <div class="my-fill h100" style="padding: 20px">
      <MyCron ref="crontabRef" :expression="expression" :hide-component="['year']"></MyCron>
    </div>
    <template #footer>
      <div style="flex: auto; padding: 20px !important">
        <el-button @click="onCancel" size="default">取 消</el-button>
        <el-button type="warning" @click="onReset">重置</el-button>
        <el-button type="primary" @click="onSure">确定</el-button>
      </div>
    </template>
  </el-drawer>
</template>

<script lang="ts" setup name="my-captcha-dialog">
import { defineAsyncComponent, ref, reactive } from 'vue'

defineProps({
  title: {
    type: String,
    default: 'Cron表达式生成器',
  },
})

const MyCron = defineAsyncComponent(() => import('./index.vue'))
const emits = defineEmits(['fill'])

const expression = ref('')
const crontabRef = ref()

const state = reactive({
  showDialog: false,
})

// 打开对话框
const open = (intervalArgument: string) => {
  if (intervalArgument) expression.value = intervalArgument
  state.showDialog = true
}

// 确定
const onSure = () => {
  emits('fill', crontabRef.value.getCron())
  onCancel()
}

// 重置
const onReset = () => {
  crontabRef.value.clearCron()
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

defineExpose({
  open,
})
</script>

<style scoped lang="scss"></style>
