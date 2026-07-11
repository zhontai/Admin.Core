<template>
  <div>
    <el-dialog
      v-model="state.showDialog"
      destroy-on-close
      :title="title"
      draggable
      :close-on-click-modal="false"
      :close-on-press-escape="false"
      width="600px"
    >
      <el-form :model="form" ref="formRef" label-width="80px">
        <el-row :gutter="35">
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item :label="t('所属平台')">
              <el-select
                v-model="form.platform"
                :disabled="!state.isCopy"
                :placeholder="t('请选择所属平台')"
                @change="handlePlatformChang"
                class="w100"
              >
                <el-option v-for="item in state.dictData[DictType.PlatForm.name]" :key="item.code" :label="item.name" :value="item.code" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item :label="t('上级菜单')">
              <el-tree-select
                v-model="form.parentId"
                :data="state.permissionTreeData"
                node-key="id"
                check-strictly
                default-expand-all
                render-after-expand
                fit-input-width
                clearable
                filterable
                class="w100"
              />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item :label="t('API接口')">
              <el-tree-select
                v-model="form.apiIds"
                :data="state.apiTreeData"
                node-key="id"
                :props="{ label: 'path' }"
                render-after-expand
                fit-input-width
                clearable
                filterable
                multiple
                collapse-tags
                collapse-tags-tooltip
                :filter-node-method="onApiFilterNode"
                class="w100"
                :default-expanded-keys="state.expandRowKeys"
                @current-change="onApiCurrentChange"
              >
                <template #default="{ data }">
                  <span class="my-flex my-flex-between">
                    <span>{{ data.label }}</span>
                    <span class="my-line-1 my-mlr-12" :title="data.path">
                      {{ data.path }}
                    </span>
                  </span>
                </template>
              </el-tree-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item :label="t('名称')" prop="label" :rules="[{ required: true, message: t('请输入名称'), trigger: ['blur', 'change'] }]">
              <el-input v-model="form.label" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item :label="t('编码')" prop="code" :rules="[{ required: true, message: t('请输入编码'), trigger: ['blur', 'change'] }]">
              <el-input v-model="form.code" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item :label="t('排序')">
              <el-input-number v-model="form.sort" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item :label="t('启用')">
              <el-switch v-model="form.enabled" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item :label="t('说明')">
              <el-input v-model="form.description" clearable type="textarea" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button auto-insert-space @click="onCancel">{{ t('取消') }}</el-button>
          <el-button auto-insert-space type="primary" @click="onSure" :loading="state.sureLoading">{{ t('确定') }}</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup name="admin/permission/permission-dot-form">
import {
  PermissionGetSimpleListOutput,
  PermissionUpdateDotInput,
  ApiGetListOutput,
  DictGetListOutput,
  PermissionType,
} from '/@/api/admin/data-contracts'
import { PermissionApi } from '/@/api/admin/Permission'
import { ApiApi } from '/@/api/admin/Api'
import { listToTree, treeToList } from '/@/utils/tree'
import eventBus from '/@/utils/mitt'
import { trimStart, replace, cloneDeep } from 'lodash-es'
import { DictApi } from '/@/api/admin/Dict'
import { t } from '/@/i18n'
import { PlatformType } from '/@/api/admin.extend/enum-contracts'
import { PermissionType as PermissionTypeEnum } from '/@/api/admin/enum-contracts'

/** 字典分类 */
const DictType = {
  PlatForm: { name: 'platform', desc: '平台' },
}

defineProps({
  title: {
    type: String,
    default: '',
  },
})

const { proxy } = getCurrentInstance() as any

const formRef = useTemplateRef('formRef')

const state = reactive({
  showDialog: false,
  sureLoading: false,
  form: { enabled: true } as PermissionUpdateDotInput,
  apiTreeData: [] as ApiGetListOutput[],
  expandRowKeys: [] as number[],
  isCopy: false,
  dictData: {
    [DictType.PlatForm.name]: [] as DictGetListOutput[] | null,
  },
  permissionTreeData: [] as PermissionGetSimpleListOutput[],
})

const { form } = toRefs(state)

// 获得字典数据
const getDictList = async () => {
  const res = await new DictApi().getList([DictType.PlatForm.name]).catch(() => {})
  if (res?.success && res.data) {
    state.dictData = markRaw(res.data)
  }
}

// 获得API接口树数据
const getApis = async () => {
  const res = await new ApiApi().getList()
  if (res?.success && res.data && res.data.length > 0) {
    state.apiTreeData = listToTree(res.data) as ApiGetListOutput[]
  } else {
    state.apiTreeData = []
  }
}

// 获得权限菜单树数据
const getPermissionTreeData = async (platform: string | undefined | null = PlatformType.Web.name) => {
  const res = await new PermissionApi()
    .getSimpleList({
      platform,
      type: PermissionTypeEnum.Menu.value as PermissionType,
    })
    .catch(() => {})
  if (res?.success) {
    state.permissionTreeData = markRaw(listToTree(res.data || []))
  }
}

// 打开对话框
const open = async (
  row: PermissionUpdateDotInput = {
    id: 0,
    enabled: true,
    parentId: undefined,
  },
  isCopy = false
) => {
  proxy.$modal.loading()
  state.isCopy = isCopy
  await getDictList()
  await getApis()
  await getPermissionTreeData(row.platform)

  state.expandRowKeys = treeToList(cloneDeep(state.apiTreeData))
    .filter((a: ApiGetListOutput) => a.parentId === 0)
    .map((a: ApiGetListOutput) => a.id) as number[]

  if (row.id > 0) {
    const res = await new PermissionApi().getDot({ id: row.id }).catch(() => {})

    if (res?.success) {
      let formData = res.data as PermissionUpdateDotInput
      formData.platform = row.platform
      formData.parentId = formData.parentId && formData.parentId > 0 ? formData.parentId : undefined
      if (isCopy) formData.id = 0
      state.form = formData
    }
  } else {
    state.form = row
  }

  proxy.$modal.closeLoading()
  state.showDialog = true
}

// API接口搜索过滤
const onApiFilterNode = (value: string, data: ApiGetListOutput) => {
  if (!value) return true
  return data.label?.indexOf(value) !== -1 || data.path?.indexOf(value) !== -1
}

// API接口选择改变
const onApiCurrentChange = (data: ApiGetListOutput) => {
  if (data?.httpMethods) {
    if (!state.form.label) {
      state.form.label = data.label
    }
    if (!state.form.code) {
      state.form.code = trimStart(replace(data.path || '', /\//g, ':'), ':')
    }
  }
}

// 平台改变
const handlePlatformChang = async (value: string = PlatformType.Web.name) => {
  proxy.$modal.loading()
  state.form.parentId = undefined
  await getPermissionTreeData(value)
  proxy.$modal.closeLoading()
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

// 确定
const onSure = () => {
  formRef.value.validate(async (valid: boolean) => {
    if (!valid) return

    state.sureLoading = true
    let res = {} as any
    state.form.parentId = state.form.parentId && state.form.parentId > 0 ? state.form.parentId : undefined
    if (state.form.id != undefined && state.form.id > 0) {
      res = await new PermissionApi().updateDot(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new PermissionApi().addDot(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }

    state.sureLoading = false

    if (res?.success) {
      eventBus.emit('refreshPermission')
      state.showDialog = false
    }
  })
}

defineExpose({
  open,
})
</script>

<style scoped lang="scss">
.my-mlr-12 {
  margin-left: 12px;
  margin-right: 12px;
}
</style>
