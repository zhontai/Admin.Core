<template>
  <div class="login-form-box">
    <component
      v-if="state.isChangePassword"
      :is="changePasswordComponents[state.changePasswordComponentName]"
      v-model:isChangePassword="state.isChangePassword"
      v-model:hasConfirmPassword="state.hasConfirmPassword"
    />

    <component
      v-if="state.isReg"
      :is="regComponents[state.regComponentName]"
      v-model:isReg="state.isReg"
      v-model:hasPassword="state.hasPassword"
      v-model:hasConfirmPassword="state.hasConfirmPassword"
    />

    <div v-if="!state.isScan && !state.isChangePassword && !state.isReg">
      <component
        :is="loginComponents[state.loginComponentName]"
        v-model:loginComponentName="state.loginComponentName"
        v-model:accountType="state.accountType"
        v-model:isChangePassword="state.isChangePassword"
        v-model:changePasswordComponentName="state.changePasswordComponentName"
        v-model:isPopup="isPopup"
        @ok="onOk"
      />
      <el-divider style="margin-top: 40px">其他方式登录</el-divider>
      <div class="login-other my-flex my-flex-center">
        <el-link
          v-for="(loginMethod, index) in loginMethods"
          :key="index"
          v-show="isShow(loginMethod)"
          :icon="loginMethod.icon"
          underline="never"
          :name="loginMethod.name"
          @click="onLogin(loginMethod)"
        >
          <el-icon v-if="loginMethod.svg"><my-icon :name="loginMethod.svg" color="var(--color)"></my-icon></el-icon>
          {{ $t(loginMethod.title) }}
        </el-link>
      </div>
    </div>
    <div v-if="showReg && !(state.isScan || state.isChangePassword)" class="login-content-main-switch" @click="onReg">
      <span>{{ state.isReg ? '登录' : '注册' }}</span>
      <div class="login-content-main-switch-delta"></div>
    </div>
  </div>
</template>

<script lang="ts" setup name="login/dialog">
import { AccountType } from '/@/api/admin/enum-contracts'
import { ComponentType } from '/@/api/admin.extend/enum-contracts'

//是否显示注册
const showReg = defineModel('showReg', { type: Boolean, default: true })
//是否弹窗
const isPopup = defineModel('isPopup', { type: Boolean, default: false })

// 登录组件
const loginComponents: any = {
  account: defineAsyncComponent(() => import('./component/account.vue')),
  mobile: defineAsyncComponent(() => import('./component/mobile.vue')),
  email: defineAsyncComponent(() => import('./component/email.vue')),
  qq: defineAsyncComponent(() => import('./component/qq.vue')),
}

// 更改密码组件
const changePasswordComponents: any = {
  mobile: defineAsyncComponent(() => import('/@/views/admin/change-password/component/mobile.vue')),
  email: defineAsyncComponent(() => import('/@/views/admin/change-password/component/email.vue')),
}

// 注册组件
const regComponents: any = {
  mobile: defineAsyncComponent(() => import('/@/views/admin/reg/component/mobile.vue')),
  email: defineAsyncComponent(() => import('/@/views/admin/reg/component/email.vue')),
}

const accountComponentName = ComponentType.Account.name
const mobileComponentName = ComponentType.Mobile.name
const emailComponentName = ComponentType.Email.name

//登录方式
const loginMethods = [
  {
    icon: 'ele-User',
    name: accountComponentName,
    title: 'message.label.one1',
  },
  {
    icon: 'ele-Iphone',
    name: mobileComponentName,
    title: 'message.label.two2',
  },
  {
    icon: 'ele-Message',
    name: emailComponentName,
    title: 'message.label.two3',
  },
  {
    svg: 'qq',
    name: 'qq',
    title: 'QQ',
  },
] as any

const emits = defineEmits(['ok'])

const state = reactive({
  visible: false,
  loginComponentName: accountComponentName, //默认账号登录
  accountType: AccountType.UserName.value, //默认用户名账号
  isScan: false,
  isChangePassword: false,
  isReg: false,
  changePasswordComponentName: emailComponentName, //默认邮箱更改密码
  regComponentName: emailComponentName, //默认邮箱注册
  hasPassword: true, //默认不用填密码
  hasConfirmPassword: false, //默认不用填确认密码
})

//是否显示
const isShow = (loginMethod: any) => {
  if (loginMethod.name === accountComponentName) {
    return !(state.loginComponentName === accountComponentName && state.accountType === AccountType.UserName.value)
  } else if (loginMethod.name === mobileComponentName) {
    return !(
      state.loginComponentName === mobileComponentName ||
      (state.loginComponentName === accountComponentName && state.accountType === AccountType.Mobile.value)
    )
  } else if (loginMethod.name == emailComponentName) {
    return !(
      state.loginComponentName === emailComponentName ||
      (state.loginComponentName === accountComponentName && state.accountType === AccountType.Email.value)
    )
  } else if (loginMethod.name == 'qq') {
    return !(state.loginComponentName === 'qq')
  }
}

//注册
const onReg = () => {
  if (state.loginComponentName == mobileComponentName) {
    state.regComponentName = mobileComponentName
  } else {
    state.regComponentName = emailComponentName
  }
  state.isReg = !state.isReg
}

//登录
const onLogin = (loginMethod: any) => {
  state.loginComponentName = loginMethod.name
  if (loginMethod.name === accountComponentName) {
    state.accountType = AccountType.UserName.value
  }
}

//登录成功
const onOk = () => {
  emits('ok')
}
</script>

<style scoped lang="scss">
:deep() {
  .login-other {
    .el-link {
      color: #7f8792;
      margin-right: 20px;
      .el-link__inner {
        font-size: 12px;
      }

      &:hover {
        color: var(--el-link-hover-text-color);
      }
    }
    .el-link .el-icon {
      margin-right: 4px;
    }
  }
}
.login-form-box {
  flex: 1;
  padding: 0px 50px 20px 50px;
  .login-content-main-switch {
    position: absolute;
    top: 0;
    right: 0;
    width: 70px;
    height: 70px;
    z-index: 0;
    overflow: hidden;
    cursor: pointer;
    transition: all ease 0.3s;
    background: var(--el-color-primary-light-1);
    span {
      position: absolute;
      color: #fff;
      top: 10px;
      right: 7px;
      font-size: 14px;
      font-weight: 500;
    }
    &-delta {
      position: absolute;
      width: 50px;
      height: 100px;
      z-index: 0;
      top: 3px;
      right: 27px;
      background: var(--el-bg-color);
      transform: rotate(-45deg);
      cursor: default;
    }
  }
  .login-content-main-scan {
    position: absolute;
    top: 0;
    right: 0;
    width: 50px;
    height: 50px;
    overflow: hidden;
    cursor: pointer;
    transition: all ease 0.3s;
    color: var(--el-color-primary);
    &-delta {
      position: absolute;
      width: 35px;
      height: 70px;
      z-index: 2;
      top: 2px;
      right: 21px;
      background: var(--el-color-white);
      transform: rotate(-45deg);
    }
    &:hover {
      opacity: 1;
      transition: all ease 0.3s;
      color: var(--el-color-primary) !important;
    }
    i {
      width: 47px;
      height: 50px;
      display: inline-block;
      font-size: 48px;
      position: absolute;
      right: 1px;
      top: 0px;
    }
  }
}
</style>
