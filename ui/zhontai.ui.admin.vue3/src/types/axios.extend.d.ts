/* eslint-disable */
import * as axios from 'axios'

// 扩展 axios 数据返回类型，可自行扩展
declare module 'axios' {
  export interface AxiosResponse<T = any> {
    success: boolean
    msg: string
  }
}
