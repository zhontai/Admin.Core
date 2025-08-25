<template>
  <el-tooltip effect="dark" content="列设置" placement="top">
    <el-button ref="colSetRef" icon="ele-Setting" circle v-bind="$attrs" />
  </el-tooltip>
  <el-popover
    ref="colSetPopoverRef"
    placement="bottom-end"
    transition="el-zoom-in-top"
    :virtual-ref="colSetRef"
    trigger="click"
    virtual-triggering
    :width="300"
    popper-class="my-col-set-popper"
    @show="onColSet"
  >
    <div class="my-col-set-tool-box">
      <el-tooltip content="拖动进行排序" placement="top-start">
        <SvgIcon name="ele-Rank" />
      </el-tooltip>
      <el-checkbox v-model="checkAll" :indeterminate="checkIndeterminate" class="ml12" label="全部" @change="onCheckAllChange" />
    </div>
    <el-scrollbar>
      <div ref="sortableRef" class="my-col-set-sortable-box">
        <div v-for="(item, index) in colsModel" :key="item.attrs.prop" :data-id="item.attrs.prop">
          <SvgIcon name="ele-Rank" class="handle" />
          <el-checkbox v-model="item.isShow" class="ml12 mr8" :label="item.attrs.label" />
        </div>
      </div>
    </el-scrollbar>
  </el-popover>
</template>

<script lang="ts" setup>
import { useTemplateRef, computed, nextTick } from 'vue'
import Sortable from 'sortablejs'

// 定义父组件传过来的列数组模型
const colsModel = defineModel({
  type: Array<{
    attrs: Record<string, any>
    slot?: string
    isShow?: boolean
  }>,
  required: true,
})

const colSetRef = useTemplateRef('colSetRef')
useTemplateRef('colSetPopoverRef')
const sortableRef = useTemplateRef('sortableRef')

// 全选
const checkAll = computed(() => {
  const headers = colsModel.value.filter((v) => v.isShow).length
  return headers === colsModel.value.length
})

// 半选
const checkIndeterminate = computed(() => {
  const headers = colsModel.value.filter((v) => v.isShow).length
  return headers > 0 && headers < colsModel.value.length
})

// 全选变更
const onCheckAllChange = <T,>(val: T) => {
  if (val) colsModel.value.forEach((v) => (v.isShow = true))
  else colsModel.value.forEach((v) => (v.isShow = false))
}

// 列设置
const onColSet = () => {
  nextTick(() => {
    const sortable = Sortable.create(sortableRef.value, {
      handle: '.handle',
      dataIdAttr: 'data-id',
      animation: 150,
      onEnd: () => {
        const headerList: Array<{ attrs: Record<string, any>; slot?: string; isShow?: boolean }> = []
        sortable.toArray().forEach((val: string) => {
          colsModel.value.forEach((v) => {
            if (v.attrs.prop === val) headerList.push({ ...v })
          })
        })
        colsModel.value = headerList
      },
    })
  })
}
</script>

<style lang="scss">
.my-col-set-popper {
  padding: 0px !important;
}
</style>
<style scoped lang="scss">
.my-col-set-tool-box {
  border-bottom: 1px solid var(--el-border-color-lighter);
  padding: 4px 10px;
}
.my-col-set-sortable-box {
  padding: 4px 10px;
  max-height: 303px;
  .handle {
    cursor: grab;
  }
}
</style>
