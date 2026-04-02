<template>
  <div class="layout-navbars-breadcrumb-user pr15" :style="{ flex: layoutUserFlexNum }">
    <el-dropdown :show-timeout="70" :hide-timeout="50" trigger="click" @command="onComponentSizeChange">
      <div class="layout-navbars-breadcrumb-user-icon">
        <i class="iconfont icon-ziti" :title="$t('组件大小')"></i>
      </div>
      <template #dropdown>
        <el-dropdown-menu>
          <el-dropdown-item command="large" :disabled="state.disabledSize === 'large'">{{ $t('大型') }}</el-dropdown-item>
          <el-dropdown-item command="default" :disabled="state.disabledSize === 'default'">{{ $t('默认') }}</el-dropdown-item>
          <el-dropdown-item command="small" :disabled="state.disabledSize === 'small'">{{ $t('小型') }}</el-dropdown-item>
        </el-dropdown-menu>
      </template>
    </el-dropdown>
    <el-dropdown :show-timeout="70" :hide-timeout="50" trigger="click" @command="onLanguageChange">
      <div class="layout-navbars-breadcrumb-user-icon">
        <i class="iconfont icon-diqiu" :title="$t('语言切换')"></i>
      </div>
      <template #dropdown>
        <el-dropdown-menu>
          <el-dropdown-item command="zh-cn" :disabled="state.disabledI18n === 'zh-cn'">{{ t('简体中文') }}</el-dropdown-item>
          <el-dropdown-item command="en" :disabled="state.disabledI18n === 'en'">English</el-dropdown-item>
          <el-dropdown-item command="zh-tw" :disabled="state.disabledI18n === 'zh-tw'">{{ t('繁体中文') }}</el-dropdown-item>
        </el-dropdown-menu>
      </template>
    </el-dropdown>
    <div class="layout-navbars-breadcrumb-user-icon" @click="onSearchClick">
      <el-icon :title="$t('菜单搜索')">
        <ele-Search />
      </el-icon>
    </div>
    <div class="layout-navbars-breadcrumb-user-icon" @click="onLayoutSetingClick">
      <i class="icon-skin iconfont" :title="$t('布局配置')"></i>
    </div>
    <div class="layout-navbars-breadcrumb-user-icon" @click="onMsgClick">
      <el-badge :is-dot="state.unread">
        <el-icon :title="$t('消息')">
          <ele-Bell />
        </el-icon>
      </el-badge>
    </div>
    <div class="layout-navbars-breadcrumb-user-icon mr10" @click="onScreenfullClick">
      <i
        class="iconfont"
        :title="state.isScreenfull ? $t('关全屏') : $t('开全屏')"
        :class="!state.isScreenfull ? 'icon-fullscreen' : 'icon-tuichuquanping'"
      ></i>
    </div>
    <el-dropdown :show-timeout="70" :hide-timeout="50" @command="onHandleCommandClick">
      <span class="layout-navbars-breadcrumb-user-link">
        <img :src="avatar" class="layout-navbars-breadcrumb-user-link-photo mr5" />
        <span class="layout-navbars-breadcrumb-user-link-name">{{ userInfos.userName ? userInfos.userName : '' }}</span>
        <el-icon class="el-icon--right">
          <ele-ArrowDown />
        </el-icon>
      </span>
      <template #dropdown>
        <el-dropdown-menu>
          <el-dropdown-item command="/platform/workbench">{{ $t('工作台') }}</el-dropdown-item>
          <el-dropdown-item command="/personal">{{ $t('个人中心') }}</el-dropdown-item>
          <el-dropdown-item command="/site-msg">{{ $t('站内信') }}</el-dropdown-item>
          <el-dropdown-item divided command="logOut">{{ $t('退出登录') }}</el-dropdown-item>
        </el-dropdown-menu>
      </template>
    </el-dropdown>
    <Search ref="searchRef" />
    <Msg ref="msgRef" />
  </div>
</template>

<script setup lang="ts" name="layoutBreadcrumbUser">
import { ElMessageBox, ElMessage } from 'element-plus'
import screenfull from 'screenfull'
import { useUserInfo } from '/@/stores/userInfo'
import { useThemeConfig } from '/@/stores/themeConfig'
import other from '/@/utils/other'
import mittBus from '/@/utils/mitt'
import { Local } from '/@/utils/storage'
import { SiteMsgApi } from '/@/api/admin/SiteMsg'
import { WebSocketClient } from '/@/utils/ws'

// 引入组件
const Msg = defineAsyncComponent(() => import('/@/layout/navBars/topBar/msg.vue'))
const Search = defineAsyncComponent(() => import('/@/layout/navBars/topBar/search.vue'))

// 定义变量内容
const { locale, t } = useI18n()
const router = useRouter()
const storesUseUserInfo = useUserInfo()
const storesThemeConfig = useThemeConfig()
const { userInfos } = storeToRefs(storesUseUserInfo)
const { themeConfig } = storeToRefs(storesThemeConfig)
const searchRef = useTemplateRef('searchRef')
const msgRef = useTemplateRef('msgRef')
const wsClient = ref<WebSocketClient | null>(null)

const state = <any>reactive({
  isScreenfull: false,
  disabledI18n: 'zh-cn',
  disabledSize: 'large',
  unread: false,
})

// 头像地址
const avatar = computed(() => {
  return userInfos.value.photo || 'https://img2.baidu.com/it/u=1978192862,2048448374&fm=253&fmt=auto&app=138&f=JPEG?w=504&h=500'
})

// 设置分割样式
const layoutUserFlexNum = computed(() => {
  let num: string | number = ''
  const { layout, isClassicSplitMenu } = themeConfig.value
  const layoutArr: string[] = ['defaults', 'columns']
  if (layoutArr.includes(layout) || (layout === 'classic' && !isClassicSplitMenu)) num = '1'
  else num = ''
  return num
})
// 全屏点击时
const onScreenfullClick = () => {
  if (!screenfull.isEnabled) {
    ElMessage.warning(t('暂不不支持全屏'))
    return false
  }
  screenfull.toggle()
  screenfull.on('change', () => {
    if (screenfull.isFullscreen) state.isScreenfull = true
    else state.isScreenfull = false
  })
}
// 消息通知点击时
const onMsgClick = () => {
  msgRef.value!.openDrawer()
}
// 布局配置 icon 点击时
const onLayoutSetingClick = () => {
  mittBus.emit('openSetingsDrawer')
}
// 下拉菜单点击时
const onHandleCommandClick = (path: string) => {
  if (path === 'logOut') {
    ElMessageBox({
      closeOnClickModal: false,
      closeOnPressEscape: false,
      title: t('提示'),
      message: t('此操作将退出登录, 是否继续?'),
      showCancelButton: true,
      confirmButtonText: t('确定'),
      cancelButtonText: t('取消'),
      buttonSize: 'default',
      beforeClose: (action, instance, done) => {
        if (action === 'confirm') {
          instance.confirmButtonLoading = true
          instance.confirmButtonText = t('退出中')
          setTimeout(() => {
            done()
            setTimeout(() => {
              instance.confirmButtonLoading = false
            }, 300)
          }, 700)
        } else {
          done()
        }
      },
    })
      .then(async () => {
        storesUseUserInfo.clear()
      })
      .catch(() => {})
  } else if (path === 'wareHouse') {
    window.open('https://gitee.com/zhontai/admin.ui.plus')
  } else {
    router.push(path)
  }
}
// 菜单搜索点击
const onSearchClick = () => {
  searchRef.value!.openSearch()
}
// 组件大小改变
const onComponentSizeChange = (size: string) => {
  Local.remove('themeConfig')
  themeConfig.value.globalComponentSize = size
  Local.set('themeConfig', themeConfig.value)
  initI18nOrSize('globalComponentSize', 'disabledSize')
}
// 语言切换
const onLanguageChange = (lang: string) => {
  Local.remove('themeConfig')
  themeConfig.value.globalI18n = lang
  Local.set('themeConfig', themeConfig.value)
  locale.value = lang
  other.useTitle()
  initI18nOrSize('globalI18n', 'disabledI18n')
}
// 初始化组件大小/i18n
const initI18nOrSize = (value: string, attr: string) => {
  state[attr] = Local.get('themeConfig')[value]
}
//检查是否有未读消息
const checkUnreadMsg = async () => {
  const res = await new SiteMsgApi().isUnread().catch(() => {})
  if (res?.success) {
    state.unread = res.data
  }
}
const initWebSocket = () => {
  wsClient.value = new WebSocketClient({
    onMessage: (event: MessageEvent) => {
      if (event.data) {
        var data = JSON.parse(event.data)
        console.log(data)
        if (data.evts?.length > 0) {
          data.evts.forEach((evt: any) => {
            mittBus.emit(evt.name, evt.data)
          })
        }
      }
    },
  })
}
// 页面加载时
onMounted(() => {
  if (Local.get('themeConfig')) {
    initI18nOrSize('globalComponentSize', 'disabledSize')
    initI18nOrSize('globalI18n', 'disabledI18n')
  }
  checkUnreadMsg()
  mittBus.off('checkUnreadMsg')
  mittBus.on('checkUnreadMsg', () => {
    checkUnreadMsg()
  })

  mittBus.off('forceOffline')
  mittBus.on('forceOffline', () => {
    storesUseUserInfo.clear()
  })

  initWebSocket()
})
</script>

<style scoped lang="scss">
.layout-navbars-breadcrumb-user {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  &-link {
    height: 50px;
    line-height: 50px;
    cursor: pointer;
    display: flex;
    align-items: center;
    white-space: nowrap;
    &-photo {
      width: 25px;
      height: 25px;
      border-radius: 100%;
    }
  }
  &-icon {
    padding: 0 10px;
    cursor: pointer;
    color: var(--next-bg-topBarColor);
    height: 50px;
    line-height: 50px;
    display: flex;
    align-items: center;
    &:hover {
      background: var(--next-color-user-hover);
      i {
        display: inline-block;
        animation: logoAnimation 0.3s ease-in-out;
      }
    }
  }
  :deep(.el-dropdown) {
    color: var(--next-bg-topBarColor);
  }
  :deep(.el-badge) {
    height: 40px;
    line-height: 40px;
    display: flex;
    align-items: center;
  }
  :deep(.el-badge__content.is-fixed) {
    top: 12px;
  }
}
</style>
