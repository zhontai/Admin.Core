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
  DevTemplateAddInput,
  DevTemplateGetListInput,
  DevTemplateUpdateInput,
  PageInputDevTemplateGetPageInput,
  ResultOutputBoolean,
  ResultOutputDevTemplateGetOutput,
  ResultOutputIEnumerableDevTemplateGetListOutput,
  ResultOutputInt64,
  ResultOutputPageOutputDevTemplateGetPageOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class DevTemplateApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags dev-template
   * @name Get
   * @summary 查询
   * @request GET:/api/dev/dev-template/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputDevTemplateGetOutput, any>({
      path: `/api/dev/dev-template/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-template
   * @name GetList
   * @summary 列表查询
   * @request POST:/api/dev/dev-template/get-list
   * @secure
   */
  getList = (data: DevTemplateGetListInput, params: RequestParams = {}) =>
    this.request<ResultOutputIEnumerableDevTemplateGetListOutput, any>({
      path: `/api/dev/dev-template/get-list`,
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
   * @tags dev-template
   * @name GetPage
   * @summary 分页查询
   * @request POST:/api/dev/dev-template/get-page
   * @secure
   */
  getPage = (data: PageInputDevTemplateGetPageInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputDevTemplateGetPageOutput, any>({
      path: `/api/dev/dev-template/get-page`,
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
   * @tags dev-template
   * @name Add
   * @summary 新增
   * @request POST:/api/dev/dev-template/add
   * @secure
   */
  add = (data: DevTemplateAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/dev/dev-template/add`,
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
   * @tags dev-template
   * @name Update
   * @summary 更新
   * @request PUT:/api/dev/dev-template/update
   * @secure
   */
  update = (data: DevTemplateUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/dev/dev-template/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-template
   * @name Delete
   * @summary 删除
   * @request DELETE:/api/dev/dev-template/delete
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
      path: `/api/dev/dev-template/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-template
   * @name BatchDelete
   * @summary 批量删除
   * @request PUT:/api/dev/dev-template/batch-delete
   * @secure
   */
  batchDelete = (data: number[], params: RequestParams = {}) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/dev/dev-template/batch-delete`,
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
   * @tags dev-template
   * @name SoftDelete
   * @summary 软删除
   * @request DELETE:/api/dev/dev-template/soft-delete
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
      path: `/api/dev/dev-template/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-template
   * @name BatchSoftDelete
   * @summary 批量软删除
   * @request PUT:/api/dev/dev-template/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/dev/dev-template/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
}
