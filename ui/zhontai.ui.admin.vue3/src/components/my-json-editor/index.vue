<template>
  <div ref="editor" class="my-json-editor h100" v-bind="$attrs"></div>
</template>

<script lang="ts" setup name="my-json-editor">
import { ref, onMounted, onBeforeUnmount } from 'vue'
import JSONEditor from 'jsoneditor'
import 'jsoneditor/dist/jsoneditor.min.css'

const json = defineModel({ type: String })

const editor = ref(null)
let jsonEditor = null as any

const get = () => {
  return jsonEditor.get()
}

onMounted(() => {
  const options = {
    mode: 'preview',
    modes: ['preview', 'code', 'text'],
    onChange: () => {},
  }
  jsonEditor = new JSONEditor(editor.value, options)
  jsonEditor.set(JSON.parse((json.value || '{}') as string))
})

onBeforeUnmount(() => {
  if (jsonEditor) {
    jsonEditor.destroy()
  }
})

defineExpose({
  get,
})
</script>

<style scoped lang="scss">
:deep() {
  .jsoneditor-poweredBy {
    display: none;
  }
}
</style>
