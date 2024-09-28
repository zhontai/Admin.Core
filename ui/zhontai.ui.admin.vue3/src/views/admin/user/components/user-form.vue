<template>
  <div>
    <el-dialog
      v-model="state.showDialog"
      destroy-on-close
      :title="title"
      draggable
      :close-on-click-modal="false"
      :close-on-press-escape="false"
      width="769px"
    >
      <el-form ref="formRef" :model="form" size="default" label-width="80px">
        <el-row :gutter="35">
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="姓名" prop="name" :rules="[{ required: true, message: '请输入姓名', trigger: ['blur', 'change'] }]">
              <el-input v-model="form.name" autocomplete="off" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item
              label="手机号"
              prop="mobile"
              :rules="[
                { required: true, message: '请输入手机号', trigger: ['blur', 'change'] },
                { validator: testMobile, trigger: ['blur', 'change'] },
              ]"
            >
              <el-input v-model="form.mobile" autocomplete="off" maxlength="11" @blur="onBlurMobile" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
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
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="主属部门" prop="orgId" :rules="[{ required: true, message: '请选择主属部门', trigger: ['change'] }]">
              <el-select v-model="form.orgId" placeholder="请选择主属部门" class="w100">
                <el-option v-for="item in state.orgs" :key="item.id" :label="item.name" :value="item.id" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="账号" prop="userName" :rules="[{ required: true, message: '请输入账号', trigger: ['blur', 'change'] }]">
              <el-input v-model="form.userName" autocomplete="off" />
            </el-form-item>
          </el-col>
          <el-col v-if="!isUpdate" :xs="12" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item prop="password" :rules="[{ validator: validatorPwd, trigger: ['blur', 'change'] }]">
              <template #label>
                <div class="my-flex-y-center">
                  密码<el-tooltip effect="dark" placement="top" hide-after="0">
                    <template #content>选填，不填则使用系统默认密码<br />字母+数字+可选特殊字符，长度在6-16之间</template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <el-input
                key="password"
                v-model="form.password"
                placeholder="选填，不填则使用系统默认密码"
                @input="onInputPwd"
                show-password
                autocomplete="off"
              />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="直属主管" prop="managerUserId">
              <my-select-user v-model="form.managerUserId" :name="form.managerUserName" clearable></my-select-user>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="角色" prop="roles">
              <el-tree-select
                v-model="form.roleIds"
                placeholder="请选择角色"
                :data="state.roleTreeData"
                node-key="id"
                :props="{ label: 'name' }"
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
          <el-col :xs="24" :sm="span" :md="span" :lg="span" :xl="span">
            <el-form-item label="邮箱" prop="email" :rules="[{ validator: testEmail, trigger: ['blur', 'change'] }]">
              <el-input v-model="form.email" autocomplete="off" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="职位">
              <el-input v-model="form.staff.position" autocomplete="off" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="性别">
              <el-select v-model="form.staff.sex" placeholder="请选择性别" class="w100">
                <el-option label="" :value="undefined" />
                <el-option v-for="item in state.sexList" :key="item.label" :label="item.label" :value="item.value" />
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
import { reactive, toRefs, getCurrentInstance, ref, watch, defineAsyncComponent, computed } from 'vue'
import { UserAddInput, UserUpdateInput, OrgListOutput, RoleGetListOutput } from '/@/api/admin/data-contracts'
import { UserApi } from '/@/api/admin/User'
import { OrgApi } from '/@/api/admin/Org'
import { RoleApi } from '/@/api/admin/Role'
import { listToTree, treeToList } from '/@/utils/tree'
import { cloneDeep } from 'lodash-es'
import { isMobile, testMobile, testEmail } from '/@/utils/test'
import { validatorPwd } from '/@/utils/validators'
import eventBus from '/@/utils/mitt'
import { FormInstance } from 'element-plus'
import { verifyCnAndSpace } from '/@/utils/toolsValidate'
import { Sex } from '/@/api/admin/enum-contracts'
import { toOptionsByValue } from '/@/utils/enum'

// 引入组件
const MySelectUser = defineAsyncComponent(() => import('./my-select-user.vue'))

defineProps({
  title: {
    type: String,
    default: '',
  },
})

const { proxy } = getCurrentInstance() as any

const orgTreeSelectRef = ref()
const formRef = ref<FormInstance>()
const state = reactive({
  showDialog: false,
  sureLoading: false,
  form: {
    orgIds: [] as any,
    roleIds: [] as any,
  } as UserAddInput & UserUpdateInput,
  orgs: [] as any,
  orgTreeData: [] as OrgListOutput[],
  roleTreeData: [] as RoleGetListOutput[],
  sexList: toOptionsByValue(Sex),
})
const { form } = toRefs(state)

const isUpdate = computed(() => {
  return state.form.id > 0
})

const span = computed(() => {
  return isUpdate.value ? 12 : 24
})

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

const getRoles = async () => {
  const res = await new RoleApi().getList().catch(() => {
    state.roleTreeData = []
  })
  if (res?.success && res.data && res.data.length > 0) {
    state.roleTreeData = listToTree(res.data)
  } else {
    state.roleTreeData = []
  }
}

// 打开对话框
const open = async (row: UserUpdateInput & UserUpdateInput) => {
  proxy.$modal.loading()

  await getOrgs()
  await getRoles()

  if (row.id > 0) {
    const res = await new UserApi().get({ id: row.id }).catch(() => {
      proxy.$modal.closeLoading()
    })

    if (res?.success) {
      state.form = res.data as UserAddInput & UserUpdateInput
    }
  } else {
    state.form = {
      orgIds: row.orgIds,
      orgId: row.orgId,
      roleIds: [] as number[],
      staff: {},
    } as UserAddInput & UserUpdateInput
  }

  proxy.$modal.closeLoading()
  state.showDialog = true
}

// 输入密码
const onInputPwd = (val: string) => {
  state.form.password = verifyCnAndSpace(val)
}

//手机号失去焦点
const onBlurMobile = () => {
  if (!state.form.userName && state.form.mobile && isMobile(state.form.mobile)) {
    state.form.userName = state.form.mobile
  }
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
    if (state.form.id != undefined && state.form.id > 0) {
      res = await new UserApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new UserApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }
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
