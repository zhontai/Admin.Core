<template>
  <my-layout>
    <el-card class="my-query-box mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form :inline="true" @submit.stop.prevent>
        <el-form-item label="平台">
          <el-select v-model="state.filter.platform" placeholder="平台" @change="onQuery" style="width: 100px">
            <el-option v-for="item in state.dictData[DictType.PlatForm.name]" :key="item.code" :label="item.name" :value="item.code" />
          </el-select>
        </el-form-item>
        <el-form-item label="视图名称">
          <el-input v-model="state.filter.label" placeholder="视图名称" @keyup.enter="onQuery" />
        </el-form-item>
        <!-- <el-form-item label="视图路径">
          <el-input v-model="state.filter.path" placeholder="视图路径" @keyup.enter="onQuery" />
        </el-form-item> -->
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          <el-button v-auth="'api:admin:view:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table
        :data="state.viewTreeData"
        style="width: 100%"
        v-loading="state.loading"
        row-key="id"
        default-expand-all
        :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
        border
      >
        <el-table-column prop="label" label="视图名称" min-width="120" show-overflow-tooltip />
        <el-table-column prop="name" label="视图命名" min-width="120" show-overflow-tooltip />
        <el-table-column prop="path" label="视图地址" min-width="120" show-overflow-tooltip />
        <el-table-column prop="sort" label="排序" width="80" align="center" show-overflow-tooltip />
        <!-- <el-table-column prop="description" label="视图描述" min-width="120" show-overflow-tooltip /> -->
        <el-table-column label="状态" width="80" align="center" show-overflow-tooltip>
          <template #default="{ row }">
            <el-tag type="success" v-if="row.enabled">启用</el-tag>
            <el-tag type="danger" v-else>禁用</el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="190" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <el-button v-auth="'api:admin:view:update'" icon="ele-EditPen" size="small" text type="primary" @click="onEdit(row)">编辑</el-button>
            <el-button v-auth="'api:admin:view:delete'" icon="ele-Delete" size="small" text type="danger" @click="onDelete(row)">删除</el-button>
            <el-button v-auth="'api:admin:view:add'" icon="ele-CopyDocument" size="small" text type="primary" @click="onCopy(row)">复制</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <view-form ref="viewFormRef" :title="state.viewFormTitle" :view-tree-data="state.viewTreeData"></view-form>
  </my-layout>
</template>

<script lang="ts" setup name="admin/view">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, defineAsyncComponent, markRaw } from 'vue'
import { ViewGetListOutput, DictGetListOutput, ViewGetListInput } from '/@/api/admin/data-contracts'
import { ViewApi } from '/@/api/admin/View'
import { listToTree, filterTree } from '/@/utils/tree'
import { cloneDeep } from 'lodash-es'
import eventBus from '/@/utils/mitt'
import { DictApi } from '/@/api/admin/Dict'
import { PlatformType } from '/@/api/admin.extend/enum-contracts'

// 引入组件
const ViewForm = defineAsyncComponent(() => import('./components/view-form.vue'))

const viewFormRef = ref()
const { proxy } = getCurrentInstance() as any

const DictType = {
  PlatForm: { name: 'platform', desc: '平台' },
}

const state = reactive({
  loading: false,
  viewFormTitle: '',
  filter: {
    platform: PlatformType.Web.name,
  } as ViewGetListInput,
  viewTreeData: [] as Array<ViewGetListOutput>,
  formViewTreeData: [] as Array<ViewGetListOutput>,
  dictData: {
    [DictType.PlatForm.name]: [] as DictGetListOutput[] | null,
  },
})

onMounted(async () => {
  await getDictList()
  onQuery()
  eventBus.off('refreshView')
  eventBus.on('refreshView', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshView')
})

const getDictList = async () => {
  const res = await new DictApi().getList([DictType.PlatForm.name]).catch(() => {})
  if (res?.success && res.data) {
    state.dictData = markRaw(res.data)
  }
}

const onQuery = async () => {
  state.loading = true
  const res = await new ViewApi()
    .getList({
      platform: state.filter?.platform,
    })
    .catch(() => {
      state.loading = false
    })
  if (res && res.data && res.data.length > 0) {
    const label = state.filter.label || ''
    state.viewTreeData = markRaw(
      filterTree(listToTree(cloneDeep(res.data)), '', {
        filterWhere: (item: any, keyword: string) => {
          return item.label?.toLocaleLowerCase().indexOf(label) > -1 || item.path?.toLocaleLowerCase().indexOf(label) > -1
        },
      })
    )
  } else {
    state.viewTreeData = []
    state.formViewTreeData = []
  }
  state.loading = false
}

const onAdd = () => {
  state.viewFormTitle = '新增视图'
  viewFormRef.value.open({
    id: 0,
    platform: state.filter.platform,
    enabled: true,
    cache: true,
  })
}

const onEdit = (row: ViewGetListOutput) => {
  state.viewFormTitle = '编辑视图'
  row.platform = state.filter.platform
  viewFormRef.value.open(row)
}

const onDelete = (row: ViewGetListOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除视图【${row.label}】?`)
    .then(async () => {
      await new ViewApi().delete({ id: row.id }, { loading: true })
      onQuery()
    })
    .catch(() => {})
}

const onCopy = (row: ViewGetListOutput) => {
  state.viewFormTitle = '新增视图'
  var view = cloneDeep(row)
  view.id = undefined
  viewFormRef.value.open(view)
}
</script>

<style scoped lang="scss"></style>
