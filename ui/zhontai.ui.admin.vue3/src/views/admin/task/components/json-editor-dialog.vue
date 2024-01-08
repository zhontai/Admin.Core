<template>
  <!-- <el-dialog
      v-model="state.showDialog"
      destroy-on-close
      :title="title"
      draggable
      :close-on-click-modal="false"
      :close-on-press-escape="false"
      width="600px"
    >
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="onCancel" size="default">取 消</el-button>
          <el-button type="primary" @click="onSure" size="default" :loading="state.sureLoading">确 定</el-button>
        </span>
      </template>
    </el-dialog> -->

  <el-drawer v-model="state.showDialog" direction="rtl" destroy-on-close :size="size">
    <template #header="{ titleId, titleClass }">
      <h4 :id="titleId" :class="titleClass">{{ title }}</h4>
      <el-icon v-if="state.isFull" class="el-drawer__btn" @click="state.isFull = !state.isFull" title="还原"><ele-CopyDocument /></el-icon>
      <el-icon v-else class="el-drawer__btn" @click="state.isFull = !state.isFull" title="最大化"><ele-FullScreen /></el-icon>
    </template>
    <div class="my-fill h100" style="padding: 20px">
      <div class="mb10 my-flex my-flex-end">
        <el-button type="primary" @click="onJsonShell" size="default">Shell</el-button>
        <el-button type="primary" @click="onJsonHttp" size="default">Http</el-button>
        <el-button type="primary" @click="onJsonFormat" size="default">格式化</el-button>
        <el-button type="primary" @click="onJsonCompress" size="default">压 缩</el-button>
      </div>
      <v-ace-editor
        v-model:value="state.content"
        @init="onJsonFormat"
        lang="json"
        :theme="state.theme"
        :options="state.options"
        :readonly="state.readOnly"
        style="height: 100%"
        class="ace-editor"
      />
    </div>
    <template #footer>
      <div style="flex: auto; padding: 20px !important">
        <el-button @click="onCancel" size="default">取 消</el-button>
        <el-button type="primary" @click="onSure" size="default">确 定</el-button>
      </div>
    </template>
  </el-drawer>
</template>

<script lang="ts" setup>
import { reactive, computed } from 'vue'

import { VAceEditor } from 'vue3-ace-editor'
import './ace-config'

defineProps({
  title: {
    type: String,
    default: 'Json编辑器',
  },
})

const emits = defineEmits(['sure'])

const state = reactive({
  showDialog: false,
  isFull: false,
  isMobile: document.body.clientWidth < 1000,
  content: '',
  topic: '',
  lang: 'json', //解析 json yaml
  theme: 'monokai', //主题 github chrome monokai
  readOnly: false, //是否只读
  options: {
    enableBasicAutocompletion: true,
    enableSnippets: true,
    enableLiveAutocompletion: true,
    tabSize: 2,
    showPrintMargin: false,
    fontSize: 13,
  },
})

const size = computed(() => {
  return state.isMobile ? '100%' : state.isFull ? '100%' : '50%'
})

const onJsonShell = () => {
  state.topic = '[system]shell'
  state.content = `{
  "fileName": "",
  "arguments": "",
  "workingDirectory": ""
}`
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
}

const jsonError = (e: any) => {
  window.console.log(`JSON字符串错误：${e.message}`)
}

// JSON格式化
const onJsonFormat = () => {
  try {
    state.content = JSON.stringify(JSON.parse(state.content), null, 2)
  } catch (e) {
    jsonError(e)
  }
}

// JSON压缩
const onJsonCompress = () => {
  try {
    state.content = JSON.stringify(JSON.parse(state.content))
  } catch (e) {
    jsonError(e)
  }
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
