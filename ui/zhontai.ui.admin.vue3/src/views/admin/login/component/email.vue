<template>
  <div>
    <el-form ref="formRef" :model="state.ruleForm" size="large" class="login-content-form">
      <el-form-item
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
            <el-icon class="el-input__icon"><ele-Iphone /></el-icon>
          </template>
        </el-input>
      </el-form-item>
      <el-form-item class="login-animation2" prop="code" :rules="[{ required: true, message: '请输入邮箱验证码', trigger: ['blur', 'change'] }]">
        <MyInputCode v-model="state.ruleForm.code" @keyup.enter="onSignIn" :email="state.ruleForm.email" :validate="validate" @send="onSend" />
      </el-form-item>
      <el-form-item class="login-animation3">
        <el-button round type="primary" v-waves class="login-content-submit" @click="onSignIn" :loading="state.loading.signIn">
          <span>{{ $t('message.mobile.btnText') }}</span>
        </el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script lang="ts" setup name="loginEmail">
import { reactive, defineAsyncComponent, ref, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import { testEmail } from '/@/utils/test'
import { AuthApi } from '/@/api/admin/Auth'
import { AuthEmailLoginInput } from '/@/api/admin/data-contracts'
import { useUserInfo } from '/@/stores/userInfo'
import { initBackEndControlRoutes } from '/@/router/backEnd'
import { Session } from '/@/utils/storage'
import { NextLoading } from '/@/utils/loading'
import { useI18n } from 'vue-i18n'
import { formatAxis } from '/@/utils/formatTime'

const MyInputCode = defineAsyncComponent(() => import('/@/components/my-input-code/index.vue'))

const { t } = useI18n()
const route = useRoute()
const router = useRouter()

const formRef = ref()
const phoneRef = ref()
// 定义变量内容
const state = reactive({
  ruleForm: {
    email: '',
    code: '',
    codeId: '',
  } as AuthEmailLoginInput,
  loading: {
    signIn: false,
  },
})

//验证手机号
const validate = (callback: Function) => {
  formRef.value.validateField('email', (isValid: boolean) => {
    if (!isValid) {
      phoneRef.value?.focus()
      return
    }
    callback?.()
  })
}

// 时间获取
const currentTime = computed(() => {
  return formatAxis(new Date())
})

//发送验证码
const onSend = (codeId: string) => {
  state.ruleForm.codeId = codeId
}

// 登录
const onSignIn = async () => {
  formRef.value.validate(async (valid: boolean) => {
    if (!valid) return

    state.loading.signIn = true
    const res = await new AuthApi().emailLogin(state.ruleForm).catch(() => {
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
  .login-content-code {
    width: 100%;
    padding: 0;
  }
  .login-content-submit {
    width: 100%;
    letter-spacing: 2px;
    font-weight: 300;
    margin-top: 15px;
  }
  .login-msg {
    color: var(--el-text-color-placeholder);
  }
}
</style>
