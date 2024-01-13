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
  DocumentAddGroupInput,
  DocumentAddImageInput,
  DocumentAddMenuInput,
  DocumentUpdateContentInput,
  DocumentUpdateGroupInput,
  DocumentUpdateMenuInput,
  ResultOutputDocumentGetContentOutput,
  ResultOutputDocumentGetGroupOutput,
  ResultOutputDocumentGetMenuOutput,
  ResultOutputIEnumerableObject,
  ResultOutputInt64,
  ResultOutputListDocumentListOutput,
  ResultOutputListString,
  ResultOutputString,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class DocumentApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags document
   * @name GetGroup
   * @summary 查询分组
   * @request GET:/api/admin/document/get-group
   * @secure
   */
  getGroup = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputDocumentGetGroupOutput, any>({
      path: `/api/admin/document/get-group`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags document
   * @name GetMenu
   * @summary 查询菜单
   * @request GET:/api/admin/document/get-menu
   * @secure
   */
  getMenu = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputDocumentGetMenuOutput, any>({
      path: `/api/admin/document/get-menu`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags document
   * @name GetContent
   * @summary 查询文档内容
   * @request GET:/api/admin/document/get-content
   * @secure
   */
  getContent = (
    query?: {
      /** @format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputDocumentGetContentOutput, any>({
      path: `/api/admin/document/get-content`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags document
   * @name GetList
   * @summary 查询文档列表
   * @request GET:/api/admin/document/get-list
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
    this.request<ResultOutputListDocumentListOutput, any>({
      path: `/api/admin/document/get-list`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags document
   * @name GetImageList
   * @summary 查询图片列表
   * @request GET:/api/admin/document/get-image-list
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
      path: `/api/admin/document/get-image-list`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags document
   * @name AddGroup
   * @summary 新增分组
   * @request POST:/api/admin/document/add-group
   * @secure
   */
  addGroup = (data: DocumentAddGroupInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/document/add-group`,
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
   * @tags document
   * @name AddMenu
   * @summary 新增菜单
   * @request POST:/api/admin/document/add-menu
   * @secure
   */
  addMenu = (data: DocumentAddMenuInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/document/add-menu`,
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
   * @tags document
   * @name AddImage
   * @summary 新增图片
   * @request POST:/api/admin/document/add-image
   * @secure
   */
  addImage = (data: DocumentAddImageInput, params: RequestParams = {}) =>
    this.request<ResultOutputInt64, any>({
      path: `/api/admin/document/add-image`,
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
   * @tags document
   * @name UpdateGroup
   * @summary 修改分组
   * @request PUT:/api/admin/document/update-group
   * @secure
   */
  updateGroup = (data: DocumentUpdateGroupInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/document/update-group`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags document
   * @name UpdateMenu
   * @summary 修改菜单
   * @request PUT:/api/admin/document/update-menu
   * @secure
   */
  updateMenu = (data: DocumentUpdateMenuInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/document/update-menu`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags document
   * @name UpdateContent
   * @summary 修改文档内容
   * @request PUT:/api/admin/document/update-content
   * @secure
   */
  updateContent = (data: DocumentUpdateContentInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/document/update-content`,
      method: 'PUT',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags document
   * @name Delete
   * @summary 彻底删除文档
   * @request DELETE:/api/admin/document/delete
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
      path: `/api/admin/document/delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags document
   * @name DeleteImage
   * @summary 彻底删除图片
   * @request DELETE:/api/admin/document/delete-image
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
      path: `/api/admin/document/delete-image`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags document
   * @name SoftDelete
   * @summary 删除文档
   * @request DELETE:/api/admin/document/soft-delete
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
      path: `/api/admin/document/soft-delete`,
      method: 'DELETE',
      query: query,
      secure: true,
      ...params,
    })
  /**
   * No description
   *
   * @tags document
   * @name GetPlainList
   * @summary 查询精简文档列表
   * @request GET:/api/admin/document/get-plain-list
   * @secure
   */
  getPlainList = (params: RequestParams = {}) =>
    this.request<ResultOutputIEnumerableObject, any>({
      path: `/api/admin/document/get-plain-list`,
      method: 'GET',
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags document
   * @name UploadImage
   * @summary 上传文档图片
   * @request POST:/api/admin/document/upload-image
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
      path: `/api/admin/document/upload-image`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.FormData,
      format: 'json',
      ...params,
    })
}
