<template>
  <el-select
    v-model="state.label"
    :placeholder="t('请选择直属主管')"
    remote
    :suffix-transition="false"
    suffix-icon="ele-MoreFilled"
    class="w100"
    v-bind="$attrs"
    @click="onOpen"
    @clear="onClear"
  >
  </el-select>

  <user-select ref="userSelectRef" :title="t('选择直属主管')" :modal="true" @sure="onSureUser" v-bind="$attrs"></user-select>
</template>

<script lang="ts" setup name="admin/user/components/my-select-user">
import { UserGetPageOutput } from '/@/api/admin/data-contracts'
import { t } from '/@/i18n'

// 引入组件
const UserSelect = defineAsyncComponent(() => import('./user-select.vue'))

const props = defineProps({
  name: String as PropType<string | undefined | null>,
  modelValue: Number as PropType<number | undefined | null>,
})

const emits = defineEmits(['update:modelValue'])

const userSelectRef = useTemplateRef('userSelectRef')

const state = reactive({
  label: props.name,
})

const onOpen = () => {
  userSelectRef.value!.open()
}

const onClear = () => {
  emits('update:modelValue', undefined)
}

const onSureUser = async (user: UserGetPageOutput) => {
  userSelectRef.value?.close()
  if ((user?.id as number) > 0) {
    state.label = user.name as string

    emits('update:modelValue', user.id)
  }
}
</script>

<style scoped lang="scss"></style>
