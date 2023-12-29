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
  DictTypeAddInput,
  DictTypeUpdateInput,
  PageInputDictTypeGetPageDto,
  ResultOutputDictTypeGetOutput,
  ResultOutputInt64,
  ResultOutputPageOutputDictTypeGetPageOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class DictTypeApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags dict-type
   * @name Get
   * @summary 查询
   * @request GET:/api/admin/dict-type/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputDictTypeGetOutput, any>({
      path: `/api/admin/dict-type/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags dict-type
   * @name GetPage
   * @summary 查询分页
   * @request POST:/api/admin/dict-type/get-page
   * @secure
   */
  getPage = (data: PageInputDictTypeGetPageDto, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputDictTypeGetPageOutput, any>({
      path: `/api/admin/dict-type/get-page`,
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
   * @tags dict-type
   * @name Add
   * @summary 新增
   * @request POST:/api/admin/dict-type/add
   * @secure
   */
  add = (data: DictTypeAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/dict-type/add`,
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
   * @tags dict-type
   * @name Update
   * @summary 修改
   * @request PUT:/api/admin/dict-type/update
   * @secure
   */
  update = (data: DictTypeUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/dict-type/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags dict-type
   * @name Delete
   * @summary 彻底删除
   * @request DELETE:/api/admin/dict-type/delete
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
      path: `/api/admin/dict-type/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags dict-type
   * @name BatchDelete
   * @summary 批量彻底删除
   * @request PUT:/api/admin/dict-type/batch-delete
   * @secure
   */
  batchDelete = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/dict-type/batch-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags dict-type
   * @name SoftDelete
   * @summary 删除
   * @request DELETE:/api/admin/dict-type/soft-delete
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
      path: `/api/admin/dict-type/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags dict-type
   * @name BatchSoftDelete
   * @summary 批量删除
   * @request PUT:/api/admin/dict-type/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/dict-type/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
}
