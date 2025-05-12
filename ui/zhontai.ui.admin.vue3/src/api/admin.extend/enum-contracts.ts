/** 组件类型 */
export const ComponentType = {
  Account: { name: 'account', value: 1, desc: '账号' },
  Mobile: { name: 'mobile', value: 2, desc: '手机' },
  Email: { name: 'email', value: 3, desc: '邮箱' },
}

/** 平台类型 */
export const PlatformType = {
  Web: { name: 'web', value: 1, desc: 'Web端' },
  App: { name: 'app', value: 2, desc: 'App端' },
  CS: { name: 'cs', value: 3, desc: 'CS端' },
}

/** 操作符 */
export const Operator = {
  equal: { label: '等于', value: 'Equal' },
  notEqual: { label: '不等于', value: 'NotEqual' },
  contains: { label: '包含', value: 'Contains' },
  notContains: { label: '不包含', value: 'NotContains' },
  startsWith: { label: '开始以', value: 'StartsWith' },
  notStartsWith: { label: '开始不是以', value: 'NotStartsWith' },
  endsWith: { label: '结束以', value: 'EndsWith' },
  notEndsWith: { label: '结束不是以', value: 'NotEndsWith' },
  lessThan: { label: '小于', value: 'LessThan' },
  lessThanOrEqual: { label: '小于等于', value: 'LessThanOrEqual' },
  greaterThan: { label: '大于', value: 'GreaterThan' },
  greaterThanOrEqual: { label: '大于等于', value: 'GreaterThanOrEqual' },
  dateRange: { label: '时间段', value: 'dateRange' },
  any: { label: '在列表', value: 'Any' },
  notAny: { label: '不在列表', value: 'NotAny' },
}
