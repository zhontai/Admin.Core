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
  ResultOutputInt64,
  ResultOutputListSearchTemplateGetListOutput,
  ResultOutputSearchTemplateGetUpdateOutput,
  SearchTemplateSaveInput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class SearchTemplateApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags search-template
   * @name Get
   * @summary 查询
   * @request GET:/api/admin/search-template/get
   * @secure
   */
  get = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputSearchTemplateGetUpdateOutput, any>({
      path: `/api/admin/search-template/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags search-template
   * @name GetList
   * @summary 查询列表
   * @request GET:/api/admin/search-template/get-list
   * @secure
   */
  getList = (
    query?: {
      /** @format int64 */
      moduleId?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputListSearchTemplateGetListOutput, any>({
      path: `/api/admin/search-template/get-list`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags search-template
   * @name Save
   * @summary 保存
   * @request POST:/api/admin/search-template/save
   * @secure
   */
  save = (data: SearchTemplateSaveInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/search-template/save`,
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
   * @tags search-template
   * @name Delete
   * @summary 彻底删除
   * @request DELETE:/api/admin/search-template/delete
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
      path: `/api/admin/search-template/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
}
