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

import { AxiosResponse } from 'axios'
import {
  PageInputSiteMsgGetPageInput,
  ResultOutputBoolean,
  ResultOutputPageOutputSiteMsgGetPageOutput,
  ResultOutputSiteMsgGetContentOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class SiteMsgApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags site-msg
   * @name GetContent
   * @summary 获得内容
   * @request GET:/api/admin/site-msg/get-content
   * @secure
   */
  getContent = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputSiteMsgGetContentOutput, any>({
      path: `/api/admin/site-msg/get-content`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags site-msg
   * @name GetPage
   * @summary 查询分页
   * @request POST:/api/admin/site-msg/get-page
   * @secure
   */
  getPage = (data: PageInputSiteMsgGetPageInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputSiteMsgGetPageOutput, any>({
      path: `/api/admin/site-msg/get-page`,
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
   * @tags site-msg
   * @name IsUnread
   * @summary 是否未读
   * @request GET:/api/admin/site-msg/is-unread
   * @secure
   */
  isUnread = (params: RequestParams = {}) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/admin/site-msg/is-unread`,
      method: 'GET',
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags site-msg
   * @name SetAllRead
   * @summary 全部标为已读
   * @request POST:/api/admin/site-msg/set-all-read
   * @secure
   */
  setAllRead = (params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/site-msg/set-all-read`,
      method: 'POST',
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags site-msg
   * @name SetRead
   * @summary 标为已读
   * @request POST:/api/admin/site-msg/set-read
   * @secure
   */
  setRead = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/site-msg/set-read`,
      method: 'POST',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags site-msg
   * @name BatchSetRead
   * @summary 批量标为已读
   * @request POST:/api/admin/site-msg/batch-set-read
   * @secure
   */
  batchSetRead = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/site-msg/batch-set-read`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags site-msg
   * @name SoftDelete
   * @summary 删除
   * @request DELETE:/api/admin/site-msg/soft-delete
   * @secure
   */
  softDelete = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/site-msg/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags site-msg
   * @name BatchSoftDelete
   * @summary 批量删除
   * @request POST:/api/admin/site-msg/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/site-msg/batch-soft-delete`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
}
