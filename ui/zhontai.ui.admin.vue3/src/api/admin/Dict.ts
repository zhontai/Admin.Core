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
  DictAddInput,
  DictUpdateInput,
  PageInputDictGetPageDto,
  ResultOutputDictGetOutput,
  ResultOutputDictionaryStringListDictGetListDto,
  ResultOutputInt64,
  ResultOutputPageOutputDictGetPageOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class DictApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags dict
   * @name Get
   * @summary 查询
   * @request GET:/api/admin/dict/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputDictGetOutput, any>({
      path: `/api/admin/dict/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags dict
   * @name GetPage
   * @summary 查询分页
   * @request POST:/api/admin/dict/get-page
   * @secure
   */
  getPage = (data: PageInputDictGetPageDto, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputDictGetPageOutput, any>({
      path: `/api/admin/dict/get-page`,
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
   * @tags dict
   * @name GetList
   * @summary 通过类型编码查询列表
   * @request POST:/api/admin/dict/get-list
   * @secure
   */
  getList = (data: string[], params: RequestParams = {}) =>
    this.request<ResultOutputDictionaryStringListDictGetListDto, any>({
      path: `/api/admin/dict/get-list`,
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
   * @tags dict
   * @name GetListByNames
   * @summary 通过类型名称查询列表
   * @request POST:/api/admin/dict/get-list-by-names
   * @secure
   */
  getListByNames = (data: string[], params: RequestParams = {}) =>
    this.request<ResultOutputDictionaryStringListDictGetListDto, any>({
      path: `/api/admin/dict/get-list-by-names`,
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
   * @tags dict
   * @name ExportList
   * @summary 导出列表
   * @request GET:/api/admin/dict/export-list
   * @secure
   */
  exportList = (params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/dict/export-list`,
      method: 'GET',
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags dict
   * @name Add
   * @summary 新增
   * @request POST:/api/admin/dict/add
   * @secure
   */
  add = (data: DictAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/dict/add`,
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
   * @tags dict
   * @name Update
   * @summary 修改
   * @request PUT:/api/admin/dict/update
   * @secure
   */
  update = (data: DictUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/dict/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags dict
   * @name Delete
   * @summary 彻底删除
   * @request DELETE:/api/admin/dict/delete
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
      path: `/api/admin/dict/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags dict
   * @name BatchDelete
   * @summary 批量彻底删除
   * @request PUT:/api/admin/dict/batch-delete
   * @secure
   */
  batchDelete = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/dict/batch-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags dict
   * @name SoftDelete
   * @summary 删除
   * @request DELETE:/api/admin/dict/soft-delete
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
      path: `/api/admin/dict/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags dict
   * @name BatchSoftDelete
   * @summary 批量删除
   * @request PUT:/api/admin/dict/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/dict/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
}
