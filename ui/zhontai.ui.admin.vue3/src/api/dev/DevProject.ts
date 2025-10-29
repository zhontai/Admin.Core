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
  DevProjectAddInput,
  DevProjectGetListInput,
  DevProjectUpdateInput,
  PageInputDevProjectGetPageInput,
  ResultOutputBoolean,
  ResultOutputDevProjectGetOutput,
  ResultOutputIEnumerableDevProjectGetListOutput,
  ResultOutputInt64,
  ResultOutputPageOutputDevProjectGetPageOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class DevProjectApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags dev-project
   * @name Get
   * @summary 查询
   * @request GET:/api/dev/dev-project/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputDevProjectGetOutput, any>({
      path: `/api/dev/dev-project/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-project
   * @name GetList
   * @summary 列表查询
   * @request POST:/api/dev/dev-project/get-list
   * @secure
   */
  getList = (data: DevProjectGetListInput, params: RequestParams = {}) =>
    this.request<ResultOutputIEnumerableDevProjectGetListOutput, any>({
      path: `/api/dev/dev-project/get-list`,
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
   * @tags dev-project
   * @name GetPage
   * @summary 分页查询
   * @request POST:/api/dev/dev-project/get-page
   * @secure
   */
  getPage = (data: PageInputDevProjectGetPageInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputDevProjectGetPageOutput, any>({
      path: `/api/dev/dev-project/get-page`,
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
   * @tags dev-project
   * @name Add
   * @summary 新增
   * @request POST:/api/dev/dev-project/add
   * @secure
   */
  add = (data: DevProjectAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/dev/dev-project/add`,
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
   * @tags dev-project
   * @name Update
   * @summary 更新
   * @request PUT:/api/dev/dev-project/update
   * @secure
   */
  update = (data: DevProjectUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/dev/dev-project/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-project
   * @name Delete
   * @summary 删除
   * @request DELETE:/api/dev/dev-project/delete
   * @secure
   */
  delete = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/dev/dev-project/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-project
   * @name BatchDelete
   * @summary 批量删除
   * @request PUT:/api/dev/dev-project/batch-delete
   * @secure
   */
  batchDelete = (data: number[], params: RequestParams = {}) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/dev/dev-project/batch-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-project
   * @name SoftDelete
   * @summary 软删除
   * @request DELETE:/api/dev/dev-project/soft-delete
   * @secure
   */
  softDelete = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/dev/dev-project/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-project
   * @name BatchSoftDelete
   * @summary 批量软删除
   * @request PUT:/api/dev/dev-project/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/dev/dev-project/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-project
   * @name BatchGenerate
   * @summary 批量生成
   * @request POST:/api/dev/dev-project/batch-generate
   * @secure
   */
  batchGenerate = (
    data: number[],
    query?: {
      /**
       * 模板组
       * @format int64
       */
      groupId?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<AxiosResponse, any>({
      path: `/api/dev/dev-project/batch-generate`,
      method: 'POST',
      query: query,
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-project
   * @name Generate
   * @summary 生成
   * @request GET:/api/dev/dev-project/generate
   * @secure
   */
  generate = (
    query?: {
      /**
       * 项目ID
       * @format int64
       */
      id?: number
      /**
       * 模板组
       * @format int64
       */
      groupId?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<AxiosResponse, any>({
      path: `/api/dev/dev-project/generate`,
      method: 'GET',
      query: query,
      secure: true,
      ...params,
    })
}
