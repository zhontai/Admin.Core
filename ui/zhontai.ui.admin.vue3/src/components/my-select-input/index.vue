<template>
  <el-input v-model="state.filter.value" class="my-input-with-select" clearable @keyup.enter="onSearch" v-bind="$attrs">
    <template v-if="state.filters.length > 0" #prepend>
      <el-select v-model="state.filter.field" style="width: 100px" @change="onChange">
        <el-option v-for="field in state.filters" :key="field.field" :label="field.description" :value="field.field" />
      </el-select>
    </template>
  </el-input>
</template>

<script lang="ts" setup name="my-select-input">
import { reactive, PropType, watch } from 'vue'
import { cloneDeep } from 'lodash-es'

const props = defineProps({
  modelValue: Object as PropType<any | undefined | null>,
  filters: {
    type: Array<DynamicFilterInfo>,
    default() {
      return []
    },
  },
})

const emits = defineEmits(['update:modelValue', 'search'])

const filters = props.filters.filter((a) => a.componentName === 'el-input')
let filter = {} as DynamicFilterInfo
if (filters.length > 0) {
  filter = cloneDeep(filters.find((a) => a.defaultSelect) || filters[0])
}

const state = reactive({
  filters: filters,
  filter: {
    field: props.modelValue?.field || filter.field,
    operator: props.modelValue?.operator || filter.operator,
    value: props.modelValue?.value || filter.value,
  } as DynamicFilterInfo,
})

const onChange = () => {
  state.filter.value = ''
}

const onSearch = () => {
  emits('search', cloneDeep(state.filter))
}

watch(
  () => state.filter,
  () => {
    emits('update:modelValue', cloneDeep(state.filter))
  },
  { deep: true }
)
</script>

<style scoped lang="scss">
.my-input-with-select :deep(.el-input-group__prepend) {
  background-color: var(--el-fill-color-blank);
}
</style>
