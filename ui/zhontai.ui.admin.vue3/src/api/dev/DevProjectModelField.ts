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
  DevProjectModelFieldAddInput,
  DevProjectModelFieldGetListInput,
  DevProjectModelFieldUpdateInput,
  PageInputDevProjectModelFieldGetPageInput,
  ResultOutputBoolean,
  ResultOutputDevProjectModelFieldGetOutput,
  ResultOutputIEnumerableDevProjectModelFieldGetListOutput,
  ResultOutputInt64,
  ResultOutputPageOutputDevProjectModelFieldGetPageOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class DevProjectModelFieldApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags dev-project-model-field
   * @name Get
   * @summary 查询
   * @request GET:/api/dev/dev-project-model-field/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputDevProjectModelFieldGetOutput, any>({
      path: `/api/dev/dev-project-model-field/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-project-model-field
   * @name GetList
   * @summary 列表查询
   * @request POST:/api/dev/dev-project-model-field/get-list
   * @secure
   */
  getList = (data: DevProjectModelFieldGetListInput, params: RequestParams = {}) =>
    this.request<ResultOutputIEnumerableDevProjectModelFieldGetListOutput, any>({
      path: `/api/dev/dev-project-model-field/get-list`,
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
   * @tags dev-project-model-field
   * @name GetPage
   * @summary 分页查询
   * @request POST:/api/dev/dev-project-model-field/get-page
   * @secure
   */
  getPage = (data: PageInputDevProjectModelFieldGetPageInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputDevProjectModelFieldGetPageOutput, any>({
      path: `/api/dev/dev-project-model-field/get-page`,
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
   * @tags dev-project-model-field
   * @name Add
   * @summary 新增
   * @request POST:/api/dev/dev-project-model-field/add
   * @secure
   */
  add = (data: DevProjectModelFieldAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/dev/dev-project-model-field/add`,
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
   * @tags dev-project-model-field
   * @name Update
   * @summary 更新
   * @request PUT:/api/dev/dev-project-model-field/update
   * @secure
   */
  update = (data: DevProjectModelFieldUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/dev/dev-project-model-field/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-project-model-field
   * @name Delete
   * @summary 删除
   * @request DELETE:/api/dev/dev-project-model-field/delete
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
      path: `/api/dev/dev-project-model-field/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-project-model-field
   * @name BatchDelete
   * @summary 批量删除
   * @request PUT:/api/dev/dev-project-model-field/batch-delete
   * @secure
   */
  batchDelete = (data: number[], params: RequestParams = {}) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/dev/dev-project-model-field/batch-delete`,
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
   * @tags dev-project-model-field
   * @name SoftDelete
   * @summary 软删除
   * @request DELETE:/api/dev/dev-project-model-field/soft-delete
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
      path: `/api/dev/dev-project-model-field/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags dev-project-model-field
   * @name BatchSoftDelete
   * @summary 批量软删除
   * @request PUT:/api/dev/dev-project-model-field/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/dev/dev-project-model-field/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
}
