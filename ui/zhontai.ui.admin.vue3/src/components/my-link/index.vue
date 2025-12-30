<template>
  <el-link
    class="my-link"
    :class="{ 'my-link--bold': bold }"
    type="primary"
    underline="never"
    :href="href"
    v-bind="$attrs"
    @click.prevent.stop="onToPage()"
  >
    <slot> </slot>
  </el-link>
</template>

<script lang="ts" setup name="my-link">
const router = useRouter()
const model = defineModel({ type: Object })

const bold = defineModel('bold', { type: Boolean, default: false })

const href = computed(() => {
  const { href } = router.resolve({
    path: model.value.path,
    query: model.value.query,
  })

  return href
})

const onToPage = () => {
  router.push({
    path: model.value.path,
    query: model.value.query,
  })
}
</script>

<style scoped lang="scss">
.my-link {
  :deep() {
    > .el-icon {
      margin-right: 2px;
    }
  }
}
.my-link--bold {
  font-weight: 600;
}
</style>
