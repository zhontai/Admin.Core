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
      width="600px"
    >
      <el-form ref="formRef" :model="form" label-width="auto" @submit="onSure" v-zoom="'.my-dialog-model'">
        <el-row :gutter="20">
          <el-col :span="24" :xs="24">
            <el-form-item
              label="所属项目"
              prop="projectId"
              :rules="[{ required: true, validator: validatorSelect, message: '请选择所属项目', trigger: ['change'] }]"
              v-show="editItemIsShow(true, true)"
            >
              <el-select v-model="state.form.projectId" :empty-values="['', null, undefined, 0]">
                <el-option v-for="item in state.selectDevProjectListData" :key="item.id" :value="item.id" :label="item.name" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="24" :xs="24">
            <el-form-item
              label="模板组"
              prop="groupIds_Values"
              :rules="[{ required: true, message: '请选择模板组', trigger: ['change'] }]"
              v-show="editItemIsShow(true, true)"
            >
              <el-select multiple v-model="state.form.groupIds_Values">
                <el-option v-for="item in state.selectDevGroupListData" :key="item.id" :value="String(item.id)" :label="item.name" />
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

<script lang="ts" setup name="dev/dev-project-gen/form">
import { DevProjectGenAddInput, DevProjectGenUpdateInput, DevProjectGetListOutput, DevGroupGetListOutput } from '/@/api/dev/data-contracts'
import { DevProjectGenApi } from '/@/api/dev/DevProjectGen'
import { DevProjectApi } from '/@/api/dev/DevProject'
import { DevGroupApi } from '/@/api/dev/DevGroup'
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
  form: {} as DevProjectGenAddInput | DevProjectGenUpdateInput | any,
  selectDevProjectListData: [] as DevProjectGetListOutput[],
  selectDevGroupListData: [] as DevGroupGetListOutput[],
})
const { form } = toRefs(state)

// 打开对话框
const open = async (row: any = {}) => {
  getDevProjectList()
  getDevGroupList()

  if (row.id > 0) {
    const res = await new DevProjectGenApi().get({ id: row.id }, { loading: true }).catch(() => {
      proxy.$modal.closeLoading()
    })

    if (res?.success) {
      state.form = res.data as DevProjectGenUpdateInput
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
const getDevGroupList = async () => {
  const res = await new DevGroupApi().getList({}).catch(() => {
    state.selectDevGroupListData = []
  })
  state.selectDevGroupListData = res?.data || []
}

const defaultToAdd = (): DevProjectGenAddInput => {
  return {
    projectId: 0,
    groupIds: '',
  } as DevProjectGenAddInput
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
      res = await new DevProjectGenApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new DevProjectGenApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }
    state.sureLoading = false

    if (res?.success) {
      eventBus.emit('refreshDevProjectGen')
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
