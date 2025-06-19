<template>
  <MyLayout>
    <el-card v-show="state.showQuery" class="my-query-box mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form class="my-form-inline" :inline="true" label-width="auto" @submit.stop.prevent>
        <el-form-item label="">
          <el-select v-model="state.filter.isRead" :empty-values="[undefined]" style="width: 90px" @change="onQuery">
            <el-option v-for="status in state.statusList" :key="status.name" :label="status.name" :value="status.value" />
          </el-select>
        </el-form-item>
        <el-form-item label="分类">
          <el-tree-select
            v-model="state.filter.typeId"
            placeholder="请选择分类"
            :data="state.msgTypeTreeData"
            node-key="id"
            :props="{ label: 'name' }"
            check-strictly
            default-expand-all
            fit-input-width
            clearable
            filterable
            @change="onQuery"
          />
        </el-form-item>
        <el-form-item label="标题">
          <el-input v-model="state.filter.title" placeholder="标题" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <div class="my-tools-box mb8 my-flex my-flex-between">
        <div>
          <el-button type="danger" :disabled="!isRowSelect" :loading="state.loadingBatchDelete" @click="onBatchDelete">删除</el-button>
          <el-button type="primary" :disabled="!isRowSelect" :loading="state.loadingBatchSetRead" @click="onBatchSetRead">标为已读</el-button>
          <el-button type="primary" :loading="state.loadingSetAllRead" @click="onSetAllRead">全部已读</el-button>
        </div>
        <div>
          <el-tooltip effect="dark" :content="state.showQuery ? '隐藏查询' : '显示查询'" placement="top">
            <el-button :icon="state.showQuery ? 'ele-ArrowUp' : 'ele-ArrowDown'" circle @click="state.showQuery = !state.showQuery" />
          </el-tooltip>
        </div>
      </div>
      <el-table
        ref="tableRef"
        :data="state.msgList"
        style="width: 100%"
        v-loading="state.loading"
        row-key="id"
        default-expand-all
        border
        :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
      >
        <el-table-column type="selection" width="55" />
        <el-table-column prop="title" label="标题" min-width="120" show-overflow-tooltip>
          <template #default="{ row }">
            <div class="my-flex my-flex-items-center">
              <MyLink
                :model-value="{
                  path: '/site-msg/detail',
                  query: { id: row.id, tagsViewName: row.title },
                }"
                icon="ele-Message"
                :type="row.isRead ? '' : 'primary'"
                :bold="!row.isRead"
              >
                {{ row.title }}
              </MyLink>
            </div>
          </template>
        </el-table-column>
        <el-table-column prop="typeName" label="消息分类" min-width="120" show-overflow-tooltip />
        <el-table-column prop="receivedTime" label="接收时间" :formatter="formatterTime" min-width="160" show-overflow-tooltip />
      </el-table>
      <div class="my-flex my-flex-end" style="margin-top: 10px">
        <el-pagination
          v-model:currentPage="state.pageInput.currentPage"
          v-model:page-size="state.pageInput.pageSize"
          :total="state.total"
          :page-sizes="[10, 20, 50, 100]"
          background
          @size-change="onSizeChange"
          @current-change="onCurrentChange"
          layout="total, sizes, prev, pager, next, jumper"
        />
      </div>
    </el-card>
  </MyLayout>
</template>

<script lang="ts" setup name="admin/site-msg">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, computed, defineAsyncComponent } from 'vue'
import eventBus from '/@/utils/mitt'
import dayjs from 'dayjs'
import { SiteMsgApi } from '/@/api/admin/SiteMsg'
import { PageInputSiteMsgGetPageInput, SiteMsgGetPageOutput, MsgTypeGetListOutput } from '/@/api/admin/data-contracts'
import { listToTree } from '/@/utils/tree'
import { MsgTypeApi } from '/@/api/admin/MsgType'
import { ElTable } from 'element-plus'

const MyLink = defineAsyncComponent(() => import('/@/components/my-link/index.vue'))

const tableRef = ref<InstanceType<typeof ElTable>>()
const { proxy } = getCurrentInstance() as any

const state = reactive({
  loading: false,
  showQuery: true,
  loadingSetAllRead: false,
  loadingBatchDelete: false,
  loadingBatchSetRead: false,
  orgFormTitle: '',
  statusList: [
    { name: '全部', value: null },
    { name: '已读', value: true },
    { name: '未读', value: false },
  ],
  filter: {
    isRead: null,
    typeId: null,
    title: '',
  },
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
  msgTypeTreeData: [] as MsgTypeGetListOutput[],
})

const selectionRows = computed(() => {
  return tableRef.value?.getSelectionRows()
})

const rowSelectCount = computed(() => {
  return selectionRows.value?.length
})

const isRowSelect = computed(() => {
  return rowSelectCount.value > 0
})

const selectionIds = computed(() => {
  return selectionRows.value?.map((a: any) => a.id)
})

onMounted(async () => {
  await getMsgTypes()
  onQuery()
  eventBus.off('refreshSiteMsg')
  eventBus.on('refreshSiteMsg', () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshSiteMsg')
})

const formatterTime = (row: any, column: any, cellValue: any) => {
  return cellValue ? dayjs(cellValue).format('YYYY-MM-DD HH:mm:ss') : ''
}

const getMsgTypes = async () => {
  const res = await new MsgTypeApi().getList().catch(() => {
    state.msgTypeTreeData = []
  })
  if (res?.success && res.data && res.data.length > 0) {
    state.msgTypeTreeData = listToTree(res.data)
  } else {
    state.msgTypeTreeData = []
  }
}

const onQuery = async () => {
  state.loading = true
  if (state.pageInput.filter) {
    state.pageInput.filter = state.filter
  }

  const res = await new SiteMsgApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.msgList = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0

  state.loading = false
}

const onSizeChange = (val: number) => {
  state.pageInput.currentPage = 1
  state.pageInput.pageSize = val
  onQuery()
}

const onCurrentChange = (val: number) => {
  state.pageInput.currentPage = val
  onQuery()
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
        onQuery()
      }
    })
    .catch(() => {})
}

const onBatchDelete = () => {
  proxy.$modal
    .confirmDelete(`确定要删除消息?`)
    .then(async () => {
      state.loadingBatchDelete = true
      const res = await new SiteMsgApi().batchSoftDelete(selectionIds.value).catch(() => {
        state.loadingBatchDelete = false
      })
      state.loadingBatchDelete = false
      if (res?.success) {
        proxy.$modal.msgSuccess('删除成功')
        onQuery()
      }
    })
    .catch(() => {})
}

const onBatchSetRead = () => {
  proxy.$modal
    .confirmDelete(`确定要标记消息为已读?`)
    .then(async () => {
      state.loadingBatchSetRead = true
      const res = await new SiteMsgApi().batchSetRead(selectionIds.value).catch(() => {
        state.loadingBatchSetRead = false
      })
      state.loadingBatchSetRead = false
      if (res?.success) {
        proxy.$modal.msgSuccess('标记已读成功')
        onQuery()
      }
    })
    .catch(() => {})
}
</script>

<style scoped lang="scss">
.my-form-inline {
  :deep() {
    .el-select {
      --el-select-width: 192px;
    }
  }
}
</style>
