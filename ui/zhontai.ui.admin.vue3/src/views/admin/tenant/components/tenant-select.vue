<template>
  <el-dialog
    v-model="state.showDialog"
    destroy-on-close
    :title="title"
    append-to-body
    draggable
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    width="780px"
  >
    <div style="padding: 0px 0px 8px 8px; background-color: var(--ba-bg-color)">
      <el-card shadow="never" :body-style="{ paddingBottom: '0' }" style="margin-top: 8px">
        <el-form :model="state.filter" :inline="true" @submit.stop.prevent>
          <el-form-item label="企业名" prop="name">
            <el-input v-model="state.filter.name" placeholder="企业名" @keyup.enter="onQuery" />
          </el-form-item>
          <el-form-item>
            <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          </el-form-item>
        </el-form>
      </el-card>

      <el-card shadow="never" style="margin-top: 8px">
        <el-table
          ref="tenantTableRef"
          :data="state.tenantListData"
          style="width: 100%"
          v-loading="state.loading"
          row-key="id"
          default-expand-all
          :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
          :highlight-current-row="!multiple"
          @row-click="onRowClick"
          @row-dblclick="onRowDbClick"
        >
          <el-table-column v-if="multiple" type="selection" width="55" />
          <el-table-column prop="name" label="企业名" min-width="80" show-overflow-tooltip />
          <el-table-column prop="code" label="企业编码" min-width="120" show-overflow-tooltip />
          <!-- <el-table-column prop="email" label="邮箱" min-width="120" show-overflow-tooltip /> -->
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
    </div>
    <template #footer>
      <span class="dialog-footer">
        <el-button @click="onCancel" size="default">取 消</el-button>
        <el-button type="primary" @click="onSure" size="default" :loading="sureLoading">确 定</el-button>
      </span>
    </template>
  </el-dialog>
</template>

<script lang="ts" setup name="admin/tenant/components/tenant-select">
import { ref, reactive } from 'vue'
import { ElTable } from 'element-plus'
import { TenantListOutput, PageInputTenantGetPageDto } from '/@/api/admin/data-contracts'
import { TenantApi } from '/@/api/admin/Tenant'

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

const tenantTableRef = ref<InstanceType<typeof ElTable>>()

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
      name: '',
    },
  } as PageInputTenantGetPageDto,
  tenantListData: [] as Array<TenantListOutput>,
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
  state.pageInput.filter = state.filter
  const res = await new TenantApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.tenantListData = res?.data?.list ?? []
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

const onRowClick = (row: TenantListOutput) => {
  // TODO: improvement typing when refactor table
  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  // @ts-expect-error
  tenantTableRef.value!.toggleRowSelection(row, props.multiple ? undefined : true)
}

const onRowDbClick = () => {
  if (!props.multiple) {
    onSure()
  }
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

// 确定
const onSure = () => {
  const selectionRows = tenantTableRef.value!.getSelectionRows() as TenantListOutput[]
  emits('sure', props.multiple ? selectionRows : selectionRows.length > 0 ? selectionRows[0] : null)
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
