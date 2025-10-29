<template>
  <MyLayout>
    <el-card class="my-query-box mt8" shadow="never" :body-style="{ paddingBottom: '0' }">
      <el-form :inline="true" label-width="auto" @submit.stop.prevent>
        <el-form-item class="my-search-box-item" label="所属模型">
          <el-select clearable v-model="state.filter.modelId" style="width: 160px" @keyup.enter="onQuery">
            <el-option v-for="item in state.selectDevProjectModelListData" :key="item.id" :value="item.id" :label="item.name" />
          </el-select>
        </el-form-item>
        <el-form-item class="my-search-box-item" label="字段名称">
          <el-input clearable v-model="state.filter.name" placeholder="" @keyup.enter="onQuery"> </el-input>
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
      <el-table
        v-loading="state.loading"
        :data="state.devProjectModelFieldListData"
        row-key="id"
        ref="listTableRef"
        border
        @selection-change="selsChange"
      >
        <el-table-column type="selection" width="50" align="center" header-align="center" />
        <el-table-column prop="modelId_Text" label="所属模型" show-overflow-tooltip min-width="120" />
        <el-table-column prop="name" label="字段名称" show-overflow-tooltip min-width="120" />
        <el-table-column prop="code" label="字段编码" show-overflow-tooltip min-width="120" />
        <el-table-column prop="dataTypeDictName" label="字段类型" show-overflow-tooltip min-width="120" />
        <el-table-column prop="propertiesDictName" label="字段属性" show-overflow-tooltip min-width="100" />
        <el-table-column prop="isRequired" label="是否必填" width="90" align="center" header-align="center">
          <template #default="{ row }">
            <el-tag type="success" v-if="row.isRequired">是</el-tag>
            <el-tag type="danger" v-else>否</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="maxLength" label="最大长度" show-overflow-tooltip min-width="100" />
        <el-table-column prop="minLength" label="最小长度" show-overflow-tooltip min-width="100" />
        <el-table-column prop="sort" label="字段顺序" show-overflow-tooltip min-width="100" />
        <el-table-column prop="description" label="字段描述" show-overflow-tooltip min-width="180" />
        <el-table-column
          v-auths="[perms.update, perms.softDelete, perms.delete]"
          label="操作"
          :width="actionColWidth"
          fixed="right"
          header-align="center"
          align="center"
        >
          <template #default="{ row }">
            <el-button v-auth="perms.update" icon="ele-EditPen" text type="primary" @click.stop="onEdit(row)">编辑</el-button>
            <my-dropdown-more v-if="auths([perms.delete, perms.softDelete])">
              <template #dropdown>
                <el-dropdown-menu>
                  <el-dropdown-item v-if="auth(perms.softDelete)" @click.stop="onSoftDelete(row)" icon="ele-DeleteFilled">软删除</el-dropdown-item>
                  <el-dropdown-item v-if="auth(perms.delete)" @click.stop="onDelete(row)" icon="ele-Delete">删除</el-dropdown-item>
                </el-dropdown-menu>
              </template>
            </my-dropdown-more>
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

    <dev-project-model-field-form ref="devProjectModelFieldFormRef" :title="state.devProjectModelFieldFormTitle"></dev-project-model-field-form>
  </MyLayout>
</template>

<script lang="ts" setup name="dev/dev-project-model-field">
import {
  PageInputDevProjectModelFieldGetPageInput,
  DevProjectModelFieldGetPageInput,
  DevProjectModelFieldGetPageOutput,
  DevProjectModelFieldGetOutput,
  DevProjectModelFieldGetListInput,
  DevProjectModelFieldGetListOutput,
  DevProjectModelGetListOutput,
} from '/@/api/dev/data-contracts'
import { DictGetListOutput } from '/@/api/admin/data-contracts'
import { DevProjectModelFieldApi } from '/@/api/dev/DevProjectModelField'
import { DevProjectModelApi } from '/@/api/dev/DevProjectModel'
import { DictApi } from '/@/api/admin/Dict'
import eventBus from '/@/utils/mitt'
import { auth, auths, authAll } from '/@/utils/authFunction'

// 引入组件
const DevProjectModelFieldForm = defineAsyncComponent(() => import('./components/dev-project-model-field-form.vue'))

const { proxy } = getCurrentInstance() as any

const devProjectModelFieldFormRef = ref()
const listTableRef = ref()

//权限配置
const perms = {
  add: 'api:dev:dev-project-model-field:add',
  update: 'api:dev:dev-project-model-field:update',
  delete: 'api:dev:dev-project-model-field:delete',
  batDelete: 'api:dev:dev-project-model-field:batch-delete',
  softDelete: 'api:dev:dev-project-model-field:soft-delete',
  batSoftDelete: 'api:dev:dev-project-model-field:batch-soft-delete',
}

const actionColWidth = authAll([perms.update, perms.softDelete]) || authAll([perms.update, perms.delete]) ? 140 : 75

const state = reactive({
  loading: false,
  devProjectModelFieldFormTitle: '',
  total: 0,
  sels: [] as Array<DevProjectModelFieldGetPageOutput>,
  filter: {
    modelId: null,
    name: null,
  } as DevProjectModelFieldGetPageInput | DevProjectModelFieldGetListInput,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
  } as PageInputDevProjectModelFieldGetPageInput,
  devProjectModelFieldListData: [] as Array<DevProjectModelFieldGetListOutput>,
  selectDevProjectModelListData: [] as DevProjectModelGetListOutput[],
  //字典相关
  dicts: {
    fieldType: [] as DictGetListOutput[],
    fieldProperties: [] as DictGetListOutput[],
  },
})

onMounted(() => {
  getDevProjectModelList()
  getDictsTree()
  onQuery()
  eventBus.off('refreshDevProjectModelField')
  eventBus.on('refreshDevProjectModelField', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refreshDevProjectModelField')
})

const getDevProjectModelList = async () => {
  const res = await new DevProjectModelApi().getList({}).catch(() => {
    state.selectDevProjectModelListData = []
  })
  state.selectDevProjectModelListData = res?.data || []
}

//获取需要使用的字典树
const getDictsTree = async () => {
  let res = await new DictApi().getList(['fieldType', 'fieldProperties'])
  if (!res?.success) return
  state.dicts = res.data as any
}

const onQuery = async () => {
  state.loading = true
  state.pageInput.filter = state.filter
  const res = await new DevProjectModelFieldApi().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  const list = res?.data?.list ?? []
  // 处理字典映射
  state.devProjectModelFieldListData = list.map((item) => {
    // 查找 dataType 对应的字典名称
    const dataTypeDict = state.dicts.fieldType?.find((dict) => dict.value === item.dataType)
    // 查找 properties 对应的字典名称
    const propertiesDict = state.dicts.fieldProperties?.find((dict) => dict.value === item.properties)

    return {
      ...item,
      dataTypeDictName: dataTypeDict?.name || item.dataType,
      propertiesDictName: propertiesDict?.name || item.properties,
    }
  })

  state.total = res?.data?.total ?? 0
  state.loading = false
}

const onAdd = () => {
  state.devProjectModelFieldFormTitle = '新增项目模型字段'
  devProjectModelFieldFormRef.value.open()
}

const onEdit = (row: DevProjectModelFieldGetOutput) => {
  state.devProjectModelFieldFormTitle = '编辑项目模型字段'
  devProjectModelFieldFormRef.value.open(row)
}

const onDelete = (row: DevProjectModelFieldGetOutput) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${row.name}】?`)
    .then(async () => {
      await new DevProjectModelFieldApi().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
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
const selsChange = (vals: DevProjectModelFieldGetPageOutput[]) => {
  state.sels = vals
}

const onBatchDelete = async () => {
  proxy.$modal?.confirmDelete(`确定要删除选择的${state.sels.length}条记录？`).then(async () => {
    const rst = await new DevProjectModelFieldApi().batchDelete(state.sels?.map((item) => item.id) as number[], {
      loading: true,
      showSuccessMessage: true,
    })
    if (rst?.success) {
      onQuery()
    }
  })
}

const onSoftDelete = async (row: DevProjectModelFieldGetOutput) => {
  proxy.$modal?.confirmDelete(`确定要移入回收站？`).then(async () => {
    const rst = await new DevProjectModelFieldApi().softDelete({ id: row.id }, { loading: true, showSuccessMessage: true })
    if (rst?.success) {
      onQuery()
    }
  })
}

const onBatchSoftDelete = async () => {
  proxy.$modal?.confirmDelete(`确定要将选择的${state.sels.length}条记录移入回收站？`).then(async () => {
    const rst = await new DevProjectModelFieldApi().batchSoftDelete(state.sels?.map((item) => item.id) as number[], {
      loading: true,
      showSuccessMessage: true,
    })
    if (rst?.success) {
      onQuery()
    }
  })
}
</script>
