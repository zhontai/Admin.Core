<template>
  <el-dialog
    v-model="state.showDialog"
    destroy-on-close
    :title="innerTitle"
    append-to-body
    draggable
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    width="780px"
  >
    <div>
      <el-tree
        ref="permissionTreeRef"
        :data="state.permissionTreeData"
        node-key="id"
        show-checkbox
        highlight-current
        default-expand-all
        check-on-click-node
        :expand-on-click-node="false"
        :props="{ class: customNodeClass }"
        :default-checked-keys="state.checkedKeys"
      />
    </div>
    <template #footer>
      <span class="dialog-footer">
        <el-button @click="onCancel" size="default">取 消</el-button>
        <el-button type="primary" @click="onSure" size="default" :loading="state.sureLoading">确 定</el-button>
      </span>
    </template>
  </el-dialog>
</template>

<script lang="ts" setup name="admin/role/components/set-role-menu">
import { ref, reactive, getCurrentInstance, computed } from 'vue'
import { RoleGetListOutput, PermissionAssignInput } from '/@/api/admin/data-contracts'
import { PermissionApi } from '/@/api/admin/Permission'
import { ElTree } from 'element-plus'
import { listToTree } from '/@/utils/tree'
import { cloneDeep } from 'lodash-es'

const props = defineProps({
  title: {
    type: String,
    default: '',
  },
})

const innerTitle = computed(() => {
  return props.title ? props.title : state.roleName ? `设置【${state.roleName}】菜单权限` : '设置菜单权限'
})

const state = reactive({
  showDialog: false,
  loading: false,
  sureLoading: false,
  permissionTreeData: [],
  roleId: 0 as number | undefined,
  roleName: '' as string | undefined | null,
  checkedKeys: [],
})

const { proxy } = getCurrentInstance() as any
const permissionTreeRef = ref<InstanceType<typeof ElTree>>()

const getRolePermissionList = async () => {
  const res = await new PermissionApi().getRolePermissionList({ roleId: state.roleId })
  state.checkedKeys = res?.success ? (res.data as never[]) : []
}

// 打开对话框
const open = async (role: RoleGetListOutput) => {
  state.roleId = role.id
  state.roleName = role.name
  proxy.$modal.loading()
  await onQuery()
  await getRolePermissionList()
  proxy.$modal.closeLoading()
  state.showDialog = true
}

// 关闭对话框
const close = () => {
  state.showDialog = false
}

const onQuery = async () => {
  state.loading = true

  const res = await new PermissionApi().getPermissionList().catch(() => {
    state.loading = false
  })
  if (res && res.data && res.data.length > 0) {
    state.permissionTreeData = listToTree(cloneDeep(res.data))
  } else {
    state.permissionTreeData = []
  }

  state.loading = false
}

const customNodeClass = (data: any) => {
  return data.row ? 'is-penultimate' : ''
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

// 确定
const onSure = async () => {
  state.sureLoading = true
  const permissionIds = permissionTreeRef.value?.getCheckedKeys(true)
  const input = { roleId: state.roleId, permissionIds: permissionIds } as PermissionAssignInput
  const res = await new PermissionApi().assign(input, { showSuccessMessage: true }).catch(() => {
    state.sureLoading = false
  })
  state.sureLoading = false

  if (res?.success) {
    state.showDialog = false
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
:deep(.is-penultimate) {
  .el-tree-node__children {
    padding-left: 65px;
    white-space: pre-wrap;
    line-height: 100%;

    .el-tree-node {
      display: inline-block;
    }

    .el-tree-node__content {
      padding-left: 12px !important;
      padding-right: 12px;

      .el-tree-node__expand-icon.is-leaf {
        display: none;
      }
    }
  }
}
</style>
