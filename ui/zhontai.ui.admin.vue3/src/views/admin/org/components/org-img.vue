<template>
  <div v-loading="state.loading" class="h100">
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
import { OrgGetListOutput } from '/@/api/admin/data-contracts'
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
  orgTreeData: [] as Array<OrgGetListOutput>,
})

onMounted(() => {
  onQuery()
  eventBus.off('refreshOrgImg')
  eventBus.on('refreshOrgImg', () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshOrgImg')
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

const onAdd = (row: OrgGetListOutput) => {
  state.orgFormTitle = '新增部门'
  orgFormRef.value.open({ parentId: row?.id })
}

const onEdit = (row: OrgGetListOutput) => {
  state.orgFormTitle = '编辑部门'
  orgFormRef.value.open(row)
}

const onDelete = (row: OrgGetListOutput) => {
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

<style lang="scss">
.zm-tree-contextmenu {
  background-color: var(--el-bg-color-overlay);
  border-color: var(--el-border-color);
  li:hover {
    background-color: var(--el-color-primary-light-9);
    color: var(--el-color-primary);
  }
}
</style>

<style scoped lang="scss">
:deep() {
  .icon-fullscreen:before {
    content: '\e603' !important;
  }
  .tree-org {
    margin-top: 5px;
  }
  .zm-tree-org {
    background-color: var(--el-bg-color-overlay);
    padding: 0px;
  }
  .tree-org-node__inner {
    background-color: var(--el-bg-color) !important;
    color: var(--el-text-color-primary) !important;
    border: 1px solid var(--el-border-color);
  }
  .tree-org-node:not(:first-child):before,
  .tree-org-node:not(:last-child):after,
  .tree-org-node:after,
  .tree-org-node__children:before {
    border-color: var(--el-border-color);
  }
  .zm-tree-handle .zm-tree-handle-item {
    background-color: var(--el-bg-color-overlay);
    color: var(--el-text-color-primary);
    border-color: var(--el-border-color);
    .zm-tree-restore {
      border-color: var(--el-text-color-primary);
    }
    .zm-tree-restore:after {
      border-top-color: var(--el-text-color-primary);
      border-right-color: var(--el-text-color-primary);
    }
    &:hover {
      background-color: var(--el-color-primary-light-9);
      color: var(--el-color-primary);
      border-color: var(--el-color-primary-light-7);
      .zm-tree-restore,
      .zm-tree-restore:after {
        border-color: var(--el-color-primary);
      }
    }
  }
}
</style>
