<template>
  <div>
    <el-dialog
      v-model="state.showDialog"
      destroy-on-close
      :title="title"
      draggable
      :close-on-click-modal="false"
      :close-on-press-escape="false"
      width="830px"
    >
      <el-form :model="form" ref="formRef" label-width="auto">
        <el-row :gutter="35">
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="分类" prop="typeId" :rules="[{ required: true, message: '请选择分类', trigger: ['change'] }]">
              <el-tree-select
                v-model="form.typeId"
                placeholder="请选择分类"
                :data="state.msgTypeTreeData"
                node-key="id"
                :props="{ label: 'name' }"
                default-expand-all
                fit-input-width
                clearable
                filterable
                class="w100"
              />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="标题" prop="title" :rules="[{ required: true, message: '请输入标题', trigger: ['blur', 'change'] }]">
              <el-input v-model="form.title" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item
              label="内容"
              prop="content"
              :rules="[
                { required: true, message: '请输入内容', trigger: ['blur', 'change'] },
                { validator: testEditorContent, trigger: ['blur', 'change'] },
              ]"
            >
              <MyEditor ref="editorRef" v-model:model-value="form.content" @onBlur="onValidateContent" @onChange="onValidateContent"></MyEditor>
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

<script lang="ts" setup name="admin/msg/form">
import { reactive, toRefs, ref, defineAsyncComponent, getCurrentInstance } from 'vue'
import { MsgUpdateInput, MsgTypeGetListOutput } from '/@/api/admin/data-contracts'
import { MsgApi } from '/@/api/admin/Msg'
import { cloneDeep } from 'lodash-es'
import eventBus from '/@/utils/mitt'
import { MsgTypeApi } from '/@/api/admin/MsgType'
import { listToTree } from '/@/utils/tree'

const MyEditor = defineAsyncComponent(() => import('/@/components/my-editor/index.vue'))

defineProps({
  title: {
    type: String,
    default: '',
  },
})

const formRef = ref()
const editorRef = ref()
const state = reactive({
  showDialog: false,
  sureLoading: false,
  form: { content: '' } as MsgUpdateInput,
  msgTypeTreeData: [] as MsgTypeGetListOutput[],
  v: null,
})

const { proxy } = getCurrentInstance() as any

const { form } = toRefs(state)

const testEditorContent = (rule: any, value: any, callback: any) => {
  if (!value) {
    callback()
  }
  if (editorRef.value.isEmpty()) {
    callback(new Error('请输入内容'))
  } else {
    callback()
  }
}

const onValidateContent = () => {
  formRef.value.validateField('content')
}

const getMsgTypes = async () => {
  const res = await new MsgTypeApi().getList().catch(() => {
    state.msgTypeTreeData = []
  })
  if (res?.success && res.data && res.data.length > 0) {
    state.msgTypeTreeData = listToTree(res.data)
  } else {
    state.msgTypeTreeData = []
  }
}

// 打开对话框
const open = async (row: MsgUpdateInput = { id: 0 }) => {
  proxy.$modal.loading()

  await getMsgTypes()

  let formData = cloneDeep(row) as MsgUpdateInput
  if (row.id > 0) {
    const res = await new MsgApi().get({ id: row.id }).catch(() => {
      proxy.$modal.closeLoading()
    })

    if (res?.success) {
      formData = res.data as MsgUpdateInput
      formData.typeId = formData.typeId && formData.typeId > 0 ? formData.typeId : undefined
    }
  }

  state.form = formData

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
    if (state.form.id != undefined && state.form.id > 0) {
      res = await new MsgApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new MsgApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }

    state.sureLoading = false

    if (res?.success) {
      eventBus.emit('refreshMsg')
      state.showDialog = false
    }
  })
}

defineExpose({
  open,
})
</script>

<style lang="scss" scoped></style>
