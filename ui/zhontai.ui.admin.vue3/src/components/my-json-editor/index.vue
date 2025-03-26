<template>
  <div ref="editor" class="my-json-editor h100" v-bind="$attrs"></div>
</template>

<script lang="ts" setup name="my-json-editor">
import { ref, onMounted, onBeforeUnmount } from 'vue'
import JSONEditor from 'jsoneditor'
import 'jsoneditor/dist/jsoneditor.min.css'
import { merge } from 'lodash-es'

const json = defineModel({ type: String })
const customOptions = defineModel('options', { type: Object })

const editor = ref(null)
let jsonEditor = null as any

onMounted(() => {
  const options = merge(
    {
      mode: 'code',
      modes: ['preview', 'code', 'text'],
      onChange: () => {},
      mainMenuBar: true,
      statusBar: true,
    },
    customOptions.value || {}
  )

  jsonEditor = new JSONEditor(editor.value, options, JSON.parse((json.value || '{}') as string))
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
