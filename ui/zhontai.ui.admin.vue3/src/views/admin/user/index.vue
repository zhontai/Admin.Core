<template>
  <my-layout>
    <pane size="20" min-size="20" max-size="35">
      <div class="my-flex-column w100 h100">
        <org-menu @node-click="onOrgNodeClick" select-first-node></org-menu>
      </div>
    </pane>
    <pane size="80">
      <div class="my-flex-column w100 h100">
        <el-card class="mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
          <el-form @submit.stop.prevent style="max-width: 640px">
            <el-row :gutter="35">
              <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
                <el-form-item>
                  <my-select-input v-model="state.pageInput.dynamicFilter" :filters="state.filters" @search="onQuery" />
                </el-form-item>
              </el-col>
              <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
                <el-form-item>
                  <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
                  <el-button type="primary" icon="ele-Search" @click="onFilter"> 高级查询 </el-button>
                  <el-button v-auth="'api:admin:user:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
                </el-form-item>
              </el-col>
            </el-row>
          </el-form>
        </el-card>

        <el-card class="my-fill mt8" shadow="never">
          <el-table v-loading="state.loading" :data="state.userListData" row-key="id" style="width: 100%">
            <el-table-column prop="userName" label="账号" width="120" show-overflow-tooltip />
            <el-table-column prop="name" label="姓名" width="120" show-overflow-tooltip>
              <template #default="{ row }"> {{ row.name }} <el-tag v-if="row.isManager" type="success">主管</el-tag> </template>
            </el-table-column>
            <el-table-column prop="mobile" label="手机号" width="120" show-overflow-tooltip />
            <el-table-column prop="email" label="邮箱" min-width="180" show-overflow-tooltip />
            <el-table-column prop="roleNames" label="角色" min-width="140" show-overflow-tooltip>
              <template #default="{ row }">
                {{ row.roleNames ? row.roleNames.join(',') : '' }}
              </template>
            </el-table-column>
            <el-table-column label="状态" width="80" align="center" fixed="right">
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
                <el-button v-auth="'api:admin:user:update'" icon="ele-EditPen" size="small" text type="primary" @click="onEdit(row)">编辑</el-button>
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
                    </el-dropdown-menu>
                  </template>
                </my-dropdown-more>
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

        <user-form ref="userFormRef" :title="state.userFormTitle"></user-form>
        <user-reset-pwd ref="userRestPwdRef" title="提示"></user-reset-pwd>
        <MyFilterDialog ref="myFilterDialogRef" :fields="state.filters" @sure="onFilterSure"></MyFilterDialog>
      </div>
    </pane>
  </my-layout>
</template>

<script lang="ts" setup name="admin/user">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, defineAsyncComponent } from 'vue'
import { UserGetPageOutput, PageInputUserGetPageDto, OrgListOutput, UserSetManagerInput } from '/@/api/admin/data-contracts'
import { UserApi } from '/@/api/admin/User'
import eventBus from '/@/utils/mitt'
import { auth } from '/@/utils/authFunction'
import { Pane } from 'splitpanes'
import { useUserInfo } from '/@/stores/userInfo'
import { Session } from '/@/utils/storage'

// 引入组件
const UserForm = defineAsyncComponent(() => import('./components/user-form.vue'))
const UserResetPwd = defineAsyncComponent(() => import('./components/user-reset-pwd.vue'))
const OrgMenu = defineAsyncComponent(() => import('/@/views/admin/org/components/org-menu.vue'))
const MyDropdownMore = defineAsyncComponent(() => import('/@/components/my-dropdown-more/index.vue'))
const MySelectInput = defineAsyncComponent(() => import('/@/components/my-select-input/index.vue'))
const MyLayout = defineAsyncComponent(() => import('/@/components/my-layout/index.vue'))
const MyFilterDialog = defineAsyncComponent(() => import('/@/components/my-filter/dialog.vue'))

const { proxy } = getCurrentInstance() as any

const userFormRef = ref()
const userRestPwdRef = ref()
const myFilterDialogRef = ref()

const storesUseUserInfo = useUserInfo()

const state = reactive({
  loading: false,
  userFormTitle: '',
  total: 0,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
    filter: {
      orgId: null,
    },
    dynamicFilter: {},
  } as PageInputUserGetPageDto,
  userListData: [] as Array<UserGetPageOutput>,
  filters: [
    {
      field: 'name',
      operator: 'Contains',
      description: '姓名',
      componentName: 'el-input',
      defaultSelect: true,
    },
    {
      field: 'mobile',
      operator: 'Contains',
      description: '手机号',
      componentName: 'el-input',
    },
    {
      field: 'email',
      operator: 'Contains',
      description: '邮箱',
      componentName: 'el-input',
    },
    {
      field: 'userName',
      operator: 'Contains',
      description: '用户名',
      componentName: 'el-input',
    },
    {
      field: 'createdTime',
      operator: 'daterange',
      description: '创建时间',
      componentName: 'el-date-picker',
      type: 'date',
      config: {
        type: 'daterange',
        format: 'YYYY-MM-DD',
        valueFormat: 'YYYY-MM-DD',
      },
    },
  ] as Array<DynamicFilterInfo>,
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

//高级查询
const onFilter = () => {
  myFilterDialogRef.value.open()
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

//修改
const onEdit = (row: UserGetPageOutput) => {
  state.userFormTitle = '编辑用户'
  userFormRef.value.open(row)
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
    .confirmDelete(`确定要一键登录【${row.name}】?`)
    .then(async () => {
      const res = await new UserApi().oneClickLogin({ userName: row.userName || '' }, { loading: true })
      if (res?.success) {
        proxy.$modal.msgSuccess('一键登录成功')
        window.requests = []
        Session.remove('tagsViewList')
        storesUseUserInfo.setToken(res.data.token)
        window.location.href = '/'
      }
    })
    .catch(() => {})
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
</script>

<style scoped lang="scss"></style>
