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
              label="所属模型"
              prop="modelId"
              :rules="[{ required: true, validator: validatorSelect, message: '请选择所属模型', trigger: ['change'] }]"
              v-show="editItemIsShow(true, true)"
            >
              <el-select clearable v-model="state.form.modelId" empty-values="['', null, undefined, 0]">
                <el-option v-for="item in state.selectDevProjectModelListData" :key="item.id" :value="item.id" :label="item.name" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12" :xs="24">
            <el-form-item
              label="字段属性"
              prop="properties"
              :rules="[{ required: true, message: '请选择所属模型', trigger: ['change'] }]"
              v-show="editItemIsShow(true, true)"
            >
              <el-select v-model="state.form.properties">
                <el-option v-for="item in state.dicts['fieldProperties']" :key="item.value" :value="item.value" :label="item.name" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12" :xs="24">
            <el-form-item
              label="字段名称"
              prop="name"
              :rules="[{ required: true, message: '请输入字段名称', trigger: ['blur', 'change'] }]"
              v-show="editItemIsShow(true, true)"
            >
              <el-input v-model="state.form.name"> </el-input>
            </el-form-item>
          </el-col>
          <el-col :span="12" :xs="24">
            <el-form-item
              label="字段编码"
              prop="code"
              :rules="[{ required: true, message: '请输入字段编码', trigger: ['blur', 'change'] }]"
              v-show="editItemIsShow(true, true)"
            >
              <el-input v-model="state.form.code"> </el-input>
            </el-form-item>
          </el-col>
          <el-col :span="12" :xs="24">
            <el-form-item
              label="字段类型"
              prop="dataType"
              :rules="[{ required: true, message: '请选择字段类型', trigger: ['change'] }]"
              v-show="editItemIsShow(true, true)"
            >
              <el-select clearable v-model="state.form.dataType">
                <el-option v-for="item in state.dicts['fieldType']" :key="item.value" :value="item.value" :label="item.name" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12" :xs="24">
            <el-form-item label="是否必填" prop="isRequired" v-show="editItemIsShow(true, true)">
              <el-switch v-model="state.form.isRequired" />
            </el-form-item>
          </el-col>
          <el-col :span="12" :xs="24">
            <el-form-item label="最小长度" prop="minLength" v-show="editItemIsShow(true, true)">
              <el-input-number v-model="state.form.minLength" placeholder=""> </el-input-number>
            </el-form-item>
          </el-col>
          <el-col :span="12" :xs="24">
            <el-form-item label="最大长度" prop="maxLength" v-show="editItemIsShow(true, true)">
              <el-input-number v-model="state.form.maxLength" placeholder=""> </el-input-number>
            </el-form-item>
          </el-col>
          <el-col :span="12" :xs="24">
            <el-form-item label="字段顺序" prop="sort" v-show="editItemIsShow(true, true)">
              <el-input-number v-model="state.form.sort" placeholder=""> </el-input-number>
            </el-form-item>
          </el-col>

          <el-col :span="24" :xs="24">
            <el-form-item label="字段描述" prop="description" v-show="editItemIsShow(true, true)">
              <el-input v-model="state.form.description" placeholder=""> </el-input>
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

<script lang="ts" setup name="dev/dev-project-model-field/form">
import { DevProjectModelFieldAddInput, DevProjectModelFieldUpdateInput, DevProjectModelGetListOutput } from '/@/api/dev/data-contracts'
import { DevProjectModelFieldApi } from '/@/api/dev/DevProjectModelField'
import { DevProjectModelApi } from '/@/api/dev/DevProjectModel'
import { DictApi } from '/@/api/admin/Dict'
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
  form: {} as DevProjectModelFieldAddInput | DevProjectModelFieldUpdateInput | any,
  selectDevProjectModelListData: [] as DevProjectModelGetListOutput[],
  //字典相关
  dicts: {
    fieldType: [] as any[],
    fieldProperties: [] as any[],
  },
})
const { form } = toRefs(state)

// 打开对话框
const open = async (row: any = {}) => {
  getDevProjectModelList()

  getDictsTree()

  if (row.id > 0) {
    const res = await new DevProjectModelFieldApi().get({ id: row.id }, { loading: true }).catch(() => {
      proxy.$modal.closeLoading()
    })

    if (res?.success) {
      state.form = res.data as DevProjectModelFieldUpdateInput
    }
  } else {
    state.form = defaultToAdd()
  }
  state.showDialog = true
}

const getDevProjectModelList = async () => {
  const res = await new DevProjectModelApi().getList({}).catch(() => {
    state.selectDevProjectModelListData = []
  })
  state.selectDevProjectModelListData = res?.data || []
}

//获取需要使用的字典树
const getDictsTree = async () => {
  let res = await new DictApi().getList(['fieldType', 'fieldProperties'])
  if (!res?.success) return
  state.dicts = res.data as any
}

const defaultToAdd = (): DevProjectModelFieldAddInput => {
  return {
    modelId: null,
    name: '',
    code: '',
    dataType: null,
    isRequired: null,
    maxLength: null,
    minLength: null,
    sort: 0,
    description: null,
    properties: '',
  } as DevProjectModelFieldAddInput
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
      res = await new DevProjectModelFieldApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new DevProjectModelFieldApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }
    state.sureLoading = false

    if (res?.success) {
      eventBus.emit('refreshDevProjectModelField')
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
