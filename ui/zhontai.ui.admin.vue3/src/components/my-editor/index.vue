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
import { IDomEditor, i18nChangeLanguage, i18nAddResources } from '@wangeditor/editor'
import { Toolbar, Editor } from '@wangeditor/editor-for-vue'
import { FileApi } from '/@/api/admin/File'
import { t, lang } from '/@/i18n'

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
    default: () => t('请输入内容...'),
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
            return t('图片网址必须以 http/https 开头')
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
  editorVal: props.modelValue as string,
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

onMounted(() => {
  if (lang.value === 'zh-TW') {
    i18nAddResources(lang.value, {
      editor: {
        more: '更多',
        justify: '對齊',
        indent: '縮排',
        image: '圖片',
        video: '影片',
      },
      common: {
        ok: '確定',
        delete: '刪除',
        enter: '換行',
      },
      blockQuote: {
        title: '引用',
      },
      codeBlock: {
        title: '程式碼區塊',
      },
      color: {
        color: '文字顏色',
        bgColor: '背景色',
        default: '預設顏色',
        clear: '清除背景色',
      },
      divider: {
        title: '分隔線',
      },
      emotion: {
        title: '表情',
      },
      fontSize: {
        title: '字號',
        default: '預設字號',
      },
      fontFamily: {
        title: '字型',
        default: '預設字型',
      },
      fullScreen: {
        title: '全螢幕',
      },
      header: {
        title: '標題',
        text: '內文',
      },
      image: {
        netImage: '網路圖片',
        delete: '刪除圖片',
        edit: '編輯圖片',
        viewLink: '查看連結',
        src: '圖片位址',
        desc: '圖片描述',
        link: '圖片連結',
      },
      indent: {
        decrease: '減少縮排',
        increase: '增加縮排',
      },
      justify: {
        left: '左對齊',
        right: '右對齊',
        center: '置中對齊',
        justify: '兩端對齊',
      },
      lineHeight: {
        title: '行高',
        default: '預設行高',
      },
      link: {
        insert: '插入連結',
        text: '連結文字',
        url: '連結位址',
        unLink: '取消連結',
        edit: '修改連結',
        view: '查看連結',
      },
      textStyle: {
        bold: '粗體',
        clear: '清除格式',
        code: '行內程式碼',
        italic: '斜體',
        sub: '下標',
        sup: '上標',
        through: '刪除線',
        underline: '底線',
      },
      undo: {
        undo: '復原',
        redo: '重做',
      },
      todo: {
        todo: '待辦',
      },
      listModule: {
        unOrderedList: '無序清單',
        orderedList: '有序清單',
      },
      tableModule: {
        deleteCol: '刪除欄',
        deleteRow: '刪除列',
        deleteTable: '刪除表格',
        widthAuto: '寬度自動調整',
        insertCol: '插入欄',
        insertRow: '插入列',
        insertTable: '插入表格',
        header: '表頭',
      },
      videoModule: {
        delete: '刪除影片',
        uploadVideo: '上傳影片',
        insertVideo: '插入影片',
        videoSrc: '影片位址',
        videoSrcPlaceHolder: '影片檔案 url 或第三方 <iframe>',
        videoPoster: '影片封面',
        videoPosterPlaceHolder: '封面圖片 url',
        ok: '確定',
        editSize: '修改尺寸',
        width: '寬度',
        height: '高度',
      },
      uploadImgModule: {
        uploadImage: '上傳圖片',
        uploadError: '{{fileName}} 上傳出錯',
      },
      highLightModule: {
        selectLang: '選擇語言',
      },
    })
  }

  i18nChangeLanguage(lang.value)
})

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
    state.editorVal = val as string
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
