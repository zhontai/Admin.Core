<template>
  <my-layout>
    <el-card class="my-query-box mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form :inline="true" @submit.stop.prevent>
        <el-form-item label="平台">
          <el-select v-model="state.filter.platform" placeholder="平台" :empty-values="[null]" @change="onQuery" style="width: 100px">
            <el-option v-for="item in state.dictData[DictType.PlatForm.name]" :key="item.code" :label="item.name" :value="item.code" />
          </el-select>
        </el-form-item>
        <el-form-item label="权限名称">
          <el-input v-model="state.filter.label" placeholder="权限名称" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          <el-dropdown
            v-auths="['api:admin:permission:addgroup', 'api:admin:permission:addmenu', 'api:admin:permission:adddot']"
            style="margin-left: 12px"
          >
            <el-button type="primary"
              >新增<el-icon class="el-icon--right"><ele-ArrowDown /></el-icon
            ></el-button>
            <template #dropdown>
              <el-dropdown-menu>
                <el-dropdown-item v-if="auth('api:admin:permission:addgroup')" @click="onAdd({ type: 1 })">新增分组</el-dropdown-item>
                <el-dropdown-item v-if="auth('api:admin:permission:addmenu')" @click="onAdd({ type: 2 })">新增菜单</el-dropdown-item>
                <el-dropdown-item v-if="auth('api:admin:permission:adddot')" @click="onAdd({ type: 3 })">新增权限点</el-dropdown-item>
              </el-dropdown-menu>
            </template>
          </el-dropdown>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table
        :data="state.permissionTreeData"
        style="width: 100%"
        v-loading="state.loading"
        row-key="id"
        :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
        :expand-row-keys="state.expandRowKeys"
        border
      >
        <el-table-column prop="label" label="权限名称" width="240" show-overflow-tooltip>
          <template #default="{ row }">
            <SvgIcon :name="row.icon" style="vertical-align: -2px" />
            {{ row.label }}
          </template>
        </el-table-column>
        <el-table-column prop="type" label="类型" width="82" show-overflow-tooltip>
          <template #default="{ row }">
            {{ row.type === 1 ? '分组' : row.type === 2 ? '菜单' : row.type === 3 ? '权限点' : '' }}
          </template>
        </el-table-column>
        <el-table-column prop="path" label="权限地址" min-width="240" show-overflow-tooltip>
          <template #default="{ row }">
            <div v-if="row.type === 1 || row.type === 2">
              {{ row.path ? '路由地址：' + row.path : '' }}
              {{ row.viewPath ? '视图地址：' + row.viewPath : '' }}
              {{ row.redirect ? '重定向地址：' + row.redirect : '' }}
              {{ row.link ? '链接地址：' + row.link : '' }}
            </div>
            <div v-if="row.type === 3">接口地址：{{ row.apiPaths }}</div>
          </template>
        </el-table-column>
        <el-table-column prop="sort" label="排序" width="82" align="center" show-overflow-tooltip />
        <el-table-column label="状态" width="82" align="center">
          <template #default="{ row }">
            <el-tag type="success" v-if="row.enabled">启用</el-tag>
            <el-tag type="danger" v-else>禁用</el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="160" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <el-button
              v-if="
                (row.type === 1 && auth('api:admin:permission:updategroup')) ||
                (row.type === 2 && auth('api:admin:permission:updatemenu')) ||
                (row.type === 3 && auth('api:admin:permission:updatedot'))
              "
              icon="ele-EditPen"
              text
              type="primary"
              @click="onEdit(row)"
              >编辑</el-button
            >
            <my-dropdown-more
              v-auths="[
                'api:admin:permission:delete',
                'api:admin:permission:addgroup',
                'api:admin:permission:addmenu',
                'api:admin:permission:adddot',
              ]"
            >
              <template #dropdown>
                <el-dropdown-menu>
                  <el-dropdown-item v-if="row.type === 1 && auth('api:admin:permission:addgroup')" @click="onAdd({ type: 1, parentId: row.id })">
                    新增分组
                  </el-dropdown-item>
                  <el-dropdown-item v-if="row.type === 1 && auth('api:admin:permission:addmenu')" @click="onAdd({ type: 2, parentId: row.id })">
                    新增菜单
                  </el-dropdown-item>
                  <el-dropdown-item v-if="row.type === 2 && auth('api:admin:permission:adddot')" @click="onAdd({ type: 3, parentId: row.id })">
                    新增权限点
                  </el-dropdown-item>
                  <el-dropdown-item v-if="auth('api:admin:permission:delete')" @click="onDelete(row)">删除</el-dropdown-item>
                  <el-dropdown-item v-if="row.type === 1 && auth('api:admin:permission:addgroup')" @click="onCopy(row)"> 复制 </el-dropdown-item>
                  <el-dropdown-item v-if="row.type === 2 && auth('api:admin:permission:addmenu')" @click="onCopy(row)"> 复制 </el-dropdown-item>
                  <el-dropdown-item v-if="row.type === 3 && auth('api:admin:permission:adddot')" @click="onCopy(row)"> 复制 </el-dropdown-item>
                </el-dropdown-menu>
              </template>
            </my-dropdown-more>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <permission-group-form
      ref="permissionGroupFormRef"
      :title="state.permissionFormTitle"
      :permission-tree-data="state.formPermissionGroupTreeData"
    ></permission-group-form>

    <permission-menu-form
      ref="permissionMenuFormRef"
      :title="state.permissionFormTitle"
      :permission-tree-data="state.formPermissionGroupTreeData"
    ></permission-menu-form>

    <permission-dot-form
      ref="permissionDotFormRef"
      :title="state.permissionFormTitle"
      :permission-tree-data="state.formPermissionMenuTreeData"
    ></permission-dot-form>
  </my-layout>
</template>

<script lang="ts" setup name="admin/permission">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, defineAsyncComponent, markRaw } from 'vue'
import { PermissionGetListOutput, PermissionGetListInput, DictGetListOutput } from '/@/api/admin/data-contracts'
import { PermissionApi } from '/@/api/admin/Permission'
import { listToTree, treeToList, filterTree } from '/@/utils/tree'
import { cloneDeep } from 'lodash-es'
import eventBus from '/@/utils/mitt'
import { auth } from '/@/utils/authFunction'
import { DictApi } from '/@/api/admin/Dict'
import { PlatformType } from '/@/api/admin.extend/enum-contracts'

// 引入组件
const PermissionGroupForm = defineAsyncComponent(() => import('./components/permission-group-form.vue'))
const PermissionMenuForm = defineAsyncComponent(() => import('./components/permission-menu-form.vue'))
const PermissionDotForm = defineAsyncComponent(() => import('./components/permission-dot-form.vue'))
const MyDropdownMore = defineAsyncComponent(() => import('/@/components/my-dropdown-more/index.vue'))

const { proxy } = getCurrentInstance() as any

const DictType = {
  PlatForm: { name: 'platform', desc: '平台' },
}

const permissionGroupFormRef = ref()
const permissionMenuFormRef = ref()
const permissionDotFormRef = ref()

const state = reactive({
  loading: false,
  permissionFormTitle: '',
  filter: {
    platform: PlatformType.Web.name,
    name: '',
  } as PermissionGetListInput,
  permissionTreeData: [] as Array<PermissionGetListOutput>,
  formPermissionGroupTreeData: [] as Array<PermissionGetListOutput>,
  formPermissionMenuTreeData: [] as Array<PermissionGetListOutput>,
  expandRowKeys: [] as string[],
  dictData: {
    [DictType.PlatForm.name]: [] as DictGetListOutput[] | null,
  },
})

onMounted(async () => {
  await getDictList()
  await onQuery()
  state.expandRowKeys = treeToList(cloneDeep(state.permissionTreeData))
    .filter((a: PermissionGetListOutput) => a.opened === true)
    .map((a: PermissionGetListOutput) => a.id + '') as string[]
  eventBus.off('refreshPermission')
  eventBus.on('refreshPermission', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshPermission')
})

const getDictList = async () => {
  const res = await new DictApi().getList([DictType.PlatForm.name]).catch(() => {})
  if (res?.success && res.data) {
    state.dictData = markRaw(res.data)
  }
}

const onQuery = async () => {
  state.loading = true
  const res = await new PermissionApi()
    .getList({
      platform: state.filter.platform,
    })
    .catch(() => {
      state.loading = false
    })
  if (res && res.data && res.data.length > 0) {
    const label = state.filter.label || ''
    state.permissionTreeData = filterTree(listToTree(cloneDeep(res.data)), '', {
      filterWhere: (item: any, keyword: string) => {
        return item.label?.toLocaleLowerCase().indexOf(label) > -1 || item.path?.toLocaleLowerCase().indexOf(label) > -1
      },
    })
    state.formPermissionGroupTreeData = listToTree(cloneDeep(res.data).filter((a: any) => a.type === 1))
    state.formPermissionMenuTreeData = listToTree(cloneDeep(res.data).filter((a: any) => a.type === 1 || a.type === 2))
  } else {
    state.permissionTreeData = []
    state.formPermissionGroupTreeData = []
    state.formPermissionMenuTreeData = []
  }
  state.loading = false
}

const onAdd = (row: PermissionGetListOutput) => {
  switch (row.type) {
    case 1:
      state.permissionFormTitle = '新增分组'
      permissionGroupFormRef.value.open({
        id: 0,
        platform: state.filter.platform,
        enabled: true,
        opened: true,
        icon: 'ele-Memo',
        parentId: row.parentId,
      })
      break
    case 2:
      state.permissionFormTitle = '新增菜单'
      permissionMenuFormRef.value.open({
        id: 0,
        platform: state.filter.platform,
        enabled: true,
        isKeepAlive: true,
        icon: 'ele-Memo',
        parentId: row.parentId,
      })
      break
    case 3:
      state.permissionFormTitle = '新增权限点'
      permissionDotFormRef.value.open({
        id: 0,
        platform: state.filter.platform,
        parentId: row.parentId,
        enabled: true,
      })
      break
  }
}

const onEdit = (row: PermissionGetListOutput) => {
  row.platform = state.filter.platform
  switch (row.type) {
    case 1:
      state.permissionFormTitle = '编辑分组'
      permissionGroupFormRef.value.open(row)
      break
    case 2:
      state.permissionFormTitle = '编辑菜单'
      permissionMenuFormRef.value.open(row)
      break
    case 3:
      state.permissionFormTitle = '编辑权限点'
      permissionDotFormRef.value.open(row)
      break
  }
}

const onCopy = (row: PermissionGetListOutput) => {
  switch (row.type) {
    case 1:
      state.permissionFormTitle = '新增分组'
      permissionGroupFormRef.value.open(row, true)
      break
    case 2:
      state.permissionFormTitle = '新增菜单'
      permissionMenuFormRef.value.open(row, true)
      break
    case 3:
      state.permissionFormTitle = '新增权限点'
      permissionDotFormRef.value.open(row, true)
      break
  }
}

const onDelete = (row: PermissionGetListOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除${row.type === 1 ? '分组' : row.type === 2 ? '菜单' : row.type === 3 ? '权限点' : ''}【${row.label}】?`)
    .then(async () => {
      await new PermissionApi().delete({ id: row.id }, { loading: true })
      onQuery()
    })
    .catch(() => {})
}
</script>

<style scoped lang="scss"></style>
