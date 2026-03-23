import { createI18n } from 'vue-i18n'
import pinia from '/@/stores/index'
import { storeToRefs } from 'pinia'
import { useThemeConfig } from '/@/stores/themeConfig'

// Element Plus 自带国际化
import enLocale from 'element-plus/es/locale/lang/en'
import zhcnLocale from 'element-plus/es/locale/lang/zh-cn'
import zhtwLocale from 'element-plus/es/locale/lang/zh-tw'

// 定义语言代码与 Element 语言包的映射
const elementLocales: Record<string, any> = {
  en: enLocale,
  'zh-cn': zhcnLocale,
  'zh-tw': zhtwLocale,
}

// 获取支持的语言列表（从 elementLocales 动态生成）
const supportedLocales = Object.keys(elementLocales)

/**
 * 合并数组中的多个对象为一个对象
 * @param objects 对象数组
 * @returns 合并后的对象
 */
function mergeObjects(objects: Record<string, any>[]): Record<string, any> {
  return objects.reduce((acc, obj) => ({ ...acc, ...obj }), {})
}

// 动态导入国际化文件
// 框架本身的国际化文件（src/i18n/**/*.ts）
const frameworkModules: Record<string, any> = import.meta.glob('./**/*.ts', { eager: true })
// 视图层的国际化文件（src/views/**/**/lang/*.ts）
const viewModules: Record<string, any> = import.meta.glob('./../views/**/**/i18n/*.ts', { eager: true })

// 按语言分类存储所有翻译片段
const messagesByLocale: Record<string, Record<string, any>[]> = {}

// 处理框架模块
for (const path in frameworkModules) {
  const match = path.match(/([^/]+)\.ts$/) // 提取文件名（不含扩展名）作为语言代码
  if (!match) continue
  const locale = match[1]
  if (!messagesByLocale[locale]) messagesByLocale[locale] = []
  messagesByLocale[locale].push(frameworkModules[path].default)
}

// 处理视图模块
for (const path in viewModules) {
  const match = path.match(/([^/]+)\.ts$/)
  if (!match) continue
  const locale = match[1]
  if (!messagesByLocale[locale]) messagesByLocale[locale] = []
  messagesByLocale[locale].push(viewModules[path].default)
}

// 构建最终的 messages 对象
const messages: Record<string, any> = {}
for (const locale of supportedLocales) {
  // 如果该语言没有翻译片段，则初始化为空对象
  const fragments = messagesByLocale[locale] || []
  messages[locale] = {
    name: locale,
    // Element Plus 语言包（注意：这里直接使用语言包对象，不是它的 el 属性）
    el: elementLocales[locale].el,
    // 合并所有翻译片段
    message: mergeObjects(fragments),
    ...mergeObjects(fragments),
  }
}

// 从 Pinia 获取当前语言设置
const stores = useThemeConfig(pinia)
const { themeConfig } = storeToRefs(stores)
// 创建 i18n 实例
export const i18n = createI18n({
  legacy: false,
  silentTranslationWarn: true,
  missingWarn: false,
  silentFallbackWarn: true,
  fallbackWarn: false,
  locale: themeConfig.value.globalI18n,
  fallbackLocale: zhcnLocale.name,
  messages,
  fallbackFormat: true,
  missing: (locale, key) => {
    // 只返回最后一段
    return key?.split('.')?.pop()
  },
})
