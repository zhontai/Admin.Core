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
  ApiAddInput,
  ApiSetEnableParamsInput,
  ApiSetEnableResultInput,
  ApiSyncInput,
  ApiUpdateInput,
  PageInputApiGetPageDto,
  ResultOutputApiGetOutput,
  ResultOutputInt64,
  ResultOutputListApiGetEnumsOutput,
  ResultOutputListApiGetListOutput,
  ResultOutputListProjectConfig,
  ResultOutputPageOutputApiEntity,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class ApiApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags api
   * @name Get
   * @summary 查询
   * @request GET:/api/admin/api/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputApiGetOutput, any>({
      path: `/api/admin/api/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags api
   * @name GetList
   * @summary 查询列表
   * @request GET:/api/admin/api/get-list
   * @secure
   */
  getList = (
    query?: {
      key?: string
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputListApiGetListOutput, any>({
      path: `/api/admin/api/get-list`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags api
   * @name GetPage
   * @summary 查询分页
   * @request POST:/api/admin/api/get-page
   * @secure
   */
  getPage = (data: PageInputApiGetPageDto, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputApiEntity, any>({
      path: `/api/admin/api/get-page`,
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
   * @tags api
   * @name Add
   * @summary 添加
   * @request POST:/api/admin/api/add
   * @secure
   */
  add = (data: ApiAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/api/add`,
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
   * @tags api
   * @name Update
   * @summary 修改
   * @request PUT:/api/admin/api/update
   * @secure
   */
  update = (data: ApiUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/api/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags api
   * @name SetEnableParams
   * @summary 设置启用请求参数
   * @request POST:/api/admin/api/set-enable-params
   * @secure
   */
  setEnableParams = (data: ApiSetEnableParamsInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/api/set-enable-params`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags api
   * @name SetEnableResult
   * @summary 设置启用响应结果
   * @request POST:/api/admin/api/set-enable-result
   * @secure
   */
  setEnableResult = (data: ApiSetEnableResultInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/api/set-enable-result`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags api
   * @name Delete
   * @summary 彻底删除
   * @request DELETE:/api/admin/api/delete
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
      path: `/api/admin/api/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags api
   * @name BatchDelete
   * @summary 批量彻底删除
   * @request PUT:/api/admin/api/batch-delete
   * @secure
   */
  batchDelete = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/api/batch-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags api
   * @name SoftDelete
   * @summary 删除
   * @request DELETE:/api/admin/api/soft-delete
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
      path: `/api/admin/api/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags api
   * @name BatchSoftDelete
   * @summary 批量删除
   * @request PUT:/api/admin/api/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/api/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags api
   * @name Sync
   * @summary 同步
   * @request POST:/api/admin/api/sync
   * @secure
   */
  sync = (data: ApiSyncInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/api/sync`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags api
   * @name GetProjects
   * @summary 获得项目列表
   * @request GET:/api/admin/api/get-projects
   * @secure
   */
  getProjects = (params: RequestParams = {}) =>
    this.request<ResultOutputListProjectConfig, any>({
      path: `/api/admin/api/get-projects`,
      method: 'GET',
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags api
   * @name GetEnums
   * @summary 获得枚举列表
   * @request GET:/api/admin/api/get-enums
   * @secure
   */
  getEnums = (params: RequestParams = {}) =>
    this.request<ResultOutputListApiGetEnumsOutput, any>({
      path: `/api/admin/api/get-enums`,
      method: 'GET',
      secure: true,
      format: 'json',
      ...params,
    })
}
