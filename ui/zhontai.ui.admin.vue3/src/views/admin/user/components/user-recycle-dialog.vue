<template>
  <el-dialog
    v-model="state.showDialog"
    destroy-on-close
    :title="title"
    append-to-body
    draggable
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    width="880px"
  >
    <div style="background-color: var(--ba-bg-color)">
      <el-card class="my-query-box" shadow="never" :body-style="{ paddingBottom: '0' }">
        <el-form :model="state.filter" :inline="true" @submit.stop.prevent>
          <el-form-item label="姓名" prop="name">
            <el-input v-model="state.filter.name" placeholder="姓名" @keyup.enter="onQuery" />
          </el-form-item>
          <el-form-item>
            <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          </el-form-item>
        </el-form>
      </el-card>

      <el-card shadow="never" style="margin-top: 8px">
        <el-table
          ref="userTableRef"
          :data="state.userListData"
          style="width: 100%"
          v-loading="state.loading"
          row-key="id"
          default-expand-all
          :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
          border
          @row-click="onRowClick"
          @current-change="onTableCurrentChange"
        >
          <el-table-column type="selection" width="55" />
          <el-table-column prop="userName" label="账号" min-width="180" show-overflow-tooltip />
          <el-table-column prop="name" label="姓名" min-width="82" show-overflow-tooltip />
          <el-table-column prop="mobile" label="手机号" min-width="120" show-overflow-tooltip />
          <el-table-column prop="orgPaths" label="部门" min-width="200" show-overflow-tooltip />
          <el-table-column prop="orgPath" label="主属部门" min-width="180" show-overflow-tooltip />
          <el-table-column prop="roleNames" label="角色" min-width="180" show-overflow-tooltip />
          <el-table-column prop="email" label="邮箱" min-width="180" show-overflow-tooltip />
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
            layout="total, sizes, prev, pager, next"
          />
        </div>
      </el-card>
    </div>
    <template #footer>
      <span class="dialog-footer">
        <el-button @click="onCancel">取 消</el-button>
        <el-button type="primary" @click="onSure" :loading="sureLoading">恢 复</el-button>
      </span>
    </template>
  </el-dialog>
</template>

<script lang="ts" setup name="admin/user/components/user-select">
import { TableInstance } from 'element-plus'
import { UserGetDeletedUserPageOutput, PageInput } from '/@/api/admin/data-contracts'
import { UserApi } from '/@/api/admin/User'
import eventBus from '/@/utils/mitt'

defineProps({
  title: {
    type: String,
    default: '用户回收站',
  },
  sureLoading: {
    type: Boolean,
    default: false,
  },
})

const { proxy } = getCurrentInstance() as any

const emits = defineEmits(['sure'])

const userTableRef = useTemplateRef<TableInstance>('userTableRef')

const state = reactive({
  showDialog: false,
  loading: false,
  filter: {
    name: '',
  },
  total: 0,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInput,
  userListData: [] as Array<UserGetDeletedUserPageOutput>,
})

// 打开对话框
const open = () => {
  state.showDialog = true

  onQuery()
}

// 关闭对话框
const close = () => {
  state.showDialog = false
}

const onQuery = async () => {
  state.loading = true
  state.pageInput.dynamicFilter = {
    field: 'name',
    operator: 0,
    value: state.filter.name,
  }
  const res = await new UserApi().getDeletedPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.userListData = res?.data?.list ?? []
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

const onRowClick = (row: UserGetDeletedUserPageOutput) => {
  userTableRef.value!.toggleRowSelection(row, undefined)
}

const currentRow = ref()
const onTableCurrentChange = (row: UserGetDeletedUserPageOutput) => {
  currentRow.value = row
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

// 确定
const onSure = () => {
  const selectionRows = userTableRef.value!.getSelectionRows() as UserGetDeletedUserPageOutput[]
  if (!(selectionRows?.length > 0)) {
    proxy.$modal.msgWarning('请勾选用户')
    return
  }

  proxy.$modal
    .confirm(`确定要恢复?`)
    .then(async () => {
      const userIds = selectionRows.map((a) => a.id) as number[]
      await new UserApi().restore({ userIds: userIds }, { loading: true, showSuccessMessage: true })
      eventBus.emit('refreshUser')
      state.showDialog = false
    })
    .catch(() => {})
}

defineExpose({
  open,
  close,
})
</script>

<style scoped lang="scss">
:deep(.el-dialog__body) {
  padding: 5px 10px;
}
</style>
