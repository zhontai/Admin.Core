<template>
  <div>
    <el-dialog
      v-model="state.showDialog"
      :title="title"
      draggable
      destroy-on-close
      :close-on-click-modal="false"
      :close-on-press-escape="false"
      class="my-dialog-model"
      :overflow="true"
    >
      <el-form ref="formRef" :model="form" label-width="auto" @submit="onSure" v-zoom="'.my-dialog-model'">
        <el-row :gutter="20">
          <el-col :span="12" :xs="24">
            <el-form-item
              label="所属项目"
              prop="projectId"
              :rules="[{ required: true, validator: validatorSelect, message: '请选择所属项目', trigger: ['change'] }]"
              v-show="editItemIsShow(true, true)"
            >
              <el-select v-model="state.form.projectId" empty-values="['', null, undefined, 0]">
                <el-option v-for="item in state.selectDevProjectListData" :key="item.id" :value="item.id" :label="item.name" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12" :xs="24">
            <el-form-item label="是否启用" prop="isEnable" v-show="editItemIsShow(true, true)">
              <el-switch v-model="state.form.isEnable" />
            </el-form-item>
          </el-col>
          <el-col :span="12" :xs="24">
            <el-form-item
              label="模型名称"
              prop="name"
              :rules="[{ required: true, message: '请输入模型名称', trigger: ['blur', 'change'] }]"
              v-show="editItemIsShow(true, true)"
            >
              <el-input v-model="state.form.name" placeholder=""> </el-input>
            </el-form-item>
          </el-col>
          <el-col :span="12" :xs="24">
            <el-form-item
              label="模型编码"
              prop="code"
              :rules="[{ required: true, message: '请输入模型编码', trigger: ['blur', 'change'] }]"
              v-show="editItemIsShow(true, true)"
            >
              <el-input v-model="state.form.code" placeholder=""> </el-input>
            </el-form-item>
          </el-col>
          <el-col :span="24" :xs="24">
            <el-form-item label="备注" prop="remark" v-show="editItemIsShow(true, true)">
              <el-input type="textarea" v-model="state.form.remark" placeholder=""> </el-input>
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

<script lang="ts" setup name="dev/dev-project-model/form">
import { DevProjectModelAddInput, DevProjectModelUpdateInput, DevProjectGetListOutput } from '/@/api/dev/data-contracts'
import { DevProjectModelApi } from '/@/api/dev/DevProjectModel'
import { DevProjectApi } from '/@/api/dev/DevProject'
import eventBus from '/@/utils/mitt'
import { validatorSelect } from '/@/utils/validators'

defineProps({
  title: {
    type: String,
    default: '',
  },
})

const { proxy } = getCurrentInstance() as any

const formRef = ref()
const state = reactive({
  showDialog: false,
  sureLoading: false,
  form: {} as DevProjectModelAddInput | DevProjectModelUpdateInput | any,
  selectDevProjectListData: [] as DevProjectGetListOutput[],
})
const { form } = toRefs(state)

// 打开对话框
const open = async (row: any = {}) => {
  getDevProjectList()

  if (row.id > 0) {
    const res = await new DevProjectModelApi().get({ id: row.id }, { loading: true }).catch(() => {
      proxy.$modal.closeLoading()
    })

    if (res?.success) {
      state.form = res.data as DevProjectModelUpdateInput
    }
  } else {
    state.form = defaultToAdd()
  }
  state.showDialog = true
}

const getDevProjectList = async () => {
  const res = await new DevProjectApi().getList({}).catch(() => {
    state.selectDevProjectListData = []
  })
  state.selectDevProjectListData = res?.data || []
}

const defaultToAdd = (): DevProjectModelAddInput => {
  return {
    projectId: 0,
    name: '',
    code: '',
    isEnable: false,
    remark: null,
  } as DevProjectModelAddInput
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
      res = await new DevProjectModelApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new DevProjectModelApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }
    state.sureLoading = false

    if (res?.success) {
      eventBus.emit('refreshDevProjectModel')
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
