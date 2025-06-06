<template>
  <el-drawer v-model="state.showDialog" direction="rtl" destroy-on-close :size="size">
    <template #header="{ titleId, titleClass }">
      <h4 :id="titleId" :class="titleClass">{{ title }}</h4>
      <el-icon v-if="state.isFull" class="el-drawer__btn" @click="state.isFull = !state.isFull" title="还原"><ele-CopyDocument /></el-icon>
      <el-icon v-else class="el-drawer__btn" @click="state.isFull = !state.isFull" title="最大化"><ele-FullScreen /></el-icon>
    </template>
    <div class="my-fill h100" style="padding: 20px">
      <div class="mb10 my-flex my-flex-end">
        <el-button type="primary" @click="onJsonShell">Shell</el-button>
        <el-button type="primary" @click="onJsonHttp">Http</el-button>
      </div>
      <MyJsonEditor ref="jsonEditorRef" v-model="state.content" :options="{ modes: [] }"></MyJsonEditor>
    </div>
    <template #footer>
      <div style="flex: auto; padding: 20px !important">
        <el-button @click="onCancel">取 消</el-button>
        <el-button type="primary" @click="onSure">确 定</el-button>
      </div>
    </template>
  </el-drawer>
</template>

<script lang="ts" setup>
import { reactive, computed, ref } from 'vue'
import MyJsonEditor from '/@/components/my-json-editor/index.vue'

defineProps({
  title: {
    type: String,
    default: 'Json编辑器',
  },
})

const emits = defineEmits(['sure'])

const jsonEditorRef = ref()

const state = reactive({
  showDialog: false,
  isFull: false,
  isMobile: document.body.clientWidth < 1000,
  content: '',
  topic: '',
})

const size = computed(() => {
  return state.isMobile ? '100%' : state.isFull ? '100%' : '50%'
})

const onJsonShell = () => {
  state.topic = '[shell]'
  state.content = `{
  "desc": "任务描述",
  "arguments": "-plaintext -d \\"{ \\\\\\"id\\\\\\": 1 }\\" \${grpcAddress} YourNamespace.YourGrpcService/YourMethod",
  "moduleName": "YourModuleName"
}`
  jsonEditorRef.value.jsonEditor.set(JSON.parse(state.content))
}

const onJsonHttp = () => {
  state.topic = '[系统预留]Http请求'
  state.content = `{
  "method": "get",
  "url": "",
  "header": {
    "Content-Type": "application/json"
  },
  "body": "{}"
}`
  jsonEditorRef.value.jsonEditor.set(JSON.parse(state.content))
}

// 打开对话框
const open = (task: any) => {
  if (task) {
    state.topic = task.topic || ''
    state.content = task.body || ''
  }
  state.showDialog = true
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

// 确定
const onSure = () => {
  emits('sure', { topic: state.topic, body: state.content })
  state.showDialog = false
}

defineExpose({
  open,
})
</script>

<style scoped lang="scss">
.el-alert {
  border-width: 0px !important;
  margin-left: 110px;
  margin-top: 10px;
}
.el-drawer__btn {
  cursor: pointer;
  margin-right: 8px;
  &:hover {
    color: var(--el-color-primary);
  }
}
</style>
