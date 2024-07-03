<template>
  <div class="my-layout">
    <el-card class="mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form :model="state.filterModel" :inline="true" @submit.stop.prevent>
        <el-form-item prop="name">
          <el-input v-model="state.filterModel.createdUserName" placeholder="操作账号" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table ref="tableRef" v-loading="state.loading" :data="state.oprationLogListData" row-key="id" style="width: 100%">
        <el-table-column prop="createdUserName" label="操作账号" width="100">
          <template #default="{ row }"> {{ row.createdUserName }}<br />{{ row.nickName }} </template>
        </el-table-column>
        <el-table-column prop="ip" label="IP地址" width="130" />
        <el-table-column prop="apiLabel" label="操作名称" min-width="220" />
        <el-table-column prop="apiPath" label="操作接口" min-width="260" />
        <el-table-column prop="elapsedMilliseconds" label="耗时(毫秒)" width="100" />
        <el-table-column prop="status" label="操作状态" width="80">
          <template #default="{ row }">
            <el-tag :type="row.status ? 'success' : 'danger'" disable-transitions>{{ row.status ? '成功' : '失败' }}</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="createdTime" label="操作时间" :formatter="formatterTime" width="160" />
        <el-table-column prop="msg" label="操作消息" width="" />
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
  </div>
</template>

<script lang="ts" setup name="admin/oprationLog">
import { reactive, onMounted, ref } from 'vue'
import { OprationLogListOutput, PageInputLogGetPageDto } from '/@/api/admin/data-contracts'
import { OprationLogApi } from '/@/api/admin/OprationLog'
import dayjs from 'dayjs'
import type { TableInstance } from 'element-plus'

const tableRef = ref<TableInstance>()

const state = reactive({
  loading: false,
  oprationLogFormTitle: '',
  filterModel: {
    createdUserName: '',
  },
  total: 0,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInputLogGetPageDto,
  oprationLogListData: [] as Array<OprationLogListOutput>,
  oprationLogLogsTitle: '',
})

onMounted(() => {
  onQuery()
})

const formatterTime = (row: any, column: any, cellValue: any) => {
  return dayjs(cellValue).format('YYYY-MM-DD HH:mm:ss')
}

const onQuery = async () => {
  state.loading = true
  state.pageInput.filter = state.filterModel
  const res = await new OprationLogApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.oprationLogListData = res?.data?.list ?? []
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
