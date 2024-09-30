<template>
  <div class="my-flex-column w100 h100">
    <el-card class="mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form :model="state.filterModel" :inline="true" @submit.stop.prevent>
        <el-form-item prop="name">
          <el-input v-model="state.filterModel.name" placeholder="名称或编码" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          <el-button v-auth="'api:admin:dict:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
          <el-button v-auth="'api:admin:dict:import-data'" icon="ele-Download" type="primary" @click="onImport"> 导入 </el-button>
          <el-button v-auth="'api:admin:dict:export-data'" icon="ele-Upload" type="primary" :loading="state.export.loading" @click="onExport">
            导出
          </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table
        v-loading="state.loading"
        :data="state.dictListData"
        row-key="id"
        style="width: 100%"
        :default-sort="state.defalutSort"
        @sort-change="onSortChange"
      >
        <el-table-column prop="name" label="名称" min-width="120" show-overflow-tooltip />
        <el-table-column prop="code" label="编码" min-width="120" show-overflow-tooltip />
        <el-table-column prop="value" label="值" width="80" sortable="custom" show-overflow-tooltip />
        <el-table-column prop="sort" label="排序" width="80" align="center" sortable="custom" show-overflow-tooltip />
        <el-table-column label="状态" width="80" align="center" show-overflow-tooltip>
          <template #default="{ row }">
            <el-tag type="success" v-if="row.enabled">启用</el-tag>
            <el-tag type="danger" v-else>禁用</el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="140" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <el-button v-auth="'api:admin:dict:update'" icon="ele-EditPen" size="small" text type="primary" @click="onEdit(row)">编辑</el-button>
            <el-button v-auth="'api:admin:dict:delete'" icon="ele-Delete" size="small" text type="danger" @click="onDelete(row)">删除</el-button>
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

    <dict-form ref="dictFormRef" :title="state.dictFormTitle"></dict-form>

    <MyImport ref="dictImportRef" :title="state.import.title" v-model="state.import"></MyImport>
  </div>
</template>

<script lang="ts" setup name="admin/dictData">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, defineAsyncComponent } from 'vue'
import { DictGetPageOutput, PageInputDictGetPageInput, DictTypeGetPageOutput, SortInput } from '/@/api/admin/data-contracts'
import { DictApi } from '/@/api/admin/Dict'
import eventBus from '/@/utils/mitt'
import dayjs from 'dayjs'

// 引入组件
const DictForm = defineAsyncComponent(() => import('./components/dict-form.vue'))
const MyImport = defineAsyncComponent(() => import('/@/components/my-import/index.vue'))

const { proxy } = getCurrentInstance() as any

const dictFormRef = ref()
const dictImportRef = ref()

const defalutSort = { prop: 'sort', order: 'ascending' }

const getSortList = (data: { prop: string; order: any }) => {
  return [
    {
      propName: data.prop,
      order: data.order === 'ascending' ? 0 : data.order === 'descending' ? 1 : undefined,
    },
  ] as [SortInput]
}

const state = reactive({
  loading: false,
  dictFormTitle: '',
  filterModel: {
    name: '',
    dictTypeId: 0,
  },
  defalutSort: defalutSort,
  total: 0,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
    sortList: getSortList(defalutSort),
  } as PageInputDictGetPageInput,
  dictListData: [] as Array<DictGetPageOutput>,
  dictTypeName: '',
  import: {
    title: '',
    action: import.meta.env.VITE_API_URL + '/api/admin/dict/import-data',
    duplicateAction: 1,
    uniqueRules: ['字典名称', '字典编码', '字典值'],
    requiredColumns: ['字典类型', '字典名称'],
  },
  export: {
    loading: false,
  },
})

onMounted(async () => {
  eventBus.off('refreshDict')
  eventBus.on('refreshDict', () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshDict')
})

const onSortChange = (data: { column: any; prop: string; order: any }) => {
  state.pageInput.sortList = getSortList(data)
  onQuery()
}

const onQuery = async () => {
  state.loading = true
  state.pageInput.filter = state.filterModel
  const res = await new DictApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })
  state.dictListData = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0
  state.loading = false
}

const onAdd = () => {
  if (!(state.filterModel.dictTypeId > 0)) {
    proxy.$modal.msgWarning('请选择字典类型')
    return
  }
  state.dictFormTitle = `新增【${state.dictTypeName}】字典数据`
  dictFormRef.value.open({ dictTypeId: state.filterModel.dictTypeId })
}

const onEdit = (row: DictGetPageOutput) => {
  state.dictFormTitle = `编辑【${state.dictTypeName}】字典数据`
  dictFormRef.value.open(row)
}

const onDelete = (row: DictGetPageOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${row.name}】?`)
    .then(async () => {
      await new DictApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

const onImport = () => {
  state.import.title = `导入【${state.dictTypeName}】字典数据`
  dictImportRef.value.open()
}

const onExport = async () => {
  state.export.loading = true

  await new DictApi()
    .exportData(
      {
        dynamicFilter: {
          filters: [{ field: 'dictTypeId', operator: 6, value: state.pageInput.filter?.dictTypeId }],
        },
        sortList: state.pageInput.sortList,
      },
      { format: 'blob', returnResponse: true }
    )
    .then((res: any) => {
      const contentDisposition = res.headers['content-disposition']
      const matchs = /filename="?([^;"]+)/i.exec(contentDisposition)
      let fileName = ''
      if (matchs && matchs.length > 1) {
        fileName = decodeURIComponent(matchs[1])
      } else {
        fileName = `数据字典列表${dayjs().format('YYYYMMDDHHmmss')}.xlsx`
      }
      const a = document.createElement('a')
      a.download = fileName
      a.href = URL.createObjectURL(res.data as Blob)
      a.click()
      URL.revokeObjectURL(a.href)
    })
    .finally(() => {
      state.export.loading = false
    })
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

const refresh = (data: DictTypeGetPageOutput) => {
  if ((data?.id as number) > 0) {
    state.filterModel.dictTypeId = data.id as number
    state.dictTypeName = data.name as string
    onQuery()
  }
}

defineExpose({
  refresh,
})
</script>

<style scoped lang="scss"></style>
