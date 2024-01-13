<template>
  <div>
    <el-form ref="formRef" :model="state.ruleForm" size="large" class="login-content-form">
      <el-form-item class="login-animation1" prop="userName" :rules="[{ required: true, message: '请输入用户名', trigger: ['blur', 'change'] }]">
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
      <el-form-item class="login-animation2" prop="password" :rules="[{ required: true, message: '请输入密码', trigger: ['blur', 'change'] }]">
        <el-input
          :type="state.isShowPassword ? 'text' : 'password'"
          :placeholder="$t('message.account.accountPlaceholder2')"
          v-model="state.ruleForm.password"
          autocomplete="off"
          @keyup.enter="onSignIn"
        >
          <template #prefix>
            <el-icon class="el-input__icon"><ele-Unlock /></el-icon>
          </template>
          <template #suffix>
            <i
              class="iconfont el-input__icon login-content-password"
              :class="state.isShowPassword ? 'icon-yincangmima' : 'icon-xianshimima'"
              @click="state.isShowPassword = !state.isShowPassword"
            >
            </i>
          </template>
        </el-input>
      </el-form-item>
      <!-- <el-form-item class="login-animation3">
      <el-col :span="15">
        <el-input
          text
          maxlength="4"
          :placeholder="$t('message.account.accountPlaceholder3')"
          v-model="state.ruleForm.code"
          clearable
          autocomplete="off"
        >
          <template #prefix>
            <el-icon class="el-input__icon"><ele-Position /></el-icon>
          </template>
        </el-input>
      </el-col>
      <el-col :span="1"></el-col>
      <el-col :span="8">
        <el-button class="login-content-code" v-waves>1234</el-button>
      </el-col>
    </el-form-item> -->
      <el-form-item class="login-animation4">
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
    </el-form>
    <MyCaptchaDialog ref="myCaptchaDialogRef" v-model="state.showDialog" @ok="onOk" />
  </div>
</template>

<script setup lang="ts" name="loginAccount">
import { reactive, computed, ref, defineAsyncComponent } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import { useI18n } from 'vue-i18n'
// import Cookies from 'js-cookie'
// import { storeToRefs } from 'pinia'
// import { useThemeConfig } from '/@/stores/themeConfig'
// import { initFrontEndControlRoutes } from '/@/router/frontEnd'
import { initBackEndControlRoutes } from '/@/router/backEnd'
import { Session } from '/@/utils/storage'
import { formatAxis } from '/@/utils/formatTime'
import { NextLoading } from '/@/utils/loading'
import { AuthApi } from '/@/api/admin/Auth'
import { AuthLoginInput } from '/@/api/admin/data-contracts'
import { useUserInfo } from '/@/stores/userInfo'

const MyCaptchaDialog = defineAsyncComponent(() => import('/@/components/my-captcha/dialog.vue'))

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
  isShowPassword: false,
  ruleForm: {
    userName: '',
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
  const res = await new AuthApi().login(state.ruleForm).catch(() => {
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
</script>

<style scoped lang="scss">
.login-content-form {
  margin-top: 20px;
  @for $i from 1 through 4 {
    .login-animation#{$i} {
      opacity: 0;
      animation-name: error-num;
      animation-duration: 0.5s;
      animation-fill-mode: forwards;
      animation-delay: calc($i/10) + s;
    }
  }
  .login-content-password {
    display: inline-block;
    width: 20px;
    cursor: pointer;
    &:hover {
      color: #909399;
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
}
</style>
