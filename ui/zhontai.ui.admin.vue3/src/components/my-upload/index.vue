<template>
  <el-upload ref="uploadRef" :action="uploadAction" :headers="uploadHeaders" :limit="1" class="avatar-uploader"
    :data="{ fileDirectory: '', fileReName: true }" drag :file-list="state.fileList" :before-upload="() => {
      state.token = storesUserInfo.getToken()
      state.uploadLoading = true
    }
      " :on-success="onSuccess" :on-error="onError" :on-remove="onRemove" :on-exceed="handleExceed">
    <img :src="state.url" v-if="state.url" class="avatar" />
    <el-icon v-else class="avatar-uploader-icon">
      <Plus />
    </el-icon>
  </el-upload>
</template>

<script lang="ts" setup name="my-upload">
import type { UploadInstance, UploadProps, UploadRawFile, UploadUserFile } from 'element-plus'
import { ref, reactive, computed, PropType, watch, getCurrentInstance } from 'vue'
import { Plus } from '@element-plus/icons-vue'
import { auth, auths, authAll } from '/@/utils/authFunction'
import { formatAxis } from '/@/utils/formatTime'
import pinia from '/@/stores/index'
import { storeToRefs } from 'pinia'
import { AxiosResponse } from 'axios'
import { useUserInfo } from '/@/stores/userInfo'
import { cloneDeep } from 'lodash-es'
const { proxy } = getCurrentInstance() as any


const uploadRef = ref()

const storesUserInfo = useUserInfo(pinia)
const props = defineProps({
  modelValue: {
    type: String as PropType<string | undefined | null>,
    default: () => '',
  },
})

const emits = defineEmits(['update:modelValue', 'input'])
console.log(props)
const state = reactive({
  url: props.modelValue,
  fileList: props.modelValue ? [{ url: props.modelValue, name: props.modelValue } as UploadUserFile] : [],
  uploadLoading: false,
  token: storesUserInfo.getToken(),
})

watch(
  () => state.url,
  () => {
    console.log(state.url)
    emits('update:modelValue', cloneDeep(state.url))
  },
  { deep: true }
)


// 上传请求头部
const uploadHeaders = computed(() => {
  return { Authorization: 'Bearer ' + state.token }
})

// 上传请求url
const uploadAction = computed(() => {
  return import.meta.env.VITE_API_URL + '/api/admin/file/upload-file'
})

// 上传成功
const onSuccess = (res: AxiosResponse) => {
  state.uploadLoading = false
  state.url = ''
  if (!res?.success) {
    if (res.msg) {
      proxy.$modal.msgError(res.msg)
    }
    return
  }
  state.url = res.data.linkUrl
  emits('input', cloneDeep(state.url))
}

// 上传失败
const onError = (error: any) => {
  state.uploadLoading = false
  let message = ''
  if (error.message) {
    try {
      message = JSON.parse(error.message)?.msg
    } catch (err) {
      message = error.message || ''
    }
  }
  if (message) proxy.$modal.msgError(message)
}
const handleExceed = (files: any) => {
  console.log(files)
  uploadRef.value?.clearFiles()
  const file = files[0] as UploadRawFile
  // file.uid = genFileId()
  uploadRef.value?.handleStart(file)
  uploadRef.value?.submit()
}
//移除
const onRemove = (file: any) => {
  state.url = ''
}
</script>

<style scoped lang="scss">
.avatar-uploader {
  width: 178px;

  .avatar {
    width: 150px;
  }
}
</style>