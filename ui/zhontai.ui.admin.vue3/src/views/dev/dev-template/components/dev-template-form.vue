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
    >
      <el-form ref="formRef" :model="form" label-width="auto" @submit="onSure" v-zoom="'.my-dialog-model'">
        <el-row :gutter="20">
          <el-col :span="12" :xs="24">
            <el-form-item
              label="模板分组"
              prop="groupId"
              :rules="[{ required: true, validator: validatorSelect, message: '请选择模板分组', trigger: ['change'] }]"
              v-show="editItemIsShow(true, true)"
            >
              <el-select v-model="state.form.groupId" empty-values="['', null, undefined, 0]">
                <el-option v-for="item in state.selectDevGroupListData" :key="item.id" :value="item.id" :label="item.name" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12" :xs="24">
            <el-form-item
              label="模板名称"
              prop="name"
              :rules="[{ required: true, message: '请输入模板名称', trigger: ['blur', 'change'] }]"
              v-show="editItemIsShow(true, true)"
            >
              <el-input v-model="state.form.name" placeholder=""> </el-input>
            </el-form-item>
          </el-col>
          <el-col :span="12" :xs="24">
            <el-form-item label="生成路径" prop="outTo" v-show="editItemIsShow(true, true)">
              <template #label>
                <div class="my-flex my-flex-items-center">
                  生成路径
                  <el-tooltip effect="dark">
                    <template #content>
                      使用 razor 视图引擎，模型定义应添加下面的代码，默认模板如下<br />
                      @{ var gen = Model as ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectGen.Dtos.DevProjectRazorRenderModel; }<br />
                      可以使用的数据 项目信息:gen.Project 模型信息：gen.Model 字段信息：gen.Fields<br />
                      代码区块：@{ //C#代码 }<br />
                      行内使用<br />
                      模型名称：@(gen.Model.Name)<br />
                      模型编码：@(gen.Model.Code)<br />
                    </template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <el-input v-model="state.form.outTo" placeholder=""> </el-input>
            </el-form-item>
          </el-col>
          <el-col :span="12" :xs="24">
            <el-form-item label="是否启用" prop="isEnable" v-show="editItemIsShow(true, true)">
              <el-switch v-model="state.form.isEnable" />
            </el-form-item>
          </el-col>
          <el-col :span="24" :xs="24">
            <el-form-item
              label="模板内容"
              prop="content"
              :rules="[{ required: true, message: '请输入模板内容', trigger: ['blur', 'change'] }]"
              v-show="editItemIsShow(true, true)"
            >
              <template #label>
                <div class="my-flex my-flex-items-center">
                  模板内容
                  <el-tooltip effect="dark">
                    <template #content>
                      使用 razor 视图引擎，模型定义应添加下面的代码，默认模板如下<br />
                      @{ var gen = Model as ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectGen.Dtos.DevProjectRazorRenderModel; }<br />
                      可以使用的数据 gen.Project gen.Model gen.Fields<br />
                      代码区块：@{ //C#代码 }<br />
                      行内使用<br />
                      模型名称：@(gen.Model.Name)<br />
                      模型编码：@(gen.Model.Code)<br />
                    </template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <el-input type="textarea" v-model="state.form.content" placeholder="" :autosize="{ minRows: 8, maxRows: 19 }"> </el-input>
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

<script lang="ts" setup name="dev/dev-template/form">
import { DevTemplateAddInput, DevTemplateUpdateInput, DevGroupGetListOutput } from '/@/api/dev/data-contracts'
import { DevTemplateApi } from '/@/api/dev/DevTemplate'
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
  openConfig: {
    title: '',
  },
  showDialog: false,
  sureLoading: false,
  form: {} as DevTemplateAddInput | DevTemplateUpdateInput | any,
  selectDevGroupListData: [] as DevGroupGetListOutput[],
})
const { form } = toRefs(state)

// 打开对话框
const open = async (row: any = {}, config: any) => {
  if (config) {
    state.openConfig = Object.assign(state.openConfig, config)
  }
  getDevGroupList()

  if (row?.id > 0) {
    const res = await new DevTemplateApi().get({ id: row.id }, { loading: true }).catch(() => {
      proxy.$modal.closeLoading()
    })

    if (res?.success) {
      state.form = res.data as DevTemplateUpdateInput
    }
  } else {
    state.form = defaultToAdd()
  }
  state.showDialog = true
}

const getDevGroupList = async () => {
  const res = await new DevGroupApi().getList({}).catch(() => {
    state.selectDevGroupListData = []
  })
  state.selectDevGroupListData = res?.data || []
}

const defaultToAdd = (): DevTemplateAddInput => {
  return {
    name: '',
    groupId: 0,
    outTo: null,
    isEnable: false,
    content: '',
  } as DevTemplateAddInput
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
      res = await new DevTemplateApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new DevTemplateApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }
    state.sureLoading = false

    if (res?.success) {
      eventBus.emit('refreshDevTemplate')
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
