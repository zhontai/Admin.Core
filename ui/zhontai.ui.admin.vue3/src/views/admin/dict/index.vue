<template>
  <MySplitPanes>
    <pane size="50" min-size="30" max-size="70">
      <dict-type @change="onChange"></dict-type>
    </pane>
    <pane>
      <dict ref="dictRef"></dict>
    </pane>
  </MySplitPanes>
</template>

<script lang="ts" setup name="admin/dict">
import { defineAsyncComponent, ref } from 'vue'
import { Pane } from 'splitpanes'
import { DictTypeGetPageOutput } from '/@/api/admin/data-contracts'

// 引入组件
const DictType = defineAsyncComponent(() => import('./dict-type.vue'))
const Dict = defineAsyncComponent(() => import('./dict.vue'))
const MySplitPanes = defineAsyncComponent(() => import('/@/components/my-layout/split-panes.vue'))

const dictRef = ref()

const onChange = (data: DictTypeGetPageOutput) => {
  dictRef.value?.refresh(data)
}
</script>

<style scoped lang="scss"></style>
