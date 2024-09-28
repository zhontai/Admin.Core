<template>
  <div class="my-layout">
    <el-card class="mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form :inline="true" @submit.stop.prevent>
        <el-form-item label="企业名称">
          <el-input v-model="state.filter.name" placeholder="企业名称" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          <el-button v-auth="'api:admin:tenant:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table v-loading="state.loading" :data="state.tenantListData" row-key="id" height="'100%'" style="width: 100%; height: 100%">
        <el-table-column prop="name" label="企业名称" min-width="120" show-overflow-tooltip />
        <el-table-column prop="code" label="企业编码" width="120" show-overflow-tooltip />
        <el-table-column prop="pkgNames" label="套餐" width="140" show-overflow-tooltip>
          <template #default="{ row }">
            {{ row.pkgNames ? row.pkgNames.join(',') : '' }}
          </template>
        </el-table-column>
        <el-table-column prop="realName" label="姓名" width="120" show-overflow-tooltip />
        <el-table-column prop="phone" label="手机号" width="120" show-overflow-tooltip />
        <!-- <el-table-column prop="email" label="邮箱" min-width="180" show-overflow-tooltip /> -->
        <el-table-column label="状态" width="80" align="center" fixed="right">
          <template #default="{ row }">
            <el-switch
              v-if="auth('api:admin:tenant:set-enable')"
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
            <el-button v-auth="'api:admin:tenant:update'" icon="ele-EditPen" size="small" text type="primary" @click="onEdit(row)">编辑</el-button>
            <my-dropdown-more v-auths="['api:admin:tenant:delete']">
              <template #dropdown>
                <el-dropdown-menu>
                  <el-dropdown-item v-if="auth('api:admin:tenant:delete')" @click="onDelete(row)">删除租户</el-dropdown-item>
                  <el-dropdown-item v-if="auth('api:admin:tenant:one-click-login')" @click="onOneClickLogin(row)">一键登录</el-dropdown-item>
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

    <tenant-form ref="tenantFormRef" :title="state.tenantFormTitle"></tenant-form>
  </div>
</template>

<script lang="ts" setup name="admin/tenant">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, defineAsyncComponent } from 'vue'
import { TenantListOutput, PageInputTenantGetPageDto } from '/@/api/admin/data-contracts'
import { TenantApi } from '/@/api/admin/Tenant'
import { UserApi } from '/@/api/admin/User'
import eventBus from '/@/utils/mitt'
import { auth } from '/@/utils/authFunction'
import { Session } from '/@/utils/storage'
import { useUserInfo } from '/@/stores/userInfo'

const storesUseUserInfo = useUserInfo()

// 引入组件
const TenantForm = defineAsyncComponent(() => import('./components/tenant-form.vue'))
const MyDropdownMore = defineAsyncComponent(() => import('/@/components/my-dropdown-more/index.vue'))

const { proxy } = getCurrentInstance() as any

const tenantFormRef = ref()

const state = reactive({
  loading: false,
  tenantFormTitle: '',
  total: 0,
  filter: {
    name: '',
  },
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInputTenantGetPageDto,
  tenantListData: [] as Array<TenantListOutput>,
})

onMounted(() => {
  onQuery()
  eventBus.off('refreshTenant')
  eventBus.on('refreshTenant', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshTenant')
})

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

const onAdd = () => {
  state.tenantFormTitle = '新增租户'
  tenantFormRef.value.open()
}

const onEdit = (row: TenantListOutput) => {
  state.tenantFormTitle = '编辑租户'
  tenantFormRef.value.open(row)
}

const onDelete = (row: TenantListOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${row.name}】?`)
    .then(async () => {
      await new TenantApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

const onSetEnable = (row: TenantListOutput & { loading: boolean }) => {
  return new Promise((resolve, reject) => {
    proxy.$modal
      .confirm(`确定要${row.enabled ? '禁用' : '启用'}【${row.name}】?`)
      .then(async () => {
        row.loading = true
        const res = await new TenantApi()
          .setEnable({ tenantId: row.id, enabled: !row.enabled }, { showSuccessMessage: true })
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
const onOneClickLogin = (row: TenantListOutput) => {
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
</script>
