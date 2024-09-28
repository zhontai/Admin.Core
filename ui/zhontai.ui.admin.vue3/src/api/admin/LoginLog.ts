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

import { LoginLogAddInput, PageInputLoginLogGetPageInput, ResultOutputInt64, ResultOutputPageOutputLoginLogGetPageOutput } from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class LoginLogApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags login-log
   * @name GetPage
   * @summary 查询分页
   * @request POST:/api/admin/login-log/get-page
   * @secure
   */
  getPage = (data: PageInputLoginLogGetPageInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputLoginLogGetPageOutput, any>({
      path: `/api/admin/login-log/get-page`,
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
   * @tags login-log
   * @name Add
   * @summary 新增
   * @request POST:/api/admin/login-log/add
   * @secure
   */
  add = (data: LoginLogAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/login-log/add`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
}
