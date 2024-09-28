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
        style="width: 100%"
        @current-change="onTableCurrentChange"
      >
        <el-table-column prop="name" label="名称" min-width="120" show-overflow-tooltip />
        <el-table-column prop="code" label="编码" min-width="120" show-overflow-tooltip />
        <el-table-column prop="sort" label="排序" width="60" align="center" show-overflow-tooltip />
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

    <dict-type-form ref="dictTypeFormRef" :title="state.dictTypeFormTitle"></dict-type-form>
  </div>
</template>

<script lang="ts" setup name="'admin/dictType'">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, nextTick, defineAsyncComponent } from 'vue'
import { DictTypeGetPageOutput, PageInputDictTypeGetPageDto } from '/@/api/admin/data-contracts'
import { DictTypeApi } from '/@/api/admin/DictType'
import eventBus from '/@/utils/mitt'

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
  total: 0,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInputDictTypeGetPageDto,
  dictTypeListData: [] as Array<DictTypeGetPageOutput>,
  lastCurrentRow: {} as DictTypeGetPageOutput,
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
  state.pageInput.filter = state.filterModel
  const res = await new DictTypeApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })
  state.dictTypeListData = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0
  if (state.dictTypeListData.length > 0) {
    nextTick(() => {
      tableRef.value!.setCurrentRow(state.dictTypeListData[0])
    })
  }
  state.loading = false
}

const onAdd = () => {
  state.dictTypeFormTitle = '新增字典分类'
  dictTypeFormRef.value.open()
}

const onEdit = (row: DictTypeGetPageOutput) => {
  state.dictTypeFormTitle = '编辑字典分类'
  dictTypeFormRef.value.open(row)
}

const onDelete = (row: DictTypeGetPageOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${row.name}】?`)
    .then(async () => {
      await new DictTypeApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
      onQuery()
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

const onTableCurrentChange = (currentRow: DictTypeGetPageOutput) => {
  if (state.lastCurrentRow?.id != currentRow?.id) {
    state.lastCurrentRow = currentRow
    emits('change', currentRow)
  }
}
</script>

<style scoped lang="scss"></style>
