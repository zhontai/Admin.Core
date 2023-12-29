<template>
  <div class="my-layout">
    <el-card class="mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form :inline="true" @submit.stop.prevent>
        <el-form-item label="视图名称">
          <el-input v-model="state.filter.name" placeholder="视图名称" @keyup.enter="onQuery" />
        </el-form-item>
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
        <el-table-column label="操作" width="160" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <el-button v-auth="'api:admin:view:update'" icon="ele-EditPen" size="small" text type="primary" @click="onEdit(row)">编辑</el-button>
            <el-button v-auth="'api:admin:view:delete'" icon="ele-Delete" size="small" text type="danger" @click="onDelete(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <view-form ref="viewFormRef" :title="state.viewFormTitle" :view-tree-data="state.viewTreeData"></view-form>
  </div>
</template>

<script lang="ts" setup name="admin/view">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, defineAsyncComponent } from 'vue'
import { ViewListOutput } from '/@/api/admin/data-contracts'
import { ViewApi } from '/@/api/admin/View'
import { listToTree, filterTree } from '/@/utils/tree'
import { cloneDeep } from 'lodash-es'
import eventBus from '/@/utils/mitt'

// 引入组件
const ViewForm = defineAsyncComponent(() => import('./components/view-form.vue'))

const viewFormRef = ref()
const { proxy } = getCurrentInstance() as any

const state = reactive({
  loading: false,
  viewFormTitle: '',
  filter: {
    name: '',
  },
  viewTreeData: [] as Array<ViewListOutput>,
  formViewTreeData: [] as Array<ViewListOutput>,
})

onMounted(() => {
  onQuery()
  eventBus.off('refreshView')
  eventBus.on('refreshView', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshView')
})

const onQuery = async () => {
  state.loading = true
  const res = await new ViewApi().getList().catch(() => {
    state.loading = false
  })
  if (res && res.data && res.data.length > 0) {
    state.viewTreeData = filterTree(listToTree(cloneDeep(res.data)), state.filter.name, {
      filterWhere: (item: any, keyword: string) => {
        return item.label?.toLocaleLowerCase().indexOf(keyword) > -1 || item.path?.toLocaleLowerCase().indexOf(keyword) > -1
      },
    })
  } else {
    state.viewTreeData = []
    state.formViewTreeData = []
  }
  state.loading = false
}

const onAdd = () => {
  state.viewFormTitle = '新增视图'
  viewFormRef.value.open()
}

const onEdit = (row: ViewListOutput) => {
  state.viewFormTitle = '编辑视图'
  viewFormRef.value.open(row)
}

const onDelete = (row: ViewListOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除视图【${row.label}】?`)
    .then(async () => {
      await new ViewApi().delete({ id: row.id }, { loading: true })
      onQuery()
    })
    .catch(() => {})
}
</script>

<style scoped lang="scss"></style>
