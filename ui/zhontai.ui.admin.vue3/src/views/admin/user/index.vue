<template>
  <MySplitPanes>
    <pane size="20" min-size="20" max-size="35">
      <div class="my-flex-column w100 h100">
        <org-menu @node-click="onOrgNodeClick" select-first-node></org-menu>
      </div>
    </pane>
    <pane size="80">
      <div class="my-flex-column w100 h100">
        <el-card v-show="state.showQuery" class="my-search-box mt8" shadow="never">
          <my-search
            :search-items="state.searchItems"
            :display-count="2"
            :col-config="{
              lg: 8,
            }"
            @search="onSearch"
          ></my-search>
        </el-card>

        <el-card class="my-fill mt8" shadow="never">
          <div class="my-tools-box mb8 my-flex my-flex-between">
            <div>
              <el-button v-auth="'api:admin:user:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
              <el-button
                v-auth="'api:admin:user:batch-set-org'"
                type="primary"
                :disabled="!isRowSelect"
                :loading="state.loadingBatchSetOrg"
                @click="onBatchSetOrg"
                >部门转移</el-button
              >
            </div>
            <div>
              <el-tooltip effect="dark" content="高级查询" placement="top">
                <el-button icon="ele-Filter" circle @click="onFilter"> </el-button>
              </el-tooltip>
              <el-tooltip effect="dark" content="回收站" placement="top">
                <el-button v-auth="'api:admin:user:restore'" circle @click="onRecycle">
                  <template #icon>
                    <el-icon>
                      <my-icon name="recycle" color="var(--color)"></my-icon>
                    </el-icon>
                  </template>
                </el-button>
              </el-tooltip>
              <el-tooltip effect="dark" :content="state.showQuery ? '隐藏查询' : '显示查询'" placement="top">
                <el-button :icon="state.showQuery ? 'ele-ArrowUp' : 'ele-ArrowDown'" circle @click="state.showQuery = !state.showQuery" />
              </el-tooltip>
            </div>
          </div>

          <el-table ref="tableRef" v-loading="state.loading" :data="state.userListData" row-key="id" border>
            <el-table-column type="selection" />
            <el-table-column prop="userName" label="账号" min-width="180" show-overflow-tooltip>
              <template #default="{ row }">
                <el-badge :type="row.online ? 'success' : 'info'" is-dot :offset="[0, 12]"></el-badge>
                {{ row.userName }}
              </template>
            </el-table-column>
            <el-table-column prop="name" label="姓名" width="130" show-overflow-tooltip>
              <template #default="{ row }">
                <div class="my-flex my-flex-items-center">
                  {{ row.name }}
                  <el-icon v-if="row.sex === 1 || row.sex === 2" class="ml4">
                    <ele-Male v-if="row.sex === 1" color="#409EFF" />
                    <ele-Female v-else-if="row.sex === 2" color="#F34D37" />
                  </el-icon>
                  <el-tag v-if="row.isManager" type="success" class="ml4">主管</el-tag>
                </div>
              </template>
            </el-table-column>
            <el-table-column prop="mobile" label="手机号" width="120" show-overflow-tooltip />
            <el-table-column prop="orgPaths" label="部门" min-width="200" show-overflow-tooltip />
            <el-table-column prop="orgPath" label="主属部门" min-width="180" show-overflow-tooltip />
            <el-table-column prop="roleNames" label="角色" min-width="180" show-overflow-tooltip />
            <el-table-column prop="email" label="邮箱" min-width="180" show-overflow-tooltip />
            <el-table-column label="状态" width="88" align="center" fixed="right">
              <template #default="{ row }">
                <el-switch
                  v-if="auth('api:admin:user:set-enable')"
                  v-model="row.enabled"
                  :loading="row.loading"
                  :active-value="true"
                  :inactive-value="false"
                  inline-prompt
                  active-text="启用"
                  inactive-text="禁用"
                  :before-change="() => onSetEnable(row)"
                />
                <template v-else>
                  <el-tag type="success" v-if="row.enabled">启用</el-tag>
                  <el-tag type="danger" v-else>禁用</el-tag>
                </template>
              </template>
            </el-table-column>
            <el-table-column label="操作" width="140" header-align="center" align="center" fixed="right">
              <template #default="{ row }">
                <el-button v-auth="'api:admin:user:update'" icon="ele-EditPen" text type="primary" @click="onEdit(row)">编辑</el-button>
                <my-dropdown-more
                  v-auths="['api:admin:user:set-manager', 'api:admin:user:reset-password', 'api:admin:user:delete', 'api:admin:user:one-click-login']"
                >
                  <template #dropdown>
                    <el-dropdown-menu>
                      <el-dropdown-item v-if="auth('api:admin:user:set-manager')" @click="onSetManager(row)"
                        >{{ row.isManager ? '取消' : '设置' }}主管</el-dropdown-item
                      >
                      <el-dropdown-item v-if="auth('api:admin:user:reset-password')" @click="onResetPwd(row)">重置密码</el-dropdown-item>
                      <el-dropdown-item v-if="auth('api:admin:user:delete')" @click="onDelete(row)">删除用户</el-dropdown-item>
                      <el-dropdown-item v-if="auth('api:admin:user:one-click-login')" @click="onOneClickLogin(row)">一键登录</el-dropdown-item>
                      <el-dropdown-item v-if="auth('api:admin:user:force-offline')" @click="onForceOffline(row)">强制下线</el-dropdown-item>
                    </el-dropdown-menu>
                  </template>
                </my-dropdown-more>
              </template>
            </el-table-column>
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
              layout="total, sizes, prev, pager, next, jumper"
            />
          </div>
        </el-card>

        <user-form ref="userFormRef" :title="state.userFormTitle"></user-form>
        <UserRecycleDialog ref="userRecycleDialogRef" multiple></UserRecycleDialog>
        <user-update-form ref="userUpdateFormRef" :title="state.userFormTitle"></user-update-form>
        <user-set-org ref="userSetOrgRef" v-model:user-ids="selectionIds"></user-set-org>
        <user-reset-pwd ref="userRestPwdRef" title="提示"></user-reset-pwd>
        <MyHighSearchDialog ref="myHighSearchDialogRef" :fields="state.searchItems" @sure="onFilterSure"></MyHighSearchDialog>
      </div>
    </pane>
  </MySplitPanes>
</template>

<script lang="ts" setup name="admin/user">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, defineAsyncComponent, computed, markRaw } from 'vue'
import { UserGetPageOutput, PageInputUserGetPageInput, OrgGetListOutput, UserSetManagerInput } from '/@/api/admin/data-contracts'
import { UserApi } from '/@/api/admin/User'
import eventBus from '/@/utils/mitt'
import { auth } from '/@/utils/authFunction'
import { Pane } from 'splitpanes'
import { useUserInfo } from '/@/stores/userInfo'
import { Session } from '/@/utils/storage'
import { ElTable } from 'element-plus'
import { Sex } from '/@/api/admin/enum-contracts'
import { toOptionsByValue } from '/@/utils/enum'

// 引入组件
const UserForm = defineAsyncComponent(() => import('./components/user-form.vue'))
const UserRecycleDialog = defineAsyncComponent(() => import('./components/user-recycle-dialog.vue'))
const UserUpdateForm = defineAsyncComponent(() => import('./components/user-update-form.vue'))
const UserSetOrg = defineAsyncComponent(() => import('./components/user-set-org.vue'))
const UserResetPwd = defineAsyncComponent(() => import('./components/user-reset-pwd.vue'))
const OrgMenu = defineAsyncComponent(() => import('/@/views/admin/org/components/org-menu.vue'))
const MyDropdownMore = defineAsyncComponent(() => import('/@/components/my-dropdown-more/index.vue'))
const MySplitPanes = defineAsyncComponent(() => import('/@/components/my-layout/split-panes.vue'))
const MyHighSearchDialog = defineAsyncComponent(() => import('/@/components/my-high-search/dialog.vue'))

const { proxy } = getCurrentInstance() as any

const tableRef = ref<InstanceType<typeof ElTable>>()
const userFormRef = ref()
const userRecycleDialogRef = ref()
const userUpdateFormRef = ref()
const userSetOrgRef = ref()
const userRestPwdRef = ref()
const myHighSearchDialogRef = ref()

const storesUseUserInfo = useUserInfo()

const state = reactive({
  loading: false,
  loadingBatchSetOrg: false,
  showQuery: true,
  userFormTitle: '',
  total: 0,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
    filter: {
      orgId: null,
    },
  } as PageInputUserGetPageInput,
  userListData: [] as Array<UserGetPageOutput>,
  searchItems: [
    {
      label: '姓名',
      field: 'name',
      operator: 'Contains',
      componentName: 'el-input',
      attrs: {
        placeholder: '请输入姓名',
      },
    },
    {
      label: '状态',
      field: 'enabled',
      operator: 'Equal',
      componentName: 'my-select',
      type: 'select',
      attrs: {
        placeholder: '请选择',
        options: [
          {
            label: '启用',
            value: 1,
          },
          {
            label: '禁用',
            value: 0,
          },
        ],
      },
    },
    {
      label: '手机号',
      field: 'mobile',
      operator: 'Contains',
      componentName: 'el-input',
      attrs: {
        placeholder: '请输入手机号',
      },
    },
    {
      label: '邮箱',
      field: 'email',
      operator: 'Contains',
      componentName: 'el-input',
      attrs: {
        placeholder: '请输入邮箱',
      },
    },
    {
      label: '性别',
      field: 'staff.sex',
      operator: 'Equal',
      componentName: 'my-select',
      type: 'select',
      attrs: {
        placeholder: '请选择',
        options: toOptionsByValue(Sex),
      },
    },
    {
      label: '创建时间',
      field: 'createdTime',
      operator: 'daterange',
      componentName: 'el-date-picker',
      type: 'date',
      attrs: {
        type: 'daterange',
        format: 'YYYY-MM-DD',
        valueFormat: 'YYYY-MM-DD',
        unlinkPanels: true,
        startPlaceholder: '开始时间',
        endPlaceholder: '结束时间',
        disabledDate: (time: any) => {
          return time.getTime() > Date.now()
        },
      },
    },
    {
      label: '账号',
      field: 'userName',
      operator: 'Contains',
      componentName: 'el-input',
      attrs: {
        placeholder: '请输入账号',
      },
    },
  ],
})

const selectionRows = computed(() => {
  return tableRef.value?.getSelectionRows()
})

const rowSelectCount = computed(() => {
  return selectionRows.value?.length || 0
})

const isRowSelect = computed(() => {
  return rowSelectCount.value > 0
})

const selectionIds = computed(() => {
  return selectionRows.value?.map((a: any) => a.id)
})

onMounted(() => {
  eventBus.off('refreshUser')
  eventBus.on('refreshUser', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshUser')
})

//查询分页
const onQuery = async () => {
  state.loading = true
  const res = await new UserApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.userListData = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0
  state.loading = false
}

const onSearch = (filter: any, dynamicFilter: any) => {
  state.pageInput.dynamicFilter = dynamicFilter
  onQuery()
}

//高级查询
const onFilter = () => {
  myHighSearchDialogRef.value.open()
}
const onFilterSure = (dynamicFilter: any) => {
  state.pageInput.dynamicFilter = dynamicFilter
  onQuery()
}

//新增
const onAdd = () => {
  state.userFormTitle = '新增用户'
  userFormRef.value.open({
    orgIds: state.pageInput.filter?.orgId && state.pageInput.filter.orgId > 0 ? [state.pageInput.filter?.orgId] : [],
    orgId: state.pageInput.filter?.orgId,
  })
}

const onRecycle = () => {
  userRecycleDialogRef.value.open()
}

//修改
const onEdit = (row: UserGetPageOutput) => {
  state.userFormTitle = '编辑用户'
  userUpdateFormRef.value.open(row)
}

//删除
const onDelete = (row: UserGetPageOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${row.name}】?`)
    .then(async () => {
      await new UserApi().softDelete({ id: row.id }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

//重置密码
const onResetPwd = (row: UserGetPageOutput) => {
  userRestPwdRef.value.open(row)
}

//设置或取消主管
const onSetManager = (row: UserGetPageOutput) => {
  if (!((state.pageInput.filter?.orgId as number) > 0)) {
    proxy.$modal.msgWarning('请选择部门')
    return
  }

  const title = row.isManager ? `确定要取消【${row.name}】的主管?` : `确定要设置【${row.name}】为主管?`
  proxy.$modal
    .confirm(title)
    .then(async () => {
      const input = { userId: row.id, orgId: state.pageInput.filter?.orgId, isManager: !row.isManager } as UserSetManagerInput
      await new UserApi().setManager(input, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

//启用或禁用
const onSetEnable = (row: UserGetPageOutput & { loading: boolean }) => {
  return new Promise((resolve, reject) => {
    proxy.$modal
      .confirm(`确定要${row.enabled ? '禁用' : '启用'}【${row.name}】?`)
      .then(async () => {
        row.loading = true
        const res = await new UserApi()
          .setEnable({ userId: row.id, enabled: !row.enabled }, { showSuccessMessage: true })
          .catch(() => {
            reject(new Error('Error'))
          })
          .finally(() => {
            row.loading = false
          })
        if (res && res.success) {
          resolve(true)
        } else {
          reject(new Error('Cancel'))
        }
      })
      .catch(() => {
        reject(new Error('Cancel'))
      })
  })
}

//一键登录
const onOneClickLogin = (row: UserGetPageOutput) => {
  proxy.$modal
    .confirm(`确定要一键登录【${row.name}】?`)
    .then(async () => {
      const res = await new UserApi().oneClickLogin({ userName: row.userName || '' }, { loading: true })
      if (res?.success) {
        proxy.$modal.msgSuccess('一键登录成功')
        window.requests = []
        Session.remove('tagsViewList')
        storesUseUserInfo.setTokenInfo(res.data)
        window.location.href = '/'
      }
    })
    .catch(() => {})
}

//强制下线
const onForceOffline = (row: UserGetPageOutput) => {
  proxy.$modal
    .confirm(`确定要强制下线【${row.name}】?`)
    .then(async () => {
      const res = await new UserApi().forceOffline({ id: row.id }, { loading: true })
      if (res?.success) {
        proxy.$modal.msgSuccess('强制下线成功')
        onQuery()
      }
    })
    .catch(() => {})
}

//部门转移
const onBatchSetOrg = () => {
  userSetOrgRef.value.open()
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

const onOrgNodeClick = (node: OrgGetListOutput | null) => {
  if (state.pageInput.filter) {
    state.pageInput.filter.orgId = node?.id
  }
  onQuery()
}
</script>

<style scoped lang="scss"></style>
