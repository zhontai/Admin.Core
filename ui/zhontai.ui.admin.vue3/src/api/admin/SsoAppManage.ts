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

import {
  PageInputSsoAppManageGetListInput,
  ResultOutputInt64,
  ResultOutputListSsoAppManageButtonOutput,
  ResultOutputObject,
  ResultOutputPageOutputSsoAppManageGetListOutput,
  ResultOutputSsoAppManageGetOutput,
  ResultOutputString,
  SsoAppManageAddInput,
  SsoAppManageUpdateInput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class SsoAppManageApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags sso-app-manage
   * @name Get
   * @summary 详情
   * @request GET:/api/admin/sso-app-manage/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputSsoAppManageGetOutput, any>({
      path: `/api/admin/sso-app-manage/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags sso-app-manage
   * @name GetPage
   * @summary 分页列表
   * @request POST:/api/admin/sso-app-manage/get-page
   * @secure
   */
  getPage = (data: PageInputSsoAppManageGetListInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputSsoAppManageGetListOutput, any>({
      path: `/api/admin/sso-app-manage/get-page`,
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
   * @tags sso-app-manage
   * @name Add
   * @summary 新增（密钥由系统自动生成）
   * @request POST:/api/admin/sso-app-manage/add
   * @secure
   */
  add = (data: SsoAppManageAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/sso-app-manage/add`,
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
   * @tags sso-app-manage
   * @name Update
   * @summary 修改
   * @request PUT:/api/admin/sso-app-manage/update
   * @secure
   */
  update = (data: SsoAppManageUpdateInput, params: RequestParams = {}) =>
    this.request<ResultOutputObject, any>({
      path: `/api/admin/sso-app-manage/update`,
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
   * @tags sso-app-manage
   * @name SoftDelete
   * @summary 删除（软删除）
   * @request DELETE:/api/admin/sso-app-manage/soft-delete
   * @secure
   */
  softDelete = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputObject, any>({
      path: `/api/admin/sso-app-manage/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags sso-app-manage
   * @name ResetSecret
   * @summary 重置应用密钥，返回新密钥（旧密钥立即失效）
   * @request POST:/api/admin/sso-app-manage/reset-secret
   * @secure
   */
  resetSecret = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputString, any>({
      path: `/api/admin/sso-app-manage/reset-secret`,
      method: 'POST',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags sso-app-manage
   * @name GetApps
   * @summary 单点登录按钮列表（仅返回启用的应用，脱敏，供前端动态渲染按钮，按 Sort 升序）
   * @request POST:/api/admin/sso-app-manage/get-apps
   * @secure
   */
  getApps = (params: RequestParams = {}) =>
    this.request<ResultOutputListSsoAppManageButtonOutput, any>({
      path: `/api/admin/sso-app-manage/get-apps`,
      method: 'POST',
      secure: true,
      format: 'json',
      ...params,
    })
}
