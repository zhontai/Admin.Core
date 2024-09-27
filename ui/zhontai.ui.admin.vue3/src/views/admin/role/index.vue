<template>
  <my-layout>
    <pane size="50" min-size="30" max-size="70">
      <div class="my-flex-column w100 h100">
        <el-card class="mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
          <el-form :inline="true" @submit.stop.prevent>
            <el-form-item label="角色名称">
              <el-input v-model="state.filter.roleName" placeholder="角色名称" @keyup.enter="onQuery" />
            </el-form-item>
            <el-form-item>
              <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
              <el-dropdown v-auth="'api:admin:role:add'" style="margin-left: 12px">
                <el-button type="primary"
                  >新增<el-icon class="el-icon--right"><ele-ArrowDown /></el-icon
                ></el-button>
                <template #dropdown>
                  <el-dropdown-menu>
                    <el-dropdown-item @click="onAdd(1)">新增分组</el-dropdown-item>
                    <el-dropdown-item @click="onAdd(2)">新增角色</el-dropdown-item>
                  </el-dropdown-menu>
                </template>
              </el-dropdown>
            </el-form-item>
          </el-form>
        </el-card>

        <el-card class="my-fill mt8" shadow="never">
          <el-table
            ref="roleTableRef"
            v-loading="state.loading"
            :data="state.roleTreeData"
            row-key="id"
            default-expand-all
            :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
            highlight-current-row
            style="width: 100%"
            @current-change="onCurrentChange"
          >
            <el-table-column prop="name" label="角色名称" min-width="120" show-overflow-tooltip />
            <el-table-column prop="sort" label="排序" width="80" align="center" show-overflow-tooltip />
            <el-table-column label="操作" width="100" fixed="right" header-align="center" align="right">
              <template #default="{ row }">
                <el-button
                  v-if="row.type === 1"
                  v-auth="'api:admin:role:add'"
                  icon="ele-Plus"
                  size="small"
                  text
                  type="primary"
                  @click="onAdd(2, row)"
                ></el-button>
                <my-dropdown-more icon-only v-auths="['api:admin:permission:assign', 'api:admin:role:update', 'api:admin:role:delete']">
                  <template #dropdown>
                    <el-dropdown-menu>
                      <el-dropdown-item v-if="row.type === 2 && auth('api:admin:permission:assign')" @click="onSetRoleMenu(row)"
                        >菜单权限</el-dropdown-item
                      >
                      <el-dropdown-item v-if="row.type === 2" @click="onSetRoleDataScope(row)">数据权限</el-dropdown-item>
                      <el-dropdown-item v-if="auth('api:admin:role:update')" @click="onEdit(row)"
                        >编辑{{ row.type === 1 ? '分组' : '角色' }}</el-dropdown-item
                      >
                      <el-dropdown-item v-if="auth('api:admin:role:delete')" @click="onDelete(row)"
                        >删除{{ row.type === 1 ? '分组' : '角色' }}</el-dropdown-item
                      >
                    </el-dropdown-menu>
                  </template>
                </my-dropdown-more>
              </template>
            </el-table-column>
          </el-table>
        </el-card>
      </div>
    </pane>
    <pane>
      <div class="my-flex-column w100 h100">
        <el-card class="mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
          <el-form :inline="true" @submit.stop.prevent>
            <el-form-item label="姓名">
              <el-input v-model="state.filter.name" placeholder="姓名" @keyup.enter="onGetRoleUserList" />
            </el-form-item>
            <el-form-item>
              <el-button type="primary" icon="ele-Search" @click="onGetRoleUserList"> 查询 </el-button>
              <el-button v-auth="'api:admin:role:add-role-user'" type="primary" icon="ele-Plus" @click="onAddUser"> 添加员工 </el-button>
              <el-button v-auth="'api:admin:role:remove-role-user'" type="danger" icon="ele-Delete" @click="onRemoveUser"> 移除员工 </el-button>
            </el-form-item>
          </el-form>
        </el-card>

        <el-card class="my-fill mt8" shadow="never">
          <el-table
            ref="userTableRef"
            v-loading="state.userListLoading"
            :data="state.userListData"
            row-key="id"
            style="width: 100%"
            @row-click="onUserRowClick"
          >
            <el-table-column type="selection" width="55" />
            <el-table-column prop="name" label="姓名" min-width="120" show-overflow-tooltip />
            <el-table-column prop="mobile" label="手机号" min-width="120" show-overflow-tooltip />
            <el-table-column prop="email" label="邮箱" min-width="180" show-overflow-tooltip />
          </el-table>
        </el-card>
      </div>
    </pane>

    <role-form ref="roleFormRef" :title="state.roleFormTitle" :role-tree-data="state.roleFormTreeData"></role-form>
    <user-select
      ref="userSelectRef"
      :title="`添加【${state.roleName}】员工`"
      multiple
      :sure-loading="state.sureLoading"
      @sure="onSureUser"
    ></user-select>
    <set-role-menu ref="setRoleMenuRef"></set-role-menu>
    <set-role-data-scope ref="setRoleDataScopeRef"></set-role-data-scope>
  </my-layout>
</template>

<script lang="ts" setup name="admin/role">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, nextTick, defineAsyncComponent } from 'vue'
import { RoleGetListOutput, RoleGetRoleUserListOutput, UserGetPageOutput, RoleAddRoleUserListInput, RoleType } from '/@/api/admin/data-contracts'
import { RoleApi } from '/@/api/admin/Role'
import { listToTree, filterTree } from '/@/utils/tree'
import { ElTable } from 'element-plus'
import { cloneDeep } from 'lodash-es'
import eventBus from '/@/utils/mitt'
import { auth } from '/@/utils/authFunction'
import { Pane } from 'splitpanes'

// 引入组件
const RoleForm = defineAsyncComponent(() => import('./components/role-form.vue'))
const SetRoleMenu = defineAsyncComponent(() => import('./components/set-role-menu.vue'))
const SetRoleDataScope = defineAsyncComponent(() => import('./components/set-role-data-scope.vue'))
const UserSelect = defineAsyncComponent(() => import('/@/views/admin/user/components/user-select.vue'))
const MyDropdownMore = defineAsyncComponent(() => import('/@/components/my-dropdown-more/index.vue'))
const MyLayout = defineAsyncComponent(() => import('/@/components/my-layout/index.vue'))

const { proxy } = getCurrentInstance() as any

const roleTableRef = ref()
const roleFormRef = ref()
const userTableRef = ref<InstanceType<typeof ElTable>>()
const userSelectRef = ref()
const setRoleMenuRef = ref()
const setRoleDataScopeRef = ref()

const state = reactive({
  loading: false,
  userListLoading: false,
  sureLoading: false,
  roleFormTitle: '',
  filter: {
    name: '',
    roleName: '',
  },
  roleTreeData: [] as any,
  roleFormTreeData: [] as any,
  userListData: [] as RoleGetRoleUserListOutput[],
  roleId: undefined as number | undefined,
  roleName: '' as string | null | undefined,
})

onMounted(() => {
  onQuery()
  eventBus.off('refreshRole')
  eventBus.on('refreshRole', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshRole')
})

const onQuery = async () => {
  state.loading = true
  const res = await new RoleApi().getList().catch(() => {
    state.loading = false
  })
  if (res && res.data && res.data.length > 0) {
    state.roleTreeData = filterTree(listToTree(cloneDeep(res.data)), state.filter.roleName)
    state.roleFormTreeData = listToTree(cloneDeep(res.data).filter((a) => a.parentId === 0))
    if (state.roleTreeData.length > 0 && state.roleTreeData[0].children?.length > 0) {
      nextTick(() => {
        roleTableRef.value!.setCurrentRow(state.roleTreeData[0].children[0])
      })
    }
  } else {
    state.roleTreeData = []
    state.roleFormTreeData = []
  }

  state.loading = false
}

const onAdd = (type: RoleType, row: RoleGetListOutput | undefined = undefined) => {
  switch (type) {
    case 1:
      state.roleFormTitle = '新增分组'
      roleFormRef.value.open({ type: 1 })
      break
    case 2:
      state.roleFormTitle = '新增角色'
      roleFormRef.value.open({ type: 2, parentId: row?.id, dataScope: 1 })
      break
  }
}

const onEdit = (row: RoleGetListOutput) => {
  switch (row.type) {
    case 1:
      state.roleFormTitle = '编辑分组'
      break
    case 2:
      state.roleFormTitle = '编辑角色'
      break
  }
  roleFormRef.value.open(row)
}

const onDelete = (row: RoleGetListOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除角色【${row.name}】?`)
    .then(async () => {
      await new RoleApi().delete({ id: row.id }, { loading: true })
      onQuery()
    })
    .catch(() => {})
}

const onGetRoleUserList = async () => {
  state.userListLoading = true
  const res = await new RoleApi().getRoleUserList({ RoleId: state.roleId, Name: state.filter.name }).catch(() => {
    state.userListLoading = false
  })
  state.userListLoading = false
  if (res?.success) {
    if (res.data && res.data.length > 0) {
      state.userListData = res.data
    } else {
      state.userListData = []
    }
  }
}

const onCurrentChange = (currentRow: RoleGetListOutput, oldCurrentRow: RoleGetListOutput) => {
  if (!currentRow) {
    return
  }

  if (currentRow?.id !== oldCurrentRow?.id && (oldCurrentRow?.id as number) > 0) {
    if ((currentRow?.parentId as number) === 0) {
      roleTableRef.value!.setCurrentRow(oldCurrentRow)
    }
  } else {
    if ((currentRow?.parentId as number) === 0) {
      roleTableRef.value!.setCurrentRow()
    }
  }

  if ((currentRow?.parentId as number) !== 0 && (oldCurrentRow?.parentId as number) !== 0 && (currentRow?.id as number) > 0) {
    state.roleId = currentRow.id
    state.roleName = currentRow.name
    onGetRoleUserList()
  }
}

const onUserRowClick = (row: RoleGetRoleUserListOutput) => {
  userTableRef.value!.toggleRowSelection(row, undefined)
}

const onAddUser = () => {
  if (!((state.roleId as number) > 0)) {
    proxy.$modal.msgWarning('请选择角色')
    return
  }
  userSelectRef.value.open({ roleId: state.roleId })
}

const onRemoveUser = () => {
  if (!((state.roleId as number) > 0)) {
    proxy.$modal.msgWarning('请选择角色')
    return
  }

  const selectionRows = userTableRef.value!.getSelectionRows() as UserGetPageOutput[]

  if (!((selectionRows.length as number) > 0)) {
    proxy.$modal.msgWarning('请选择员工')
    return
  }

  proxy.$modal
    .confirm(`确定要移除吗?`)
    .then(async () => {
      const userIds = selectionRows?.map((a) => a.id)
      const input = { roleId: state.roleId, userIds } as RoleAddRoleUserListInput
      await new RoleApi().removeRoleUser(input, { loading: true })
      onGetRoleUserList()
    })
    .catch(() => {})
}

const onSureUser = async (users: UserGetPageOutput[]) => {
  if (!(users?.length > 0)) {
    userSelectRef.value.close()
    return
  }

  state.sureLoading = true
  const userIds = users?.map((a) => a.id)
  const input = { roleId: state.roleId, userIds } as RoleAddRoleUserListInput
  await new RoleApi().addRoleUser(input, { showSuccessMessage: true }).catch(() => {
    state.sureLoading = false
  })
  state.sureLoading = false
  userSelectRef.value.close()
  onGetRoleUserList()
}

const onSetRoleMenu = (role: RoleGetListOutput) => {
  if (!((role?.id as number) > 0)) {
    proxy.$modal.msgWarning('请选择角色')
    return
  }
  setRoleMenuRef.value.open(role)
}

const onSetRoleDataScope = (role: RoleGetListOutput) => {
  if (!((role?.id as number) > 0)) {
    proxy.$modal.msgWarning('请选择角色')
    return
  }
  setRoleDataScopeRef.value.open(role)
}
</script>

<style scoped lang="scss"></style>
