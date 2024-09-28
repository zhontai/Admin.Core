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
          <el-col v-if="form.type === 2" :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="上级分组" prop="parentId" :rules="[{ required: true, message: '请选择上级分组', trigger: ['change'] }]">
              <el-tree-select
                v-model="form.parentId"
                :data="roleTreeData"
                node-key="id"
                :props="{ label: 'name' }"
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
            <el-form-item label="名称" prop="name" :rules="[{ required: true, message: '请输入名称', trigger: ['blur', 'change'] }]">
              <el-input v-model="form.name" clearable />
            </el-form-item>
          </el-col>
          <el-col v-if="form.type === 2" :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="编码" prop="code">
              <el-input v-model="form.code" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="排序">
              <el-input-number v-model="form.sort" />
            </el-form-item>
          </el-col>
          <el-col v-if="form.type === 2" :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="说明">
              <el-input v-model="form.description" clearable type="textarea" />
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

<script lang="ts" setup name="admin/role/form">
import { reactive, toRefs, ref, PropType } from 'vue'
import { RoleGetListOutput, RoleUpdateInput } from '/@/api/admin/data-contracts'
import { RoleApi } from '/@/api/admin/Role'
import { cloneDeep } from 'lodash-es'
import eventBus from '/@/utils/mitt'

defineProps({
  title: {
    type: String,
    default: '',
  },
  roleTreeData: {
    type: Array as PropType<RoleGetListOutput[]>,
    default: () => [],
  },
})

const formRef = ref()
const state = reactive({
  showDialog: false,
  sureLoading: false,
  form: {} as RoleUpdateInput,
})

const { form } = toRefs(state)

// 打开对话框
const open = async (row: RoleUpdateInput = { id: 0 }) => {
  let formData = cloneDeep(row) as RoleUpdateInput
  if (row.id > 0) {
    const res = await new RoleApi().get({ id: row.id }, { loading: true })

    if (res?.success) {
      formData = res.data as RoleUpdateInput
      formData.parentId = formData.parentId && formData.parentId > 0 ? formData.parentId : undefined
    } else {
      return
    }
  }

  state.form = formData
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
      res = await new RoleApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new RoleApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }

    state.sureLoading = false

    if (res?.success) {
      eventBus.emit('refreshRole')
      state.showDialog = false
    }
  })
}

defineExpose({
  open,
})
</script>
