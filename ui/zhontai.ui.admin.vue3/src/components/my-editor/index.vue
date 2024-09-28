<template>
  <div class="editor-container">
    <Toolbar :editor="editorRef" :mode="mode" />
    <Editor :mode="mode" :defaultConfig="state.editorConfig" :style="{ height }" v-model="state.editorVal"
      @onCreated="handleCreated" @onChange="handleChange" />
  </div>
</template>

<script setup lang="ts" name="wngEditor">
// https://www.wangeditor.com/v5/for-frame.html#vue3
import '@wangeditor/editor/dist/css/style.css'
import { reactive, shallowRef, watch, onBeforeUnmount } from 'vue'
import { IDomEditor } from '@wangeditor/editor'
import { Toolbar, Editor } from '@wangeditor/editor-for-vue'
import pinia from '/@/stores/index'
import { useUserInfo } from '/@/stores/userInfo'
const storesUserInfo = useUserInfo(pinia)

// 定义父组件传过来的值
const props = defineProps({
  // 是否禁用
  disable: {
    type: Boolean,
    default: () => false,
  },
  // 内容框默认 placeholder
  placeholder: {
    type: String,
    default: () => '请输入内容...',
  },
  // https://www.wangeditor.com/v5/getting-started.html#mode-%E6%A8%A1%E5%BC%8F
  // 模式，可选 <default|simple>，默认 default
  mode: {
    type: String,
    default: () => 'default',
  },
  // 高度
  height: {
    type: String,
    default: () => '310px',
  },
  // 双向绑定，用于获取 editor.getHtml()
  modelValue: String,
  // 双向绑定，用于获取 editor.getText()
  getText: String,
})

// 定义子组件向父组件传值/事件
const emit = defineEmits(['update:modelValue', 'update:getText'])

// 定义变量内容
const editorRef = shallowRef()
const state = reactive({
  editorConfig: {
    placeholder: props.placeholder,
    MENU_CONF: {
      uploadImage: {
        server: import.meta.env.VITE_API_URL + '/api/admin/file/upload-file',
        allowedFileTypes: ['image/*'],
        fieldName: 'file',
        headers: {
          Authorization: 'Bearer ' + storesUserInfo.getToken(),
        },
        customInsert(res: any, insertFn: any) {
          let url = res.data.linkUrl
          let alt = ''
          let href = ''
          insertFn(url, alt, href)
        }
      }
    }
  },
  editorVal: props.modelValue,
})

// 编辑器回调函数
const handleCreated = (editor: IDomEditor) => {
  editorRef.value = editor
}
// 编辑器内容改变时
const handleChange = (editor: IDomEditor) => {
  emit('update:modelValue', editor.getHtml())
  emit('update:getText', editor.getText())
}
// 页面销毁时
onBeforeUnmount(() => {
  const editor = editorRef.value
  if (editor == null) return
  editor.destroy()
})
// 监听是否禁用改变
watch(
  () => props.disable,
  (bool) => {
    const editor = editorRef.value
    if (editor == null) return
    bool ? editor.disable() : editor.enable()
  },
  {
    deep: true,
  }
)
// 监听双向绑定值改变，用于回显
watch(
  () => props.modelValue,
  (val) => {
    state.editorVal = val
  },
  {
    deep: true,
  }
)
</script>
