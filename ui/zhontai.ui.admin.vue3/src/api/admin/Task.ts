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
  PageInputTaskGetPageDto,
  ResultOutputPageOutputTaskListOutput,
  ResultOutputString,
  ResultOutputTaskGetOutput,
  TaskAddInput,
  TaskUpdateInput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class TaskApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags task
   * @name GetAlerEmail
   * @summary 查询报警邮件
   * @request GET:/api/admin/task/get-aler-email
   * @secure
   */
  getAlerEmail = (
    query?: {
      id?: string
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputString, any>({
      path: `/api/admin/task/get-aler-email`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags task
   * @name Get
   * @summary 查询
   * @request GET:/api/admin/task/get
   * @secure
   */
  get = (
    query?: {
      id?: string
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputTaskGetOutput, any>({
      path: `/api/admin/task/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags task
   * @name GetPage
   * @summary 查询分页
   * @request POST:/api/admin/task/get-page
   * @secure
   */
  getPage = (data: PageInputTaskGetPageDto, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputTaskListOutput, any>({
      path: `/api/admin/task/get-page`,
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
   * @tags task
   * @name Add
   * @summary 新增
   * @request POST:/api/admin/task/add
   * @secure
   */
  add = (data: TaskAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputString, any>({
      path: `/api/admin/task/add`,
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
   * @tags task
   * @name Update
   * @summary 修改
   * @request PUT:/api/admin/task/update
   * @secure
   */
  update = (data: TaskUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/task/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags task
   * @name Pause
   * @summary 暂停任务
   * @request POST:/api/admin/task/pause
   * @secure
   */
  pause = (
    query: {
      id: string
    },
    params: RequestParams = {}
  ) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/task/pause`,
      method: 'POST',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags task
   * @name Resume
   * @summary 启动任务
   * @request POST:/api/admin/task/resume
   * @secure
   */
  resume = (
    query: {
      id: string
    },
    params: RequestParams = {}
  ) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/task/resume`,
      method: 'POST',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags task
   * @name Run
   * @summary 执行任务
   * @request POST:/api/admin/task/run
   * @secure
   */
  run = (
    query: {
      id: string
    },
    params: RequestParams = {}
  ) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/task/run`,
      method: 'POST',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags task
   * @name Delete
   * @summary 删除任务
   * @request DELETE:/api/admin/task/delete
   * @secure
   */
  delete = (
    query: {
      id: string
    },
    params: RequestParams = {}
  ) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/task/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
}
