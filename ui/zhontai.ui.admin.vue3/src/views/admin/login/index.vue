<template>
  <el-scrollbar>
    <div class="login-container flex">
      <div class="login-left">
        <div class="login-left-logo">
          <img :src="logoMini" />
          <div class="login-left-logo-text">
            <span>{{ getThemeConfig.globalViceTitle }}</span>
            <span class="login-left-logo-text-msg">{{ getThemeConfig.globalViceTitleMsg }}</span>
          </div>
        </div>
        <div class="login-left-img">
          <img :src="loginMain" />
        </div>
        <img :src="loginBg" class="login-left-waves" />
      </div>
      <div class="login-right flex">
        <div class="login-right-warp flex-margin">
          <span class="login-right-warp-one"></span>
          <span class="login-right-warp-two"></span>
          <div class="login-right-warp-mian">
            <div class="login-right-warp-main-header"></div>
            <div class="login-right-warp-main-form">
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
                />
                <el-divider style="margin-top: 40px">其他方式登录</el-divider>
                <div class="login-other my-flex my-flex-center">
                  <el-link
                    v-for="(loginMethod, index) in loginMethods"
                    :key="index"
                    v-show="isShow(loginMethod)"
                    :icon="loginMethod.icon"
                    :underline="false"
                    :name="loginMethod.name"
                    @click="onLogin(loginMethod)"
                  >
                    <el-icon v-if="loginMethod.svg"><my-icon :name="loginMethod.svg" color="var(--color)"></my-icon></el-icon>
                    {{ $t(loginMethod.title) }}
                  </el-link>
                </div>
              </div>
              <div v-if="!(state.isScan || state.isChangePassword)" class="login-content-main-switch" @click="onReg">
                <span>{{ state.isReg ? '登录' : '注册' }}</span>
                <div class="login-content-main-switch-delta"></div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </el-scrollbar>
</template>

<script setup lang="ts" name="loginIndex">
import { defineAsyncComponent, onMounted, reactive, computed } from 'vue'
import { storeToRefs } from 'pinia'
import { useThemeConfig } from '/@/stores/themeConfig'
import { NextLoading } from '/@/utils/loading'
import logoMini from '/@/assets/logo-mini.svg'
import loginMain from '/@/assets/login-main.svg'
import loginBg from '/@/assets/login-bg.svg'
import { AccountType } from '/@/api/admin/enum-contracts'
import { ComponentType } from '/@/api/admin.extend/enum-contracts'

// 引入组件
const loginComponents: any = {
  account: defineAsyncComponent(() => import('./component/account.vue')),
  mobile: defineAsyncComponent(() => import('./component/mobile.vue')),
  email: defineAsyncComponent(() => import('./component/email.vue')),
  qq: defineAsyncComponent(() => import('./component/qq.vue')),
}

const changePasswordComponents: any = {
  mobile: defineAsyncComponent(() => import('/@/views/admin/change-password/component/mobile.vue')),
  email: defineAsyncComponent(() => import('/@/views/admin/change-password/component/email.vue')),
}

const regComponents: any = {
  mobile: defineAsyncComponent(() => import('/@/views/admin/reg/component/mobile.vue')),
  email: defineAsyncComponent(() => import('/@/views/admin/reg/component/email.vue')),
}

const accountComponentName = ComponentType.Account.name
const mobileComponentName = ComponentType.Mobile.name
const emailComponentName = ComponentType.Email.name

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

// 定义变量内容
const storesThemeConfig = useThemeConfig()
const { themeConfig } = storeToRefs(storesThemeConfig)
const state = reactive({
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

// 获取布局配置信息
const getThemeConfig = computed(() => {
  return themeConfig.value
})

// 页面加载时
onMounted(() => {
  NextLoading.done()
})

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
</script>

<style scoped lang="scss">
:deep() {
  .el-scrollbar__view {
    height: 100%;
  }
  .el-divider__text {
    font-size: 12px;
    color: #7f8792;
  }

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
.login-container {
  height: 100%;
  min-height: 500px;
  background: var(--el-color-white);
  .login-left {
    flex: 1;
    position: relative;
    background-color: rgba(211, 239, 255, 1);
    margin-right: 100px;
    .login-left-logo {
      display: flex;
      align-items: center;
      position: absolute;
      top: 50px;
      left: 80px;
      z-index: 1;
      animation: logoAnimation 0.3s ease;
      img {
        width: 52px;
        height: 52px;
      }
      .login-left-logo-text {
        display: flex;
        flex-direction: column;
        span {
          margin-left: 10px;
          font-size: 28px;
          color: var(--el-color-primary);
        }
        .login-left-logo-text-msg {
          font-size: 12px;
          color: var(--el-color-primary);
        }
      }
    }
    .login-left-img {
      position: absolute;
      top: 50%;
      left: 50%;
      transform: translate(-50%, -50%);
      width: 100%;
      height: 52%;
      img {
        width: 100%;
        height: 100%;
        animation: error-num 0.6s ease;
      }
    }
    .login-left-waves {
      position: absolute;
      top: 0;
      left: 100%;
      height: 100%;
    }
  }
  .login-right {
    width: 700px;
    .login-right-warp {
      border: 1px solid var(--el-color-primary-light-3);
      border-radius: 3px;
      width: 500px;
      height: 500px;
      position: relative;
      overflow: hidden;
      background-color: var(--el-color-white);
      .login-right-warp-one,
      .login-right-warp-two {
        position: absolute;
        display: block;
        width: inherit;
        height: inherit;
        &::before,
        &::after {
          content: '';
          position: absolute;
          z-index: 1;
        }
      }
      .login-right-warp-one {
        &::before {
          filter: hue-rotate(0deg);
          top: 0px;
          left: 0;
          width: 100%;
          height: 3px;
          background: linear-gradient(90deg, transparent, var(--el-color-primary));
          animation: loginLeft 3s linear infinite;
        }
        &::after {
          filter: hue-rotate(60deg);
          top: -100%;
          right: 2px;
          width: 3px;
          height: 100%;
          background: linear-gradient(180deg, transparent, var(--el-color-primary));
          animation: loginTop 3s linear infinite;
          animation-delay: 0.7s;
        }
      }
      .login-right-warp-two {
        &::before {
          filter: hue-rotate(120deg);
          bottom: 2px;
          right: -100%;
          width: 100%;
          height: 3px;
          background: linear-gradient(270deg, transparent, var(--el-color-primary));
          animation: loginRight 3s linear infinite;
          animation-delay: 1.4s;
        }
        &::after {
          filter: hue-rotate(300deg);
          bottom: -100%;
          left: 0px;
          width: 3px;
          height: 100%;
          background: linear-gradient(360deg, transparent, var(--el-color-primary));
          animation: loginBottom 3s linear infinite;
          animation-delay: 2.1s;
        }
      }
      .login-right-warp-mian {
        display: flex;
        flex-direction: column;
        height: 100%;
        .login-right-warp-main-header {
          height: 60px;
          line-height: 60px;
          font-size: 27px;
          text-align: center;
          letter-spacing: 3px;
          animation: logoAnimation 0.3s ease;
          animation-delay: 0.3s;
          color: var(--el-text-color-primary);
        }
        .login-right-warp-main-form {
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
              background: var(--el-color-white);
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
      }
    }
  }
}
</style>
