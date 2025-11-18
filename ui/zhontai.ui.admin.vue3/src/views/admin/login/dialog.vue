<template>
  <el-dialog
    v-model="showLoginDialog"
    :title="$t('欢迎登录中台账号')"
    draggable
    append-to-body
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    width="480px"
    v-bind="$attrs"
    class="login-dialog"
  >
    <div class="my-flex my-flex-center" style="min-height: 410px">
      <div class="w100 login-box">
        <LoginForm :show-reg="false" :is-popup="true" @ok="onOk" />
      </div>
    </div>
  </el-dialog>
</template>

<script lang="ts" setup name="login/dialog">
import { ElMessage } from 'element-plus'
import { useI18n } from 'vue-i18n'
import { useUserInfo } from '/@/stores/userInfo'

const LoginForm = defineAsyncComponent(() => import('./form.vue'))

const { t } = useI18n()

const emits = defineEmits(['ok'])

const storesUserInfo = useUserInfo()
const { userInfos } = storeToRefs(storesUserInfo)

// 设置锁屏时组件显示隐藏
const showLoginDialog = computed(() => {
  return userInfos.value.showLoginDialog
})

//登录成功
const onOk = () => {
  ElMessage.success(t('登录成功'))
  userInfos.value.showLoginDialog = false
  emits('ok')
}

//打开窗口
const open = () => {
  userInfos.value.showLoginDialog = true
}

defineExpose({
  open,
})
</script>

<style scoped lang="scss">
.login-box {
  padding: 20px;
  position: relative;
}
</style>
<style lang="scss">
.login-dialog {
  .el-dialog__body {
    padding: 0px !important;
  }
}
</style>
