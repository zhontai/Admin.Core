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
  PageInputPkgGetPageDto,
  PageInputPkgGetPkgTenantListInput,
  PkgAddInput,
  PkgAddPkgTenantListInput,
  PkgSetPkgPermissionsInput,
  PkgUpdateInput,
  ResultOutputInt64,
  ResultOutputListInt64,
  ResultOutputListPkgGetListOutput,
  ResultOutputListPkgGetPkgTenantListOutput,
  ResultOutputPageOutputPkgGetPageOutput,
  ResultOutputPageOutputPkgGetPkgTenantListOutput,
  ResultOutputPkgGetOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class PkgApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags pkg
   * @name Get
   * @summary 查询
   * @request GET:/api/admin/pkg/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputPkgGetOutput, any>({
      path: `/api/admin/pkg/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags pkg
   * @name GetList
   * @summary 查询列表
   * @request GET:/api/admin/pkg/get-list
   * @secure
   */
  getList = (
    query?: {
      /** 名称 */
      Name?: string
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputListPkgGetListOutput, any>({
      path: `/api/admin/pkg/get-list`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags pkg
   * @name GetPage
   * @summary 查询分页
   * @request POST:/api/admin/pkg/get-page
   * @secure
   */
  getPage = (data: PageInputPkgGetPageDto, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputPkgGetPageOutput, any>({
      path: `/api/admin/pkg/get-page`,
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
   * @tags pkg
   * @name GetPkgTenantList
   * @summary 查询套餐租户列表
   * @request GET:/api/admin/pkg/get-pkg-tenant-list
   * @secure
   */
  getPkgTenantList = (
    query?: {
      /** 租户名 */
      TenantName?: string
      /**
       * 套餐Id
       * @format int64
       */
      PkgId?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputListPkgGetPkgTenantListOutput, any>({
      path: `/api/admin/pkg/get-pkg-tenant-list`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags pkg
   * @name GetPkgTenantPage
   * @summary 查询套餐租户分页
   * @request POST:/api/admin/pkg/get-pkg-tenant-page
   * @secure
   */
  getPkgTenantPage = (data: PageInputPkgGetPkgTenantListInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputPkgGetPkgTenantListOutput, any>({
      path: `/api/admin/pkg/get-pkg-tenant-page`,
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
   * @tags pkg
   * @name GetPkgPermissionList
   * @summary 查询套餐权限列表
   * @request GET:/api/admin/pkg/get-pkg-permission-list
   * @secure
   */
  getPkgPermissionList = (
    query?: {
      /**
       * 套餐编号
       * @format int64
       */
      pkgId?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputListInt64, any>({
      path: `/api/admin/pkg/get-pkg-permission-list`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags pkg
   * @name SetPkgPermissions
   * @summary 设置套餐权限
   * @request POST:/api/admin/pkg/set-pkg-permissions
   * @secure
   */
  setPkgPermissions = (data: PkgSetPkgPermissionsInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/pkg/set-pkg-permissions`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags pkg
   * @name AddPkgTenant
   * @summary 添加套餐租户
   * @request POST:/api/admin/pkg/add-pkg-tenant
   * @secure
   */
  addPkgTenant = (data: PkgAddPkgTenantListInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/pkg/add-pkg-tenant`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags pkg
   * @name RemovePkgTenant
   * @summary 移除套餐租户
   * @request POST:/api/admin/pkg/remove-pkg-tenant
   * @secure
   */
  removePkgTenant = (data: PkgAddPkgTenantListInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/pkg/remove-pkg-tenant`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags pkg
   * @name Add
   * @summary 新增
   * @request POST:/api/admin/pkg/add
   * @secure
   */
  add = (data: PkgAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/pkg/add`,
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
   * @tags pkg
   * @name Update
   * @summary 修改
   * @request PUT:/api/admin/pkg/update
   * @secure
   */
  update = (data: PkgUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/pkg/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags pkg
   * @name Delete
   * @summary 彻底删除
   * @request DELETE:/api/admin/pkg/delete
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
      path: `/api/admin/pkg/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags pkg
   * @name BatchDelete
   * @summary 批量彻底删除
   * @request PUT:/api/admin/pkg/batch-delete
   * @secure
   */
  batchDelete = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/pkg/batch-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags pkg
   * @name SoftDelete
   * @summary 删除
   * @request DELETE:/api/admin/pkg/soft-delete
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
      path: `/api/admin/pkg/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags pkg
   * @name BatchSoftDelete
   * @summary 批量删除
   * @request PUT:/api/admin/pkg/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/pkg/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
}
