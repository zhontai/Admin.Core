<template>
  <el-input v-model="state.value" placeholder="请选择图标" class="w100" @clear="onClear" v-bind="$attrs">
    <template #prepend>
      <SvgIcon :name="state.value" />
    </template>
    <template #append>
      <el-button icon="ele-MoreFilled" @click="onOpen" />
    </template>
  </el-input>

  <icon-select ref="iconSelectRef" title="选择图标" :modal="true" @sure="onSure" v-model="state.value" v-bind="$attrs"></icon-select>
</template>

<script lang="ts" setup>
import { ref, reactive, PropType, watch } from 'vue'
import iconSelect from './icon-select.vue'

// const iconSelect = defineAsyncComponent(() => import('./icon-select.vue'))

const props = defineProps({
  modelValue: String as PropType<string | undefined | null>,
})

const emits = defineEmits(['update:modelValue'])

const iconSelectRef = ref()

const state = reactive({
  value: props.modelValue,
})

const onOpen = () => {
  iconSelectRef.value!.open()
}

const onClear = () => {
  emits('update:modelValue', undefined)
}

const onSure = async (value: string) => {
  iconSelectRef.value.close()
  if (value) {
    state.value = value
    emits('update:modelValue', value)
  }
}

watch(
  () => state.value,
  () => {
    emits('update:modelValue', state.value)
  }
)
</script>

<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
  name: 'my-select-icon',
})
</script>

<style scoped lang="scss"></style>
