<template>
  <MyLayout>
    <el-card class="my-query-box mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form :inline="true" label-width="auto" @submit.stop.prevent>
        <el-form-item label="项目名称">
          <el-input clearable v-model="state.filter.name" placeholder="" @keyup.enter="onQuery"> </el-input>
        </el-form-item>
        <el-form-item label="项目编码">
          <el-input clearable v-model="state.filter.code" placeholder="" @keyup.enter="onQuery"> </el-input>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Search" @click="onQuery">查询</el-button>
        </el-form-item>
      </el-form>
    </el-card>

    <el-card class="my-fill mt8" shadow="never">
      <div class="my-tools-box mb8 my-flex my-flex-between">
        <div>
          <el-space wrap :size="12">
            <el-button type="primary" v-auth="perms.add" icon="ele-Plus" @click="onAdd">新增</el-button>
            <el-dropdown :placement="'bottom-end'" v-if="auths([perms.batSoftDelete, perms.batDelete])">
              <el-button type="primary"
                >批量操作 <el-icon class="el-icon--right"><ele-ArrowDown /></el-icon
              ></el-button>
              <template #dropdown>
                <el-dropdown-menu>
                  <el-dropdown-item :disabled="state.sels.length == 0" @click.stop="batchGenCode" icon="ele-Position">批量生成代码</el-dropdown-item>
                  <el-dropdown-item
                    v-if="auth(perms.batSoftDelete)"
                    :disabled="state.sels.length == 0"
                    @click="onBatchSoftDelete"
                    icon="ele-DeleteFilled"
                    >批量软删除</el-dropdown-item
                  >
                  <el-dropdown-item v-if="auth(perms.batDelete)" :disabled="state.sels.length == 0" @click="onBatchDelete" icon="ele-Delete"
                    >批量删除</el-dropdown-item
                  >
                </el-dropdown-menu>
              </template>
            </el-dropdown>
          </el-space>
        </div>
        <div></div>
      </div>
      <el-table v-loading="state.loading" :data="state.devProjectListData" row-key="id" ref="listTableRef" border @selection-change="selsChange">
        <el-table-column type="selection" width="50" align="center" header-align="center" />
        <el-table-column prop="name" label="项目名称" show-overflow-tooltip min-width="160" />
        <el-table-column prop="code" label="项目编码" show-overflow-tooltip min-width="160" />
        <el-table-column prop="groupId_Text" label="模板分组" show-overflow-tooltip min-width="180" />
        <el-table-column label="状态" width="82" align="center">
          <template #default="{ row }">
            <el-tag type="success" v-if="row.isEnable">启用</el-tag>
            <el-tag type="danger" v-else>禁用</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="remark" label="备注" show-overflow-tooltip min-width="180" />
        <el-table-column v-auths="[perms.update, perms.softDelete, perms.delete]" label="操作" :width="actionColWidth" fixed="right">
          <template #default="{ row }">
            <el-button v-auth="perms.update" icon="ele-EditPen" text type="primary" @click.stop="onEdit(row)">编辑</el-button>
            <my-dropdown-more v-if="auths([perms.delete, perms.softDelete])">
              <template #dropdown>
                <el-dropdown-menu>
                  <el-dropdown-item @click.stop="genCode(row)" icon="ele-Position">生成代码</el-dropdown-item>
                  <el-dropdown-item v-if="auth(perms.softDelete)" @click.stop="onSoftDelete(row)" icon="ele-DeleteFilled">软删除</el-dropdown-item>
                  <el-dropdown-item v-if="auth(perms.delete)" @click.stop="onDelete(row)" icon="ele-Delete">删除</el-dropdown-item>
                </el-dropdown-menu>
              </template>
            </my-dropdown-more>
          </template>
        </el-table-column>
      </el-table>

      <div class="my-flex my-flex-end mt10">
        <el-pagination
          v-model:currentPage="state.pageInput.currentPage"
          v-model:page-size="state.pageInput.pageSize"
          :total="state.total"
          :page-sizes="[10, 20, 50, 100]"
          background
          @size-change="onSizeChange"
          @current-change="onCurrentChange"
          layout="total, sizes, prev, pager, next, jumper"
        />
      </div>
    </el-card>

    <dev-project-form ref="devProjectFormRef" :title="state.devProjectFormTitle"></dev-project-form>
  </MyLayout>
</template>

<script lang="ts" setup name="dev/dev-project">
import {
  PageInputDevProjectGetPageInput,
  DevProjectGetPageInput,
  DevProjectGetPageOutput,
  DevProjectGetOutput,
  DevProjectGetListInput,
  DevProjectGetListOutput,
  DevGroupGetListOutput,
} from '/@/api/dev/data-contracts'
import { DevProjectApi } from '/@/api/dev/DevProject'
import { DevGroupApi } from '/@/api/dev/DevGroup'
import eventBus from '/@/utils/mitt'
import { auth, auths, authAll } from '/@/utils/authFunction'

// 引入组件
const DevProjectForm = defineAsyncComponent(() => import('./components/dev-project-form.vue'))

const { proxy } = getCurrentInstance() as any

const devProjectFormRef = ref()
const listTableRef = ref()

//权限配置
const perms = {
  add: 'api:dev:dev-project:add',
  update: 'api:dev:dev-project:update',
  delete: 'api:dev:dev-project:delete',
  batDelete: 'api:dev:dev-project:batch-delete',
  softDelete: 'api:dev:dev-project:soft-delete',
  batSoftDelete: 'api:dev:dev-project:batch-soft-delete',
}

const actionColWidth = authAll([perms.update, perms.softDelete]) || authAll([perms.update, perms.delete]) ? 140 : 75

const state = reactive({
  loading: false,
  devProjectFormTitle: '',
  total: 0,
  sels: [] as Array<DevProjectGetPageOutput>,
  filter: {
    name: null,
    code: null,
  } as DevProjectGetPageInput | DevProjectGetListInput,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInputDevProjectGetPageInput,
  devProjectListData: [] as Array<DevProjectGetListOutput>,
  selectDevGroupListData: [] as DevGroupGetListOutput[],
})

onMounted(() => {
  getDevGroupList()
  onQuery()
  eventBus.off('refreshDevProject')
  eventBus.on('refreshDevProject', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshDevProject')
})

const getDevGroupList = async () => {
  const res = await new DevGroupApi().getList({}).catch(() => {
    state.selectDevGroupListData = []
  })
  state.selectDevGroupListData = res?.data || []
}

const onQuery = async () => {
  state.loading = true
  state.pageInput.filter = state.filter
  const res = await new DevProjectApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.devProjectListData = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0
  state.loading = false
}

const onAdd = () => {
  state.devProjectFormTitle = '新增项目'
  devProjectFormRef.value.open()
}

const onEdit = (row: DevProjectGetOutput) => {
  state.devProjectFormTitle = '编辑项目'
  devProjectFormRef.value.open(row)
}

const onDelete = (row: DevProjectGetOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${row.name}】?`)
    .then(async () => {
      await new DevProjectApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

const onSizeChange = (val: number) => {
  state.pageInput.pageSize = val
  onQuery()
}

const onCurrentChange = (val: number) => {
  state.pageInput.currentPage = val
  onQuery()
}
const selsChange = (vals: DevProjectGetPageOutput[]) => {
  state.sels = vals
}

const onBatchDelete = async () => {
  proxy.$modal?.confirmDelete(`确定要删除选择的${state.sels.length}条记录？`).then(async () => {
    const rst = await new DevProjectApi().batchDelete(state.sels?.map((item) => item.id) as number[], { loading: true, showSuccessMessage: true })
    if (rst?.success) {
      onQuery()
    }
  })
}

const onSoftDelete = async (row: DevProjectGetOutput) => {
  proxy.$modal?.confirmDelete(`确定要移入回收站？`).then(async () => {
    const rst = await new DevProjectApi().softDelete({ id: row.id }, { loading: true, showSuccessMessage: true })
    if (rst?.success) {
      onQuery()
    }
  })
}

const onBatchSoftDelete = async () => {
  proxy.$modal?.confirmDelete(`确定要将选择的${state.sels.length}条记录移入回收站？`).then(async () => {
    const rst = await new DevProjectApi().batchSoftDelete(state.sels?.map((item) => item.id) as number[], { loading: true, showSuccessMessage: true })
    if (rst?.success) {
      onQuery()
    }
  })
}
const genCode = async (row: DevProjectGetOutput) => {
  await new DevProjectApi().generate({ id: row.id, groupId: row.groupId }, { loading: true, showSuccessMessage: true })
}

const batchGenCode = async () => {
  if (state.sels.length == 0) return proxy.$modal.msgWarning('请选择要生成的项目')

  await new DevProjectApi().batchGenerate(
    state.sels.map((s) => s.id) as number[],
    {
      groupId: state.sels[0].groupId,
    },
    { loading: true, showSuccessMessage: true }
  )
}
</script>
