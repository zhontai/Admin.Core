<template>
  <el-dropdown style="margin-left: 10px" v-bind="$attrs">
    <el-button size="small" text type="primary"
      >{{ innerButtonText }}<el-icon class="el-icon--right"><component :is="innerIcon" /></el-icon
    ></el-button>
    <template #dropdown>
      <slot name="dropdown" />
    </template>
  </el-dropdown>
</template>

<script lang="ts">
import { defineComponent, computed, PropType } from 'vue'

export default defineComponent({
  name: 'MyDropdownMore',
  props: {
    iconOnly: {
      type: Boolean,
      default: false,
    },
    icon: {
      type: String as PropType<string | undefined | null>,
      default: '',
    },
    buttonText: {
      type: String as PropType<string | undefined | null>,
      default: '',
    },
  },
  setup(props) {
    const innerIcon = computed(() => {
      return props.icon ? props.icon : props.iconOnly ? 'ele-MoreFilled' : 'ele-ArrowDown'
    })

    const innerButtonText = computed(() => {
      return props.iconOnly ? '' : props.buttonText ? props.buttonText : '更多'
    })

    return {
      innerIcon,
      innerButtonText,
    }
  },
})
</script>

<style scoped lang="scss"></style>
