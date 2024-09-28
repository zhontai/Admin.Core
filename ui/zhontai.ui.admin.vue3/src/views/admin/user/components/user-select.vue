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
    <div style="padding: 0px 0px 8px 8px; background-color: var(--ba-bg-color)">
      <el-row :gutter="8" style="width: 100%">
        <el-col :xs="24" :sm="9">
          <div class="my-flex-column h100">
            <org-menu @node-click="onOrgNodeClick" class="my-flex-fill"></org-menu>
          </div>
        </el-col>
        <el-col :xs="24" :sm="15">
          <el-card shadow="never" :body-style="{ paddingBottom: '0' }" style="margin-top: 8px">
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
              :highlight-current-row="!multiple"
              @row-click="onRowClick"
              @row-dblclick="onRowDbClick"
              @current-change="onTableCurrentChange"
            >
              <el-table-column v-if="multiple" type="selection" width="55" />
              <el-table-column prop="name" label="姓名" min-width="80" show-overflow-tooltip />
              <el-table-column prop="mobile" label="手机号" min-width="120" show-overflow-tooltip />
              <el-table-column prop="email" label="邮箱" min-width="180" show-overflow-tooltip />
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
                layout="total, sizes, prev, pager, next"
              />
            </div>
          </el-card>
        </el-col>
      </el-row>
    </div>
    <template #footer>
      <span class="dialog-footer">
        <el-button @click="onCancel" size="default">取 消</el-button>
        <el-button type="primary" @click="onSure" size="default" :loading="sureLoading">确 定</el-button>
      </span>
    </template>
  </el-dialog>
</template>

<script lang="ts" setup name="admin/user/components/user-select">
import { ref, reactive, defineAsyncComponent } from 'vue'
import { ElTable } from 'element-plus'
import { UserGetPageOutput, PageInputUserGetPageDto, OrgListOutput } from '/@/api/admin/data-contracts'
import { UserApi } from '/@/api/admin/User'

// 引入组件
const OrgMenu = defineAsyncComponent(() => import('/@/views/admin/org/components/org-menu.vue'))

const props = defineProps({
  title: {
    type: String,
    default: '',
  },
  multiple: {
    type: Boolean,
    default: false,
  },
  sureLoading: {
    type: Boolean,
    default: false,
  },
})

const emits = defineEmits(['sure'])

const userTableRef = ref<InstanceType<typeof ElTable>>()

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
    filter: {
      orgId: null,
    },
  } as PageInputUserGetPageDto,
  userListData: [] as Array<UserGetPageOutput>,
})

// 打开对话框
const open = () => {
  state.showDialog = true
  if (state.pageInput.filter) {
    state.pageInput.filter.orgId = null
  }

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
  const res = await new UserApi().getPage(state.pageInput).catch(() => {
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

const onOrgNodeClick = (node: OrgListOutput | null) => {
  if (state.pageInput.filter) {
    state.pageInput.filter.orgId = node?.id
  }
  onQuery()
}

const onRowClick = (row: UserGetPageOutput) => {
  userTableRef.value!.toggleRowSelection(row, props.multiple ? undefined : true)
}

const onRowDbClick = () => {
  if (!props.multiple) {
    onSure()
  }
}

const currentRow = ref()
const onTableCurrentChange = (row: UserGetPageOutput) => {
  currentRow.value = row
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

// 确定
const onSure = () => {
  if (props.multiple) {
    const selectionRows = userTableRef.value!.getSelectionRows() as UserGetPageOutput[]
    emits('sure', selectionRows)
  } else {
    emits('sure', currentRow.value)
  }
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
