<template>
  <el-scrollbar>
    <div class="login-container flex">
      <div class="login-left" :class="getThemeConfig.isDark ? '' : 'login-left-light'">
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
      </div>
      <div class="login-right flex">
        <div class="login-right-warp flex-margin">
          <span class="login-right-warp-one"></span>
          <span class="login-right-warp-two"></span>
          <div class="login-right-warp-mian">
            <div class="login-right-warp-main-header"></div>
            <div class="login-right-warp-main-form">
              <LoginForm />
            </div>
          </div>
        </div>
      </div>
    </div>
  </el-scrollbar>
</template>

<script setup lang="ts" name="loginIndex">
import { useThemeConfig } from '/@/stores/themeConfig'
import { NextLoading } from '/@/utils/loading'
import logoMini from '/@/assets/logo-mini.svg'
import loginMain from '/@/assets/login-main.svg'

const LoginForm = defineAsyncComponent(() => import('./form.vue'))

// 定义变量内容
const storesThemeConfig = useThemeConfig()
const { themeConfig } = storeToRefs(storesThemeConfig)

// 获取布局配置信息
const getThemeConfig = computed(() => {
  return themeConfig.value
})

// 页面加载时
onMounted(() => {
  NextLoading.done()
})
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
}
.login-container {
  height: 100%;
  min-height: 500px;
  background: var(--el-bg-color);
  .login-left {
    flex: 1;
    position: relative;
    background-color: var(--el-bg-color-page);
    &.login-left-light {
      background-color: rgba(211, 239, 255, 1);
    }
    .login-left-logo {
      display: flex;
      align-items: center;
      position: absolute;
      top: 20px;
      left: 20px;
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
          color: var(--el-text-color-primary);
        }
        .login-left-logo-text-msg {
          font-size: 12px;
          color: var(--el-text-color-secondary);
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
    background: var(--el-bg-color);
    .login-right-warp {
      border: 1px solid var(--el-color-primary-light-3);
      border-radius: 3px;
      width: 500px;
      height: 500px;
      position: relative;
      overflow: hidden;
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
      }
    }
  }
}
</style>
