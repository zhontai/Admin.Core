<template>
  <MyLayout>
    <el-card class="my-query-box mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form :inline="true" @submit.stop.prevent>
        <el-form-item label="分类名称">
          <el-input v-model="state.filter.msgTypeName" placeholder="分类名称" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          <el-button v-auth="'api:admin:msg-type:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
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
        <el-table-column prop="name" label="分类名称" min-width="120" show-overflow-tooltip />
        <el-table-column prop="code" label="分类编码" min-width="120" show-overflow-tooltip />
        <el-table-column prop="sort" label="排序" width="82" align="center" show-overflow-tooltip />
        <el-table-column label="操作" width="210" fixed="right" header-align="center" align="right">
          <template #default="{ row }">
            <el-button v-if="row.parentId === 0" v-auth="'api:admin:msg-type:add'" icon="ele-Plus" text type="primary" @click="onAdd(row)"
              >添加</el-button
            >
            <el-button v-auth="'api:admin:msg-type:update'" icon="ele-EditPen" text type="primary" @click="onEdit(row)">编辑</el-button>
            <el-button v-auth="'api:admin:msg-type:delete'" icon="ele-Delete" text type="danger" @click="onDelete(row)">删除</el-button>
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
  state.msgTypeFormTitle = '新增消息分类'
  msgTypeFormRef.value?.open({ id: 0, parentId: row?.id })
}

const onEdit = (row: MsgTypeGetListOutput) => {
  state.msgTypeFormTitle = '编辑消息分类'
  msgTypeFormRef.value?.open(row as MsgTypeUpdateInput)
}

const onDelete = (row: MsgTypeGetListOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除消息分类【${row.name}】?`)
    .then(async () => {
      await new MsgTypeApi().delete({ id: row.id }, { loading: true })
      onQuery()
    })
    .catch(() => {})
}
</script>

<style scoped lang="scss"></style>
