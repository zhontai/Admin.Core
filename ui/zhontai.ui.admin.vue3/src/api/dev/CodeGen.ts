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
  CodeGenUpdateInput,
  ResultOutputBaseDataGetOutput,
  ResultOutputCodeGenGetOutput,
  ResultOutputIEnumerableCodeGenGetOutput,
  ResultOutputString,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class CodeGenApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags code-gen
   * @name GetBaseData
   * @summary 获取数据库列表
   * @request GET:/api/dev/code-gen/get-base-data
   * @secure
   */
  getBaseData = (params: RequestParams = {}) =>
    this.request<ResultOutputBaseDataGetOutput, any>({
      path: `/api/dev/code-gen/get-base-data`,
      method: 'GET',
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags code-gen
   * @name GetTables
   * @summary 获取表列表
   * @request GET:/api/dev/code-gen/get-tables
   * @secure
   */
  getTables = (
    query?: {
      dbkey?: string
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputIEnumerableCodeGenGetOutput, any>({
      path: `/api/dev/code-gen/get-tables`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags code-gen
   * @name GetList
   * @summary 获取列表
   * @request GET:/api/dev/code-gen/get-list
   * @secure
   */
  getList = (
    query?: {
      dbkey?: string
      tableName?: string
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputIEnumerableCodeGenGetOutput, any>({
      path: `/api/dev/code-gen/get-list`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags code-gen
   * @name Get
   * @summary 查询
   * @request GET:/api/dev/code-gen/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputCodeGenGetOutput, any>({
      path: `/api/dev/code-gen/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags code-gen
   * @name Delete
   * @summary 删除
   * @request DELETE:/api/dev/code-gen/delete
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
      path: `/api/dev/code-gen/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags code-gen
   * @name Update
   * @summary 更新
   * @request POST:/api/dev/code-gen/update
   * @secure
   */
  update = (data: CodeGenUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/dev/code-gen/update`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags code-gen
   * @name BatchGenerate
   * @summary 批量生成
   * @request POST:/api/dev/code-gen/batch-generate
   * @secure
   */
  batchGenerate = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/dev/code-gen/batch-generate`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags code-gen
   * @name Generate
   * @summary 生成
   * @request GET:/api/dev/code-gen/generate
   * @secure
   */
  generate = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<AxiosResponse, any>({
      path: `/api/dev/code-gen/generate`,
      method: 'GET',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags code-gen
   * @name Compile
   * @summary 生成迁移代码
   * @request GET:/api/dev/code-gen/compile
   * @secure
   */
  compile = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputString, any>({
      path: `/api/dev/code-gen/compile`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags code-gen
   * @name GenCompile
   * @summary 执行迁移到数据库
   * @request PUT:/api/dev/code-gen/gen-compile
   * @secure
   */
  genCompile = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<AxiosResponse, any>({
      path: `/api/dev/code-gen/gen-compile`,
      method: 'PUT',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags code-gen
   * @name GenMenu
   * @summary 生成菜单
   * @request PUT:/api/dev/code-gen/gen-menu
   * @secure
   */
  genMenu = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<AxiosResponse, any>({
      path: `/api/dev/code-gen/gen-menu`,
      method: 'PUT',
      query: query,
      secure: true,
      ...params,
    })
}
