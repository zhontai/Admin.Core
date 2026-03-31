<template>
  <MyLayout>
    <el-card class="my-query-box mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form :inline="true" @submit.stop.prevent>
        <el-form-item :label="t('分类名称')">
          <el-input v-model="state.filter.msgTypeName" :placeholder="t('分类名称')" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item>
          <el-button auto-insert-space type="primary" icon="ele-Search" @click="onQuery">{{ t('查询') }}</el-button>
          <el-button auto-insert-space v-auth="'api:admin:msg-type:add'" type="primary" icon="ele-Plus" @click="onAdd">{{ t('新增') }}</el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table
        ref="msgTypeTableRef"
        v-loading="state.loading"
        :data="state.msgTypeTreeData"
        row-key="id"
        default-expand-all
        :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
        style="width: 100%"
        border
      >
        <el-table-column prop="name" :label="t('分类名称')" min-width="120" show-overflow-tooltip />
        <el-table-column prop="code" :label="t('分类编码')" min-width="120" show-overflow-tooltip />
        <el-table-column prop="sort" :label="t('排序')" width="82" align="center" show-overflow-tooltip />
        <el-table-column :label="t('操作')" width="210" fixed="right" header-align="center" align="right">
          <template #default="{ row }">
            <el-button
              auto-insert-space
              v-if="row.parentId === 0"
              v-auth="'api:admin:msg-type:add'"
              icon="ele-Plus"
              text
              type="primary"
              @click="onAdd(row)"
            >
              {{ t('添加') }}
            </el-button>
            <el-button auto-insert-space v-auth="'api:admin:msg-type:update'" icon="ele-EditPen" text type="primary" @click="onEdit(row)">{{
              t('编辑')
            }}</el-button>
            <el-button auto-insert-space v-auth="'api:admin:msg-type:delete'" icon="ele-Delete" text type="danger" @click="onDelete(row)">{{
              t('删除')
            }}</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <msg-type-form ref="msgTypeFormRef" :title="state.msgTypeFormTitle" :tree-data="state.msgTypeFormTreeData"></msg-type-form>
  </MyLayout>
</template>

<script lang="ts" setup name="admin/msg-type">
import { MsgTypeGetListOutput, MsgTypeUpdateInput } from '/@/api/admin/data-contracts'
import { MsgTypeApi } from '/@/api/admin/MsgType'
import { listToTree, filterTree } from '/@/utils/tree'
import { ElTable } from 'element-plus'
import { cloneDeep } from 'lodash-es'
import eventBus from '/@/utils/mitt'
import { t } from '/@/i18n'

// 引入组件
const MsgTypeForm = defineAsyncComponent(() => import('./components/msg-type-form.vue'))

const { proxy } = getCurrentInstance() as any

const msgTypeTableRef = useTemplateRef('msgTypeTableRef')
const msgTypeFormRef = useTemplateRef('msgTypeFormRef')

const state = reactive({
  loading: false,
  userListLoading: false,
  sureLoading: false,
  msgTypeFormTitle: '',
  filter: {
    name: '',
    msgTypeName: '',
  },
  msgTypeTreeData: [] as any,
  msgTypeFormTreeData: [] as any,
  msgTypeId: undefined as number | undefined,
  msgTypeName: '' as string | null | undefined,
})

onMounted(() => {
  onQuery()
  eventBus.off('refreshMsgType')
  eventBus.on('refreshMsgType', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshMsgType')
})

const onQuery = async () => {
  state.loading = true
  const res = await new MsgTypeApi().getList().catch(() => {
    state.loading = false
  })
  if (res && res.data && res.data.length > 0) {
    state.msgTypeTreeData = filterTree(listToTree(cloneDeep(res.data)), state.filter.msgTypeName)
    state.msgTypeFormTreeData = listToTree(cloneDeep(res.data).filter((a) => a.parentId === 0))
    if (state.msgTypeTreeData.length > 0 && state.msgTypeTreeData[0].children?.length > 0) {
      nextTick(() => {
        msgTypeTableRef.value!.setCurrentRow(state.msgTypeTreeData[0].children[0])
      })
    }
  } else {
    state.msgTypeTreeData = []
    state.msgTypeFormTreeData = []
  }

  state.loading = false
}

const onAdd = (row: MsgTypeGetListOutput | undefined = undefined) => {
  state.msgTypeFormTitle = t('新增消息分类')
  msgTypeFormRef.value?.open({ id: 0, parentId: row?.id })
}

const onEdit = (row: MsgTypeGetListOutput) => {
  state.msgTypeFormTitle = t('编辑消息分类')
  msgTypeFormRef.value?.open(row as MsgTypeUpdateInput)
}

const onDelete = (row: MsgTypeGetListOutput) => {
  proxy.$modal
    .confirmDelete(t('确定要删除消息分类【{name}】?', { name: row.name }))
    .then(async () => {
      await new MsgTypeApi().delete({ id: row.id }, { loading: true })
      onQuery()
    })
    .catch(() => {})
}
</script>

<style scoped lang="scss"></style>
