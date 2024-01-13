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

import { AxiosResponse } from 'axios'
import {
  ResultOutputInt64,
  ResultOutputListViewListOutput,
  ResultOutputViewGetOutput,
  ViewAddInput,
  ViewSyncInput,
  ViewUpdateInput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class ViewApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags view
   * @name Get
   * @summary 查询
   * @request GET:/api/admin/view/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputViewGetOutput, any>({
      path: `/api/admin/view/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags view
   * @name GetList
   * @summary 查询列表
   * @request GET:/api/admin/view/get-list
   * @secure
   */
  getList = (
    query?: {
      key?: string
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputListViewListOutput, any>({
      path: `/api/admin/view/get-list`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags view
   * @name Add
   * @summary 新增
   * @request POST:/api/admin/view/add
   * @secure
   */
  add = (data: ViewAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/view/add`,
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
   * @tags view
   * @name Update
   * @summary 修改
   * @request PUT:/api/admin/view/update
   * @secure
   */
  update = (data: ViewUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/view/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags view
   * @name Delete
   * @summary 彻底删除
   * @request DELETE:/api/admin/view/delete
   * @secure
   */
  delete = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/view/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags view
   * @name BatchDelete
   * @summary 批量彻底删除
   * @request PUT:/api/admin/view/batch-delete
   * @secure
   */
  batchDelete = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/view/batch-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags view
   * @name SoftDelete
   * @summary 删除
   * @request DELETE:/api/admin/view/soft-delete
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
      path: `/api/admin/view/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags view
   * @name BatchSoftDelete
   * @summary 批量删除
   * @request PUT:/api/admin/view/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/view/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags view
   * @name Sync
   * @summary 同步
   * @request POST:/api/admin/view/sync
   * @secure
   */
  sync = (data: ViewSyncInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/view/sync`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
}
