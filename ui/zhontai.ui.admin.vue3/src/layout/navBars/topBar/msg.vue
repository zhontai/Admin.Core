<template>
  <el-drawer v-model="state.visible" direction="rtl" destroy-on-close size="384px" @closed="onClosed">
    <template #header="{ titleId, titleClass }">
      <div class="my-flex my-flex-between mr20">
        <span :id="titleId" :class="titleClass">{{ $t('站内信') }}</span>
        <el-link underline="never" type="primary" class="f12" @click="onShowMore">查看更多</el-link>
      </div>
    </template>

    <div class="my-flex-column msg-box">
      <div class="my-flex my-flex-between msg-tools">
        <el-segmented v-model="state.isRead" :options="state.segmentedOptions" @change="onQuery" />
        <el-button v-if="!isEmpty" icon="ele-CircleCheck" link type="primary" :loading="state.loadingSetAllRead" @click="onSetAllRead">
          全部已读
        </el-button>
      </div>
      <div class="my-flex-fill" style="overflow: hidden" v-loading="state.loading">
        <el-scrollbar>
          <div>
            <el-empty v-if="isEmpty" description="暂无消息" />
            <template v-else>
              <div v-for="(msg, index) in state.msgList" :key="msg.msgId" class="msg-item" @click="onToDetail(msg)">
                <div class="msg-item__title" :class="{ 'msg-item__title--unread': !msg.isRead }">{{ msg.title }}</div>
                <div class="msg-item__time">{{ formatterTime(msg.receivedTime) }}</div>
                <el-button v-if="!msg.isRead" class="msg-item__read" link type="primary" @click.prevent.stop="onSetRead(msg)">标为已读</el-button>
              </div>
            </template>
          </div>
        </el-scrollbar>
      </div>
    </div>
  </el-drawer>
</template>

<script setup lang="ts" name="layoutBreadcrumbMsg">
import { SiteMsgApi } from '/@/api/admin/SiteMsg'
import { PageInputSiteMsgGetPageInput, SiteMsgGetPageOutput } from '/@/api/admin/data-contracts'
import dayjs from 'dayjs'
import eventBus from '/@/utils/mitt'

const { proxy } = getCurrentInstance() as any
const router = useRouter()

const state = reactive({
  visible: false,
  loading: false,
  isRead: null,
  loadingSetAllRead: false,
  segmentedOptions: [
    {
      label: '全部',
      value: null,
    },
    {
      label: '未读',
      value: false,
    },
  ],
  pageInput: {
    currentPage: 1,
    pageSize: 20,
    filter: {
      isRead: null,
      typeId: null,
      title: '',
    },
  } as PageInputSiteMsgGetPageInput,
  total: 0,
  msgList: [] as SiteMsgGetPageOutput[],
})

const isEmpty = computed(() => {
  return !(state.msgList?.length > 0)
})
const formatterTime = (time: any) => {
  return time ? dayjs(time).format('YYYY-MM-DD HH:mm:ss') : ''
}

const onShowMore = () => {
  state.visible = false
  router.push('/site-msg')
}

const onQuery = async () => {
  state.loading = true
  if (state.pageInput.filter) {
    state.pageInput.filter.isRead = state.isRead
  }

  const res = await new SiteMsgApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.msgList = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0

  state.loading = false
}

const onSetAllRead = () => {
  proxy.$modal
    .confirm(`确认标记所有消息为已读吗？`)
    .then(async () => {
      state.loadingSetAllRead = true
      const res = await new SiteMsgApi().setAllRead().catch(() => {
        state.loadingSetAllRead = false
      })

      state.loadingSetAllRead = false
      if (res?.success) {
        proxy.$modal.msgSuccess('标记所有已读成功')
        eventBus.emit('refreshSiteMsg')
        eventBus.emit('checkUnreadMsg')
        onQuery()
      }
    })
    .catch(() => {})
}

const onSetRead = async (msg: SiteMsgGetPageOutput) => {
  msg.isRead = true
  const res = await new SiteMsgApi().setRead({ id: msg.id }).catch(() => {})
  if (res?.success) {
    eventBus.emit('refreshSiteMsg')
    eventBus.emit('checkUnreadMsg')
    onQuery()
  }
}

const onToDetail = (row: SiteMsgGetPageOutput) => {
  state.visible = false
  router.push({
    path: '/site-msg/detail',
    query: { id: row.id, tagsViewName: row.title },
  })
}

const openDrawer = () => {
  state.visible = true
  onQuery()
}

const onClosed = () => {
  state.isRead = null
}

// 暴露变量
defineExpose({
  openDrawer,
})
</script>

<style scoped lang="scss">
.msg-box {
  background-color: var(--next-bg-main-color);
  height: 100%;

  .msg-tools {
    padding: 10px 15px;
    margin-bottom: 10px;
    background-color: var(--el-bg-color);
    border-bottom: 1px solid var(--next-border-color-light);
  }

  .msg-item {
    margin-bottom: 10px;
    padding: 10px 15px;
    background-color: var(--el-bg-color);
    font-size: 14px;
    cursor: pointer;
    position: relative;

    .msg-item__title--unread {
      font-weight: 600;
    }
    .msg-item__title {
      color: var(--el-text-color-primary);
    }
    .msg-item__time {
      font-size: 12px;
      color: var(--el-text-color-secondary);
      margin-top: 5px;
    }

    .msg-item__read {
      position: absolute;
      top: 10px;
      right: 15px;
      font-size: 12px;
      display: none;
    }

    &:hover {
      .msg-item__read {
        display: block;
      }
      background-color: var(--el-fill-color);
    }
  }
}
</style>
