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
  PageInputRegionGetPageInput,
  RegionAddInput,
  RegionGetListInput,
  RegionLevel,
  RegionSetEnableInput,
  RegionSetHotInput,
  RegionUpdateInput,
  ResultOutputInt64,
  ResultOutputListRegionGetChildListOutput,
  ResultOutputPageOutputRegionGetPageOutput,
  ResultOutputRegionGetOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class RegionApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags region
   * @name Get
   * @summary 查询
   * @request GET:/api/admin/region/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputRegionGetOutput, any>({
      path: `/api/admin/region/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags region
   * @name GetChildList
   * @summary 查询下级列表
   * @request POST:/api/admin/region/get-child-list
   * @secure
   */
  getChildList = (data: RegionGetListInput, params: RequestParams = {}) =>
    this.request<ResultOutputListRegionGetChildListOutput, any>({
      path: `/api/admin/region/get-child-list`,
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
   * @tags region
   * @name GetPage
   * @summary 查询分页
   * @request POST:/api/admin/region/get-page
   * @secure
   */
  getPage = (data: PageInputRegionGetPageInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputRegionGetPageOutput, any>({
      path: `/api/admin/region/get-page`,
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
   * @tags region
   * @name Add
   * @summary 新增
   * @request POST:/api/admin/region/add
   * @secure
   */
  add = (data: RegionAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/region/add`,
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
   * @tags region
   * @name Update
   * @summary 修改
   * @request PUT:/api/admin/region/update
   * @secure
   */
  update = (data: RegionUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/region/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags region
   * @name Delete
   * @summary 彻底删除
   * @request DELETE:/api/admin/region/delete
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
      path: `/api/admin/region/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags region
   * @name SoftDelete
   * @summary 删除
   * @request DELETE:/api/admin/region/soft-delete
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
      path: `/api/admin/region/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags region
   * @name SetEnable
   * @summary 设置启用
   * @request POST:/api/admin/region/set-enable
   * @secure
   */
  setEnable = (data: RegionSetEnableInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/region/set-enable`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags region
   * @name SetHot
   * @summary 设置热门
   * @request POST:/api/admin/region/set-hot
   * @secure
   */
  setHot = (data: RegionSetHotInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/region/set-hot`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags region
   * @name SyncData
   * @summary 同步数据
   * @request POST:/api/admin/region/sync-data
   * @secure
   */
  syncData = (data: RegionLevel, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/region/sync-data`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
}
