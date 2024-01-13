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
  PageInputRoleGetPageDto,
  ResultOutputInt64,
  ResultOutputListRoleGetListOutput,
  ResultOutputListRoleGetRoleUserListOutput,
  ResultOutputPageOutputRoleGetPageOutput,
  ResultOutputRoleGetOutput,
  RoleAddInput,
  RoleAddRoleUserListInput,
  RoleSetDataScopeInput,
  RoleUpdateInput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class RoleApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags role
   * @name Get
   * @summary 查询
   * @request GET:/api/admin/role/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputRoleGetOutput, any>({
      path: `/api/admin/role/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags role
   * @name GetList
   * @summary 查询列表
   * @request GET:/api/admin/role/get-list
   * @secure
   */
  getList = (
    query?: {
      /** 名称 */
      Name?: string
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputListRoleGetListOutput, any>({
      path: `/api/admin/role/get-list`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags role
   * @name GetPage
   * @summary 查询分页
   * @request POST:/api/admin/role/get-page
   * @secure
   */
  getPage = (data: PageInputRoleGetPageDto, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputRoleGetPageOutput, any>({
      path: `/api/admin/role/get-page`,
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
   * @tags role
   * @name GetRoleUserList
   * @summary 查询角色用户列表
   * @request GET:/api/admin/role/get-role-user-list
   * @secure
   */
  getRoleUserList = (
    query?: {
      /** 姓名 */
      Name?: string
      /**
       * 角色Id
       * @format int64
       */
      RoleId?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputListRoleGetRoleUserListOutput, any>({
      path: `/api/admin/role/get-role-user-list`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags role
   * @name AddRoleUser
   * @summary 添加角色用户
   * @request POST:/api/admin/role/add-role-user
   * @secure
   */
  addRoleUser = (data: RoleAddRoleUserListInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/role/add-role-user`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags role
   * @name RemoveRoleUser
   * @summary 移除角色用户
   * @request POST:/api/admin/role/remove-role-user
   * @secure
   */
  removeRoleUser = (data: RoleAddRoleUserListInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/role/remove-role-user`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags role
   * @name Add
   * @summary 新增
   * @request POST:/api/admin/role/add
   * @secure
   */
  add = (data: RoleAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/role/add`,
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
   * @tags role
   * @name Update
   * @summary 修改
   * @request PUT:/api/admin/role/update
   * @secure
   */
  update = (data: RoleUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/role/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags role
   * @name Delete
   * @summary 彻底删除
   * @request DELETE:/api/admin/role/delete
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
      path: `/api/admin/role/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags role
   * @name BatchDelete
   * @summary 批量彻底删除
   * @request PUT:/api/admin/role/batch-delete
   * @secure
   */
  batchDelete = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/role/batch-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags role
   * @name SoftDelete
   * @summary 删除
   * @request DELETE:/api/admin/role/soft-delete
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
      path: `/api/admin/role/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags role
   * @name BatchSoftDelete
   * @summary 批量删除
   * @request PUT:/api/admin/role/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/role/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags role
   * @name SetDataScope
   * @summary 设置数据权限
   * @request POST:/api/admin/role/set-data-scope
   * @secure
   */
  setDataScope = (data: RoleSetDataScopeInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/role/set-data-scope`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
}
