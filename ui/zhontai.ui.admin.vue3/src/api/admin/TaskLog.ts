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

import { PageInputTaskLogGetPageInput, ResultOutputPageOutputTaskLog } from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class TaskLogApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags task-log
   * @name GetPage
   * @summary 查询分页
   * @request POST:/api/admin/task-log/get-page
   * @secure
   */
  getPage = (data: PageInputTaskLogGetPageInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputTaskLog, any>({
      path: `/api/admin/task-log/get-page`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
}
