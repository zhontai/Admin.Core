<template>
  <div class="my-flex-column w100 h100">
    <el-card class="my-query-box mt8" shadow="never">
      <el-form :model="state.input" :inline="true" @submit.stop.prevent>
        <el-form-item prop="name">
          <el-input v-model="state.input.name" :placeholder="t('字典名称或编码')" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item>
          <el-button auto-insert-space type="primary" icon="ele-Search" @click="onQuery">{{ t('查询') }}</el-button>
          <el-button auto-insert-space v-auth="'api:admin:dict:add'" type="primary" icon="ele-Plus" @click="onAdd">{{ t('新增') }}</el-button>
          <el-button auto-insert-space v-auth="'api:admin:dict:import-data'" icon="ele-Download" type="primary" @click="onImport">
            {{ t('导入') }}
          </el-button>
          <el-button
            auto-insert-space
            v-auth="'api:admin:dict:export-data'"
            icon="ele-Upload"
            type="primary"
            :loading="state.export.loading"
            @click="onExport"
            >{{ t('导出') }}</el-button
          >
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table
        v-loading="state.loading"
        :data="state.dictListData"
        row-key="id"
        :default-sort="state.defalutSort"
        default-expand-all
        :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
        border
        style="width: 100%"
        @sort-change="onSortChange"
      >
        <el-table-column prop="name" :label="t('名称')" min-width="120" sortable="custom" show-overflow-tooltip>
          <template #default="{ row }">
            <el-badge :type="row.enabled ? 'success' : 'info'" is-dot :offset="[0, 12]"></el-badge>
            {{ row.name }}
          </template>
        </el-table-column>
        <el-table-column prop="code" :label="t('编码')" min-width="120" sortable="custom" show-overflow-tooltip />
        <el-table-column prop="value" :label="t('值')" width="90" sortable="custom" show-overflow-tooltip />
        <el-table-column prop="sort" :label="t('排序')" width="90" align="center" sortable="custom" show-overflow-tooltip />
        <el-table-column :label="t('操作')" width="145" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <el-button auto-insert-space v-auth="'api:admin:dict:update'" icon="ele-EditPen" text type="primary" @click="onEdit(row)">{{
              t('编辑')
            }}</el-button>
            <el-button auto-insert-space v-auth="'api:admin:dict:delete'" icon="ele-Delete" text type="danger" @click="onDelete(row)">{{
              t('删除')
            }}</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <dict-form ref="dictFormRef" :title="state.dictFormTitle"></dict-form>

    <MyImport ref="dictImportRef" :title="state.import.title" v-model="state.import"></MyImport>
  </div>
</template>

<script lang="ts" setup name="admin/dictData">
import { DictGetAllOutput, DictGetAllInput, SortInput, DictTypeGetListOutput } from '/@/api/admin/data-contracts'
import { DictApi } from '/@/api/admin/Dict'
import eventBus from '/@/utils/mitt'
import dayjs from 'dayjs'
import { RequestParams } from '/@/api/admin/http-client'
import { listToTree, filterList } from '/@/utils/tree'
import { t } from '/@/i18n'

// 引入组件
const DictForm = defineAsyncComponent(() => import('./components/dict-form.vue'))
const MyImport = defineAsyncComponent(() => import('/@/components/my-import/index.vue'))

const { proxy } = getCurrentInstance() as any

const dictFormRef = useTemplateRef('dictFormRef')
const dictImportRef = useTemplateRef('dictImportRef')

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
  defalutSort: defalutSort,
  total: 0,
  input: {
    name: '',
    dictTypeId: 0,
    sortList: getSortList(defalutSort),
  } as DictGetAllInput,
  dictListData: [] as Array<DictGetAllOutput>,
  dictType: {} as DictTypeGetListOutput,
  import: {
    title: '',
    action: window.__ENV_CONFIG__.VITE_API_URL + '/api/admin/dict/import-data',
    downloadTemplate: (params: RequestParams) => new DictApi().downloadTemplate(params),
    downloadErrorMark: (query: any, params: RequestParams) => new DictApi().downloadErrorMark(query, params),
    duplicateAction: 1,
    uniqueRules: [t('字典名称'), t('字典编码'), t('字典值')],
    requiredColumns: [t('字典类型'), t('字典名称')],
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
  state.input.sortList = getSortList(data)
  onQuery()
}

const onQuery = async () => {
  state.loading = true
  const res = await new DictApi().getAll(state.input).catch(() => {
    state.loading = false
  })
  if (res && res.data && res.data.length > 0) {
    state.dictListData = listToTree(
      state.input.name
        ? filterList(res.data, state.input.name, {
            filterWhere: (item: any, filterword: string) => {
              return item.name?.toLocaleLowerCase().indexOf(filterword) > -1
            },
          })
        : res.data
    )
  } else {
    state.dictListData = []
  }
  state.loading = false
}

const onAdd = () => {
  if (!((state.input.dictTypeId as number) > 0)) {
    proxy.$modal.msgWarning(t('请选择字典类型'))
    return
  }
  state.dictFormTitle = t('新增【{name}】字典数据', { name: state.dictType.name })
  dictFormRef.value?.open({ dictTypeId: state.input.dictTypeId }, { isTree: state.dictType.isTree })
}

const onEdit = (row: DictGetAllOutput) => {
  state.dictFormTitle = t('编辑【{name}】字典数据', { name: state.dictType.name })
  dictFormRef.value?.open(row, { isTree: state.dictType.isTree })
}

const onDelete = (row: DictGetAllOutput) => {
  proxy.$modal
    .confirmDelete(t('确定要删除【{name}】?', { name: row.name }))
    .then(async () => {
      await new DictApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

const onImport = () => {
  state.import.title = t('导入【{name}】字典数据', { name: state.dictType.name })
  dictImportRef.value?.open()
}

const onExport = async () => {
  state.export.loading = true

  await new DictApi()
    .exportData(
      {
        dynamicFilter: {
          filters: [{ field: 'dictTypeId', operator: 6, value: state.input.dictTypeId }],
        },
        sortList: state.input.sortList,
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
        fileName = `${t('数据字典列表')}${dayjs().format('YYYYMMDDHHmmss')}.xlsx`
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

const refresh = (data: DictTypeGetListOutput) => {
  if ((data?.id as number) > 0) {
    state.input.dictTypeId = data.id as number
    if (state.dictType.id !== data.id) {
      onQuery()
    }
    state.dictType = data
  }
}

defineExpose({
  refresh,
})
</script>

<style scoped lang="scss"></style>
