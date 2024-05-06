<template>
  <svg :style="style" v-bind="$attrs" aria-hidden="true">
    <use :xlink:href="symbolId" :fill="color" />
  </svg>
</template>

<script lang="ts" setup name="my-svg-icon">
import { computed } from 'vue'

const props = defineProps({
  prefix: {
    type: String,
    default: 'icon',
  },
  name: {
    type: String,
    required: true,
  },
  color: {
    type: String,
    default: '#333',
  },
  size: {
    type: Number,
    default: () => 14,
  },
})

const symbolId = computed(() => `#${props.prefix}-${props.name}`)
const style = computed(() => `width:${props.size}px;height:${props.size}px;`)
</script>

<style scoped lang="scss">
.my-input-with-select :deep(.el-input-group__prepend) {
  background-color: var(--el-fill-color-blank);
}
</style>
