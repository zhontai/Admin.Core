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
      <el-form :model="form" ref="formRef" size="default" label-width="110px">
        <el-row :gutter="35">
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="任务标题" prop="topic" :rules="[{ required: true, message: '请输入任务标题', trigger: ['blur', 'change'] }]">
              <el-input v-model="form.topic" clearable />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item label="任务参数" prop="body">
              <template #label>
                <div class="my-flex-y-center">
                  任务参数<el-tooltip effect="dark" placement="top" hide-after="0">
                    <template #content>设置Json数据</template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <el-input v-model="form.body" clearable type="textarea" rows="6" />
              <el-link icon="ele-Edit" :underline="false" style="line-height: normal; margin-top: 5px" @click="onOpenJson">Json</el-link>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-form-item prop="alarmEmail">
              <template #label>
                <div class="my-flex-y-center">
                  报警邮件<el-tooltip effect="dark" placement="top" hide-after="0">
                    <template #content>多个邮件地址用逗号分隔</template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <el-input v-model="form.alarmEmail" clearable placeholder="多个邮件地址用逗号分隔" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="12" :lg="12" :xl="12">
            <el-form-item prop="failRetryCount">
              <template #label>
                <div class="my-flex-y-center">失败重试次数</div>
              </template>
              <el-input-number v-model="form.failRetryCount" :min="0" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="12" :lg="12" :xl="12">
            <el-form-item prop="failRetryCount">
              <template #label>
                <div class="my-flex-y-center">重试间隔（秒）</div>
              </template>
              <el-input-number v-model="form.failRetryInterval" :min="0" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="12" :lg="12" :xl="12">
            <el-form-item prop="round" :rules="[{ required: true, message: '请输入执行轮数', trigger: ['blur', 'change'] }]">
              <template #label>
                <div class="my-flex-y-center">
                  执行轮次<el-tooltip effect="dark" placement="top" hide-after="0">
                    <template #content>循环多少次，-1为无限循环</template>
                    <SvgIcon name="ele-InfoFilled" class="ml5" />
                  </el-tooltip>
                </div>
              </template>
              <el-input-number v-model="form.round" :min="-1" :disabled="form.interval === 21" />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="12" :lg="12" :xl="12">
            <el-form-item label="定时类型" prop="interval" :rules="[{ required: true, message: '请选择定时类型', trigger: ['change'] }]">
              <el-select v-model="form.interval" style="width: 150px" @change="onIntervalChange">
                <el-option v-for="item in state.intervals" :key="item.value" :label="item.label" :value="item.value" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
            <el-space fill class="w100">
              <el-form-item
                label="定时参数"
                prop="intervalArgument"
                :rules="[{ required: true, message: '请输入定时参数', trigger: ['blur', 'change'] }]"
              >
                <el-input v-model="form.intervalArgument" clearable>
                  <template #append v-if="form.interval === 21">
                    <el-button icon="ele-Clock" @click="onOpenCronDialog" />
                  </template>
                </el-input>
              </el-form-item>
              <el-alert v-if="form.interval === 1" type="info" :closable="false">
                设置 5 则每5秒触发，执行N次
                <br />
                设置 5, 5, 10, 10, 60, 60 则每次按不同的间隔秒数触发，执行6次
              </el-alert>
              <el-alert v-else-if="form.interval === 11" type="info" :closable="false"> 设置 08:00:00 则每天 08:00:00 触发，执行N次 </el-alert>
              <el-alert v-else-if="form.interval === 12" type="info" :closable="false">
                设置 1:08:00:00 则每周一 08:00:00 触发
                <br />
                设置 0:08:00:00 则每周日 08:00:00 触发
              </el-alert>
              <el-alert v-else-if="form.interval === 13" type="info" :closable="false">
                设置 1:08:00:00 则每月1日 08:00:00 触发
                <br />
                设置 -1:08:00:00 则每月最后一日 08:00:00 触发
              </el-alert>
              <el-alert v-else-if="form.interval === 21" type="info" :closable="false">
                设置 0/10 * * * * ? 则从0秒开始每10秒执行一次
                <br />
                <pre style="line-height: 20px">
new FreeSchedulerBuilder()
...
.UseCustomInterval(task =>
{
    //利用 cron 功能库解析 task.IntervalArgument 得到下一次执行时间
    //与当前时间相减，得到 TimeSpan，若返回 null 则任务完成
    return TimeSpan.FromSeconds(5);
})
.Build();
                  </pre
                >
              </el-alert>
            </el-space>
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

    <MyCronDialog ref="myCronDialogRef" @fill="onFillCron"></MyCronDialog>

    <JsonEditorDialog ref="jsonEditorDialogRef" @sure="onSureArgs"></JsonEditorDialog>
  </div>
</template>

<script lang="ts" setup name="admin/task/form">
import { reactive, toRefs, ref, defineAsyncComponent } from 'vue'
import { TaskUpdateInput } from '/@/api/admin/data-contracts'
import { TaskApi } from '/@/api/admin/Task'
import { cloneDeep } from 'lodash-es'
import eventBus from '/@/utils/mitt'

const MyCronDialog = defineAsyncComponent(() => import('/@/components/my-cron/dialog.vue'))

const JsonEditorDialog = defineAsyncComponent(() => import('./json-editor-dialog.vue'))

defineProps({
  title: {
    type: String,
    default: '',
  },
})

const formRef = ref()
const myCronDialogRef = ref()
const jsonEditorDialogRef = ref()
const state = reactive({
  showDialog: false,
  sureLoading: false,
  form: {} as TaskUpdateInput,
  intervals: [
    { label: '按秒触发', value: 1 },
    { label: '每天', value: 11 },
    { label: '每周几', value: 12 },
    { label: '每月第几日', value: 13 },
    { label: 'Cron表达式', value: 21 },
  ],
})

const { form } = toRefs(state)

//确定Cron表达式
const onFillCron = (value: any) => {
  form.value.intervalArgument = value
}

//确定任务参数
const onSureArgs = (task: any) => {
  form.value.topic = task.topic
  form.value.body = task.body
}

// 打开对话框
const open = async (row: TaskUpdateInput = { id: '' }) => {
  let formData = cloneDeep(row) as TaskUpdateInput
  if (row.id) {
    const res = await new TaskApi().get({ id: row.id }, { loading: true })

    if (res?.success) {
      formData = res.data as TaskUpdateInput
    }
  }

  state.form = formData
  state.showDialog = true
}

//打开Cron对话框
const onOpenCronDialog = () => {
  myCronDialogRef.value.open(state.form.intervalArgument)
}

//打开Json对话框
const onOpenJson = () => {
  jsonEditorDialogRef.value.open(state.form)
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
    if (state.form.id) {
      res = await new TaskApi().update(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    } else {
      res = await new TaskApi().add(state.form, { showSuccessMessage: true }).catch(() => {
        state.sureLoading = false
      })
    }

    state.sureLoading = false

    if (res?.success) {
      eventBus.emit('refreshTask')
      state.showDialog = false
    }
  })
}

const onIntervalChange = () => {
  state.form.intervalArgument = ''
  if (state.form.interval === 21) state.form.round = -1
}

defineExpose({
  open,
})
</script>

<style scoped lang="scss">
.el-alert {
  border-width: 0px !important;
  margin-left: 110px;
  margin-top: 10px;
}
</style>
