<template>
  <div>
    <el-form ref="formRef" :model="form" size="large" class="login-content-form">
      <div class="login-title">注册账号</div>
      <el-form-item
        class="login-animation1"
        prop="email"
        :rules="[
          { required: true, message: '请输入邮箱地址', trigger: ['blur', 'change'] },
          { validator: testEmail, trigger: ['blur', 'change'] },
        ]"
      >
        <el-input ref="emailRef" text :placeholder="$t('message.email.placeholder1')" v-model="form.email" clearable autocomplete="off">
          <template #prefix>
            <el-icon class="el-input__icon"><ele-Promotion /></el-icon>
          </template>
        </el-input>
      </el-form-item>
      <el-form-item class="login-animation2" prop="code" :rules="[{ required: true, message: '请输入邮箱验证码', trigger: ['blur', 'change'] }]">
        <MyInputCode v-model="form.code" :email="form.email" :validate="validatorEmail" @send="onSend" />
      </el-form-item>
      <el-form-item
        v-if="hasPassword"
        class="login-animation3"
        prop="password"
        :rules="[
          { required: true, message: '请输入密码', trigger: ['blur', 'change'] },
          { validator: validatorPwd, trigger: ['blur', 'change'] },
        ]"
      >
        <el-input v-model="form.password" :placeholder="'输入密码'" show-password autocomplete="off" clearable @input="onInputPassword">
          <template #prefix>
            <el-icon class="el-input__icon"><ele-Unlock /></el-icon>
          </template>
        </el-input>
      </el-form-item>
      <el-form-item
        class="login-animation3 mb10"
        prop="corpName"
        :rules="[{ required: true, message: '请填写完整企业名称', trigger: ['blur', 'change'] }]"
      >
        <el-input ref="emailRef" text :placeholder="$t('请填写完整企业名称')" v-model="form.corpName" clearable autocomplete="off">
          <template #prefix>
            <el-icon class="el-input__icon"><ele-OfficeBuilding /></el-icon>
          </template>
        </el-input>
      </el-form-item>
      <el-form-item class="login-animation4 mb5">
        <el-button round type="primary" v-waves class="login-content-submit" :loading="state.loading" @click="onReg">
          <span>{{ $t('注册') }}</span>
        </el-button>
      </el-form-item>
      <el-form-item class="login-animation5 mb5 login-agree" prop="agree" :rules="[{ validator: validatorAgree, trigger: ['change'] }]">
        <div class="my-flex my-flex-items-center f12">
          <el-checkbox v-model="form.agree" size="small">我已阅读并同意</el-checkbox>
          <div class="my-flex my-flex-items-center ml5">
            <el-link :underline="false" type="primary" class="f12" target="_blank" href="https://zhontai.net/admin/introduce.html">服务协议</el-link
            >、
            <el-link :underline="false" type="primary" class="f12" target="_blank" href="https://zhontai.net/admin/introduce.html">隐私政策</el-link>
          </div>
        </div>
      </el-form-item>
    </el-form>
  </div>
</template>

<script lang="ts" setup name="admin/reg-email">
import { reactive, toRefs, ref, defineAsyncComponent } from 'vue'
import { AuthRegByEmailInput } from '/@/api/admin/data-contracts'
import { AuthApi } from '/@/api/admin/Auth'
import { verifyCnAndSpace } from '/@/utils/toolsValidate'
import { validatorPwd, validatorAgree } from '/@/utils/validators'
import { testEmail } from '/@/utils/test'
import { ElMessage } from 'element-plus'

const MyInputCode = defineAsyncComponent(() => import('/@/components/my-input-code/index.vue'))
const isReg = defineModel('isReg', { type: Boolean, default: false })
const hasPassword = defineModel('hasPassword', { type: Boolean, default: false })

const formRef = ref()
const emailRef = ref()
const state = reactive({
  showDialog: false,
  loading: false,
  form: {
    agree: false,
    email: '',
    code: '',
    codeId: '',
    password: '',
    corpName: '中台',
  } as AuthRegByEmailInput & { agree: false },
})
const { form } = toRefs(state)

//验证邮箱
const validatorEmail = (callback: Function) => {
  formRef.value.validateField('email', (isValid: boolean) => {
    if (!isValid) {
      emailRef.value?.focus()
      return
    }
    callback?.()
  })
}

//发送验证码
const onSend = (codeId: string) => {
  state.form.codeId = codeId
}

// 输入密码
const onInputPassword = (val: string) => {
  state.form.password = verifyCnAndSpace(val)
}

// 打开对话框
const open = async () => {
  state.showDialog = true
  state.form = {} as AuthRegByEmailInput & { agree: false }
}

// 注册
const onReg = () => {
  formRef.value.validate(async (valid: boolean) => {
    if (!valid) return

    state.loading = true
    const res = await new AuthApi().regByEmail(state.form, { showSuccessMessage: false }).catch(() => {
      state.loading = false
    })
    state.loading = false

    if (res?.success) {
      ElMessage.success('注册成功')
      isReg.value = false
    }
  })
}

defineExpose({
  open,
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
  .login-remind {
    color: #7f8792;
    margin-right: 5px;
  }
  @for $i from 1 through 5 {
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
  .f12 {
    font-size: 12px;
  }
  :deep() {
    .el-checkbox__input.is-checked + .el-checkbox__label {
      color: unset;
    }
    .login-agree.el-form-item--large .el-form-item__content {
      line-height: unset !important;
    }
  }
}
</style>
