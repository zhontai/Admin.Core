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
  PageInputPrintTemplateGetPageInput,
  PrintTemplateAddInput,
  PrintTemplateSetEnableInput,
  PrintTemplateUpdateInput,
  PrintTemplateUpdateTemplateInput,
  ResultOutputInt64,
  ResultOutputPageOutputPrintTemplateGetPageOutput,
  ResultOutputPrintTemplateGetOutput,
  ResultOutputPrintTemplateGetUpdateTemplateOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class PrintTemplateApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags print-template
   * @name Get
   * @summary 查询
   * @request GET:/api/admin/print-template/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputPrintTemplateGetOutput, any>({
      path: `/api/admin/print-template/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags print-template
   * @name GetUpdateTemplate
   * @summary 查询修改模板
   * @request GET:/api/admin/print-template/get-update-template
   * @secure
   */
  getUpdateTemplate = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputPrintTemplateGetUpdateTemplateOutput, any>({
      path: `/api/admin/print-template/get-update-template`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags print-template
   * @name GetPage
   * @summary 查询分页
   * @request POST:/api/admin/print-template/get-page
   * @secure
   */
  getPage = (data: PageInputPrintTemplateGetPageInput, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputPrintTemplateGetPageOutput, any>({
      path: `/api/admin/print-template/get-page`,
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
   * @tags print-template
   * @name Add
   * @summary 新增
   * @request POST:/api/admin/print-template/add
   * @secure
   */
  add = (data: PrintTemplateAddInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/print-template/add`,
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
   * @tags print-template
   * @name Update
   * @summary 修改
   * @request PUT:/api/admin/print-template/update
   * @secure
   */
  update = (data: PrintTemplateUpdateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/print-template/update`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags print-template
   * @name UpdateTemplate
   * @summary 修改模板
   * @request PUT:/api/admin/print-template/update-template
   * @secure
   */
  updateTemplate = (data: PrintTemplateUpdateTemplateInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/print-template/update-template`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags print-template
   * @name SetEnable
   * @summary 设置启用
   * @request POST:/api/admin/print-template/set-enable
   * @secure
   */
  setEnable = (data: PrintTemplateSetEnableInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/print-template/set-enable`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags print-template
   * @name Delete
   * @summary 彻底删除
   * @request DELETE:/api/admin/print-template/delete
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
      path: `/api/admin/print-template/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags print-template
   * @name BatchDelete
   * @summary 批量彻底删除
   * @request PUT:/api/admin/print-template/batch-delete
   * @secure
   */
  batchDelete = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/print-template/batch-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags print-template
   * @name SoftDelete
   * @summary 删除
   * @request DELETE:/api/admin/print-template/soft-delete
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
      path: `/api/admin/print-template/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags print-template
   * @name BatchSoftDelete
   * @summary 批量删除
   * @request PUT:/api/admin/print-template/batch-soft-delete
   * @secure
   */
  batchSoftDelete = (data: number[], params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/print-template/batch-soft-delete`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
}
