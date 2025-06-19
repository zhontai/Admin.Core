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
      <el-form ref="formRef" :model="form" label-width="80px">
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
            <el-form-item label="账号" prop="userName" :rules="[{ required: true, message: '请输入账号', trigger: ['blur', 'change'] }]">
              <el-input v-model="form.userName" autocomplete="off" />
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
          <el-button @click="onCancel">取 消</el-button>
          <el-button type="primary" @click="onSure" :loading="state.sureLoading">确 定</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup name="admin/user/form">
import { reactive, toRefs, getCurrentInstance, ref, defineAsyncComponent, computed } from 'vue'
import { UserAddInput, UserUpdateInput, RoleGetListOutput } from '/@/api/admin/data-contracts'
import { UserApi } from '/@/api/admin/User'
import { RoleApi } from '/@/api/admin/Role'
import { listToTree } from '/@/utils/tree'
import { isMobile, testMobile, testEmail } from '/@/utils/test'
import eventBus from '/@/utils/mitt'
import { FormInstance } from 'element-plus'
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

const formRef = ref<FormInstance>()
const state = reactive({
  showDialog: false,
  sureLoading: false,
  form: {
    roleIds: [] as any,
  } as UserAddInput & UserUpdateInput,
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
      roleIds: [] as number[],
      staff: {},
    } as UserAddInput & UserUpdateInput
  }

  proxy.$modal.closeLoading()
  state.showDialog = true
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
