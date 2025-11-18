/**
 * pinia 类型定义
 */

// 用户信息
declare interface UserInfos<T = any> {
  token: string
  authBtnList: string[]
  photo: string
  roles: string[]
  time: number
  userName: string
  showLoginDialog: boolean
  [key: string]: T
}
declare interface UserInfosState {
  userInfos: UserInfos
}

// 路由缓存列表
declare interface KeepAliveNamesState {
  keepAliveNames: string[]
  cachedViews: string[]
}

// 后端返回原始路由(未处理时)
declare interface RequestOldRoutesState {
  requestOldRoutes: string[]
}

// TagsView 路由列表
declare interface TagsViewRoutesState<T = any> {
  tagsViewRoutes: T[]
  isTagsViewCurrenFull: boolean
}

// 路由列表
declare interface RoutesListState<T = any> {
  routesList: T[]
  isColumnsMenuHover: boolean
  isColumnsNavHover: boolean
}

// 布局配置
declare interface ThemeConfigState {
  themeConfig: {
    isMobile: boolean
    isDrawer: boolean
    primary: string
    topBar: string
    topBarColor: string
    isTopBarColorGradual: boolean
    menuBar: string
    menuBarColor: string
    menuBarActiveColor: string
    isMenuBarColorGradual: boolean
    columnsMenuBar: string
    columnsMenuBarColor: string
    columnsMenuBarActiveColor: string
    isColumnsMenuBarColorGradual: boolean
    isColumnsMenuHoverPreload: boolean
    isCollapse: boolean
    isUniqueOpened: boolean
    isFixedHeader: boolean
    isFixedHeaderChange: boolean
    isClassicSplitMenu: boolean
    isLockScreen: boolean
    lockScreenTime: number
    isShowLogo: boolean
    isShowLogoChange: boolean
    isBreadcrumb: boolean
    isTagsview: boolean
    isBreadcrumbIcon: boolean
    isTagsviewIcon: boolean
    isCacheTagsView: boolean
    isSortableTagsView: boolean
    isShareTagsView: boolean
    isFooter: boolean
    isGrayscale: boolean
    isInvert: boolean
    isDark: boolean
    isWatermark: boolean
    watermarkText: string
    tagsStyle: string
    animation: string
    columnsAsideStyle: string
    columnsAsideLayout: string
    layout: string
    isRequestRoutes: boolean
    globalTitle: string
    globalViceTitle: string
    globalViceTitleMsg: string
    globalI18n: string
    globalComponentSize: string
    isCreateWebHistory: boolean
  }
}
