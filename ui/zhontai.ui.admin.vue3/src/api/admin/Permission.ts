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
  PermissionAddApiInput,
  PermissionAddDotInput,
  PermissionAddGroupInput,
  PermissionAddMenuInput,
  PermissionAssignInput,
  PermissionSaveTenantPermissionsInput,
  PermissionUpdateDotInput,
  PermissionUpdateGroupInput,
  PermissionUpdateMenuInput,
  ResultOutputIEnumerableObject,
  ResultOutputInt64,
  ResultOutputListInt64,
  ResultOutputListPermissionListOutput,
  ResultOutputPermissionGetDotOutput,
  ResultOutputPermissionGetGroupOutput,
  ResultOutputPermissionGetMenuOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class PermissionApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags permission
   * @name GetGroup
   * @summary 查询分组
   * @request GET:/api/admin/permission/get-group
   * @secure
   */
  getGroup = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputPermissionGetGroupOutput, any>({
      path: `/api/admin/permission/get-group`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags permission
   * @name GetMenu
   * @summary 查询菜单
   * @request GET:/api/admin/permission/get-menu
   * @secure
   */
  getMenu = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputPermissionGetMenuOutput, any>({
      path: `/api/admin/permission/get-menu`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags permission
   * @name GetDot
   * @summary 查询权限点
   * @request GET:/api/admin/permission/get-dot
   * @secure
   */
  getDot = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputPermissionGetDotOutput, any>({
      path: `/api/admin/permission/get-dot`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags permission
   * @name GetList
   * @summary 查询权限列表
   * @request GET:/api/admin/permission/get-list
   * @secure
   */
  getList = (
    query?: {
      key?: string
      /** @format date-time */
      start?: string
      /** @format date-time */
      end?: string
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputListPermissionListOutput, any>({
      path: `/api/admin/permission/get-list`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags permission
   * @name GetPermissionList
   * @summary 查询授权权限列表
   * @request GET:/api/admin/permission/get-permission-list
   * @secure
   */
  getPermissionList = (params: RequestParams = {}) =>
    this.request<ResultOutputIEnumerableObject, any>({
      path: `/api/admin/permission/get-permission-list`,
      method: 'GET',
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags permission
   * @name GetRolePermissionList
   * @summary 查询角色权限列表
   * @request GET:/api/admin/permission/get-role-permission-list
   * @secure
   */
  getRolePermissionList = (
    query?: {
      /**
       * @format int64
       * @default 0
       */
      roleId?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputListInt64, any>({
      path: `/api/admin/permission/get-role-permission-list`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags permission
   * @name GetTenantPermissionList
   * @summary 查询租户权限列表
   * @request GET:/api/admin/permission/get-tenant-permission-list
   * @deprecated
   * @secure
   */
  getTenantPermissionList = (
    query?: {
      /** @format int64 */
      tenantId?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputListInt64, any>({
      path: `/api/admin/permission/get-tenant-permission-list`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags permission
   * @name AddGroup
   * @summary 新增分组
   * @request POST:/api/admin/permission/add-group
   * @secure
   */
  addGroup = (data: PermissionAddGroupInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/permission/add-group`,
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
   * @tags permission
   * @name AddMenu
   * @summary 新增菜单
   * @request POST:/api/admin/permission/add-menu
   * @secure
   */
  addMenu = (data: PermissionAddMenuInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/permission/add-menu`,
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
   * @tags permission
   * @name AddApi
   * @summary 新增接口
   * @request POST:/api/admin/permission/add-api
   * @secure
   */
  addApi = (data: PermissionAddApiInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/permission/add-api`,
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
   * @tags permission
   * @name AddDot
   * @summary 新增权限点
   * @request POST:/api/admin/permission/add-dot
   * @secure
   */
  addDot = (data: PermissionAddDotInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/permission/add-dot`,
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
   * @tags permission
   * @name UpdateGroup
   * @summary 修改分组
   * @request PUT:/api/admin/permission/update-group
   * @secure
   */
  updateGroup = (data: PermissionUpdateGroupInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/permission/update-group`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags permission
   * @name UpdateMenu
   * @summary 修改菜单
   * @request PUT:/api/admin/permission/update-menu
   * @secure
   */
  updateMenu = (data: PermissionUpdateMenuInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/permission/update-menu`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags permission
   * @name UpdateDot
   * @summary 修改权限点
   * @request PUT:/api/admin/permission/update-dot
   * @secure
   */
  updateDot = (data: PermissionUpdateDotInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/permission/update-dot`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags permission
   * @name Delete
   * @summary 彻底删除
   * @request DELETE:/api/admin/permission/delete
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
      path: `/api/admin/permission/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags permission
   * @name SoftDelete
   * @summary 删除
   * @request DELETE:/api/admin/permission/soft-delete
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
      path: `/api/admin/permission/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags permission
   * @name Assign
   * @summary 保存角色权限
   * @request POST:/api/admin/permission/assign
   * @secure
   */
  assign = (data: PermissionAssignInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/permission/assign`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags permission
   * @name SaveTenantPermissions
   * @summary 保存租户权限
   * @request POST:/api/admin/permission/save-tenant-permissions
   * @deprecated
   * @secure
   */
  saveTenantPermissions = (data: PermissionSaveTenantPermissionsInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/permission/save-tenant-permissions`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
}
