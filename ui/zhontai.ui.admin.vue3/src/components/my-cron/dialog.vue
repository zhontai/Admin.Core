<template>
  <el-drawer v-model="state.showDialog" :title="title" direction="rtl" destroy-on-close size="620">
    <div class="my-fill h100" style="padding: 10px">
      <MyCron ref="crontabRef" :expression="expression" :hide-component="['year']"></MyCron>
    </div>
    <template #footer>
      <div style="flex: auto; padding: 20px !important">
        <el-button @click="onReset">重 置</el-button>
        <el-button @click="onCancel">取 消</el-button>
        <el-button type="primary" @click="onSure">确 定</el-button>
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
