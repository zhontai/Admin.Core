<template>
  <div class="my-layout">
    <el-card class="mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form :model="state.filterModel" :inline="true" @submit.stop.prevent>
        <el-form-item prop="name">
          <el-input v-model="state.filterModel.createdUserName" placeholder="登录账户" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table v-loading="state.loading" :data="state.loginLogListData" row-key="id" style="width: 100%">
        <el-table-column prop="createdUserName" label="登录账号" width="100">
          <template #default="{ row }"> {{ row.createdUserName }}<br />{{ row.nickName }} </template>
        </el-table-column>
        <el-table-column prop="ip" label="IP地址" width="130" />
        <el-table-column prop="browser" label="浏览器" width="100" />
        <el-table-column prop="os" label="操作系统" width="100" />
        <el-table-column prop="elapsedMilliseconds" label="耗时(毫秒)" width="100" />
        <el-table-column prop="status" label="登录状态" width="80">
          <template #default="{ row }">
            <el-tag :type="row.status ? 'success' : 'danger'" disable-transitions>{{ row.status ? '成功' : '失败' }}</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="createdTime" label="登录时间" :formatter="formatterTime" width="160" />
        <el-table-column prop="msg" label="登录消息" width="" />
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

<script lang="ts" setup name="admin/loginLog">
import { reactive, onMounted } from 'vue'
import { LoginLogListOutput, PageInputLogGetPageDto } from '/@/api/admin/data-contracts'
import { LoginLogApi } from '/@/api/admin/LoginLog'
import dayjs from 'dayjs'

const state = reactive({
  loading: false,
  loginLogFormTitle: '',
  filterModel: {
    createdUserName: '',
  },
  total: 0,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInputLogGetPageDto,
  loginLogListData: [] as Array<LoginLogListOutput>,
  loginLogLogsTitle: '',
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
  const res = await new LoginLogApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.loginLogListData = res?.data?.list ?? []
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
</script>

<style scoped lang="scss"></style>
