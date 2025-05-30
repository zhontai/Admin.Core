<template>
  <el-select>
    <el-option v-for="item in options" :key="item.label" :label="item.label" :value="item.value" />
  </el-select>
</template>

<script lang="ts" setup name="my-select">
defineProps({
  options: {
    type: Array<SelectOptionType>,
    default: () => [],
  },
})
</script>
