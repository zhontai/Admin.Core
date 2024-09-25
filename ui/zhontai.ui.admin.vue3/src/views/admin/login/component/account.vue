<template>
  <div>
    <el-form ref="formRef" :model="state.ruleForm" size="large" class="login-content-form">
      <div class="login-title">
        <span class="login-title-showy">{{ getDescByValue(AccountType, state.ruleForm.accountType as number) }}密码</span>登录
      </div>
      <el-form-item
        v-if="state.ruleForm.accountType == AccountType.UserName.value"
        class="login-animation1"
        prop="userName"
        :rules="[{ required: true, message: '请输入账号', trigger: ['blur', 'change'] }]"
      >
        <el-input
          text
          :placeholder="$t('message.account.accountPlaceholder1')"
          v-model="state.ruleForm.userName"
          clearable
          autocomplete="off"
          @keyup.enter="onSignIn"
        >
          <template #prefix>
            <el-icon class="el-input__icon"><ele-User /></el-icon>
          </template>
        </el-input>
      </el-form-item>
      <el-form-item
        v-if="state.ruleForm.accountType == AccountType.Mobile.value"
        class="login-animation1"
        prop="mobile"
        :rules="[
          { required: true, message: '请输入手机号', trigger: ['blur', 'change'] },
          { validator: testMobile, trigger: ['blur', 'change'] },
        ]"
      >
        <el-input
          ref="phoneRef"
          text
          :placeholder="$t('message.mobile.placeholder1')"
          maxlength="11"
          v-model="state.ruleForm.mobile"
          clearable
          autocomplete="off"
          @keyup.enter="onSignIn"
        >
          <template #prefix>
            <el-icon class="el-input__icon"><ele-Iphone /></el-icon>
          </template>
        </el-input>
      </el-form-item>
      <el-form-item
        v-if="state.ruleForm.accountType == AccountType.Email.value"
        class="login-animation1"
        prop="email"
        :rules="[
          { required: true, message: '请输入邮箱地址', trigger: ['blur', 'change'] },
          { validator: testEmail, trigger: ['blur', 'change'] },
        ]"
      >
        <el-input
          ref="phoneRef"
          text
          :placeholder="$t('message.email.placeholder1')"
          v-model="state.ruleForm.email"
          clearable
          autocomplete="off"
          @keyup.enter="onSignIn"
        >
          <template #prefix>
            <el-icon class="el-input__icon"><ele-Promotion /></el-icon>
          </template>
        </el-input>
      </el-form-item>
      <el-form-item class="login-animation2" prop="password" :rules="[{ required: true, message: '请输入密码', trigger: ['blur', 'change'] }]">
        <el-input
          :placeholder="$t('message.account.accountPlaceholder2')"
          v-model="state.ruleForm.password"
          show-password
          autocomplete="off"
          @keyup.enter="onSignIn"
        >
          <template #prefix>
            <el-icon class="el-input__icon"><ele-Unlock /></el-icon>
          </template>
        </el-input>
      </el-form-item>
      <el-form-item class="login-animation4 mb12">
        <el-button
          type="primary"
          class="login-content-submit"
          round
          v-waves
          @click="onSignIn"
          :disabled="state.disabled.signIn"
          :loading="state.loading.signIn"
        >
          <span>{{ $t('message.account.accountBtnText') }}</span>
        </el-button>
      </el-form-item>
      <div
        class="login-animation4 my-flex f12 mt10"
        :class="state.ruleForm.accountType == AccountType.UserName.value ? 'my-flex-end' : 'my-flex-between'"
      >
        <el-link
          v-if="state.ruleForm.accountType == AccountType.Mobile.value"
          :underline="false"
          type="primary"
          class="f12"
          @click="loginComponentName = ComponentType.Mobile.name"
          >手机验证码登录</el-link
        >
        <el-link
          v-if="state.ruleForm.accountType == AccountType.Email.value"
          :underline="false"
          type="primary"
          class="f12"
          @click="loginComponentName = ComponentType.Email.name"
          >邮箱验证码登录</el-link
        >
        <el-link :underline="false" type="primary" class="f12" @click="onForgotPassword">忘记密码</el-link>
      </div>
    </el-form>
    <MyCaptchaDialog ref="myCaptchaDialogRef" v-model="state.showDialog" @ok="onOk" />
  </div>
</template>

<script setup lang="ts" name="loginAccount">
import { reactive, computed, ref, defineAsyncComponent, watchEffect } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import { useI18n } from 'vue-i18n'
import { sm4 } from 'sm-crypto-v2'
import { initBackEndControlRoutes } from '/@/router/backEnd'
import { Session } from '/@/utils/storage'
import { formatAxis } from '/@/utils/formatTime'
import { NextLoading } from '/@/utils/loading'
import { AuthApi } from '/@/api/admin/Auth'
import { AuthLoginInput, AccountType as AccountTypeEnum } from '/@/api/admin/data-contracts'
import { useUserInfo } from '/@/stores/userInfo'
import { cloneDeep } from 'lodash-es'
import { testMobile, testEmail } from '/@/utils/test'
import { AccountType } from '/@/api/admin/enum-contracts'
import { getDescByValue } from '/@/utils/enum'
import { ComponentType } from '/@/api/admin.extend/enum-contracts'

const MyCaptchaDialog = defineAsyncComponent(() => import('/@/components/my-captcha/dialog.vue'))
const loginComponentName = defineModel('loginComponentName', { type: String })
const accountType = defineModel('accountType', { type: Number, default: AccountType.UserName.value })
const isChangePassword = defineModel('isChangePassword', { type: Boolean, default: false })
const changePasswordComponentName = defineModel('changePasswordComponentName', { type: String })

// 定义变量内容
const { t } = useI18n()
// const storesThemeConfig = useThemeConfig()
// const { themeConfig } = storeToRefs(storesThemeConfig)
const route = useRoute()
const router = useRouter()
const formRef = ref()
const myCaptchaDialogRef = ref()

const state = reactive({
  showDialog: false,
  ruleForm: {
    userName: '',
    mobile: '',
    email: '',
    accountType: accountType.value,
    password: '',
    captchaId: '',
    captchaData: '',
  } as AuthLoginInput,
  loading: {
    signIn: false,
  },
  disabled: {
    signIn: false,
  },
})

// 时间获取
const currentTime = computed(() => {
  return formatAxis(new Date())
})

//忘记密码
const onForgotPassword = () => {
  if (state.ruleForm.accountType == AccountType.Email.value) changePasswordComponentName.value = ComponentType.Email.name
  else if (state.ruleForm.accountType == AccountType.Mobile.value) changePasswordComponentName.value = ComponentType.Mobile.name
  isChangePassword.value = true
}

//验证通过
const onOk = (data: any) => {
  state.showDialog = false
  //开始登录
  state.ruleForm.captchaId = data.captchaId
  state.ruleForm.captchaData = JSON.stringify(data.track)
  login()
}

//登录
const login = async () => {
  state.loading.signIn = true
  const loginForm = cloneDeep(state.ruleForm)
  //登录时获取SM4加密参数
  const resPwd = await new AuthApi().getPasswordEncryptKey()
  if (resPwd && resPwd.success) {
    loginForm.passwordKey = resPwd.data?.key
    let encryptData = sm4.encrypt(loginForm.password, resPwd.data?.encryptKey as string, {
      output: 'string',
      mode: 'cbc',
      iv: resPwd.data?.iv as string,
    })
    loginForm.password = encryptData.toString()
  }

  const res = await new AuthApi().login(loginForm).catch(() => {
    state.loading.signIn = false
  })
  if (!res?.success) {
    state.loading.signIn = false
    return
  }

  const token = res.data?.token
  useUserInfo().setToken(token)
  // 添加完动态路由，再进行 router 跳转，否则可能报错 No match found for location with path "/"
  const isNoPower = await initBackEndControlRoutes()
  // 执行完 initBackEndControlRoutes，再执行 signInSuccess
  signInSuccess(isNoPower)
}

// 点击登录
const onSignIn = async () => {
  formRef.value.validate(async (valid: boolean) => {
    if (!valid) return

    //检查是否开启验证码登录
    state.disabled.signIn = true
    const res = await new AuthApi()
      .isCaptcha()
      .catch(() => {})
      .finally(() => {
        state.disabled.signIn = false
      })

    if (res?.success) {
      if (res.data) {
        state.showDialog = true
        //刷新滑块拼图
        myCaptchaDialogRef.value?.refresh()
      } else login()
    }
  })
}
// 登录成功后的跳转
const signInSuccess = (isNoPower: boolean | undefined) => {
  if (isNoPower) {
    ElMessage.warning('抱歉，您没有分配权限，请联系管理员')
    useUserInfo().removeToken()
    Session.clear()
  } else {
    // 初始化登录成功时间问候语
    let currentTimeInfo = currentTime.value
    // 登录成功，跳到转首页
    // 如果是复制粘贴的路径，非首页/登录页，那么登录成功后重定向到对应的路径中
    if (route.query?.redirect) {
      router.push({
        path: <string>route.query?.redirect,
        query: Object.keys(<string>route.query?.params).length > 0 ? JSON.parse(<string>route.query?.params) : '',
      })
    } else {
      router.push('/')
    }
    // 登录成功提示
    const signInText = t('message.signInText')
    ElMessage.success(`${currentTimeInfo}，${signInText}`)
    // 添加 loading，防止第一次进入界面时出现短暂空白
    NextLoading.start()
  }
  state.loading.signIn = false
}

watchEffect(() => {
  state.ruleForm.accountType = accountType.value as AccountTypeEnum
})
</script>

<style scoped lang="scss">
.login-content-form {
  .login-title {
    margin-bottom: 50px;
    font-size: 27px;
    text-align: center;
    letter-spacing: 3px;
    color: var(--el-text-color-primary);
    position: relative;
  }
  @for $i from 1 through 4 {
    .login-animation#{$i} {
      opacity: 0;
      animation-name: error-num;
      animation-duration: 0.5s;
      animation-fill-mode: forwards;
      animation-delay: calc($i/10) + s;
    }
  }
  .login-content-code {
    width: 100%;
    padding: 0;
    font-weight: bold;
    letter-spacing: 5px;
  }
  .login-content-submit {
    width: 100%;
    letter-spacing: 2px;
    font-weight: 300;
    margin-top: 15px;
  }

  .f12 {
    font-size: 12px;
  }
}
</style>
