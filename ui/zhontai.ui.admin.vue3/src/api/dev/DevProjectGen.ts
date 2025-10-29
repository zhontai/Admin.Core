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
  DevProjectGenAddInput,
  DevProjectGenGenerateInput,
  DevProjectGenGetListInput,
  DevProjectGenPreviewMenuInput,
  DevProjectGenUpdateInput,
  PageInputDevProjectGenGetPageInput,
  ResultOutputActionResult,
  ResultOutputBoolean,
  ResultOutputDevProjectGenGetOutput,
  ResultOutputIEnumerableDevProjectGenGetListOutput,
  ResultOutputIEnumerableDevProjectGenPreviewMenuOutput,
  ResultOutputInt64,
  ResultOutputListDevProjectGenGenerateOutput,
  ResultOutputPageOutputDevProjectGenGetPageOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class DevProjectGenApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags dev-project-gen
   * @name Get
   * @summary 查询
   * @request GET:/api/dev/dev-project-gen/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputDevProjectGenGetOutput, any>({
      path: `/api/dev/dev-project-gen/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-project-gen
   * @name GetList
   * @summary 列表查询
   * @request POST:/api/dev/dev-project-gen/get-list
   * @secure
   */
  getList = (data: DevProjectGenGetListInput, params: RequestParams = {}) =>
    this.request<ResultOutputIEnumerableDevProjectGenGetListOutput, any>({
      path: `/api/dev/dev-project-gen/get-list`,
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
   * @tags dev-project-gen
   * @name GetPage
   * @summary 分页查询
   * @request POST:/api/dev/dev-project-gen/get-page
   * @secure
   */
  getPage = (data: PageInputDevProjectGenGetPageInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputDevProjectGenGetPageOutput, any>({
      path: `/api/dev/dev-project-gen/get-page`,
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
   * @tags dev-project-gen
   * @name Add
   * @summary 新增
   * @request POST:/api/dev/dev-project-gen/add
   * @secure
   */
  add = (data: DevProjectGenAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/dev/dev-project-gen/add`,
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
   * @tags dev-project-gen
   * @name Update
   * @summary 更新
   * @request PUT:/api/dev/dev-project-gen/update
   * @secure
   */
  update = (data: DevProjectGenUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/dev/dev-project-gen/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-project-gen
   * @name Delete
   * @summary 删除
   * @request DELETE:/api/dev/dev-project-gen/delete
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
      path: `/api/dev/dev-project-gen/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-project-gen
   * @name BatchDelete
   * @summary 批量删除
   * @request PUT:/api/dev/dev-project-gen/batch-delete
   * @secure
   */
  batchDelete = (data: number[], params: RequestParams = {}) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/dev/dev-project-gen/batch-delete`,
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
   * @tags dev-project-gen
   * @name BatchSoftDelete
   * @summary 批量软删除
   * @request PUT:/api/dev/dev-project-gen/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/dev/dev-project-gen/batch-soft-delete`,
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
   * @tags dev-project-gen
   * @name GetPreviewMenu
   * @summary 生成预览菜单
   * @request POST:/api/dev/dev-project-gen/get-preview-menu
   * @secure
   */
  getPreviewMenu = (data: DevProjectGenPreviewMenuInput, params: RequestParams = {}) =>
    this.request<ResultOutputIEnumerableDevProjectGenPreviewMenuOutput, any>({
      path: `/api/dev/dev-project-gen/get-preview-menu`,
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
   * @tags dev-project-gen
   * @name Generate
   * @summary 生成
   * @request POST:/api/dev/dev-project-gen/generate
   * @secure
   */
  generate = (data: DevProjectGenGenerateInput, params: RequestParams = {}) =>
    this.request<ResultOutputListDevProjectGenGenerateOutput, any>({
      path: `/api/dev/dev-project-gen/generate`,
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
   * @tags dev-project-gen
   * @name Down
   * @summary 下载
   * @request POST:/api/dev/dev-project-gen/down
   * @secure
   */
  down = (data: DevProjectGenGenerateInput, params: RequestParams = {}) =>
    this.request<ResultOutputActionResult, any>({
      path: `/api/dev/dev-project-gen/down`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
}
