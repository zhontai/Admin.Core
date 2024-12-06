<template>
  <div class="w100 h100 my-msg-card-box">
    <el-card class="w100 h100" shadow="never">
      <div class="h100 my-msg-box">
        <div class="h100 w-e-text-container my-msg-content-box" v-html="state.msg.content"></div>
      </div>
    </el-card>
  </div>
</template>

<script lang="ts" setup name="admin/site-msg/detail">
import { reactive, onMounted } from 'vue'
import { SiteMsgApi } from '/@/api/admin/SiteMsg'
import { SiteMsgGetContentOutput } from '/@/api/admin/data-contracts'
import { LocationQuery, useRoute } from 'vue-router'

const route = useRoute()

const state = reactive({
  loading: false,
  query: {} as LocationQuery,
  msg: {} as SiteMsgGetContentOutput,
})

onMounted(() => {
  state.query = route.query
  onQuery()
})

const onQuery = async () => {
  state.loading = true

  const res = await new SiteMsgApi().getContent({ id: Number(state.query.id) }).catch(() => {
    state.loading = false
  })

  state.msg.content = res?.data?.content

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
    background-color: #fff;

    .my-msg-content-box {
      min-width: 320px;
      max-width: 700px;
      border: 1px solid #f6f6f6;
      background-color: #f7f8fa;
      margin: auto;
      padding: 30px;
    }
  }
}
</style>
