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
import { OrgAddInput, OrgUpdateInput, ResultOutputInt64, ResultOutputListOrgListOutput, ResultOutputOrgGetOutput } from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class OrgApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags org
   * @name Get
   * @summary 查询
   * @request GET:/api/admin/org/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputOrgGetOutput, any>({
      path: `/api/admin/org/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags org
   * @name GetList
   * @summary 查询列表
   * @request GET:/api/admin/org/get-list
   * @secure
   */
  getList = (
    query?: {
      key?: string
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputListOrgListOutput, any>({
      path: `/api/admin/org/get-list`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags org
   * @name Add
   * @summary 新增
   * @request POST:/api/admin/org/add
   * @secure
   */
  add = (data: OrgAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/org/add`,
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
   * @tags org
   * @name Update
   * @summary 修改
   * @request PUT:/api/admin/org/update
   * @secure
   */
  update = (data: OrgUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/org/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags org
   * @name Delete
   * @summary 彻底删除
   * @request DELETE:/api/admin/org/delete
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
      path: `/api/admin/org/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags org
   * @name SoftDelete
   * @summary 删除
   * @request DELETE:/api/admin/org/soft-delete
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
      path: `/api/admin/org/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
}
