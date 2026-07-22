/* eslint-disable */
/* tslint:disable */
// @ts-nocheck
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

import { ResultOutputSsoUserInfo, ResultOutputString, SsoTicketInput, SsoValidateInput } from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class SsoApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags sso
   * @name GenerateTicket
   * @summary 生成票据（已登录用户点击"单点登录"按钮时调用）
   * @request POST:/api/admin/sso/generate-ticket
   * @secure
   */
  generateTicket = (data: SsoTicketInput, params: RequestParams = {}) =>
    this.request<ResultOutputString, any>({
      path: `/api/admin/sso/generate-ticket`,
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
 * @tags sso
 * @name ValidateTicket
 * @summary 校验并消费票据（第三方系统后端调用，服务端对服务端）
校验签名、时间戳，消费后返回用户信息
 * @request POST:/api/admin/sso/validate-ticket
 * @secure
 */
  validateTicket = (data: SsoValidateInput, params: RequestParams = {}) =>
    this.request<ResultOutputSsoUserInfo, any>({
      path: `/api/admin/sso/validate-ticket`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
}
