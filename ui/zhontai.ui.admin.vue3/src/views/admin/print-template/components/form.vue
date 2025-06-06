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
            <el-form-item label="模板名称" prop="name" :rules="[{ required: true, message: '请输入模板名称', trigger: ['blur', 'change'] }]">
              <el-input v-model="form.name" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="模板编码" prop="code" :rules="[{ required: true, message: '请输入模板编码', trigger: ['blur', 'change'] }]">
              <el-input v-model="form.code" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="排序">
              <el-input-number v-model="form.sort" class="w100" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12">
            <el-form-item label="启用">
              <el-switch v-model="form.enabled" />
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

<script lang="ts" setup name="admin/print-template-form">
import { reactive, toRefs, ref } from 'vue'
import { PrintTemplateUpdateInput, PrintTemplateGetOutput } from '/@/api/admin/data-contracts'
import { PrintTemplateApi } from '/@/api/admin/PrintTemplate'
import eventBus from '/@/utils/mitt'

defineProps({
  title: {
    type: String,
    default: '',
  },
})

const formRef = ref()
const state = reactive({
  showDialog: false,
  sureLoading: false,
  form: {
    enabled: true,
  } as PrintTemplateUpdateInput & PrintTemplateGetOutput,
})

const { form } = toRefs(state)

// 打开对话框
const open = async (row: any = {}) => {
  if (row.id > 0) {
    const res = await new PrintTemplateApi().get({ id: row.id }, { loading: true })

    if (res?.success) {
      let formData = res.data as PrintTemplateUpdateInput & PrintTemplateGetOutput
      state.form = formData
    }
  } else {
    state.form = {
      enabled: true,
    } as PrintTemplateUpdateInput & PrintTemplateGetOutput
  }

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

    if (state.form.id != undefined && state.form.id > 0) {
      res = await new PrintTemplateApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new PrintTemplateApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }

    state.sureLoading = false

    if (res?.success) {
      eventBus.emit('refreshPrintTemplate')
      state.showDialog = false
    }
  })
}

defineExpose({
  open,
})
</script>
