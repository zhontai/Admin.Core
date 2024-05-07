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
      <el-form :model="form" ref="formRef" size="default" label-width="80px">
        <el-row :gutter="35">
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="上级分组">
              <el-tree-select
                v-model="form.parentId"
                :data="permissionTreeData"
                node-key="id"
                check-strictly
                default-expand-all
                render-after-expand
                fit-input-width
                clearable
                class="w100"
              />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="名称" prop="label" :rules="[{ required: true, message: '请输入名称', trigger: ['blur', 'change'] }]">
              <el-input v-model="form.label" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="路由地址" prop="path" :rules="[{ required: true, message: '请输入路由地址', trigger: ['blur', 'change'] }]">
              <el-input v-model="form.path" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="重定向">
              <el-input v-model="form.redirect" clearable placeholder="重定向地址" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="图标" prop="icon">
              <my-select-icon v-model="form.icon" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="排序">
              <el-input-number v-model="form.sort" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="启用">
              <el-switch v-model="form.enabled" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="展开">
              <el-switch v-model="form.opened" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="隐藏">
              <el-switch v-model="form.hidden" />
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

<script lang="ts" setup name="admin/permission/permission-group-form">
import { reactive, toRefs, getCurrentInstance, ref, PropType, defineAsyncComponent } from 'vue'
import { PermissionListOutput, PermissionUpdateGroupInput } from '/@/api/admin/data-contracts'
import { PermissionApi } from '/@/api/admin/Permission'
import eventBus from '/@/utils/mitt'

// 引入组件
const MySelectIcon = defineAsyncComponent(() => import('/@/components/my-select-icon/index.vue'))

defineProps({
  title: {
    type: String,
    default: '',
  },
  permissionTreeData: {
    type: Array as PropType<PermissionListOutput[]>,
    default: () => [],
  },
})
const { proxy } = getCurrentInstance() as any

const formRef = ref()
const state = reactive({
  showDialog: false,
  sureLoading: false,
  form: { enabled: true, opened: true } as PermissionUpdateGroupInput,
})
const { form } = toRefs(state)

// 打开对话框
const open = async (row: any = {}) => {
  proxy.$modal.loading()
  if (row.id > 0) {
    const res = await new PermissionApi().getGroup({ id: row.id }).catch(() => {
      proxy.$modal.closeLoading()
    })

    if (res?.success) {
      let formData = res.data as PermissionUpdateGroupInput
      formData.parentId = formData.parentId && formData.parentId > 0 ? formData.parentId : undefined
      state.form = formData
    }
  } else {
    state.form = { enabled: true, opened: true, icon: 'ele-Memo', parentId: row.parentId } as PermissionUpdateGroupInput
  }
  proxy.$modal.closeLoading()
  state.showDialog = true
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
      res = await new PermissionApi().updateGroup(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new PermissionApi().addGroup(state.form, { showSuccessMessage: true }).catch(() => {
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
