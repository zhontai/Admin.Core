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
  FileDeleteInput,
  PageInputFileGetPageDto,
  ResultOutputFileEntity,
  ResultOutputListFileEntity,
  ResultOutputPageOutputFileGetPageOutput,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class FileApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags file
   * @name GetPage
   * @summary 查询分页
   * @request POST:/api/admin/file/get-page
   * @secure
   */
  getPage = (data: PageInputFileGetPageDto, params: RequestParams = {}) =>
    this.request<ResultOutputPageOutputFileGetPageOutput, any>({
      path: `/api/admin/file/get-page`,
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
   * @tags file
   * @name Delete
   * @summary 删除
   * @request POST:/api/admin/file/delete
   * @secure
   */
  delete = (data: FileDeleteInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/file/delete`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags file
   * @name UploadFile
   * @summary 上传文件
   * @request POST:/api/admin/file/upload-file
   * @secure
   */
  uploadFile = (
    data: {
      /** @format binary */
      file: File
    },
    query?: {
      /**
       * 文件目录
       * @default ""
       */
      fileDirectory?: string
      /**
       * 文件重命名
       * @default true
       */
      fileReName?: boolean
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputFileEntity, any>({
      path: `/api/admin/file/upload-file`,
      method: 'POST',
      query: query,
      body: data,
      secure: true,
      type: ContentType.FormData,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags file
   * @name UploadFiles
   * @summary 上传多文件
   * @request POST:/api/admin/file/upload-files
   * @secure
   */
  uploadFiles = (
    data: {
      files: File[]
    },
    query?: {
      /**
       * 文件目录
       * @default ""
       */
      fileDirectory?: string
      /**
       * 文件重命名
       * @default true
       */
      fileReName?: boolean
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputListFileEntity, any>({
      path: `/api/admin/file/upload-files`,
      method: 'POST',
      query: query,
      body: data,
      secure: true,
      type: ContentType.FormData,
      format: 'json',
      ...params,
    })
}
