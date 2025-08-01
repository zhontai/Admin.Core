<template>
  <div class="my-flex-column w100 h100">
    <el-card class="my-query-box mt8" shadow="never">
      <el-form :model="state.filterModel" :inline="true" @submit.stop.prevent>
        <el-form-item prop="name">
          <el-input v-model="state.filterModel.name" placeholder="字典分类名称或编码" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          <el-button v-auth="'api:admin:dict:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table
        ref="tableRef"
        v-loading="state.loading"
        :data="state.dictTypeListData"
        row-key="id"
        highlight-current-row
        default-expand-all
        :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
        border
        style="width: 100%"
        @current-change="onTableCurrentChange"
      >
        <el-table-column prop="name" label="名称" min-width="140" show-overflow-tooltip>
          <template #default="{ row }">
            <el-badge :type="row.enabled ? 'success' : 'info'" is-dot :offset="[0, 12]"></el-badge>
            {{ row.name }}
          </template>
        </el-table-column>
        <el-table-column prop="code" label="编码" min-width="120" show-overflow-tooltip />
        <el-table-column prop="sort" label="树形" width="70" align="center">
          <template #default="{ row }">
            <el-tag v-if="row.isTree" type="success">是</el-tag>
            <el-tag v-else type="info">否</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="sort" label="排序" width="70" align="center" show-overflow-tooltip />
        <el-table-column label="操作" width="145" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <el-button v-auth="'api:admin:dict:update'" icon="ele-EditPen" text type="primary" @click="onEdit(row)">编辑</el-button>
            <el-button v-auth="'api:admin:dict:delete'" icon="ele-Delete" text type="danger" @click="onDelete(row)">删除</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <dict-type-form ref="dictTypeFormRef" :title="state.dictTypeFormTitle"></dict-type-form>
  </div>
</template>

<script lang="ts" setup name="admin/dictType">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, defineAsyncComponent } from 'vue'
import { DictTypeGetListOutput } from '/@/api/admin/data-contracts'
import { DictTypeApi } from '/@/api/admin/DictType'
import eventBus from '/@/utils/mitt'
import { listToTree, filterList } from '/@/utils/tree'

// 引入组件
const DictTypeForm = defineAsyncComponent(() => import('./components/dict-type-form.vue'))

const { proxy } = getCurrentInstance() as any

const tableRef = ref()
const dictTypeFormRef = ref()

const emits = defineEmits(['change'])

const state = reactive({
  loading: false,
  dictTypeFormTitle: '',
  filterModel: {
    name: '',
  },
  dictTypeListData: [] as Array<DictTypeGetListOutput>,
})

onMounted(() => {
  onQuery()
  eventBus.off('refreshDictType')
  eventBus.on('refreshDictType', () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshDictType')
})

const onQuery = async () => {
  state.loading = true
  const res = await new DictTypeApi().getList({ ...state.filterModel }).catch(() => {
    state.loading = false
  })
  if (res && res.data && res.data.length > 0) {
    state.dictTypeListData = listToTree(
      state.filterModel.name
        ? filterList(res.data, state.filterModel.name, {
            filterWhere: (item: any, filterword: string) => {
              return item.name?.toLocaleLowerCase().indexOf(filterword) > -1
            },
          })
        : res.data
    )
  } else {
    state.dictTypeListData = []
  }
  state.loading = false
}

const onAdd = () => {
  state.dictTypeFormTitle = '新增字典分类'
  dictTypeFormRef.value.open()
}

const onEdit = (row: DictTypeGetListOutput) => {
  state.dictTypeFormTitle = '编辑字典分类'
  dictTypeFormRef.value.open(row)
}

const onDelete = (row: DictTypeGetListOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${row.name}】?`)
    .then(async () => {
      await new DictTypeApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

const onTableCurrentChange = (currentRow: DictTypeGetListOutput) => {
  emits('change', currentRow)
}
</script>

<style scoped lang="scss"></style>
