<template>
  <div v-loading="state.loading" class="h100" style="padding-top: 10px">
    <vue3-tree-org
      ref="orgRef"
      :data="state.data"
      :props="{
        id: 'id',
        pid: 'parentId',
        label: 'name',
        expand: 'expand',
        children: 'children',
      }"
      center
      :horizontal="false"
      :collapsable="false"
      :only-one-node="false"
      :clone-node-drag="false"
      :node-draggable="false"
      :define-menus="[
        { name: '复制文本', command: 'copy' },
        { name: '新增部门', command: 'onAdd' },
        { name: '编辑部门', command: 'onEdit' },
        { name: '删除部门', command: 'onDelete' },
      ]"
      :label-style="state.style"
      :filter-node-method="filterNodeMethod"
      @on-node-dblclick="onNodeDblclick"
      @on-contextmenu="onContextmenu"
      v-bind="$attrs"
    >
      <template v-if="state.showOrgCount" v-slot:expand="{ node }">
        <div>{{ node.children.length }}</div>
      </template>
    </vue3-tree-org>

    <org-form ref="orgFormRef" :title="state.orgFormTitle"></org-form>
  </div>
</template>

<script lang="ts" setup name="admin/org-tree-img">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, defineAsyncComponent } from 'vue'
import { OrgListOutput } from '/@/api/admin/data-contracts'
import { OrgApi } from '/@/api/admin/Org'
import { listToTree } from '/@/utils/tree'
import eventBus from '/@/utils/mitt'

// 引入组件
const OrgForm = defineAsyncComponent(() => import('./org-form.vue'))

const { proxy } = getCurrentInstance() as any

const orgRef = ref()
const orgFormRef = ref()

const state = reactive({
  loading: false,
  orgFormTitle: '',
  filter: {
    name: '',
  },
  style: {
    background: '#fff',
    color: '#5e6d82',
  },
  showOrgCount: true,
  data: [] as any,
  orgTreeData: [] as Array<OrgListOutput>,
})

onMounted(() => {
  onQuery()
  eventBus.off('refreshOrg')
  eventBus.on('refreshOrg', () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshOrg')
})

const onQuery = async () => {
  state.loading = true
  const res = await new OrgApi().getList().catch(() => {
    state.loading = false
  })
  if (res && res.data && res.data.length > 0) {
    const data = listToTree(res.data, {
      extraData: {
        expand: true,
      },
    })
    state.orgTreeData = data
    state.data = data?.length > 0 ? data[0] : {}
    state.data['disabled'] = true
  } else {
    state.data = []
  }
  state.loading = false
}

const onAdd = (row: OrgListOutput) => {
  state.orgFormTitle = '新增部门'
  orgFormRef.value.open({ parentId: row?.id })
}

const onEdit = (row: OrgListOutput) => {
  state.orgFormTitle = '编辑部门'
  orgFormRef.value.open(row)
}

const onDelete = (row: OrgListOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除部门【${row.name}】?`)
    .then(async () => {
      await new OrgApi().delete({ id: row.id }, { loading: true })
      onQuery()
    })
    .catch(() => {})
}

const onContextmenu = ({ node, command }: any) => {
  switch (command) {
    case 'onAdd':
      onAdd(node.$$data)
      break
    case 'onEdit':
      onEdit(node.$$data)
      break
    case 'onDelete':
      onDelete(node.$$data)
      break
  }
}

const filter = (filterword: string) => {
  orgRef.value.filter(filterword)
}

const filterNodeMethod = (value: string, data: any) => {
  if (!value) return true
  return data.label.indexOf(value) !== -1
}

const onNodeDblclick = (e: any, data: any) => {}

defineExpose({
  filter,
})
</script>

<style scoped lang="scss">
:deep() {
  .icon-fullscreen:before {
    content: '\e603' !important;
  }
  .tree-org {
    margin-top: 5px;
  }
}
</style>
