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
    type: [Number, String],
    default: 14,
  },
})

const symbolId = computed(() => `#${props.prefix}-${props.name}`)
const style = computed(() => {
  let sizeStyle = ''
  if (typeof props.size === 'number') {
    sizeStyle = `width:${props.size}px;height:${props.size}px;`
  } else if (typeof props.size === 'string') {
    sizeStyle = `width:${props.size};height:${props.size};`
  }
  return sizeStyle
})
</script>

<style scoped lang="scss">
.my-input-with-select :deep(.el-input-group__prepend) {
  background-color: var(--el-fill-color-blank);
}
</style>
