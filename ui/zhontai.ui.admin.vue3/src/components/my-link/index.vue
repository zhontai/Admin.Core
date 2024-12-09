<template>
  <el-link type="primary" :underline="false" :href="href" @click.prevent.stop="onToPage()">
    <slot> </slot>
  </el-link>
</template>

<script lang="ts" setup name="my-link">
import { computed } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const model = defineModel({ type: Object })

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

<style scoped lang="scss"></style>
