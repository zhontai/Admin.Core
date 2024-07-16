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
  AuthChangePasswordByEmailInput,
  AuthChangePasswordByMobileInput,
  AuthEmailLoginInput,
  AuthLoginInput,
  AuthMobileLoginInput,
  ResultOutputAuthGetPasswordEncryptKeyOutput,
  ResultOutputAuthGetUserInfoOutput,
  ResultOutputAuthGetUserPermissionsOutput,
  ResultOutputAuthUserProfileDto,
  ResultOutputBoolean,
  ResultOutputListAuthUserMenuDto,
  ResultOutputObject,
} from './data-contracts'
import { ContentType, HttpClient, RequestParams } from './http-client'

export class AuthApi<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags auth
   * @name GetPasswordEncryptKey
   * @summary 查询密钥
   * @request GET:/api/admin/auth/get-password-encrypt-key
   * @secure
   */
  getPasswordEncryptKey = (params: RequestParams = {}) =>
    this.request<ResultOutputAuthGetPasswordEncryptKeyOutput, any>({
      path: `/api/admin/auth/get-password-encrypt-key`,
      method: 'GET',
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags auth
   * @name GetUserProfile
   * @summary 查询用户个人信息
   * @request GET:/api/admin/auth/get-user-profile
   * @secure
   */
  getUserProfile = (params: RequestParams = {}) =>
    this.request<ResultOutputAuthUserProfileDto, any>({
      path: `/api/admin/auth/get-user-profile`,
      method: 'GET',
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags auth
   * @name GetUserMenus
   * @summary 查询用户菜单列表
   * @request GET:/api/admin/auth/get-user-menus
   * @secure
   */
  getUserMenus = (params: RequestParams = {}) =>
    this.request<ResultOutputListAuthUserMenuDto, any>({
      path: `/api/admin/auth/get-user-menus`,
      method: 'GET',
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags auth
   * @name GetUserPermissions
   * @summary 查询用户权限列表
   * @request GET:/api/admin/auth/get-user-permissions
   * @secure
   */
  getUserPermissions = (params: RequestParams = {}) =>
    this.request<ResultOutputAuthGetUserPermissionsOutput, any>({
      path: `/api/admin/auth/get-user-permissions`,
      method: 'GET',
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags auth
   * @name GetUserInfo
   * @summary 查询用户信息
   * @request GET:/api/admin/auth/get-user-info
   * @secure
   */
  getUserInfo = (params: RequestParams = {}) =>
    this.request<ResultOutputAuthGetUserInfoOutput, any>({
      path: `/api/admin/auth/get-user-info`,
      method: 'GET',
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags auth
   * @name Login
   * @summary 登录
   * @request POST:/api/admin/auth/login
   * @secure
   */
  login = (data: AuthLoginInput, params: RequestParams = {}) =>
    this.request<ResultOutputObject, any>({
      path: `/api/admin/auth/login`,
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
   * @tags auth
   * @name MobileLogin
   * @summary 手机登录
   * @request POST:/api/admin/auth/mobile-login
   * @secure
   */
  mobileLogin = (data: AuthMobileLoginInput, params: RequestParams = {}) =>
    this.request<ResultOutputObject, any>({
      path: `/api/admin/auth/mobile-login`,
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
   * @tags auth
   * @name EmailLogin
   * @summary 邮箱登录
   * @request POST:/api/admin/auth/email-login
   * @secure
   */
  emailLogin = (data: AuthEmailLoginInput, params: RequestParams = {}) =>
    this.request<ResultOutputObject, any>({
      path: `/api/admin/auth/email-login`,
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
   * @tags auth
   * @name ChangePasswordByEmail
   * @summary 邮箱更改密码
   * @request POST:/api/admin/auth/change-password-by-email
   * @secure
   */
  changePasswordByEmail = (data: AuthChangePasswordByEmailInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/auth/change-password-by-email`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
   * No description
   *
   * @tags auth
   * @name ChangePasswordByMobile
   * @summary 手机更改密码
   * @request POST:/api/admin/auth/change-password-by-mobile
   * @secure
   */
  changePasswordByMobile = (data: AuthChangePasswordByMobileInput, params: RequestParams = {}) =>
    this.request<AxiosResponse, any>({
      path: `/api/admin/auth/change-password-by-mobile`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      ...params,
    })
  /**
 * No description
 *
 * @tags auth
 * @name Refresh
 * @summary 刷新Token
以旧换新
 * @request GET:/api/admin/auth/refresh
 * @secure
 */
  refresh = (
    query: {
      token: string
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultOutputObject, any>({
      path: `/api/admin/auth/refresh`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
   * No description
   *
   * @tags auth
   * @name IsCaptcha
   * @summary 是否开启验证码
   * @request GET:/api/admin/auth/is-captcha
   * @secure
   */
  isCaptcha = (params: RequestParams = {}) =>
    this.request<ResultOutputBoolean, any>({
      path: `/api/admin/auth/is-captcha`,
      method: 'GET',
      secure: true,
      format: 'json',
      ...params,
    })
}
