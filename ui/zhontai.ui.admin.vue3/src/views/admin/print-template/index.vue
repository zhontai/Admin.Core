<template>
  <my-layout>
    <el-card class="my-query-box mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form ref="filterFormRef" :model="state.filter" :inline="true" label-width="auto" :label-position="'left'" @submit.stop.prevent>
        <el-form-item label="模板名称" prop="name">
          <el-input v-model="state.filter.name" placeholder="模板名称" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item label="模板编码" prop="code">
          <el-input v-model="state.filter.code" placeholder="模板编码" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          <el-button icon="ele-RefreshLeft" text bg @click="onReset"> 重置 </el-button>
          <el-button v-if="auth('api:admin:print-template:add')" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table v-loading="state.loading" :data="state.dataList" default-expand-all highlight-current-row style="width: 100%" border>
        <el-table-column prop="name" label="模板名称" min-width="120" show-overflow-tooltip />
        <el-table-column prop="code" label="模板编码" min-width="120" show-overflow-tooltip />
        <el-table-column prop="sort" label="排序" width="80" align="center" show-overflow-tooltip />
        <el-table-column label="状态" width="80" align="center" fixed="right">
          <template #default="{ row }">
            <el-switch
              v-if="auth('api:admin:print-template:set-enable')"
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
        <el-table-column label="操作" width="160" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <el-button v-auth="'api:admin:print-template:update'" size="small" text type="primary" @click="onEdit(row)"> 编辑 </el-button>
            <el-button v-auth="'api:admin:print-template:design'" size="small" text type="primary" @click="onDesign(row)"> 设计 </el-button>
            <el-button v-auth="'api:admin:print-template:delete'" size="small" text type="danger" @click="onDelete(row)"> 删除 </el-button>
          </template>
        </el-table-column>
      </el-table>
      <div class="my-flex my-flex-end" style="margin-top: 10px">
        <el-pagination
          v-model:currentPage="state.pageInput.currentPage"
          v-model:page-size="state.pageInput.pageSize"
          :total="state.total"
          :page-sizes="[10, 20, 50, 100]"
          size="small"
          background
          @size-change="onSizeChange"
          @current-change="onCurrentChange"
          layout="total, sizes, prev, pager, next, jumper"
        />
      </div>
    </el-card>

    <PrintTemplateForm ref="formRef" :title="state.formTitle"></PrintTemplateForm>
    <PrintTemplateDesignDialog ref="designDialogRef" :title="state.designTitle"></PrintTemplateDesignDialog>
  </my-layout>
</template>

<script lang="ts" setup name="admin/print-template">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, defineAsyncComponent } from 'vue'
import { PageInputPrintTemplateGetPageInput, PrintTemplateGetPageOutput } from '/@/api/admin/data-contracts'
import { PrintTemplateApi } from '/@/api/admin/PrintTemplate'
import eventBus from '/@/utils/mitt'
import { auth } from '/@/utils/authFunction'
import type { FormInstance } from 'element-plus'

// 引入组件
const PrintTemplateForm = defineAsyncComponent(() => import('./components/form.vue'))
const PrintTemplateDesignDialog = defineAsyncComponent(() => import('./components/design-dialog.vue'))

const { proxy } = getCurrentInstance() as any

const filterFormRef = ref<FormInstance>()
const formRef = ref()
const designDialogRef = ref()

const state = reactive({
  loading: false,
  formTitle: '',
  designTitle: '',
  total: 0,
  filter: {
    name: '',
    code: '',
  },
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInputPrintTemplateGetPageInput,
  dataList: [] as Array<PrintTemplateGetPageOutput>,
})

onMounted(async () => {
  await onQuery()

  eventBus.off('refreshPrintTemplate')
  eventBus.on('refreshPrintTemplate', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshPrintTemplate')
})

const onSizeChange = (val: number) => {
  state.pageInput.currentPage = 1
  state.pageInput.pageSize = val
  onQuery()
}

const onCurrentChange = (val: number) => {
  state.pageInput.currentPage = val
  onQuery()
}

const onQuery = async () => {
  state.loading = true
  state.pageInput.filter = state.filter
  const res = await new PrintTemplateApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.dataList = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0

  state.loading = false
}

const onReset = () => {
  filterFormRef.value!.resetFields()

  onQuery()
}

const onAdd = () => {
  state.formTitle = '新增打印模板'
  formRef.value.open()
}

const onEdit = (row: PrintTemplateGetPageOutput) => {
  state.formTitle = '编辑打印模板'
  formRef.value.open(row)
}

const onDesign = (row: PrintTemplateGetPageOutput) => {
  state.designTitle = row.name ? row.name : '设计打印模板'
  designDialogRef.value.open(row)
}

const onDelete = (row: PrintTemplateGetPageOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除打印模板【${row.name}】?`)
    .then(async () => {
      await new PrintTemplateApi().delete({ id: row.id }, { loading: true })
      onQuery()
    })
    .catch(() => {})
}

//启用或禁用
const onSetEnable = (row: PrintTemplateGetPageOutput & { loading: boolean }) => {
  return new Promise((resolve, reject) => {
    proxy.$modal
      .confirm(`确定要${row.enabled ? '禁用' : '启用'}【${row.name}】?`)
      .then(async () => {
        row.loading = true
        const res = await new PrintTemplateApi()
          .setEnable({ printTemplateId: row.id, enabled: !row.enabled }, { showSuccessMessage: true })
          .catch(() => {
            reject(new Error('Error'))
          })
          .finally(() => {
            row.loading = false
          })
        if (res && res.success) {
          resolve(true)
          onQuery()
        } else {
          reject(new Error('Cancel'))
        }
      })
      .catch(() => {
        reject(new Error('Cancel'))
      })
  })
}
</script>

<style scoped lang="scss"></style>
