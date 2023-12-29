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

import { OprationLogAddInput, PageInputLogGetPageDto, ResultOutputInt64, ResultOutputPageOutputOprationLogListOutput } from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class OprationLogApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags opration-log
   * @name GetPage
   * @summary 查询分页
   * @request POST:/api/admin/opration-log/get-page
   * @secure
   */
  getPage = (data: PageInputLogGetPageDto, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputOprationLogListOutput, any>({
      path: `/api/admin/opration-log/get-page`,
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
   * @tags opration-log
   * @name Add
   * @summary 新增
   * @request POST:/api/admin/opration-log/add
   * @secure
   */
  add = (data: OprationLogAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/opration-log/add`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
}
