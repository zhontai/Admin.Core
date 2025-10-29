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
  DevGroupAddInput,
  DevGroupGetListInput,
  DevGroupUpdateInput,
  PageInputDevGroupGetPageInput,
  ResultOutputBoolean,
  ResultOutputDevGroupGetOutput,
  ResultOutputIEnumerableDevGroupGetListOutput,
  ResultOutputInt64,
  ResultOutputPageOutputDevGroupGetPageOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class DevGroupApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags dev-group
   * @name Get
   * @summary 查询
   * @request GET:/api/dev/dev-group/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputDevGroupGetOutput, any>({
      path: `/api/dev/dev-group/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-group
   * @name GetList
   * @summary 列表查询
   * @request POST:/api/dev/dev-group/get-list
   * @secure
   */
  getList = (data: DevGroupGetListInput, params: RequestParams = {}) =>
    this.request<ResultOutputIEnumerableDevGroupGetListOutput, any>({
      path: `/api/dev/dev-group/get-list`,
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
   * @tags dev-group
   * @name GetPage
   * @summary 分页查询
   * @request POST:/api/dev/dev-group/get-page
   * @secure
   */
  getPage = (data: PageInputDevGroupGetPageInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputDevGroupGetPageOutput, any>({
      path: `/api/dev/dev-group/get-page`,
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
   * @tags dev-group
   * @name Add
   * @summary 新增
   * @request POST:/api/dev/dev-group/add
   * @secure
   */
  add = (data: DevGroupAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/dev/dev-group/add`,
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
   * @tags dev-group
   * @name Update
   * @summary 更新
   * @request PUT:/api/dev/dev-group/update
   * @secure
   */
  update = (data: DevGroupUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/dev/dev-group/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-group
   * @name Delete
   * @summary 删除
   * @request DELETE:/api/dev/dev-group/delete
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
      path: `/api/dev/dev-group/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-group
   * @name BatchDelete
   * @summary 批量删除
   * @request PUT:/api/dev/dev-group/batch-delete
   * @secure
   */
  batchDelete = (data: number[], params: RequestParams = {}) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/dev/dev-group/batch-delete`,
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
   * @tags dev-group
   * @name SoftDelete
   * @summary 软删除
   * @request DELETE:/api/dev/dev-group/soft-delete
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
      path: `/api/dev/dev-group/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-group
   * @name BatchSoftDelete
   * @summary 批量软删除
   * @request PUT:/api/dev/dev-group/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/dev/dev-group/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
}
