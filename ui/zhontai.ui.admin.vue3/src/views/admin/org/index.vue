<template>
  <div class="my-layout">
    <el-card class="mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form :inline="true" @submit.stop.prevent>
        <el-form-item label="部门名称">
          <el-input v-model="state.filter.name" placeholder="部门名称" @keyup.enter="onQuery" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery"> 查询 </el-button>
          <el-button v-auth="'api:admin:org:add'" type="primary" icon="ele-Plus" @click="onAdd"> 新增 </el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <el-table
        :data="state.orgTreeData"
        style="width: 100%"
        v-loading="state.loading"
        row-key="id"
        default-expand-all
        :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
      >
        <el-table-column prop="name" label="部门名称" min-width="120" show-overflow-tooltip />
        <el-table-column prop="code" label="部门编码" min-width="120" show-overflow-tooltip />
        <el-table-column prop="value" label="部门值" min-width="80" show-overflow-tooltip />
        <el-table-column prop="sort" label="排序" width="80" align="center" show-overflow-tooltip />
        <el-table-column label="状态" width="80" align="center" show-overflow-tooltip>
          <template #default="{ row }">
            <el-tag type="success" v-if="row.enabled">启用</el-tag>
            <el-tag type="danger" v-else>禁用</el-tag>
          </template>
        </el-table-column>
        <el-table-column label="操作" width="160" fixed="right" header-align="center" align="center">
          <template #default="{ row }">
            <el-button
              v-if="auth('api:admin:org:update') && row.parentId > 0"
              icon="ele-EditPen"
              size="small"
              text
              type="primary"
              @click="onEdit(row)"
              >编辑</el-button
            >
            <el-button
              v-if="auth('api:admin:org:delete') && row.parentId > 0"
              icon="ele-Delete"
              size="small"
              text
              type="danger"
              @click="onDelete(row)"
              >删除</el-button
            >
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <org-form ref="orgFormRef" :title="state.orgFormTitle" :org-tree-data="state.orgTreeData"></org-form>
  </div>
</template>

<script lang="ts" setup name="admin/org">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, defineAsyncComponent } from 'vue'
import { OrgListOutput } from '/@/api/admin/data-contracts'
import { OrgApi } from '/@/api/admin/Org'
import { listToTree, filterTree } from '/@/utils/tree'
import eventBus from '/@/utils/mitt'
import { auth } from '/@/utils/authFunction'

// 引入组件
const OrgForm = defineAsyncComponent(() => import('./components/org-form.vue'))

const { proxy } = getCurrentInstance() as any

const orgFormRef = ref()

const state = reactive({
  loading: false,
  orgFormTitle: '',
  filter: {
    name: '',
  },
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
    state.orgTreeData = filterTree(listToTree(res.data), state.filter.name)
  } else {
    state.orgTreeData = []
  }
  state.loading = false
}

const onAdd = () => {
  state.orgFormTitle = '新增部门'
  orgFormRef.value.open()
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
</script>

<style scoped lang="scss"></style>
