import { t } from '/@/i18n'

type EnumType = {
  [key: string]: {
    name: string
    value: number | string
    desc: string
  }
}

// 下拉选项的接口
interface DropdownOption {
  label: string
  value: string | number | undefined
}

// 下拉选项生成函数的参数接口
interface DropdownParams {
  includeUnknown?: boolean // 是否包含 "Unknown" 选项，默认为 false
  includeAll?: boolean // 是否包含 "全部" 选项，默认为 false
  allOption?: DropdownOption // 自定义 "全部" 选项，默认为 { label: '全部', value: undefined }
}

/** 根据枚举 name 查找描述 */
export function getDescByName<T extends EnumType, K extends keyof T>(enumObj: T, key: K): string | undefined {
  return enumObj[key]?.desc || ''
}

/** 根据枚举 value 查找描述 */
export function getDescByValue<T extends EnumType>(enumObj: T, value: T[keyof T]['value']): string | undefined {
  for (const [key, item] of Object.entries(enumObj)) {
    if (item.value === value) {
      return item.desc
    }
  }
  return ''
}

/** 枚举转换为下拉选项列表（使用值作为value） */
export function toOptionsByValue<T extends EnumType>(
  enumObj: T,
  params: DropdownParams = { includeUnknown: false, includeAll: false }
): DropdownOption[] {
  let options = Object.values(enumObj).reduce((options, item) => {
    if (params.includeUnknown || item.name !== 'Unknown') {
      options.push({ label: t(item.desc), value: item.value })
    }

    return options
  }, [] as DropdownOption[])
  if (params.includeAll) {
    options.unshift(params.allOption || { label: t('全部'), value: undefined })
  }
  return options
}

/** 转换为下拉选项列表（使用名称作为value） */
export function toOptionsByName<T extends EnumType>(
  enumObj: T,
  params: DropdownParams = { includeUnknown: false, includeAll: false }
): DropdownOption[] {
  let options = Object.values(enumObj).reduce((options, item) => {
    if (params.includeUnknown || item.name !== 'Unknown') {
      options.push({ label: item.desc, value: item.name })
    }
    return options
  }, [] as DropdownOption[])
  if (params.includeAll) {
    options.unshift(params.allOption || { label: t('全部'), value: undefined })
  }
  return options
}

export default {
  getDescByName,
  getDescByValue,
  toOptionsByName,
  toOptionsByValue,
}
