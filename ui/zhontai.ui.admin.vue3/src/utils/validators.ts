import { verifyPasswordHybrid } from '/@/utils/toolsValidate'

/**
 * 密码
 */
export const validatorPwd = (rule: any, value: any, callback: any) => {
  if (!value) {
    callback()
  }
  if (!verifyPasswordHybrid(value)) {
    callback(new Error('字母+数字+可选特殊字符，长度在6-16之间'))
  } else {
    callback()
  }
}

/**
 * 同意
 */
export const validatorAgree = (rule: any, value: any, callback: any) => {
  if (value) {
    callback()
  } else {
    callback(new Error('请勾选同意协议'))
  }
}
