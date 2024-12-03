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
  MsgTypeAddInput,
  MsgTypeUpdateInput,
  ResultOutputInt64,
  ResultOutputListMsgTypeGetListOutput,
  ResultOutputMsgTypeGetOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class MsgTypeApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags msg-type
   * @name Get
   * @summary 查询
   * @request GET:/api/admin/msg-type/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputMsgTypeGetOutput, any>({
      path: `/api/admin/msg-type/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags msg-type
   * @name GetList
   * @summary 查询列表
   * @request GET:/api/admin/msg-type/get-list
   * @secure
   */
  getList = (
    query?: {
      /** 名称 */
      Name?: string
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputListMsgTypeGetListOutput, any>({
      path: `/api/admin/msg-type/get-list`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags msg-type
   * @name Add
   * @summary 新增
   * @request POST:/api/admin/msg-type/add
   * @secure
   */
  add = (data: MsgTypeAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/msg-type/add`,
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
   * @tags msg-type
   * @name Update
   * @summary 修改
   * @request PUT:/api/admin/msg-type/update
   * @secure
   */
  update = (data: MsgTypeUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/msg-type/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags msg-type
   * @name Delete
   * @summary 彻底删除
   * @request DELETE:/api/admin/msg-type/delete
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
      path: `/api/admin/msg-type/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags msg-type
   * @name BatchDelete
   * @summary 批量彻底删除
   * @request PUT:/api/admin/msg-type/batch-delete
   * @secure
   */
  batchDelete = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/msg-type/batch-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags msg-type
   * @name SoftDelete
   * @summary 删除
   * @request DELETE:/api/admin/msg-type/soft-delete
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
      path: `/api/admin/msg-type/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags msg-type
   * @name BatchSoftDelete
   * @summary 批量删除
   * @request PUT:/api/admin/msg-type/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/msg-type/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
}
