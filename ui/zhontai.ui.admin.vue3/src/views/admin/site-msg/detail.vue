<template>
  <div class="w100 h100 my-msg-card-box">
    <div class="h100 my-msg-box">
      <div class="h100 w-e-text-container my-msg-content-box" v-html="state.msg.content"></div>
    </div>
  </div>
</template>

<script lang="ts" setup name="admin/site-msg/detail">
import { reactive, onMounted } from 'vue'
import { SiteMsgApi } from '/@/api/admin/SiteMsg'
import { SiteMsgGetContentOutput } from '/@/api/admin/data-contracts'
import { LocationQuery, useRoute } from 'vue-router'
import eventBus from '/@/utils/mitt'

const route = useRoute()

const state = reactive({
  loading: false,
  query: {} as LocationQuery,
  msg: {} as SiteMsgGetContentOutput,
})

onMounted(async () => {
  state.query = route.query
  await getContent()
  if (!state.msg?.isRead) await sedRead()
})

const sedRead = async () => {
  const res = await new SiteMsgApi().setRead({ id: (state.query.id || 0) as number }).catch(() => {})
  if (res?.success) {
    eventBus.emit('refreshSiteMsg')
    eventBus.emit('checkUnreadMsg')
  }
}

const getContent = async () => {
  state.loading = true

  const res = await new SiteMsgApi().getContent({ id: Number(state.query.id) }).catch(() => {
    state.loading = false
  })

  state.msg = res?.data as SiteMsgGetContentOutput

  state.loading = false
}
</script>

<style scoped lang="scss">
.my-msg-card-box {
  padding: 10px;
  :deep() {
    .el-card__body {
      height: 100%;
    }
  }
  .my-msg-box {
    padding: 20px;

    .my-msg-content-box {
      min-width: 320px;
      max-width: 700px;
      border: 1px solid var(--next-border-color-light);
      background-color: var(--el-fill-color-blank);
      color: var(--el-text-color-primary);
      margin: auto;
      padding: 30px;
    }
  }
}
</style>
