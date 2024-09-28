<template>
  <el-drawer v-model="state.showDialog" direction="ltr" :size="size">
    <template #header="{ titleId, titleClass }">
      <h4 :id="titleId" :class="titleClass">{{ title }}</h4>
      <el-icon v-if="state.isFull" class="el-drawer__btn" @click="state.isFull = !state.isFull" title="还原"><ele-CopyDocument /></el-icon>
      <el-icon v-else class="el-drawer__btn" @click="state.isFull = !state.isFull" title="最大化"><ele-FullScreen /></el-icon>
    </template>
    <div class="my-fill h100">
      <el-table v-loading="state.loading" :data="state.taskLogListData" row-key="id" style="width: 100%">
        <el-table-column prop="round" label="当前次数" width="90" />
        <el-table-column prop="success" label="状态" width="90">
          <template #default="{ row }">
            <el-tag v-if="!row.success" type="danger" disable-transitions>失败</el-tag>
            <el-tag v-else type="success" disable-transitions>成功</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="elapsedMilliseconds" label="耗时(ms)" width="100" />
        <el-table-column prop="exception" label="异常" min-width="180" />

        <el-table-column prop="createTime" label="创建时间" :formatter="formatterTime" width="160" />
        <el-table-column prop="remark" label="备注" min-width="180" />
      </el-table>
      <div class="my-flex my-flex-end" style="margin-top: 20px; padding: 0px 10px">
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
    </div>
    <template #footer>
      <div style="flex: auto; padding: 20px !important">
        <el-button @click="onQuery" type="primary" size="default">刷 新</el-button>
        <el-button @click="onCancel" size="default">取 消</el-button>
      </div>
    </template>
  </el-drawer>
</template>

<script lang="ts" setup name="admin/taskLog">
import { reactive, computed } from 'vue'
import { ResultOutputPageOutputTaskLog, PageInputTaskLogGetPageDto, TaskListOutput } from '/@/api/admin/data-contracts'
import { TaskLogApi } from '/@/api/admin/TaskLog'
import dayjs from 'dayjs'

defineProps({
  title: {
    type: String,
    default: '',
  },
})

const state = reactive({
  showDialog: false,
  loading: false,
  isFull: false,
  isMobile: document.body.clientWidth < 1000,
  taskLogFormTitle: '',
  total: 0,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
    filter: {
      taskId: null,
    },
  } as PageInputTaskLogGetPageDto,
  taskLogListData: [] as Array<ResultOutputPageOutputTaskLog>,
})

const size = computed(() => {
  return state.isMobile ? '100%' : state.isFull ? '100%' : '50%'
})

const formatterTime = (row: any, column: any, cellValue: any) => {
  return dayjs(cellValue).format('YYYY-MM-DD HH:mm:ss')
}

const onQuery = async () => {
  state.loading = true
  const res = await new TaskLogApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.taskLogListData = res?.data?.list ?? []
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

// 打开对话框
const open = (row: TaskListOutput) => {
  if (state.pageInput.filter) state.pageInput.filter.taskId = row.id
  onQuery()
  state.showDialog = true
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

defineExpose({
  open,
})
</script>

<style scoped lang="scss">
.my-drawer-body-padding {
  padding: 10px;
}

.el-drawer__btn {
  cursor: pointer;
  margin-right: 8px;
  &:hover {
    color: var(--el-color-primary);
  }
}
</style>
