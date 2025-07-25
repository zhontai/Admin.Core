<template>
  <el-dialog
    v-model="state.showDialog"
    destroy-on-close
    append-to-body
    draggable
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    width="780px"
  >
    <template #header="{ close, titleId, titleClass }">
      <div class="my-header">
        <div :id="titleId" :class="titleClass">
          设置{{ innerTitle }}
          <el-select v-model="state.platform" placeholder="请选择所属平台" style="width: 100px" @change="onQuery">
            <el-option v-for="item in state.dictData[DictType.PlatForm.name]" :key="item.code" :label="item.name" :value="item.code" />
          </el-select>
          菜单权限
        </div>
      </div>
    </template>
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
        <el-button @click="onCancel">取 消</el-button>
        <el-button type="primary" @click="onSure" :loading="state.sureLoading">确 定</el-button>
      </span>
    </template>
  </el-dialog>
</template>

<script lang="ts" setup name="admin/role/components/set-role-menu">
import { ref, reactive, getCurrentInstance, computed, markRaw } from 'vue'
import { RoleGetListOutput, PermissionAssignInput, DictGetListOutput, PermissionGetPermissionListOutput } from '/@/api/admin/data-contracts'
import { PermissionApi } from '/@/api/admin/Permission'
import { ElTree } from 'element-plus'
import { DictApi } from '/@/api/admin/Dict'
import { PlatformType } from '/@/api/admin.extend/enum-contracts'

/** 字典分类 */
const DictType = {
  PlatForm: { name: 'platform', desc: '平台' },
}

const props = defineProps({
  title: {
    type: String,
    default: '',
  },
})

const innerTitle = computed(() => {
  return props.title ? props.title : state.roleName ? `${state.roleName}` : ''
})

const state = reactive({
  showDialog: false,
  loading: false,
  sureLoading: false,
  permissionTreeData: [] as PermissionGetPermissionListOutput[],
  roleId: 0 as number | undefined,
  roleName: '' as string | undefined | null,
  checkedKeys: [],
  platform: PlatformType.Web.name,
  dictData: {
    [DictType.PlatForm.name]: [] as DictGetListOutput[] | null,
  },
})

const { proxy } = getCurrentInstance() as any
const permissionTreeRef = ref<InstanceType<typeof ElTree>>()

const getDictList = async () => {
  const res = await new DictApi().getList([DictType.PlatForm.name]).catch(() => {})
  if (res?.success && res.data) {
    state.dictData = markRaw(res.data)
  }
}

const getRolePermissionList = async () => {
  const res = await new PermissionApi().getRolePermissionList({ roleId: state.roleId })
  state.checkedKeys = res?.success ? (res.data as never[]) : []
}

// 打开对话框
const open = async (role: RoleGetListOutput) => {
  await getDictList()
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

  const res = await new PermissionApi().getPermissionList({ platform: state.platform }).catch(() => {
    state.loading = false
  })
  if (res && res.data && res.data.length > 0) {
    state.permissionTreeData = res.data
  } else {
    state.permissionTreeData = []
  }

  state.loading = false
}

const customNodeClass = (data: any, node: any) => {
  let isPenultimate = true
  for (const key in data.children) {
    if (data.children[key]?.children?.length ?? 0 > 0) {
      isPenultimate = false
      break
    }
  }
  return data.children?.length > 0 && isPenultimate ? `is-penultimate level${node.level}` : ''
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

// 确定
const onSure = async () => {
  state.sureLoading = true
  const permissionIds = permissionTreeRef.value?.getCheckedKeys(true)
  const input = {
    platform: state.platform,
    roleId: state.roleId,
    permissionIds: permissionIds,
  } as PermissionAssignInput
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
  &.level1 {
    .el-tree-node__children {
      padding-left: 36px;
    }
  }
  &.level2 {
    .el-tree-node__children {
      padding-left: 54x;
    }
  }
  &.level3 {
    .el-tree-node__children {
      padding-left: 72px;
    }
  }
  &.level4 {
    .el-tree-node__children {
      padding-left: 90px;
    }
  }
}
</style>
