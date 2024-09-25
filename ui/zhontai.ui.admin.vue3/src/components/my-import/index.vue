<template>
  <div class="my-import">
    <el-dialog
      v-model="state.showDialog"
      destroy-on-close
      draggable
      :close-on-click-modal="false"
      :close-on-press-escape="false"
      width="520px"
      v-bind="$attrs"
    >
      <el-steps :active="state.step" simple>
        <el-step title="上传文件" icon="ele-Upload" />
        <el-step title="导入数据" icon="ele-Download" />
        <el-step title="完成导入" icon="ele-CircleCheck" />
      </el-steps>
      <div v-show="state.step === 1">
        <div class="my-import__step">
          <div class="my-import__title mt20">一、请按照模板的格式准备要导入的数据</div>
          <div class="my-import__content">
            <div class="my-import__download">
              <el-button type="primary" link :loading="state.download.loadingTemplate" @click="onDownloadTemplate">下载模板</el-button>
            </div>
            <div class="my-import__notice">
              <div>注意事项:</div>
              <div>1、表头名称不可更改，表头行不能删除</div>
              <div>2、表头列顺序可以调整，不需要的列可以删减</div>
              <div v-if="requiredColumns">3、其中{{ requiredColumns }}为必埴项，必须保留</div>
              <div>{{ requiredColumns ? 4 : 3 }}、导入文件请不要超过 1 MB</div>
            </div>
          </div>
        </div>
        <div class="my-import__step">
          <div class="my-import__title">二、请选择数据重复时的操作方式</div>
          <div class="my-import__content mt10">
            <el-select v-model="state.data.duplicateAction" style="width: 220px" :size="'mini'">
              <el-option v-for="status in state.duplicateActionList" :key="status.name" :label="status.name" :value="status.value" />
            </el-select>
            <div class="mt6" style="font-size: 12px" v-if="uniqueRules">查重规则: {{ uniqueRules }}</div>
          </div>
        </div>
        <div class="my-import__step">
          <div class="my-import__title">三、请选择需要导入的 excel.xlsx 文件</div>
          <div class="my-import__content mt10">
            <el-upload
              ref="fileUploadRef"
              limit="1"
              :action="model.action"
              :headers="uploadHeaders"
              :data="getUploadData"
              :auto-upload="false"
              :on-exceed="onExceed"
              :on-progress="onProgress"
              :on-success="onSuccess"
              :on-error="onError"
              :before-upload="onBeforeUpload"
            >
              <template #trigger>
                <el-button icon="ele-Paperclip" text bg>选择文件</el-button>
              </template>
            </el-upload>
            <!-- <div class="mt10">请选择文件编码</div>
          <div class="mt10">
            <el-select v-model="state.data.fileEncoding" style="width: 220px" :size="'mini'">
              <el-option v-for="status in state.fileEncodingList" :key="status.name" :label="status.name" :value="status.value" />
            </el-select>
          </div> -->
          </div>
        </div>
      </div>

      <div v-show="state.step !== 1" class="mt20 pb20">
        <el-progress :text-inside="true" :stroke-width="26" :percentage="state.percent" :stroke-linecap="'square'" status="success" />

        <div v-if="state.step === 3 && !state.showErrorMark" class="mt10">
          导入完成，共 {{ state.importResult.total }} 条 <el-text type="warning">{{ result }}</el-text>
        </div>

        <div class="mt10" v-if="state.showErrorMark">
          <el-button type="danger" link :loading="state.download.loadingErrorMark" @click="onDownloadErrorMark">下载错误标记文件</el-button>
        </div>
      </div>

      <template #footer>
        <span class="dialog-footer">
          <el-button @click="onCancel" size="default">{{ state.step === 1 ? '取 消' : '关闭' }}</el-button>
          <el-button v-if="state.step === 1" type="primary" @click="onUpload" size="default" :loading="state.uploading">开始导入</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup name="my-import">
import { reactive, computed, ref } from 'vue'
import eventBus from '/@/utils/mitt'
import { useUserInfo } from '/@/stores/userInfo'
import type { UploadProps, UploadInstance, UploadUserFile, UploadRawFile, UploadProgressEvent, UploadFile } from 'element-plus'
import { ElMessage, genFileId, ElNotification } from 'element-plus'
import { DictApi } from '/@/api/admin/Dict'
import dayjs from 'dayjs'
import { cloneDeep, merge } from 'lodash-es'
import { plus } from '/@/utils/digit'

const model = defineModel({ type: Object })

const fileUploadRef = ref<UploadInstance>()

const storesUserInfo = useUserInfo()

const initState = {
  token: storesUserInfo.getToken(),
  showDialog: false,
  data: {
    fileId: '',
    duplicateAction: model.value.duplicateAction,
    //fileEncoding: model.value.fileEncoding,
  },
  duplicateActionList: [
    { name: '不导入', value: 0 },
    { name: '直接覆盖', value: 1 },
  ],
  download: {
    loadingTemplate: false,
    loadingErrorMark: false,
  },
  showErrorMark: false,
  uploading: false,
  step: 1,
  fileName: '',
  percent: 0,
  importResult: {
    total: 0,
    insertCount: 0,
    updateCount: 0,
  },
  // fileEncodingList: [{ name: 'GB18030(简体中文)', value: 0 }],
}
const state = reactive(cloneDeep(initState))

const uploadHeaders = computed(() => {
  return { Authorization: 'Bearer ' + state.token }
})

const uniqueRules = computed(() => {
  return model.value.uniqueRules?.map((rule: string) => '【' + rule + '】')?.join('、')
})

const result = computed(() => {
  const total = plus(state.importResult.insertCount, state.importResult.updateCount)

  if (state.importResult.total === 0 || total === 0) {
    return '无一成功'
  }

  if (state.importResult.total === total) {
    return '全部成功'
  }

  return `成功 ${total} 条`
})

const requiredColumns = computed(() => {
  return model.value.requiredColumns?.join('、')
})

const getUploadData = () => {
  state.token = storesUserInfo.getToken()
  return state.data
}

//超出限制
const onExceed = (files: File[], uploadFiles: UploadUserFile[]) => {
  fileUploadRef.value!.clearFiles()
  const file = files[0] as UploadRawFile
  file.uid = genFileId()
  fileUploadRef.value!.handleStart(file)
}

const onBeforeUpload: UploadProps['beforeUpload'] = (rawFile) => {
  state.data.fileId = rawFile.uid + ''
  state.fileName = rawFile.name
  state.step = 2
  state.uploading = true
}

//上传失败
const onError: UploadProps['onError'] = (error) => {
  state.showErrorMark = true
  let message = ''
  if (error.message) {
    try {
      message = JSON.parse(error.message)?.msg
    } catch (err) {
      message = error.message || ''
    }
  }
  if (message) {
    ElMessage({
      message: message,
      type: 'error',
    })
  }

  state.step = 3
  state.uploading = false
}

// 上传中
const onProgress: UploadProps['onProgress'] = (evt: UploadProgressEvent, uploadFile: UploadFile) => {
  state.percent = evt.percent
  //uploadFile.percentage
}

// 上传成功
const onSuccess: UploadProps['onSuccess'] = (response) => {
  if (response?.success) {
    state.importResult = response.data
    ElMessage({
      message: '导入成功',
      type: 'success',
    })
    eventBus.emit('refreshDict')
  } else {
    state.showErrorMark = true
    if (response?.msg) {
      ElNotification({
        title: '提示',
        message: response?.msg?.replace(/[\r\n]+/g, '<br/>'),
        type: 'error',
        duration: 0,
        dangerouslyUseHTMLString: true,
        position: 'bottom-right',
      })
    }
  }

  state.step = 3
  state.uploading = false
}

//下载模板
const onDownloadTemplate = async () => {
  state.download.loadingTemplate = true

  await new DictApi()
    .downloadTemplate({ format: 'blob', returnResponse: true })
    .then((res: any) => {
      const contentDisposition = res.headers['content-disposition']
      const matchs = /filename="?([^;"]+)/i.exec(contentDisposition)
      let fileName = ''
      if (matchs && matchs.length > 1) {
        fileName = decodeURIComponent(matchs[1])
      } else {
        fileName = `模板文件${dayjs().format('YYYYMMDDHHmmss')}.xlsx`
      }
      const a = document.createElement('a')
      a.download = fileName
      a.href = URL.createObjectURL(res.data as Blob)
      a.click()
      URL.revokeObjectURL(a.href)
    })
    .finally(() => {
      state.download.loadingTemplate = false
    })
}

//下载错误标记文件
const onDownloadErrorMark = async () => {
  state.download.loadingErrorMark = true

  await new DictApi()
    .downloadErrorMark({ fileId: state.data.fileId, fileName: state.fileName }, { format: 'blob', returnResponse: true, showErrorMessage: false })
    .then((res: any) => {
      state.showErrorMark = false
      const contentDisposition = res.headers['content-disposition']
      const matchs = /filename="?([^;"]+)/i.exec(contentDisposition)
      let fileName = ''
      if (matchs && matchs.length > 1) {
        fileName = decodeURIComponent(matchs[1])
      } else {
        fileName = `错误标记文件${dayjs().format('YYYYMMDDHHmmss')}.xlsx`
      }
      const a = document.createElement('a')
      a.download = fileName
      a.href = URL.createObjectURL(res.data as Blob)
      a.click()
      URL.revokeObjectURL(a.href)
    })
    .catch((error) => {
      if (error.response && error.response.data instanceof Blob) {
        const reader = new FileReader()
        reader.onload = function (e) {
          try {
            if (e.target && e.target.result) {
              const res = JSON.parse(e.target.result as string)
              if (res.msg) {
                ElMessage({
                  message: res.msg,
                  type: 'error',
                })
              }
            }
          } catch (parseError) {
            console.error('Failed to parse JSON from Blob:', parseError)
          }
        }
        reader.readAsText(error.response.data)
      } else {
        ElMessage({
          message: '请重新导入数据，再下载错误标记文件',
          type: 'error',
        })
      }
    })
    .finally(() => {
      state.download.loadingErrorMark = false
    })
}

// 打开对话框
const open = async (row: any = {}) => {
  merge(state, cloneDeep(initState))
  state.showDialog = true
}

// 取消
const onCancel = () => {
  state.showDialog = false
}

// 开始导入
const onUpload = () => {
  fileUploadRef.value!.submit()
}

defineExpose({
  open,
})
</script>

<style scoped lang="scss">
.my-import {
  &__step {
    margin-bottom: 20px;
  }
  &__content {
    padding-left: 28px;
  }
  &__download {
    margin-top: 4px;
  }
  &__notice {
    font-size: 12px;
    color: var(--el-color-info);
    margin-top: 10px;
    div {
      padding: 4px 0px;
    }
  }
  :deep() {
    .el-steps--simple {
      padding: 12px;
    }
    .el-step.is-simple .el-step__arrow:after,
    .el-step.is-simple .el-step__arrow:before {
      height: 11px;
    }
    .el-step.is-simple .el-step__arrow:before {
      transform: rotate(-45deg) translateY(-3px);
    }
    .el-step.is-simple .el-step__arrow:after {
      transform: rotate(45deg) translateY(3px);
    }

    .el-progress-bar__outer,
    .el-progress-bar__inner {
      border-radius: 0px;
    }
  }
}
</style>
