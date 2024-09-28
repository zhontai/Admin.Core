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
  ExportInput,
  PageInputDictGetPageInput,
  ResultOutputDictGetOutput,
  ResultOutputDictionaryStringListDictGetListDto,
  ResultOutputImportOutput,
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
  getPage = (data: PageInputDictGetPageInput, params: RequestParams = {}) =>
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
   * @name DownloadTemplate
   * @summary 下载导入模板
   * @request POST:/api/admin/dict/download-template
   * @secure
   */
  downloadTemplate = (params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/dict/download-template`,
      method: 'POST',
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags dict
   * @name DownloadErrorMark
   * @summary 下载错误标记文件
   * @request POST:/api/admin/dict/download-error-mark
   * @secure
   */
  downloadErrorMark = (
    query?: {
      fileId?: string
      fileName?: string
    },
    params: RequestParams = {}
  ) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/dict/download-error-mark`,
      method: 'POST',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags dict
   * @name ExportData
   * @summary 导出数据
   * @request POST:/api/admin/dict/export-data
   * @secure
   */
  exportData = (data: ExportInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/dict/export-data`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags dict
   * @name ImportData
   * @summary 导入数据
   * @request POST:/api/admin/dict/import-data
   * @secure
   */
  importData = (
    data: {
      /** @format binary */
      file: File
    },
    query?: {
      /** @format int32 */
      duplicateAction?: number
      fileId?: string
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputImportOutput, any>({
      path: `/api/admin/dict/import-data`,
      method: 'POST',
      query: query,
      body: data,
      secure: true,
      type: ContentType.FormData,
      format: 'json',
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
