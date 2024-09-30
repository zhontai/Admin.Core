<template>
  <el-cascader
    placeholder="请选择上级地区"
    :options="state.regionList"
    :props="cascaderProps"
    :persistent="false"
    filterable
    clearable
    class="w100"
    @change="onChange"
    v-bind="$attrs"
    ><template #default="{ data }">
      <span>{{ data.name }}</span>
      <MySvgIcon v-if="data.hot" name="hot" color="#ea322b" size="12" class="ml5"></MySvgIcon>
    </template>
  </el-cascader>
</template>

<script lang="ts" setup>
import { reactive } from 'vue'
import type { CascaderProps } from 'element-plus'
import { RegionApi } from '/@/api/admin/Region'

const emits = defineEmits(['update:parentId', 'change'])

const state = reactive({
  regionList: [] as any,
})

const cascaderProps: CascaderProps = {
  checkStrictly: true,
  value: 'id',
  label: 'name',
  lazy: true,
  lazyLoad(node, resolve) {
    const value = node.value as number
    new RegionApi()
      .getChildList({ parentId: value > 0 ? value : 0 })
      .then((r) => {
        resolve(r.data as any)
      })
      .catch(() => {
        resolve([])
      })
  },
}

const onChange = (value: any) => {
  emits('update:parentId', value && value.length > 0 ? value[value.length - 1] : '')
}
</script>
