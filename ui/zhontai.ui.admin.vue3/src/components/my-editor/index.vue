<template>
  <div class="editor-container">
    <Toolbar :editor="editorRef" :mode="mode" />
    <Editor
      :mode="mode"
      :defaultConfig="state.editorConfig"
      :style="{ height }"
      v-model="state.editorVal"
      @onCreated="handleCreated"
      @onChange="handleChange"
      @onBlur="onBlur"
    />
  </div>
</template>

<script setup lang="ts" name="wngEditor">
// https://www.wangeditor.com/v5/for-frame.html#vue3
import '@wangeditor/editor/dist/css/style.css'
import { reactive, shallowRef, watch, onBeforeUnmount, PropType } from 'vue'
import { IDomEditor } from '@wangeditor/editor'
import { Toolbar, Editor } from '@wangeditor/editor-for-vue'
import { FileApi } from '/@/api/admin/File'

type InsertFnType = (url: string, alt: string, href: string) => void
type InsertVideoFnType = (url: string, poster: string) => void

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
  modelValue: String as PropType<string | undefined | null>,
  // 双向绑定，用于获取 editor.getText()
  getText: String as PropType<string | undefined | null>,
})

// 定义子组件向父组件传值/事件
const emit = defineEmits(['update:modelValue', 'update:getText', 'onBlur', 'onChange'])

// 定义变量内容
const editorRef = shallowRef()
const state = reactive({
  editorConfig: {
    placeholder: props.placeholder,
    MENU_CONF: {
      uploadImage: {
        fieldName: 'file',
        customUpload(file: File, insertFn: InsertFnType) {
          new FileApi().uploadFile({ file: file }).then((res) => {
            if (res?.success) {
              const url = res.data?.linkUrl as string
              insertFn(url, res.data?.fileName as string, url)
            }
          })
        },
      },
      insertImage: {
        checkImage(src: string, alt: string, href: string): boolean | string | undefined {
          if (!src) {
            return
          }
          if (src.indexOf('http') !== 0) {
            return '图片网址必须以 http/https 开头'
          }
          return true
        },
      },
      uploadVideo: {
        fieldName: 'file',
        customUpload(file: File, insertFn: InsertVideoFnType) {
          new FileApi().uploadFile({ file: file }).then((res) => {
            if (res?.success) {
              const url = res.data?.linkUrl as string
              insertFn(url, '')
            }
          })
        },
      },
    },
  },
  editorVal: props.modelValue,
})

const onBlur = () => {
  emit('onBlur')
}

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
  (val, oVal) => {
    if (oVal) emit('onChange')
    state.editorVal = val
  },
  {
    deep: true,
  }
)

const isEmpty = () => {
  return editorRef.value.isEmpty()
}

defineExpose({
  isEmpty,
})
</script>
