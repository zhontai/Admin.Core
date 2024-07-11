/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

import {
  ResultOutputCaptchaData,
  ResultOutputString,
  ResultOutputValidateResult,
  SendEmailCodeInput,
  SendSmsCodeInput,
  SlideTrack,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class CaptchaApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags captcha
   * @name Generate
   * @summary 生成
   * @request POST:/api/admin/captcha/generate
   * @secure
   */
  generate = (
    query?: {
      /** 验证码id */
      captchaId?: string
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputCaptchaData, any>({
      path: `/api/admin/captcha/generate`,
      method: 'POST',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags captcha
   * @name Check
   * @summary 验证
   * @request POST:/api/admin/captcha/check
   * @secure
   */
  check = (
    data: SlideTrack,
    query?: {
      /** 验证码Id */
      captchaId?: string
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputValidateResult, any>({
      path: `/api/admin/captcha/check`,
      method: 'POST',
      query: query,
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags captcha
   * @name SendSmsCode
   * @summary 发送短信验证码
   * @request POST:/api/admin/captcha/send-sms-code
   * @secure
   */
  sendSmsCode = (data: SendSmsCodeInput, params: RequestParams = {}) =>
    this.request<ResultOutputString, any>({
      path: `/api/admin/captcha/send-sms-code`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags captcha
   * @name SendEmailCode
   * @summary 发送邮件验证码
   * @request POST:/api/admin/captcha/send-email-code
   * @secure
   */
  sendEmailCode = (data: SendEmailCodeInput, params: RequestParams = {}) =>
    this.request<ResultOutputString, any>({
      path: `/api/admin/captcha/send-email-code`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
}
