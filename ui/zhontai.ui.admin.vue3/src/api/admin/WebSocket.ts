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

import { ResultOutputBoolean, ResultOutputObject, WebSocketPreConnectInput } from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class WebSocketApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags web-socket
   * @name PreConnect
   * @summary 获取websocket分区
   * @request POST:/api/admin/web-socket/pre-connect
   * @secure
   */
  preConnect = (data: WebSocketPreConnectInput, params: RequestParams = {}) =>
    this.request<ResultOutputObject, any>({
      path: `/api/admin/web-socket/pre-connect`,
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
   * @tags web-socket
   * @name IsUseIm
   * @summary 是否使用im
   * @request GET:/api/admin/web-socket/is-use-im
   * @secure
   */
  isUseIm = (params: RequestParams = {}) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/admin/web-socket/is-use-im`,
      method: 'GET',
      secure: true,
      format: 'json',
      ...params,
    })
}
