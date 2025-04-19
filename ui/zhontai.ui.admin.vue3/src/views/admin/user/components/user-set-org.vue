<template>
  <div>
    <el-dialog
      v-model="state.showDialog"
      destroy-on-close
      :title="title"
      draggable
      :close-on-click-modal="false"
      :close-on-press-escape="false"
      width="475px"
    >
      <el-form ref="formRef" :model="form" size="default" label-width="80px">
        <el-row :gutter="35">
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="部门" prop="orgIds" :rules="[{ required: true, message: '请选择部门', trigger: ['change'] }]">
              <el-tree-select
                ref="orgTreeSelectRef"
                v-model="form.orgIds"
                placeholder="请选择部门"
                :data="state.orgTreeData"
                node-key="id"
                :props="{ label: 'name' }"
                check-strictly
                default-expand-all
                render-after-expand
                fit-input-width
                clearable
                multiple
                collapse-tags
                collapse-tags-tooltip
                filterable
                class="w100"
              />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="主属部门" prop="orgId" :rules="[{ required: true, message: '请选择主属部门', trigger: ['change'] }]">
              <el-select v-model="form.orgId" placeholder="请选择主属部门" class="w100">
                <el-option v-for="item in state.orgs" :key="item.id" :label="item.name" :value="item.id" />
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="onCancel" size="default">取 消</el-button>
          <el-button type="primary" @click="onSure" size="default" :loading="state.sureLoading">确 定</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup name="admin/user/form">
import { reactive, toRefs, getCurrentInstance, ref, watch } from 'vue'
import { UserBatchSetOrgInput, OrgGetListOutput } from '/@/api/admin/data-contracts'
import { UserApi } from '/@/api/admin/User'
import { OrgApi } from '/@/api/admin/Org'
import { listToTree, treeToList } from '/@/utils/tree'
import { cloneDeep } from 'lodash-es'
import eventBus from '/@/utils/mitt'
import { FormInstance } from 'element-plus'

const userIds = defineModel('userIds', { type: Array, default: [] })

defineProps({
  title: {
    type: String,
    default: '部门转移',
  },
})

const { proxy } = getCurrentInstance() as any

const orgTreeSelectRef = ref()
const formRef = ref<FormInstance>()
const state = reactive({
  showDialog: false,
  sureLoading: false,
  form: {
    userIds: userIds.value as number[],
  } as UserBatchSetOrgInput,
  orgs: [] as any,
  orgTreeData: [] as OrgGetListOutput[],
})
const { form } = toRefs(state)

watch(
  () => state.form.orgIds,
  (value) => {
    if (value && value.length > 0) {
      let orgs = [] as any
      treeToList(cloneDeep(state.orgTreeData)).forEach((a: any) => {
        if (value.some((b) => a.id === b)) {
          orgs.push(a)
        }
      })
      state.orgs = orgs
    } else {
      state.orgs = []
    }
  },
  {
    immediate: true,
  }
)

watch(
  () => state.orgs,
  () => {
    if (state.orgs?.some((a: any) => a.id === state.form.orgId)) {
      return
    }
    state.form.orgId = state.orgs && state.orgs.length > 0 ? state.orgs[0].id : undefined
  },
  {
    immediate: true,
    deep: true,
  }
)

const getOrgs = async () => {
  const res = await new OrgApi().getList().catch(() => {
    state.orgTreeData = []
  })
  if (res?.success && res.data && res.data.length > 0) {
    state.orgTreeData = listToTree(res.data)
  } else {
    state.orgTreeData = []
  }
}

// 打开对话框
const open = async () => {
  proxy.$modal.loading()
  state.form.orgId = undefined
  state.form.orgIds = []
  await getOrgs()

  proxy.$modal.closeLoading()
  state.showDialog = true
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

// 确定
const onSure = () => {
  formRef.value!.validate(async (valid: boolean) => {
    if (!valid) return

    state.sureLoading = true
    let res = {} as any
    res = await new UserApi()
      .batchSetOrg(
        {
          userIds: userIds.value as number[],
          orgIds: state.form.orgIds,
          orgId: state.form.orgId,
        },
        { showSuccessMessage: true }
      )
      .catch(() => {
        state.sureLoading = false
      })
    state.sureLoading = false

    if (res?.success) {
      eventBus.emit('refreshUser')
      state.showDialog = false
    }
  })
}

defineExpose({
  open,
})
</script>
