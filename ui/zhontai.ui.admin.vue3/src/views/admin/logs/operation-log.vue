<template>
  <div class="my-layout">
    <el-card class="mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form ref="filterFormRef" :model="state.filter" :inline="true" label-width="auto" :label-position="'left'" @submit.stop.prevent>
        <el-form-item label="操作账号" prop="createdUserName">
          <el-input v-model="state.filter.createdUserName" placeholder="操作账号" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item label="操作状态" prop="status">
          <el-select v-model="state.filter.status" :empty-values="[null]" style="width: 120px" @change="onQuery">
            <el-option v-for="status in state.statusList" :key="status.name" :label="status.name" :value="status.value" />
          </el-select>
        </el-form-item>
        <el-form-item label="操作接口" prop="api">
          <el-input v-model="state.filter.api" placeholder="操作接口" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item label="操作IP" prop="ip">
          <el-input v-model="state.filter.ip" placeholder="操作IP" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item label="操作时间">
          <MyDateRange v-model:startDate="state.filter.addStartTime" v-model:endDate="state.filter.addEndTime" :shortcuts="[]" style="width: 230px" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          <el-button icon="ele-RefreshLeft" text bg @click="onReset(filterFormRef)"> 重置 </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table ref="tableRef" v-loading="state.loading" :data="state.operationLogListData" row-key="id" style="width: 100%">
        <el-table-column prop="createdUserName" label="操作账号" min-width="150" show-overflow-tooltip>
          <template #default="{ row }">
            <el-badge :type="row.status ? 'success' : 'danger'" is-dot :offset="[0, 12]"></el-badge>
            {{ row.createdUserName }}<br />{{ row.nickName }}
          </template>
        </el-table-column>
        <el-table-column prop="apiLabel" label="操作名称" min-width="220" show-overflow-tooltip />
        <el-table-column prop="apiPath" label="操作接口" min-width="260" show-overflow-tooltip />
        <el-table-column prop="ip" label="IP地址" min-width="150">
          <template #default="{ row }"> {{ row.ip }} {{ row.isp }} </template>
        </el-table-column>
        <el-table-column prop="country" label="IP所在地" min-width="150" show-overflow-tooltip>
          <template #default="{ row }"> {{ row.country }} {{ row.province }} {{ row.city }} </template>
        </el-table-column>
        <el-table-column prop="elapsedMilliseconds" label="耗时 ms" min-width="100" />
        <el-table-column prop="createdTime" label="操作时间" :formatter="formatterTime" min-width="160" />
        <el-table-column label="操作" width="100" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <el-button size="small" text type="primary" @click="onShowDetails(row)">查看详情</el-button>
          </template>
        </el-table-column>
      </el-table>
      <div class="my-flex my-flex-end" style="margin-top: 20px">
        <el-pagination
          v-model:currentPage="state.pageInput.currentPage"
          v-model:page-size="state.pageInput.pageSize"
          :total="state.total"
          :page-sizes="[10, 20, 50, 100]"
          small
          background
          @size-change="onSizeChange"
          @current-change="onCurrentChange"
          layout="total, sizes, prev, pager, next, jumper"
        />
      </div>
    </el-card>

    <Details ref="detailsRef"></Details>
  </div>
</template>

<script lang="ts" setup name="admin/operation-log">
import { reactive, onMounted, ref, defineAsyncComponent } from 'vue'
import { OperationLogGetPageOutput, PageInputOperationLogGetPageInput, OperationLogGetPageInput } from '/@/api/admin/data-contracts'
import { OperationLogApi } from '/@/api/admin/OperationLog'
import dayjs from 'dayjs'
import type { FormInstance, TableInstance } from 'element-plus'

const filterFormRef = ref<FormInstance>()
const tableRef = ref<TableInstance>()
const detailsRef = ref()

const MyDateRange = defineAsyncComponent(() => import('/@/components/my-date-range/index.vue'))
const Details = defineAsyncComponent(() => import('./components/details.vue'))

const state = reactive({
  loading: false,
  oprationLogFormTitle: '',
  filter: {} as OperationLogGetPageInput,
  total: 0,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInputOperationLogGetPageInput,
  operationLogListData: [] as Array<OperationLogGetPageOutput>,
  operationLogLogsTitle: '',
  statusList: [
    { name: '全部', value: undefined },
    { name: '成功', value: true },
    { name: '失败', value: false },
  ],
  details: {},
})

onMounted(() => {
  onQuery()
})

const formatterTime = (row: OperationLogGetPageOutput, column: any, cellValue: any) => {
  return dayjs(cellValue).format('YYYY-MM-DD HH:mm:ss')
}

const onShowDetails = (row: OperationLogGetPageOutput) => {
  detailsRef.value!.open(row)
}

const onReset = (formEl: FormInstance | undefined) => {
  if (!formEl) return
  state.filter.addStartTime = undefined
  state.filter.addEndTime = undefined
  formEl.resetFields()

  onQuery()
}

const onQuery = async () => {
  state.loading = true
  state.pageInput.filter = state.filter
  const res = await new OperationLogApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.operationLogListData = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0
  state.loading = false
}

const onSizeChange = (val: number) => {
  state.pageInput.currentPage = 1
  state.pageInput.pageSize = val
  onQuery()
}

const onCurrentChange = async (val: number) => {
  state.pageInput.currentPage = val
  await onQuery()
  tableRef.value?.setScrollTop(0)
}
</script>

<style scoped lang="scss"></style>
