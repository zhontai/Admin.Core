<template>
  <MySplitter>
    <el-splitter-panel size="45%" min="30%" max="60%">
      <dict-type @change="onChange"></dict-type>
    </el-splitter-panel>
    <el-splitter-panel>
      <dict ref="dictRef"></dict>
    </el-splitter-panel>
  </MySplitter>
</template>

<script lang="ts" setup name="admin/dict">
import { defineAsyncComponent, ref } from 'vue'
import { DictTypeGetPageOutput } from '/@/api/admin/data-contracts'

// 引入组件
const DictType = defineAsyncComponent(() => import('./dict-type.vue'))
const Dict = defineAsyncComponent(() => import('./dict.vue'))
const MySplitter = defineAsyncComponent(() => import('/@/components/my-layout/splitter.vue'))

const dictRef = ref()

const onChange = (data: DictTypeGetPageOutput) => {
  dictRef.value?.refresh(data)
}
</script>

<style scoped lang="scss"></style>
