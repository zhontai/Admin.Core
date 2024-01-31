<template>
  <div class="my-layout">
    <el-card class="mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form :inline="true" @submit.stop.prevent>
        <el-form-item>
          <el-input v-model="state.filter.topic" placeholder="任务名称" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          <el-button v-auth="'api:admin:task:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table v-loading="state.loading" :data="state.taskListData" row-key="id" style="width: 100%">
        <el-table-column prop="id" label="任务编号" width="126" />
        <el-table-column prop="topic" label="任务名称" min-width="120" show-overflow-tooltip />
        <el-table-column prop="status" label="任务状态" width="80">
          <template #default="{ row }">
            <el-tag v-if="row.status === 0 || row.status === 'Running'" disable-transitions>运行中</el-tag>
            <el-tag v-if="row.status === 1 || row.status === 'Paused'" type="info" disable-transitions>停止</el-tag>
            <el-tag v-if="row.status === 2 || row.status === 'Completed'" type="success" disable-transitions>完成</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="round" label="运行次数" width="80" />
        <el-table-column prop="currentRound" label="当前次数" width="80" />
        <el-table-column prop="errorTimes" label="失败次数" width="80">
          <template #default="{ row }">
            <el-text class="mx-1" type="danger">{{ row.errorTimes }}</el-text>
          </template>
        </el-table-column>
        <el-table-column prop="body" label="任务数据" min-width="260" />
        <el-table-column prop="interval" label="定时类型" width="100" :formatter="formatterInterval" />
        <el-table-column prop="intervalArgument" label="定时参数" min-width="180" />
        <el-table-column prop="createTime" label="创建时间" :formatter="formatterTime" width="100" />
        <el-table-column prop="lastRunTime" label="最后运行时间" :formatter="formatterTime" width="120" />
        <el-table-column label="操作" width="180" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <div class="my-flex">
              <el-button v-auth="'api:admin:task-log:get-page'" icon="ele-Tickets" size="small" text type="primary" @click="onShowLogs(row)"
                >日志</el-button
              >
              <el-button v-auth="'api:admin:task:update'" icon="ele-Edit" size="small" text type="primary" @click="onUpdate(row)">修改</el-button>
              <el-button v-auth="'api:admin:task:delete'" icon="ele-Delete" size="small" text type="danger" @click="onDelete(row)">删除</el-button>
            </div>

            <div class="my-flex">
              <el-button v-auth="'api:admin:task:run'" icon="ele-Promotion" size="small" text type="primary" @click="onRun(row)">执行</el-button>
              <el-button v-auth="'api:admin:task:add'" icon="ele-CopyDocument" size="small" text type="primary" @click="onCopy(row)">复制</el-button>
              <el-button
                v-if="row.status === 1 || row.status === 'Paused'"
                v-auth="'api:admin:task:pause'"
                icon="ele-CaretRight"
                size="small"
                text
                type="primary"
                @click="onStart(row)"
                >启动</el-button
              >
              <el-button
                v-if="row.status === 0 || row.status === 'Running'"
                v-auth="'api:admin:task:resume'"
                icon="ele-VideoPause"
                size="small"
                text
                type="primary"
                @click="onPause(row)"
                >停止</el-button
              >
            </div>
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

    <task-logs ref="taskLogsRef" :title="state.taskLogsTitle"></task-logs>
    <task-form ref="taskFormRef" :title="state.taskFormTitle"></task-form>
  </div>
</template>

<script lang="ts" setup name="admin/task">
import { ref, reactive, onMounted, onBeforeMount, getCurrentInstance, defineAsyncComponent } from 'vue'
import { TaskListOutput, PageInputTaskGetPageDto } from '/@/api/admin/data-contracts'
import { TaskApi } from '/@/api/admin/Task'
import dayjs from 'dayjs'
import eventBus from '/@/utils/mitt'
import { cloneDeep } from 'lodash-es'

// 引入组件
const TaskLogs = defineAsyncComponent(() => import('./components/task-logs.vue'))
const TaskForm = defineAsyncComponent(() => import('./components/task-form.vue'))

const { proxy } = getCurrentInstance() as any

const taskLogsRef = ref()
const taskFormRef = ref()

const state = reactive({
  loading: false,
  taskFormTitle: '',
  filter: {
    topic: '',
  },
  total: 0,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInputTaskGetPageDto,
  taskListData: [] as Array<TaskListOutput>,
  taskLogsTitle: '',
})

onMounted(() => {
  onQuery()
  eventBus.off('refreshTask')
  eventBus.on('refreshTask', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshTask')
})

const formatterInterval = (row: any, column: any, cellValue: any) => {
  let label = ''
  switch (cellValue) {
    case 1:
    case 'SEC':
      label = '按秒触发'
      break
    case 11:
    case 'RunOnDay':
      label = '每天'
      break
    case 12:
    case 'RunOnWeek':
      label = '每周几'
      break
    case 13:
    case 'RunOnMonth':
      label = '每月第几日'
      break
    case 21:
    case 'Custom':
      label = 'Cron表达式'
      break
  }
  return label
}

const formatterTime = (row: any, column: any, cellValue: any) => {
  return dayjs(cellValue).format('YYYY-MM-DD HH:mm:ss')
}

const onQuery = async () => {
  state.loading = true
  state.pageInput.filter = state.filter
  const res = await new TaskApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.taskListData = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0
  state.loading = false
}

const onAdd = () => {
  state.taskFormTitle = '新增任务'
  taskFormRef.value.open()
}

const onUpdate = (row: TaskListOutput) => {
  state.taskFormTitle = '修改任务'
  taskFormRef.value.open(row)
}

const onCopy = (row: TaskListOutput) => {
  state.taskFormTitle = '新增任务'
  var task = cloneDeep(row)
  task.id = null
  taskFormRef.value.open(task)
}

// 查看日志
const onShowLogs = (row: TaskListOutput) => {
  state.taskLogsTitle = `${row.topic}${row.id}运行日志`
  taskLogsRef.value.open(row)
}

const onRun = (row: TaskListOutput) => {
  proxy.$modal
    .confirmDelete(`确定要运行【${row.topic}】任务?`)
    .then(async () => {
      await new TaskApi().run({ id: row.id as string }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

const onPause = (row: TaskListOutput) => {
  proxy.$modal
    .confirmDelete(`确定要停止【${row.topic}】任务?`)
    .then(async () => {
      await new TaskApi().pause({ id: row.id as string }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

const onStart = (row: TaskListOutput) => {
  proxy.$modal
    .confirmDelete(`确定要启动【${row.topic}】任务?`)
    .then(async () => {
      await new TaskApi().resume({ id: row.id as string }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

const onDelete = (row: TaskListOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${row.topic}】任务?`)
    .then(async () => {
      await new TaskApi().delete({ id: row.id as string }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

const onSizeChange = (val: number) => {
  state.pageInput.pageSize = val
  onQuery()
}

const onCurrentChange = (val: number) => {
  state.pageInput.currentPage = val
  onQuery()
}
</script>

<style scoped lang="scss"></style>
