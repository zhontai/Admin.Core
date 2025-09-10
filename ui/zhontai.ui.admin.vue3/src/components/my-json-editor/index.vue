<template>
  <div ref="editor" class="my-json-editor h100" v-bind="$attrs"></div>
</template>

<script lang="ts" setup name="my-json-editor">
import JSONEditor from 'jsoneditor'
import 'jsoneditor/dist/jsoneditor.min.css'
import { mergeWith, isObject } from 'lodash-es'
import { storeToRefs } from 'pinia'
import '/@/theme/ace-cloud9-night.scss'
import { useThemeConfig } from '/@/stores/themeConfig'

const storesThemeConfig = useThemeConfig()
const { themeConfig } = storeToRefs(storesThemeConfig)
const json = defineModel({ type: [String, Object] })
const customOptions = defineModel('options', { type: Object })

const editor = ref(null)
const jsonEditor = ref()

const customizer = (objValue: any, srcValue: any) => {
  if (isObject(objValue) && isObject(srcValue)) {
    return srcValue
  }
  return srcValue
}

onMounted(() => {
  const options = mergeWith(
    {},
    themeConfig.value.isDark
      ? {
          theme: {
            cssClass: 'ace-cloud9-night ',
            isDark: true,
          },
        }
      : {},
    {
      mode: 'code',
      modes: ['preview', 'code', 'text'],
      onChange: () => {
        try {
          const newValue = jsonEditor.value.get()
          json.value = typeof json.value === 'string' ? JSON.stringify(newValue, null, 2) : newValue
        } catch (error) {}
      },
      mainMenuBar: true,
      statusBar: true,
      enableSort: false,
      enableTransform: false,
    },
    customOptions.value || {},
    customizer
  )

  jsonEditor.value = new JSONEditor(
    editor.value,
    options,
    JSON.parse((typeof json.value === 'string' ? json.value : JSON.stringify(json.value)) || '{}')
  )
})

onBeforeUnmount(() => {
  if (jsonEditor) {
    jsonEditor.value.destroy()
  }
})

defineExpose({
  jsonEditor,
})
</script>

<style lang="scss">
.jsoneditor-modal .pico-modal-header {
  color: var(--el-color-white);
  background-color: var(--el-color-primary);
}
html.dark {
  .jsoneditor-menu {
    background-color: var(--el-bg-color-overlay);
  }
}
</style>
<style scoped lang="scss">
:deep() {
  .jsoneditor {
    border-color: var(--el-border-color);
  }
  .jsoneditor-menu {
    color: var(--el-text-color-primary);
    background-color: var(--el-color-primary);
    border-bottom: 1px solid var(--el-border-color);
  }
  .jsoneditor-statusbar {
    background-color: var(--el-bg-color);
    color: var(--el-text-color-secondary);
    border-top-color: var(--el-border-color);
  }
  .jsoneditor-contextmenu .jsoneditor-menu {
    background-color: var(--el-bg-color-overlay);
    border-color: var(--el-border-color-dark);
  }
  .jsoneditor-contextmenu .jsoneditor-menu button {
    color: var(--el-text-color-primary);
  }
  .jsoneditor-contextmenu .jsoneditor-menu button:focus,
  .jsoneditor-contextmenu .jsoneditor-menu button:hover {
    background-color: var(--el-fill-color-light);
  }
  .jsoneditor-contextmenu .jsoneditor-menu li button.jsoneditor-selected,
  .jsoneditor-contextmenu .jsoneditor-menu li button.jsoneditor-selected:focus,
  .jsoneditor-contextmenu .jsoneditor-menu li button.jsoneditor-selected:hover {
    background-color: var(--el-fill-color-light);
    color: var(--el-color-primary);
  }
  textarea.jsoneditor-text {
    background-color: var(--el-bg-color-overlay);
    color: var(--el-text-color-primary);
  }
  .jsoneditor-poweredBy {
    display: none;
  }
}
</style>
