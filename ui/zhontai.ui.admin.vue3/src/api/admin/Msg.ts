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
  MsgAddInput,
  MsgAddMsgUserListInput,
  MsgUpdateInput,
  PageInputMsgGetPageInput,
  ResultOutputInt64,
  ResultOutputListMsgGetMsgUserListOutput,
  ResultOutputMsgGetOutput,
  ResultOutputPageOutputMsgGetPageOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class MsgApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags msg
   * @name Get
   * @summary 查询
   * @request GET:/api/admin/msg/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputMsgGetOutput, any>({
      path: `/api/admin/msg/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags msg
   * @name GetPage
   * @summary 查询分页
   * @request POST:/api/admin/msg/get-page
   * @secure
   */
  getPage = (data: PageInputMsgGetPageInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputMsgGetPageOutput, any>({
      path: `/api/admin/msg/get-page`,
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
   * @tags msg
   * @name GetMsgUserList
   * @summary 查询消息用户列表
   * @request GET:/api/admin/msg/get-msg-user-list
   * @secure
   */
  getMsgUserList = (
    query?: {
      /** @format int64 */
      MsgId?: number
      Name?: string
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputListMsgGetMsgUserListOutput, any>({
      path: `/api/admin/msg/get-msg-user-list`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags msg
   * @name AddMsgUser
   * @summary 添加消息用户
   * @request POST:/api/admin/msg/add-msg-user
   * @secure
   */
  addMsgUser = (data: MsgAddMsgUserListInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/msg/add-msg-user`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags msg
   * @name RemoveMsgUser
   * @summary 移除消息用户
   * @request POST:/api/admin/msg/remove-msg-user
   * @secure
   */
  removeMsgUser = (data: MsgAddMsgUserListInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/msg/remove-msg-user`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags msg
   * @name Add
   * @summary 新增
   * @request POST:/api/admin/msg/add
   * @secure
   */
  add = (data: MsgAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/msg/add`,
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
   * @tags msg
   * @name Update
   * @summary 修改
   * @request PUT:/api/admin/msg/update
   * @secure
   */
  update = (data: MsgUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/msg/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags msg
   * @name Delete
   * @summary 彻底删除
   * @request DELETE:/api/admin/msg/delete
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
      path: `/api/admin/msg/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags msg
   * @name BatchDelete
   * @summary 批量彻底删除
   * @request PUT:/api/admin/msg/batch-delete
   * @secure
   */
  batchDelete = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/msg/batch-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags msg
   * @name SoftDelete
   * @summary 删除
   * @request DELETE:/api/admin/msg/soft-delete
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
      path: `/api/admin/msg/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags msg
   * @name BatchSoftDelete
   * @summary 批量删除
   * @request PUT:/api/admin/msg/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/msg/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
}
