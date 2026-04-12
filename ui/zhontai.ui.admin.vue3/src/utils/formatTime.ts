import { t } from '/@/i18n'

/**
 * 时间日期转换
 * @param date 当前时间，new Date() 格式
 * @param format 需要转换的时间格式字符串
 * @description format 字符串随意，如 `YYYY-mm、YYYY-mm-dd`
 * @description format 季度："YYYY-mm-dd HH:MM:SS QQQQ"
 * @description format 星期："YYYY-mm-dd HH:MM:SS WWW"
 * @description format 几周："YYYY-mm-dd HH:MM:SS ZZZ"
 * @description format 季度 + 星期 + 几周："YYYY-mm-dd HH:MM:SS WWW QQQQ ZZZ"
 * @returns 返回拼接后的时间字符串
 */
export function formatDate(date: Date, format: string): string {
  let we = date.getDay() // 星期
  let z = getWeek(date) // 周
  let qut = Math.floor((date.getMonth() + 3) / 3).toString() // 季度
  const opt: { [key: string]: string } = {
    'Y+': date.getFullYear().toString(), // 年
    'm+': (date.getMonth() + 1).toString(), // 月(月份从0开始，要+1)
    'd+': date.getDate().toString(), // 日
    'H+': date.getHours().toString(), // 时
    'M+': date.getMinutes().toString(), // 分
    'S+': date.getSeconds().toString(), // 秒
    'q+': qut, // 季度
  }
  // 中文数字 (星期)
  const week: { [key: string]: string } = {
    '0': t('日'),
    '1': t('一'),
    '2': t('二'),
    '3': t('三'),
    '4': t('四'),
    '5': t('五'),
    '6': t('六'),
  }
  // 中文数字（季度）
  const quarter: { [key: string]: string } = {
    '1': t('一'),
    '2': t('二'),
    '3': t('三'),
    '4': t('四'),
  }
  if (/(W+)/.test(format))
    format = format.replace(
      RegExp.$1,
      RegExp.$1.length > 1 ? (RegExp.$1.length > 2 ? t('星期{we}', { we: week[we] }) : t('周{we}', { we: week[we] })) : week[we]
    )
  if (/(Q+)/.test(format)) format = format.replace(RegExp.$1, RegExp.$1.length == 4 ? t('第{qut}季度', { qut: quarter[qut] }) : quarter[qut])
  if (/(Z+)/.test(format)) format = format.replace(RegExp.$1, RegExp.$1.length == 3 ? t('第{z}周', { z: z }) : z + '')
  for (let k in opt) {
    let r = new RegExp('(' + k + ')').exec(format)
    // 若输入的长度不为1，则前面补零
    if (r) format = format.replace(r[1], RegExp.$1.length == 1 ? opt[k] : opt[k].padStart(RegExp.$1.length, '0'))
  }
  return format
}

/**
 * 获取当前日期是第几周
 * @param dateTime 当前传入的日期值
 * @returns 返回第几周数字值
 */
export function getWeek(dateTime: Date): number {
  let temptTime = new Date(dateTime.getTime())
  // 周几
  let weekday = temptTime.getDay() || 7
  // 周1+5天=周六
  temptTime.setDate(temptTime.getDate() - weekday + 1 + 5)
  let firstDay = new Date(temptTime.getFullYear(), 0, 1)
  let dayOfWeek = firstDay.getDay()
  let spendDay = 1
  if (dayOfWeek != 0) spendDay = 7 - dayOfWeek + 1
  firstDay = new Date(temptTime.getFullYear(), 0, 1 + spendDay)
  let d = Math.ceil((temptTime.valueOf() - firstDay.valueOf()) / 86400000)
  let result = Math.ceil(d / 7)
  return result
}

/**
 * 将时间转换为 `几秒前`、`几分钟前`、`几小时前`、`几天前`
 * @param param 当前时间，new Date() 格式或者字符串时间格式
 * @param format 需要转换的时间格式字符串
 * @description param 10秒：  10 * 1000
 * @description param 1分：   60 * 1000
 * @description param 1小时： 60 * 60 * 1000
 * @description param 24小时：60 * 60 * 24 * 1000
 * @description param 3天：   60 * 60* 24 * 1000 * 3
 * @returns 返回拼接后的时间字符串
 */
export function formatPast(param: string | Date, format: string = 'YYYY-mm-dd'): string {
  // 传入格式处理、存储转换值
  let t: any, s: number
  // 获取js 时间戳
  let time: number = new Date().getTime()
  // 是否是对象
  typeof param === 'string' || 'object' ? (t = new Date(param).getTime()) : (t = param)
  // 当前时间戳 - 传入时间戳
  time = Number.parseInt(`${time - t}`)
  if (time < 10000) {
    // 10秒内
    return '刚刚'
  } else if (time < 60000 && time >= 10000) {
    // 超过10秒少于1分钟内
    s = Math.floor(time / 1000)
    return t('{s}秒前', { s: s })
  } else if (time < 3600000 && time >= 60000) {
    // 超过1分钟少于1小时
    s = Math.floor(time / 60000)
    return t('{s}分钟前', { s: s })
  } else if (time < 86400000 && time >= 3600000) {
    // 超过1小时少于24小时
    s = Math.floor(time / 3600000)
    return t('{s}小时前', { s: s })
  } else if (time < 259200000 && time >= 86400000) {
    // 超过1天少于3天内
    s = Math.floor(time / 86400000)
    return t('{s}天前', { s: s })
  } else if (time < 604800000 && time >= 259200000) {
    // 超过3天少于1周内
    s = Math.floor(time / 604800000)
    return t('{s}周前', { s: s })
  } else if (time < 31536000000 && time >= 604800000) {
    // 超过1周少于1年内
    s = Math.floor(time / 31536000000)
    return t('{s}月前', { s: s })
  } else if (time >= 31536000000) {
    // 超过1年
    s = Math.floor(time / 31536000000)
    return t('{s}年前', { s: s })
  } else {
    // 超过3天
    let date = typeof param === 'string' || 'object' ? new Date(param) : param
    return formatDate(date, format)
  }
}

/**
 * 时间问候语
 * @param param 当前时间，new Date() 格式
 * @description param 调用 `formatAxis(new Date())` 输出 `上午好`
 * @returns 返回拼接后的时间字符串
 */
export function formatAxis(param: Date): string {
  let hour: number = new Date(param).getHours()
  if (hour < 6) return t('凌晨好')
  else if (hour < 9) return t('早上好')
  else if (hour < 12) return t('上午好')
  else if (hour < 14) return t('中午好')
  else if (hour < 17) return t('下午好')
  else if (hour < 19) return t('傍晚好')
  else if (hour < 22) return t('晚上好')
  else return t('夜里好')
}
