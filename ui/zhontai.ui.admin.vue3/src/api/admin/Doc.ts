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
  DocAddGroupInput,
  DocAddImageInput,
  DocAddMenuInput,
  DocUpdateContentInput,
  DocUpdateGroupInput,
  DocUpdateMenuInput,
  ResultOutputDocGetContentOutput,
  ResultOutputDocGetGroupOutput,
  ResultOutputDocGetMenuOutput,
  ResultOutputIEnumerableObject,
  ResultOutputInt64,
  ResultOutputListDocListOutput,
  ResultOutputListString,
  ResultOutputString,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class DocApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags doc
   * @name GetGroup
   * @summary 查询分组
   * @request GET:/api/admin/doc/get-group
   * @secure
   */
  getGroup = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputDocGetGroupOutput, any>({
      path: `/api/admin/doc/get-group`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags doc
   * @name GetMenu
   * @summary 查询菜单
   * @request GET:/api/admin/doc/get-menu
   * @secure
   */
  getMenu = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputDocGetMenuOutput, any>({
      path: `/api/admin/doc/get-menu`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags doc
   * @name GetContent
   * @summary 查询文档内容
   * @request GET:/api/admin/doc/get-content
   * @secure
   */
  getContent = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputDocGetContentOutput, any>({
      path: `/api/admin/doc/get-content`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags doc
   * @name GetList
   * @summary 查询文档列表
   * @request GET:/api/admin/doc/get-list
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
    this.request<ResultOutputListDocListOutput, any>({
      path: `/api/admin/doc/get-list`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags doc
   * @name GetImageList
   * @summary 查询图片列表
   * @request GET:/api/admin/doc/get-image-list
   * @secure
   */
  getImageList = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputListString, any>({
      path: `/api/admin/doc/get-image-list`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags doc
   * @name AddGroup
   * @summary 新增分组
   * @request POST:/api/admin/doc/add-group
   * @secure
   */
  addGroup = (data: DocAddGroupInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/doc/add-group`,
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
   * @tags doc
   * @name AddMenu
   * @summary 新增菜单
   * @request POST:/api/admin/doc/add-menu
   * @secure
   */
  addMenu = (data: DocAddMenuInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/doc/add-menu`,
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
   * @tags doc
   * @name AddImage
   * @summary 新增图片
   * @request POST:/api/admin/doc/add-image
   * @secure
   */
  addImage = (data: DocAddImageInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/doc/add-image`,
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
   * @tags doc
   * @name UpdateGroup
   * @summary 修改分组
   * @request PUT:/api/admin/doc/update-group
   * @secure
   */
  updateGroup = (data: DocUpdateGroupInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/doc/update-group`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags doc
   * @name UpdateMenu
   * @summary 修改菜单
   * @request PUT:/api/admin/doc/update-menu
   * @secure
   */
  updateMenu = (data: DocUpdateMenuInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/doc/update-menu`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags doc
   * @name UpdateContent
   * @summary 修改文档内容
   * @request PUT:/api/admin/doc/update-content
   * @secure
   */
  updateContent = (data: DocUpdateContentInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/doc/update-content`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags doc
   * @name Delete
   * @summary 彻底删除文档
   * @request DELETE:/api/admin/doc/delete
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
      path: `/api/admin/doc/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags doc
   * @name DeleteImage
   * @summary 彻底删除图片
   * @request DELETE:/api/admin/doc/delete-image
   * @secure
   */
  deleteImage = (
    query?: {
      /** @format int64 */
      documentId?: number
      url?: string
    },
    params: RequestParams = {}
  ) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/doc/delete-image`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags doc
   * @name SoftDelete
   * @summary 删除文档
   * @request DELETE:/api/admin/doc/soft-delete
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
      path: `/api/admin/doc/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags doc
   * @name GetPlainList
   * @summary 查询精简文档列表
   * @request GET:/api/admin/doc/get-plain-list
   * @secure
   */
  getPlainList = (params: RequestParams = {}) =>
    this.request<ResultOutputIEnumerableObject, any>({
      path: `/api/admin/doc/get-plain-list`,
      method: 'GET',
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags doc
   * @name UploadImage
   * @summary 上传文档图片
   * @request POST:/api/admin/doc/upload-image
   * @secure
   */
  uploadImage = (
    data: {
      /**
       * 上传文件
       * @format binary
       */
      File?: File
      /**
       * 文档编号
       * @format int64
       */
      Id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputString, any>({
      path: `/api/admin/doc/upload-image`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.FormData,
      format: 'json',
      ...params,
    })
}
