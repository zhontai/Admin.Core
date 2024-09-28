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
  OperationLogAddInput,
  PageInputOperationLogGetPageInput,
  ResultOutputInt64,
  ResultOutputPageOutputOperationLogGetPageOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class OperationLogApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags operation-log
   * @name GetPage
   * @summary 查询分页
   * @request POST:/api/admin/operation-log/get-page
   * @secure
   */
  getPage = (data: PageInputOperationLogGetPageInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputOperationLogGetPageOutput, any>({
      path: `/api/admin/operation-log/get-page`,
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
   * @tags operation-log
   * @name Add
   * @summary 新增
   * @request POST:/api/admin/operation-log/add
   * @secure
   */
  add = (data: OperationLogAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/operation-log/add`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
}
