<template>
  <div ref="editor" class="my-json-editor h100" v-bind="$attrs"></div>
</template>

<script lang="ts" setup name="my-json-editor">
import { ref, onMounted, onBeforeUnmount, watch } from 'vue'
import JSONEditor from 'jsoneditor'
import 'jsoneditor/dist/jsoneditor.min.css'
import { mergeWith, isObject } from 'lodash-es'

const json = defineModel({ type: [String, Object] })
const customOptions = defineModel('options', { type: Object })

const editor = ref(null)
let jsonEditor = null as any

const customizer = (objValue: any, srcValue: any) => {
  if (isObject(objValue) && isObject(srcValue)) {
    return srcValue
  }
  return srcValue
}

onMounted(() => {
  const options = mergeWith(
    {},
    {
      mode: 'code',
      modes: ['preview', 'code', 'text'],
      onChange: () => {
        try {
          const newValue = jsonEditor.get()
          json.value = typeof newValue === 'string' ? JSON.stringify(newValue) : newValue
        } catch (error) {}
      },
      mainMenuBar: true,
      statusBar: true,
    },
    customOptions.value || {},
    customizer
  )

  jsonEditor = new JSONEditor(editor.value, options, JSON.parse((typeof json.value === 'string' ? json.value : JSON.stringify(json.value)) || '{}'))
})

onBeforeUnmount(() => {
  if (jsonEditor) {
    jsonEditor.destroy()
  }
})

defineExpose({
  jsonEditor,
})
</script>

<style scoped lang="scss">
:deep() {
  .jsoneditor-poweredBy {
    display: none;
  }
}
</style>
