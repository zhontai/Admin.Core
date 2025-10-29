<template>
  <div>
    <el-dialog
      v-model="state.showDialog"
      :title="state.openConfig.title || title"
      draggable
      destroy-on-close
      :close-on-click-modal="false"
      :close-on-press-escape="false"
      class="my-dialog-model"
      :overflow="true"
      width="600px"
    >
      <el-form ref="formRef" :model="form" label-width="auto" @submit="onSure" v-zoom="'.my-dialog-model'">
        <el-row :gutter="20">
          <el-col>
            <el-form-item
              label="名称"
              prop="name"
              :rules="[{ required: true, message: '请输入名称', trigger: ['blur', 'change'] }]"
              v-show="editItemIsShow(true, true)"
            >
              <el-input v-model="state.form.name" placeholder=""> </el-input>
            </el-form-item>
          </el-col>
          <el-col>
            <el-form-item label="备注" prop="remark" v-show="editItemIsShow(true, true)">
              <el-input type="textarea" v-model="state.form.remark" placeholder="" rows="4"> </el-input>
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

<script lang="ts" setup name="dev/dev-group/form">
import { DevGroupAddInput, DevGroupUpdateInput } from '/@/api/dev/data-contracts'
import { DevGroupApi } from '/@/api/dev/DevGroup'

import eventBus from '/@/utils/mitt'

defineProps({
  title: {
    type: String,
    default: '',
  },
})

const { proxy } = getCurrentInstance() as any

const formRef = ref()
const state = reactive({
  openConfig: {
    title: '',
  },
  showDialog: false,
  sureLoading: false,
  form: {} as DevGroupAddInput | DevGroupUpdateInput | any,
})
const { form } = toRefs(state)

// 打开对话框
const open = async (row: any = {}, config: any = {}) => {
  if (config) {
    state.openConfig = Object.assign(state.openConfig, config)
  }

  if (row?.id > 0) {
    const res = await new DevGroupApi().get({ id: row.id }, { loading: true }).catch(() => {
      proxy.$modal.closeLoading()
    })

    if (res?.success) {
      state.form = res.data as DevGroupUpdateInput
    }
  } else {
    state.form = defaultToAdd()
  }
  state.showDialog = true
}

const defaultToAdd = (): DevGroupAddInput => {
  return {
    name: '',
    remark: null,
  } as DevGroupAddInput
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
      res = await new DevGroupApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new DevGroupApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }
    state.sureLoading = false

    if (res?.success) {
      eventBus.emit('refreshDevGroup')
      state.showDialog = false
    }
  })
}

const editItemIsShow = (add: Boolean, edit: Boolean): Boolean => {
  if (add && edit) return true
  let isEdit = state.form.id != undefined && state.form.id > 0
  if (add && !isEdit) return true
  if (edit && isEdit) return true
  return false
}

defineExpose({
  open,
})
</script>
