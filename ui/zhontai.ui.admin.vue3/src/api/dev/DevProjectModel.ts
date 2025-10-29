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
  DevProjectModelAddInput,
  DevProjectModelGetListInput,
  DevProjectModelUpdateInput,
  PageInputDevProjectModelGetPageInput,
  ResultOutputBoolean,
  ResultOutputDevProjectModelGetOutput,
  ResultOutputIEnumerableDevProjectModelGetListOutput,
  ResultOutputInt64,
  ResultOutputPageOutputDevProjectModelGetPageOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class DevProjectModelApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags dev-project-model
   * @name Get
   * @summary 查询
   * @request GET:/api/dev/dev-project-model/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputDevProjectModelGetOutput, any>({
      path: `/api/dev/dev-project-model/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-project-model
   * @name GetList
   * @summary 列表查询
   * @request POST:/api/dev/dev-project-model/get-list
   * @secure
   */
  getList = (data: DevProjectModelGetListInput, params: RequestParams = {}) =>
    this.request<ResultOutputIEnumerableDevProjectModelGetListOutput, any>({
      path: `/api/dev/dev-project-model/get-list`,
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
   * @tags dev-project-model
   * @name GetPage
   * @summary 分页查询
   * @request POST:/api/dev/dev-project-model/get-page
   * @secure
   */
  getPage = (data: PageInputDevProjectModelGetPageInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputDevProjectModelGetPageOutput, any>({
      path: `/api/dev/dev-project-model/get-page`,
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
   * @tags dev-project-model
   * @name Add
   * @summary 新增
   * @request POST:/api/dev/dev-project-model/add
   * @secure
   */
  add = (data: DevProjectModelAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/dev/dev-project-model/add`,
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
   * @tags dev-project-model
   * @name Update
   * @summary 更新
   * @request PUT:/api/dev/dev-project-model/update
   * @secure
   */
  update = (data: DevProjectModelUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/dev/dev-project-model/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-project-model
   * @name Delete
   * @summary 删除
   * @request DELETE:/api/dev/dev-project-model/delete
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
      path: `/api/dev/dev-project-model/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-project-model
   * @name BatchDelete
   * @summary 批量删除
   * @request PUT:/api/dev/dev-project-model/batch-delete
   * @secure
   */
  batchDelete = (data: number[], params: RequestParams = {}) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/dev/dev-project-model/batch-delete`,
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
   * @tags dev-project-model
   * @name SoftDelete
   * @summary 软删除
   * @request DELETE:/api/dev/dev-project-model/soft-delete
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
      path: `/api/dev/dev-project-model/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-project-model
   * @name BatchSoftDelete
   * @summary 批量软删除
   * @request PUT:/api/dev/dev-project-model/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/dev/dev-project-model/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
}
