<template>
  <my-layout>
    <el-card class="my-query-box mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form :inline="true" @submit.stop.prevent>
        <el-form-item :label="t('企业名称')">
          <el-input v-model="state.filter.name" :placeholder="t('企业名称')" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item>
          <el-button auto-insert-space type="primary" icon="ele-Search" @click="onQuery">{{ t('查询') }}</el-button>
          <el-button auto-insert-space v-auth="'api:admin:tenant:add'" type="primary" icon="ele-Plus" @click="onAdd">{{ t('新增') }}</el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table v-loading="state.loading" :data="state.tenantListData" row-key="id" height="'100%'" style="width: 100%; height: 100%" border>
        <el-table-column prop="name" :label="t('企业名称')" min-width="120" show-overflow-tooltip />
        <el-table-column prop="code" :label="t('企业编码')" width="120" show-overflow-tooltip />
        <el-table-column prop="pkgNames" :label="t('套餐')" width="140" show-overflow-tooltip>
          <template #default="{ row }">
            {{ row.pkgNames ? row.pkgNames.join(',') : '' }}
          </template>
        </el-table-column>
        <el-table-column prop="realName" :label="t('姓名')" width="120" show-overflow-tooltip />
        <el-table-column prop="phone" :label="t('手机号')" width="120" show-overflow-tooltip />
        <!-- <el-table-column prop="email" label="邮箱" min-width="180" show-overflow-tooltip /> -->
        <el-table-column :label="t('状态')" width="88" align="center" fixed="right">
          <template #default="{ row }">
            <el-switch
              v-if="auth('api:admin:tenant:set-enable')"
              v-model="row.enabled"
              :loading="row.loading"
              :active-value="true"
              :inactive-value="false"
              inline-prompt
              :active-text="t('启用')"
              :inactive-text="t('禁用')"
              :before-change="() => onSetEnable(row)"
            />
            <template v-else>
              <el-tag type="success" v-if="row.enabled">{{ t('启用') }}</el-tag>
              <el-tag type="danger" v-else>{{ t('禁用') }}</el-tag>
            </template>
          </template>
        </el-table-column>
        <el-table-column :label="t('操作')" width="140" header-align="center" align="center" fixed="right">
          <template #default="{ row }">
            <el-button auto-insert-space v-auth="'api:admin:tenant:update'" icon="ele-EditPen" text type="primary" @click="onEdit(row)">
              {{ t('编辑') }}
            </el-button>
            <my-dropdown-more v-auths="['api:admin:tenant:delete']">
              <template #dropdown>
                <el-dropdown-menu>
                  <el-dropdown-item v-if="auth('api:admin:tenant:delete')" @click="onDelete(row)">{{ t('删除租户') }}</el-dropdown-item>
                  <el-dropdown-item v-if="auth('api:admin:tenant:one-click-login')" @click="onOneClickLogin(row)">
                    {{ t('一键登录') }}
                  </el-dropdown-item>
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

    <tenant-form ref="tenantFormRef" :title="state.tenantFormTitle"></tenant-form>
  </my-layout>
</template>

<script lang="ts" setup name="admin/tenant">
import { TenantGetPageOutput, PageInputTenantGetPageInput } from '/@/api/admin/data-contracts'
import { TenantApi } from '/@/api/admin/Tenant'
import eventBus from '/@/utils/mitt'
import { auth } from '/@/utils/authFunction'
import { Session } from '/@/utils/storage'
import { useUserInfo } from '/@/stores/userInfo'
import { t } from '/@/i18n'

const storesUseUserInfo = useUserInfo()

// 引入组件
const TenantForm = defineAsyncComponent(() => import('./components/tenant-form.vue'))
const MyDropdownMore = defineAsyncComponent(() => import('/@/components/my-dropdown-more/index.vue'))

const { proxy } = getCurrentInstance() as any

const tenantFormRef = useTemplateRef('tenantFormRef')

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
  } as PageInputTenantGetPageInput,
  tenantListData: [] as Array<TenantGetPageOutput>,
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
  state.tenantFormTitle = t('新增租户')
  tenantFormRef.value?.open()
}

const onEdit = (row: TenantGetPageOutput) => {
  state.tenantFormTitle = t('编辑租户')
  tenantFormRef.value?.open(row)
}

const onDelete = (row: TenantGetPageOutput) => {
  proxy.$modal
    .confirmDelete(t('确定要删除【{name}】?', { name: row.name }))
    .then(async () => {
      await new TenantApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

const onSetEnable = (row: TenantGetPageOutput & { loading: boolean }) => {
  return new Promise((resolve, reject) => {
    proxy.$modal
      .confirm(t('确定要{action}【{name}】?', { action: row.enabled ? t('禁用') : t('启用'), name: row.name }))
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
const onOneClickLogin = (row: TenantGetPageOutput) => {
  proxy.$modal
    .confirmDelete(t('确定要一键登录【{name}】?', { name: row.name }))
    .then(async () => {
      const res = await new TenantApi().oneClickLogin({ tenantId: row.id }, { loading: true })
      if (res?.success) {
        proxy.$modal.msgSuccess(t('一键登录成功'))
        window.requests = []
        Session.remove('tagsViewList')
        storesUseUserInfo.setTokenInfo(res.data)
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
