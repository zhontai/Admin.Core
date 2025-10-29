<template>
  <MyLayout>
    <el-card class="my-query-box mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form :inline="true" label-width="auto" @submit.stop.prevent>
        <el-form-item class="my-search-box-item" label="所属项目">
          <el-select clearable v-model="state.filter.projectId" placeholder="" style="width: 160px" @keyup.enter="onQuery">
            <el-option v-for="item in state.selectDevProjectListData" :key="item.id" :value="item.id" :label="item.name" />
          </el-select>
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
      <el-table v-loading="state.loading" :data="state.devProjectGenListData" row-key="id" ref="listTableRef" border @selection-change="selsChange">
        <el-table-column type="selection" width="50" align="center" header-align="center" />
        <el-table-column prop="projectId_Text" label="所属项目" show-overflow-tooltip width />
        <el-table-column prop="groupIds_Texts" label="模板组" show-overflow-tooltip width>
          <template #default="{ row }">
            {{ row.groupIds_Texts ? row.groupIds_Texts.join(',') : '' }}
          </template>
        </el-table-column>
        <el-table-column
          v-auths="[perms.update, perms.softDelete, perms.delete]"
          label="操作"
          fixed="right"
          width="240"
          align="center"
          header-align="center"
        >
          <template #default="{ row }">
            <el-button v-auth="perms.update" icon="ele-EditPen" text type="primary" @click.stop="onEdit(row)">编辑</el-button>
            <el-button v-auth="perms.update" icon="ele-View" text type="primary" @click.stop="onPreview(row)">预览</el-button>
            <el-button v-auth="perms.update" icon="ele-Position" text type="primary" @click.stop="genCode(row)">生成</el-button>
          </template>
        </el-table-column>
      </el-table>

      <div class="my-flex my-flex-end" style="margin-top: 20px">
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

    <dev-project-gen-form ref="devProjectGenFormRef" :title="state.devProjectGenFormTitle"></dev-project-gen-form>
  </MyLayout>
</template>

<script lang="ts" setup name="dev/dev-project-gen">
import {
  PageInputDevProjectGenGetPageInput,
  DevProjectGenGetPageInput,
  DevProjectGenGetPageOutput,
  DevProjectGenGetOutput,
  DevProjectGenGetListInput,
  DevProjectGenGetListOutput,
  DevProjectGetListOutput,
  DevGroupGetListOutput,
} from '/@/api/dev/data-contracts'
import { DevProjectGenApi } from '/@/api/dev/DevProjectGen'
import { DevProjectApi } from '/@/api/dev/DevProject'
import { DevGroupApi } from '/@/api/dev/DevGroup'
import eventBus from '/@/utils/mitt'
import { auth, auths, authAll } from '/@/utils/authFunction'
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()
// 引入组件
const DevProjectGenForm = defineAsyncComponent(() => import('./components/dev-project-gen-form.vue'))

const { proxy } = getCurrentInstance() as any

const devProjectGenFormRef = ref()
const listTableRef = ref()

//权限配置
const perms = {
  add: 'api:dev:dev-project-gen:add',
  update: 'api:dev:dev-project-gen:update',
  delete: 'api:dev:dev-project-gen:delete',
  batDelete: 'api:dev:dev-project-gen:batch-delete',
  softDelete: 'api:dev:dev-project-gen:soft-delete',
  batSoftDelete: 'api:dev:dev-project-gen:batch-soft-delete',
}

const actionColWidth = authAll([perms.update, perms.softDelete]) || authAll([perms.update, perms.delete]) ? 140 : 75

const state = reactive({
  loading: false,
  devProjectGenFormTitle: '',
  total: 0,
  sels: [] as Array<DevProjectGenGetPageOutput>,
  filter: {
    projectId: null,
  } as DevProjectGenGetPageInput | DevProjectGenGetListInput,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInputDevProjectGenGetPageInput,
  devProjectGenListData: [] as Array<DevProjectGenGetListOutput>,
  selectDevProjectListData: [] as DevProjectGetListOutput[],
  selectDevGroupListData: [] as DevGroupGetListOutput[],
})

onMounted(() => {
  getDevProjectList()
  getDevGroupList()
  onQuery()
  eventBus.off('refreshDevProjectGen')
  eventBus.on('refreshDevProjectGen', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshDevProjectGen')
})

const getDevProjectList = async () => {
  const res = await new DevProjectApi().getList({}).catch(() => {
    state.selectDevProjectListData = []
  })
  state.selectDevProjectListData = res?.data || []
}
const getDevGroupList = async () => {
  const res = await new DevGroupApi().getList({}).catch(() => {
    state.selectDevGroupListData = []
  })
  state.selectDevGroupListData = res?.data || []
}

const onQuery = async () => {
  state.loading = true
  state.pageInput.filter = state.filter
  const res = await new DevProjectGenApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.devProjectGenListData = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0
  state.loading = false
}

const onAdd = () => {
  state.devProjectGenFormTitle = '新增项目生成'
  devProjectGenFormRef.value.open()
}

const onEdit = (row: DevProjectGenGetOutput) => {
  state.devProjectGenFormTitle = '编辑项目生成'
  devProjectGenFormRef.value.open(row)
}

const onDelete = (row: DevProjectGenGetOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${row.projectId_Text}】?`)
    .then(async () => {
      await new DevProjectGenApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
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
const selsChange = (vals: DevProjectGenGetPageOutput[]) => {
  state.sels = vals
}

const onBatchDelete = async () => {
  proxy.$modal?.confirmDelete(`确定要删除选择的${state.sels.length}条记录？`).then(async () => {
    const rst = await new DevProjectGenApi().batchDelete(state.sels?.map((item) => item.id) as number[], { loading: true, showSuccessMessage: true })
    if (rst?.success) {
      onQuery()
    }
  })
}

const onBatchSoftDelete = async () => {
  proxy.$modal?.confirmDelete(`确定要将选择的${state.sels.length}条记录移入回收站？`).then(async () => {
    const rst = await new DevProjectGenApi().batchSoftDelete(state.sels?.map((item) => item.id) as number[], {
      loading: true,
      showSuccessMessage: true,
    })
    if (rst?.success) {
      onQuery()
    }
  })
}

const onPreview = async (row: DevProjectGenGetOutput) => {
  //预览跳转到页面，查看模板组下的模板及生成的代码
  router.push({
    path: '/dev/dev-project-gen/preview',
    query: {
      projectId: row.projectId,
      groupIds: row.groupIds,
    },
  })
}

const genCode = async (row: DevProjectGenGetOutput) => {
  new DevProjectGenApi()
    .down(
      { projectId: row.projectId, groupIds: row.groupIds_Values?.map((s) => Number(s)) },
      {
        loading: false,
        showErrorMessage: false,
        format: 'blob',
      }
    )
    .then((res) => {
      const a = document.createElement('a')
      a.href = URL.createObjectURL(res as Blob)
      a.download = '源码.zip'
      a.click()
    })
}
</script>
