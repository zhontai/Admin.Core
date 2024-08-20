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

/**
 * 账号类型:UserName=1,Mobile=2,Email=3
 * @format int32
 */
export type AccountType = 1 | 2 | 3

/** 添加 */
export interface ApiAddInput {
  /**
   * 所属模块
   * @format int64
   */
  parentId?: number | null
  /** 接口名称 */
  label?: string | null
  /** 接口地址 */
  path?: string | null
  /** 接口提交方法 */
  httpMethods?: string | null
  /** 说明 */
  description?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
}

/** 接口管理 */
export interface ApiEntity {
  /**
   * 主键Id
   * @format int64
   */
  id?: number
  /**
   * 创建者用户Id
   * @format int64
   */
  createdUserId?: number | null
  /**
   * 创建者用户名
   * @maxLength 50
   */
  createdUserName?: string | null
  /**
   * 创建者姓名
   * @maxLength 50
   */
  createdUserRealName?: string | null
  /**
   * 创建时间
   * @format date-time
   */
  createdTime?: string | null
  /**
   * 修改者用户Id
   * @format int64
   */
  modifiedUserId?: number | null
  /**
   * 修改者用户名
   * @maxLength 50
   */
  modifiedUserName?: string | null
  /**
   * 修改者姓名
   * @maxLength 50
   */
  modifiedUserRealName?: string | null
  /**
   * 修改时间
   * @format date-time
   */
  modifiedTime?: string | null
  /** 是否删除 */
  isDeleted?: boolean
  /**
   * 所属模块
   * @format int64
   */
  parentId?: number
  /** 接口命名 */
  name?: string | null
  /** 接口名称 */
  label?: string | null
  /** 接口地址 */
  path?: string | null
  /** 接口提交方法 */
  httpMethods?: string | null
  /** 说明 */
  description?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
  childs?: ApiEntity[] | null
  permissions?: PermissionEntity[] | null
}

/** 枚举 */
export interface ApiGetEnumsOutput {
  /** 名称 */
  name?: string | null
  /** 描述 */
  desc?: string | null
  /** 选项列表 */
  options?: Options[] | null
}

export interface ApiGetOutput {
  /**
   * 所属模块
   * @format int64
   */
  parentId?: number | null
  /** 接口名称 */
  label?: string | null
  /** 接口地址 */
  path?: string | null
  /** 接口提交方法 */
  httpMethods?: string | null
  /** 说明 */
  description?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
  /**
   * 接口Id
   * @format int64
   */
  id: number
}

export interface ApiGetPageDto {
  /** 接口名称 */
  label?: string | null
}

export interface ApiListOutput {
  /**
   * 接口Id
   * @format int64
   */
  id?: number
  /**
   * 接口父级
   * @format int64
   */
  parentId?: number | null
  /** 接口命名 */
  name?: string | null
  /** 接口名称 */
  label?: string | null
  /** 接口地址 */
  path?: string | null
  /** 接口提交方法 */
  httpMethods?: string | null
  /** 说明 */
  description?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
}

/** 接口 */
export interface ApiModel {
  /** 请求方法 */
  httpMethods?: string | null
  /** 请求地址 */
  path?: string | null
}

/** 接口同步Dto */
export interface ApiSyncDto {
  /** 接口名称 */
  label?: string | null
  /** 接口地址 */
  path?: string | null
  /** 父级路径 */
  parentPath?: string | null
  /** 接口提交方法 */
  httpMethods?: string | null
}

/** 接口同步 */
export interface ApiSyncInput {
  apis?: ApiSyncDto[] | null
}

/** 修改 */
export interface ApiUpdateInput {
  /**
   * 所属模块
   * @format int64
   */
  parentId?: number | null
  /** 接口名称 */
  label?: string | null
  /** 接口地址 */
  path?: string | null
  /** 接口提交方法 */
  httpMethods?: string | null
  /** 说明 */
  description?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
  /**
   * 接口Id
   * @format int64
   */
  id: number
}

/** 邮箱更改密码 */
export interface AuthChangePasswordByEmailInput {
  /**
   * 邮箱地址
   * @minLength 1
   */
  email: string
  /**
   * 验证码
   * @minLength 1
   */
  code: string
  /**
   * 验证码Id
   * @minLength 1
   */
  codeId: string
  /**
   * 新密码
   * @minLength 1
   */
  newPassword: string
  /** 确认新密码 */
  confirmPassword?: string | null
}

/** 手机更改密码 */
export interface AuthChangePasswordByMobileInput {
  /**
   * 手机号
   * @minLength 1
   */
  mobile: string
  /**
   * 验证码
   * @minLength 1
   */
  code: string
  /**
   * 验证码Id
   * @minLength 1
   */
  codeId: string
  /**
   * 新密码
   * @minLength 1
   */
  newPassword: string
  /** 确认新密码 */
  confirmPassword?: string | null
}

/** 邮箱登录信息 */
export interface AuthEmailLoginInput {
  /**
   * 邮箱地址
   * @minLength 1
   */
  email: string
  /**
   * 验证码
   * @minLength 1
   */
  code: string
  /**
   * 验证码Id
   * @minLength 1
   */
  codeId: string
}

export interface AuthGetPasswordEncryptKeyOutput {
  /** 缓存键 */
  key?: string | null
  /** 密码加密密钥 */
  encryptKey?: string | null
  /** 密码加密向量 */
  iv?: string | null
}

export interface AuthGetUserInfoOutput {
  /** 用户个人信息 */
  user?: AuthUserProfileDto
  /** 用户菜单列表 */
  menus?: AuthUserMenuDto[] | null
  /** 用户权限列表 */
  permissions?: string[] | null
}

export interface AuthGetUserPermissionsOutput {
  /** 用户权限列表 */
  permissions?: string[] | null
}

/** 登录信息 */
export interface AuthLoginInput {
  /** 用户名 */
  userName?: string | null
  /** 手机号 */
  mobile?: string | null
  /** 邮箱地址 */
  email?: string | null
  /** 账号类型:UserName=1,Mobile=2,Email=3 */
  accountType?: AccountType
  /**
   * 密码
   * @minLength 1
   */
  password: string
  /** 密码键 */
  passwordKey?: string | null
  /** 验证码Id */
  captchaId?: string | null
  /** 验证码数据 */
  captchaData?: string | null
}

/** 手机号登录信息 */
export interface AuthMobileLoginInput {
  /**
   * 手机号
   * @minLength 1
   */
  mobile: string
  /**
   * 验证码
   * @minLength 1
   */
  code: string
  /**
   * 验证码Id
   * @minLength 1
   */
  codeId: string
}

/** 邮箱注册 */
export interface AuthRegByEmailInput {
  /**
   * 邮箱地址
   * @minLength 1
   */
  email: string
  /**
   * 验证码
   * @minLength 1
   */
  code: string
  /**
   * 验证码Id
   * @minLength 1
   */
  codeId: string
  /**
   * 密码
   * @minLength 1
   */
  password: string
  /**
   * 企业名称
   * @minLength 1
   */
  corpName: string
}

/** 手机号注册 */
export interface AuthRegByMobileInput {
  /**
   * 手机号
   * @minLength 1
   */
  mobile: string
  /**
   * 验证码
   * @minLength 1
   */
  code: string
  /**
   * 验证码Id
   * @minLength 1
   */
  codeId: string
  /**
   * 密码
   * @minLength 1
   */
  password: string
  /**
   * 企业名称
   * @minLength 1
   */
  corpName: string
}

export interface AuthUserMenuDto {
  /**
   * 权限Id
   * @format int64
   */
  id?: number
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /** 路由地址 */
  path?: string | null
  /** 路由命名 */
  name?: string | null
  /** 视图地址 */
  viewPath?: string | null
  /** 重定向地址 */
  redirect?: string | null
  /** 权限名称 */
  label?: string | null
  /** 图标 */
  icon?: string | null
  /** 打开 */
  opened?: boolean | null
  /** 隐藏 */
  hidden?: boolean
  /** 打开新窗口 */
  newWindow?: boolean | null
  /** 链接外显 */
  external?: boolean | null
  /** 是否缓存 */
  isKeepAlive?: boolean
  /** 是否固定 */
  isAffix?: boolean
  /** 链接地址 */
  link?: string | null
  /** 是否内嵌窗口 */
  isIframe?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number | null
}

/** 用户个人信息 */
export interface AuthUserProfileDto {
  /** 账号 */
  userName?: string | null
  /** 姓名 */
  name?: string | null
  /** 手机号 */
  mobile?: string | null
  /** 昵称 */
  nickName?: string | null
  /** 头像 */
  avatar?: string | null
  /** 企业 */
  corpName?: string | null
  /** 职位 */
  position?: string | null
  /** 主属部门 */
  deptName?: string | null
  /** 水印文案 */
  watermarkText?: string | null
}

export interface CaptchaData {
  id?: string | null
  backgroundImage?: string | null
  sliderImage?: string | null
}

/**
 * 数据范围:All=1,DeptWithChild=2,Dept=3,Self=4,Custom=5
 * @format int32
 */
export type DataScope = 1 | 2 | 3 | 4 | 5

/**
 * MySql=0,SqlServer=1,PostgreSQL=2,Oracle=3,Sqlite=4,OdbcOracle=5,OdbcSqlServer=6,OdbcMySql=7,OdbcPostgreSQL=8,Odbc=9,OdbcDameng=10,MsAccess=11,Dameng=12,OdbcKingbaseES=13,ShenTong=14,KingbaseES=15,Firebird=16,Custom=17,ClickHouse=18,GBase=19,QuestDb=20,Xugu=21,CustomOracle=22,CustomSqlServer=23,CustomMySql=24,CustomPostgreSQL=25
 * @format int32
 */
export type DataType = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12 | 13 | 14 | 15 | 16 | 17 | 18 | 19 | 20 | 21 | 22 | 23 | 24 | 25

/** 添加字典 */
export interface DictAddInput {
  /**
   * 字典类型Id
   * @format int64
   */
  dictTypeId?: number
  /**
   * 字典名称
   * @minLength 1
   */
  name: string
  /** 字典编码 */
  code?: string | null
  /** 字典值 */
  value?: string | null
  /** 描述 */
  description?: string | null
  /** 启用 */
  enabled?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
}

export interface DictGetListDto {
  /** 字典类型编码 */
  dictTypeCode?: string | null
  /** 字典类型名称 */
  dictTypeName?: string | null
  /**
   * 主键Id
   * @format int64
   */
  id?: number
  /** 字典名称 */
  name?: string | null
  /** 字典编码 */
  code?: string | null
  /** 字典值 */
  value?: string | null
}

export interface DictGetOutput {
  /**
   * 字典类型Id
   * @format int64
   */
  dictTypeId?: number
  /**
   * 字典名称
   * @minLength 1
   */
  name: string
  /** 字典编码 */
  code?: string | null
  /** 字典值 */
  value?: string | null
  /** 描述 */
  description?: string | null
  /** 启用 */
  enabled?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /**
   * 主键Id
   * @format int64
   */
  id: number
}

export interface DictGetPageDto {
  /**
   * 字典类型Id
   * @format int64
   */
  dictTypeId?: number
  /** 字典名称 */
  name?: string | null
}

export interface DictGetPageOutput {
  /**
   * 主键Id
   * @format int64
   */
  id?: number
  /** 字典名称 */
  name?: string | null
  /** 字典编码 */
  code?: string | null
  /** 字典值 */
  value?: string | null
  /** 启用 */
  enabled?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
}

/** 添加字典类型 */
export interface DictTypeAddInput {
  /**
   * 字典类型名称
   * @minLength 1
   */
  name: string
  /** 字典类型编码 */
  code?: string | null
  /** 描述 */
  description?: string | null
  /** 启用 */
  enabled?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
}

export interface DictTypeGetOutput {
  /**
   * 字典类型名称
   * @minLength 1
   */
  name: string
  /** 字典类型编码 */
  code?: string | null
  /** 描述 */
  description?: string | null
  /** 启用 */
  enabled?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /**
   * 主键Id
   * @format int64
   */
  id: number
}

export interface DictTypeGetPageDto {
  /** 字典名称 */
  name?: string | null
}

export interface DictTypeGetPageOutput {
  /**
   * 主键Id
   * @format int64
   */
  id?: number
  /** 字典名称 */
  name?: string | null
  /** 字典编码 */
  code?: string | null
  /** 启用 */
  enabled?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
}

/** 修改 */
export interface DictTypeUpdateInput {
  /**
   * 字典类型名称
   * @minLength 1
   */
  name: string
  /** 字典类型编码 */
  code?: string | null
  /** 描述 */
  description?: string | null
  /** 启用 */
  enabled?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /**
   * 主键Id
   * @format int64
   */
  id: number
}

/** 修改 */
export interface DictUpdateInput {
  /**
   * 字典类型Id
   * @format int64
   */
  dictTypeId?: number
  /**
   * 字典名称
   * @minLength 1
   */
  name: string
  /** 字典编码 */
  code?: string | null
  /** 字典值 */
  value?: string | null
  /** 描述 */
  description?: string | null
  /** 启用 */
  enabled?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /**
   * 主键Id
   * @format int64
   */
  id: number
}

export interface DocumentAddGroupInput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /** 文档类型:Group=1,Markdown=2 */
  type?: DocumentType
  /** 名称 */
  label?: string | null
  /** 命名 */
  name?: string | null
  /** 打开 */
  opened?: boolean | null
}

export interface DocumentAddImageInput {
  /**
   * 用户Id
   * @format int64
   */
  documentId?: number
  /** 请求路径 */
  url?: string | null
}

export interface DocumentAddMenuInput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /** 文档类型:Group=1,Markdown=2 */
  type?: DocumentType
  /** 命名 */
  name?: string | null
  /** 名称 */
  label?: string | null
  /** 说明 */
  description?: string | null
}

export interface DocumentGetContentOutput {
  /**
   * 编号
   * @format int64
   */
  id?: number
  /** 名称 */
  label?: string | null
  /** 内容 */
  content?: string | null
}

export interface DocumentGetGroupOutput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /** 文档类型:Group=1,Markdown=2 */
  type?: DocumentType
  /** 名称 */
  label?: string | null
  /** 命名 */
  name?: string | null
  /** 打开 */
  opened?: boolean | null
  /**
   * 编号
   * @format int64
   */
  id: number
}

export interface DocumentGetMenuOutput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /** 文档类型:Group=1,Markdown=2 */
  type?: DocumentType
  /** 命名 */
  name?: string | null
  /** 名称 */
  label?: string | null
  /** 说明 */
  description?: string | null
  /**
   * 编号
   * @format int64
   */
  id: number
}

export interface DocumentListOutput {
  /**
   * 编号
   * @format int64
   */
  id?: number
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /** 名称 */
  label?: string | null
  /** 文档类型:Group=1,Markdown=2 */
  type?: DocumentType
  /** 命名 */
  name?: string | null
  /** 描述 */
  description?: string | null
  /** 组打开 */
  opened?: boolean | null
}

/**
 * 文档类型:Group=1,Markdown=2
 * @format int32
 */
export type DocumentType = 1 | 2

export interface DocumentUpdateContentInput {
  /**
   * 编号
   * @format int64
   */
  id: number
  /** 名称 */
  label?: string | null
  /** 内容 */
  content?: string | null
  /** Html */
  html?: string | null
}

export interface DocumentUpdateGroupInput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /** 文档类型:Group=1,Markdown=2 */
  type?: DocumentType
  /** 名称 */
  label?: string | null
  /** 命名 */
  name?: string | null
  /** 打开 */
  opened?: boolean | null
  /**
   * 编号
   * @format int64
   */
  id: number
}

export interface DocumentUpdateMenuInput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /** 文档类型:Group=1,Markdown=2 */
  type?: DocumentType
  /** 命名 */
  name?: string | null
  /** 名称 */
  label?: string | null
  /** 说明 */
  description?: string | null
  /**
   * 编号
   * @format int64
   */
  id: number
}

export interface DynamicFilterInfo {
  field?: string | null
  /** Contains=0,StartsWith=1,EndsWith=2,NotContains=3,NotStartsWith=4,NotEndsWith=5,Equal=6,Equals=7,Eq=8,NotEqual=9,GreaterThan=10,GreaterThanOrEqual=11,LessThan=12,LessThanOrEqual=13,Range=14,DateRange=15,Any=16,NotAny=17,Custom=18 */
  operator?: DynamicFilterOperator
  value?: any
  /** And=0,Or=1 */
  logic?: DynamicFilterLogic
  filters?: DynamicFilterInfo[] | null
}

/**
 * And=0,Or=1
 * @format int32
 */
export type DynamicFilterLogic = 0 | 1

/**
 * Contains=0,StartsWith=1,EndsWith=2,NotContains=3,NotStartsWith=4,NotEndsWith=5,Equal=6,Equals=7,Eq=8,NotEqual=9,GreaterThan=10,GreaterThanOrEqual=11,LessThan=12,LessThanOrEqual=13,Range=14,DateRange=15,Any=16,NotAny=17,Custom=18
 * @format int32
 */
export type DynamicFilterOperator = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12 | 13 | 14 | 15 | 16 | 17 | 18

export interface FileDeleteInput {
  /**
   * 文件Id
   * @format int64
   */
  id: number
}

/** 文件 */
export interface FileEntity {
  /**
   * 主键Id
   * @format int64
   */
  id?: number
  /**
   * 创建者用户Id
   * @format int64
   */
  createdUserId?: number | null
  /**
   * 创建者用户名
   * @maxLength 50
   */
  createdUserName?: string | null
  /**
   * 创建者姓名
   * @maxLength 50
   */
  createdUserRealName?: string | null
  /**
   * 创建时间
   * @format date-time
   */
  createdTime?: string | null
  /**
   * 修改者用户Id
   * @format int64
   */
  modifiedUserId?: number | null
  /**
   * 修改者用户名
   * @maxLength 50
   */
  modifiedUserName?: string | null
  /**
   * 修改者姓名
   * @maxLength 50
   */
  modifiedUserRealName?: string | null
  /**
   * 修改时间
   * @format date-time
   */
  modifiedTime?: string | null
  /** 是否删除 */
  isDeleted?: boolean
  /** Invalid=0,Minio=1,Aliyun=2,QCloud=3,Qiniu=4,HuaweiCloud=5,BaiduCloud=6,Ctyun=7 */
  provider?: OSSProvider
  /** 存储桶名称 */
  bucketName?: string | null
  /** 文件目录 */
  fileDirectory?: string | null
  /**
   * 文件Guid
   * @format uuid
   */
  fileGuid?: string
  /** 保存文件名 */
  saveFileName?: string | null
  /** 文件名 */
  fileName?: string | null
  /** 文件扩展名 */
  extension?: string | null
  /**
   * 文件字节长度
   * @format int64
   */
  size?: number
  /** 文件大小格式化 */
  sizeFormat?: string | null
  /** 链接地址 */
  linkUrl?: string | null
  /** md5码，防止上传重复文件 */
  md5?: string | null
}

export interface FileGetPageDto {
  /** 文件名 */
  fileName?: string | null
}

export interface FileGetPageOutput {
  /**
   * 文件Id
   * @format int64
   */
  id?: number
  /** OSS供应商 */
  providerName?: string | null
  /** 存储桶名称 */
  bucketName?: string | null
  /** 文件目录 */
  fileDirectory?: string | null
  /**
   * 文件Guid
   * @format uuid
   */
  fileGuid?: string
  /** 文件名 */
  fileName?: string | null
  /** 文件扩展名 */
  extension?: string | null
  /** 文件大小格式化 */
  sizeFormat?: string | null
  /** 链接地址 */
  linkUrl?: string | null
  /** 创建者 */
  createdUserName?: string | null
  /**
   * 创建时间
   * @format date-time
   */
  createdTime?: string | null
  /** 修改者 */
  modifiedUserName?: string | null
  /**
   * 修改时间
   * @format date-time
   */
  modifiedTime?: string | null
}

export interface LogGetPageDto {
  /** 创建者 */
  createdUserName?: string | null
}

/** 添加 */
export interface LoginLogAddInput {
  /**
   * 租户Id
   * @format int64
   */
  tenantId?: number | null
  /** 姓名 */
  name?: string | null
  /** IP */
  ip?: string | null
  /** 浏览器 */
  browser?: string | null
  /** 操作系统 */
  os?: string | null
  /** 设备 */
  device?: string | null
  /** 浏览器信息 */
  browserInfo?: string | null
  /**
   * 耗时（毫秒）
   * @format int64
   */
  elapsedMilliseconds?: number
  /** 操作状态 */
  status?: boolean | null
  /** 操作消息 */
  msg?: string | null
  /** 操作结果 */
  result?: string | null
  /**
   * 创建者Id
   * @format int64
   */
  createdUserId?: number | null
  /** 创建者 */
  createdUserName?: string | null
}

export interface LoginLogListOutput {
  /**
   * 编号
   * @format int64
   */
  id?: number
  /** 昵称 */
  nickName?: string | null
  /** 创建者 */
  createdUserName?: string | null
  /** IP */
  ip?: string | null
  /** 浏览器 */
  browser?: string | null
  /** 操作系统 */
  os?: string | null
  /** 设备 */
  device?: string | null
  /**
   * 耗时（毫秒）
   * @format int64
   */
  elapsedMilliseconds?: number
  /** 操作状态 */
  status?: boolean
  /** 操作消息 */
  msg?: string | null
  /**
   * 创建时间
   * @format date-time
   */
  createdTime?: string | null
}

/**
 * Invalid=0,Minio=1,Aliyun=2,QCloud=3,Qiniu=4,HuaweiCloud=5,BaiduCloud=6,Ctyun=7
 * @format int32
 */
export type OSSProvider = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7

/** 添加 */
export interface OprationLogAddInput {
  /** 姓名 */
  name?: string | null
  /** 接口名称 */
  apiLabel?: string | null
  /** 接口地址 */
  apiPath?: string | null
  /** 接口提交方法 */
  apiMethod?: string | null
  /** IP */
  ip?: string | null
  /** 浏览器 */
  browser?: string | null
  /** 操作系统 */
  os?: string | null
  /** 设备 */
  device?: string | null
  /** 浏览器信息 */
  browserInfo?: string | null
  /**
   * 耗时（毫秒）
   * @format int64
   */
  elapsedMilliseconds?: number
  /** 操作状态 */
  status?: boolean | null
  /** 操作消息 */
  msg?: string | null
  /** 操作参数 */
  params?: string | null
  /** 操作结果 */
  result?: string | null
}

export interface OprationLogListOutput {
  /**
   * 编号
   * @format int64
   */
  id?: number
  /** 昵称 */
  nickName?: string | null
  /** 创建者 */
  createdUserName?: string | null
  /** 接口名称 */
  apiLabel?: string | null
  /** 接口地址 */
  apiPath?: string | null
  /** 接口提交方法 */
  apiMethod?: string | null
  /** IP */
  ip?: string | null
  /** 浏览器 */
  browser?: string | null
  /** 操作系统 */
  os?: string | null
  /** 设备 */
  device?: string | null
  /**
   * 耗时（毫秒）
   * @format int64
   */
  elapsedMilliseconds?: number
  /** 操作状态 */
  status?: boolean
  /** 操作消息 */
  msg?: string | null
  /**
   * 创建时间
   * @format date-time
   */
  createdTime?: string | null
}

/** 选项 */
export interface Options {
  /** 名称 */
  name?: string | null
  /** 描述 */
  desc?: string | null
  /**
   * 值
   * @format int64
   */
  value?: number
}

/** 添加 */
export interface OrgAddInput {
  /**
   * 父级
   * @format int64
   */
  parentId?: number
  /** 名称 */
  name?: string | null
  /** 编码 */
  code?: string | null
  /** 值 */
  value?: string | null
  /** 启用 */
  enabled?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 描述 */
  description?: string | null
}

/** 组织架构 */
export interface OrgEntity {
  /**
   * 主键Id
   * @format int64
   */
  id?: number
  /**
   * 创建者用户Id
   * @format int64
   */
  createdUserId?: number | null
  /**
   * 创建者用户名
   * @maxLength 50
   */
  createdUserName?: string | null
  /**
   * 创建者姓名
   * @maxLength 50
   */
  createdUserRealName?: string | null
  /**
   * 创建时间
   * @format date-time
   */
  createdTime?: string | null
  /**
   * 修改者用户Id
   * @format int64
   */
  modifiedUserId?: number | null
  /**
   * 修改者用户名
   * @maxLength 50
   */
  modifiedUserName?: string | null
  /**
   * 修改者姓名
   * @maxLength 50
   */
  modifiedUserRealName?: string | null
  /**
   * 修改时间
   * @format date-time
   */
  modifiedTime?: string | null
  /** 是否删除 */
  isDeleted?: boolean
  /**
   * 租户Id
   * @format int64
   */
  tenantId?: number | null
  /**
   * 父级
   * @format int64
   */
  parentId?: number
  /** 名称 */
  name?: string | null
  /** 编码 */
  code?: string | null
  /** 值 */
  value?: string | null
  /**
   * 成员数
   * @format int32
   */
  memberCount?: number
  /** 启用 */
  enabled?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 描述 */
  description?: string | null
  /** 员工列表 */
  staffs?: UserStaffEntity[] | null
  /** 用户列表 */
  users?: UserEntity[] | null
  /** 角色列表 */
  roles?: RoleEntity[] | null
  /** 子级列表 */
  childs?: OrgEntity[] | null
}

export interface OrgGetOutput {
  /**
   * 父级
   * @format int64
   */
  parentId?: number
  /** 名称 */
  name?: string | null
  /** 编码 */
  code?: string | null
  /** 值 */
  value?: string | null
  /** 启用 */
  enabled?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 描述 */
  description?: string | null
  /**
   * 主键Id
   * @format int64
   */
  id: number
}

export interface OrgListOutput {
  /**
   * 主键Id
   * @format int64
   */
  id?: number
  /**
   * 父级
   * @format int64
   */
  parentId?: number
  /** 名称 */
  name?: string | null
  /** 编码 */
  code?: string | null
  /** 值 */
  value?: string | null
  /** 启用 */
  enabled?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 描述 */
  description?: string | null
  /**
   * 创建时间
   * @format date-time
   */
  createdTime?: string | null
}

/** 修改 */
export interface OrgUpdateInput {
  /**
   * 父级
   * @format int64
   */
  parentId?: number
  /** 名称 */
  name?: string | null
  /** 编码 */
  code?: string | null
  /** 值 */
  value?: string | null
  /** 启用 */
  enabled?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 描述 */
  description?: string | null
  /**
   * 主键Id
   * @format int64
   */
  id: number
}

/** 分页信息输入 */
export interface PageInputApiGetPageDto {
  /**
   * 当前页标
   * @format int32
   */
  currentPage?: number
  /**
   * 每页大小
   * @format int32
   */
  pageSize?: number
  dynamicFilter?: DynamicFilterInfo
  filter?: ApiGetPageDto
}

/** 分页信息输入 */
export interface PageInputDictGetPageDto {
  /**
   * 当前页标
   * @format int32
   */
  currentPage?: number
  /**
   * 每页大小
   * @format int32
   */
  pageSize?: number
  dynamicFilter?: DynamicFilterInfo
  filter?: DictGetPageDto
}

/** 分页信息输入 */
export interface PageInputDictTypeGetPageDto {
  /**
   * 当前页标
   * @format int32
   */
  currentPage?: number
  /**
   * 每页大小
   * @format int32
   */
  pageSize?: number
  dynamicFilter?: DynamicFilterInfo
  filter?: DictTypeGetPageDto
}

/** 分页信息输入 */
export interface PageInputFileGetPageDto {
  /**
   * 当前页标
   * @format int32
   */
  currentPage?: number
  /**
   * 每页大小
   * @format int32
   */
  pageSize?: number
  dynamicFilter?: DynamicFilterInfo
  filter?: FileGetPageDto
}

/** 分页信息输入 */
export interface PageInputLogGetPageDto {
  /**
   * 当前页标
   * @format int32
   */
  currentPage?: number
  /**
   * 每页大小
   * @format int32
   */
  pageSize?: number
  dynamicFilter?: DynamicFilterInfo
  filter?: LogGetPageDto
}

/** 分页信息输入 */
export interface PageInputPkgGetPageDto {
  /**
   * 当前页标
   * @format int32
   */
  currentPage?: number
  /**
   * 每页大小
   * @format int32
   */
  pageSize?: number
  dynamicFilter?: DynamicFilterInfo
  filter?: PkgGetPageDto
}

/** 分页信息输入 */
export interface PageInputPkgGetPkgTenantListInput {
  /**
   * 当前页标
   * @format int32
   */
  currentPage?: number
  /**
   * 每页大小
   * @format int32
   */
  pageSize?: number
  dynamicFilter?: DynamicFilterInfo
  filter?: PkgGetPkgTenantListInput
}

/** 分页信息输入 */
export interface PageInputRegionGetPageInput {
  /**
   * 当前页标
   * @format int32
   */
  currentPage?: number
  /**
   * 每页大小
   * @format int32
   */
  pageSize?: number
  dynamicFilter?: DynamicFilterInfo
  filter?: RegionGetPageInput
}

/** 分页信息输入 */
export interface PageInputRoleGetPageDto {
  /**
   * 当前页标
   * @format int32
   */
  currentPage?: number
  /**
   * 每页大小
   * @format int32
   */
  pageSize?: number
  dynamicFilter?: DynamicFilterInfo
  filter?: RoleGetPageDto
}

/** 分页信息输入 */
export interface PageInputTaskGetPageInput {
  /**
   * 当前页标
   * @format int32
   */
  currentPage?: number
  /**
   * 每页大小
   * @format int32
   */
  pageSize?: number
  dynamicFilter?: DynamicFilterInfo
  filter?: TaskGetPageInput
}

/** 分页信息输入 */
export interface PageInputTaskLogGetPageDto {
  /**
   * 当前页标
   * @format int32
   */
  currentPage?: number
  /**
   * 每页大小
   * @format int32
   */
  pageSize?: number
  dynamicFilter?: DynamicFilterInfo
  filter?: TaskLogGetPageDto
}

/** 分页信息输入 */
export interface PageInputTenantGetPageDto {
  /**
   * 当前页标
   * @format int32
   */
  currentPage?: number
  /**
   * 每页大小
   * @format int32
   */
  pageSize?: number
  dynamicFilter?: DynamicFilterInfo
  filter?: TenantGetPageDto
}

/** 分页信息输入 */
export interface PageInputUserGetPageDto {
  /**
   * 当前页标
   * @format int32
   */
  currentPage?: number
  /**
   * 每页大小
   * @format int32
   */
  pageSize?: number
  dynamicFilter?: DynamicFilterInfo
  /** 用户分页查询条件 */
  filter?: UserGetPageDto
}

/** 分页信息输出 */
export interface PageOutputApiEntity {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: ApiEntity[] | null
}

/** 分页信息输出 */
export interface PageOutputDictGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: DictGetPageOutput[] | null
}

/** 分页信息输出 */
export interface PageOutputDictTypeGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: DictTypeGetPageOutput[] | null
}

/** 分页信息输出 */
export interface PageOutputFileGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: FileGetPageOutput[] | null
}

/** 分页信息输出 */
export interface PageOutputLoginLogListOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: LoginLogListOutput[] | null
}

/** 分页信息输出 */
export interface PageOutputOprationLogListOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: OprationLogListOutput[] | null
}

/** 分页信息输出 */
export interface PageOutputPkgGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: PkgGetPageOutput[] | null
}

/** 分页信息输出 */
export interface PageOutputPkgGetPkgTenantListOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: PkgGetPkgTenantListOutput[] | null
}

/** 分页信息输出 */
export interface PageOutputRegionGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: RegionGetPageOutput[] | null
}

/** 分页信息输出 */
export interface PageOutputRoleGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: RoleGetPageOutput[] | null
}

/** 分页信息输出 */
export interface PageOutputTaskListOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: TaskListOutput[] | null
}

/** 分页信息输出 */
export interface PageOutputTaskLog {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: TaskLog[] | null
}

/** 分页信息输出 */
export interface PageOutputTenantListOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: TenantListOutput[] | null
}

/** 分页信息输出 */
export interface PageOutputUserGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: UserGetPageOutput[] | null
}

/**
 * 密码加密类型:MD5Encrypt32=0,PasswordHasher=1
 * @format int32
 */
export type PasswordEncryptType = 0 | 1

export interface PermissionAddApiInput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /**
   * 接口
   * @format int64
   */
  apiId?: number | null
  /** 权限名称 */
  label?: string | null
  /** 权限编码 */
  code?: string | null
  /** 说明 */
  description?: string | null
  /** 隐藏 */
  hidden?: boolean
  /** 图标 */
  icon?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
}

export interface PermissionAddDotInput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /** 关联接口 */
  apiIds?: number[] | null
  /** 权限名称 */
  label?: string | null
  /** 权限编码 */
  code?: string | null
  /** 说明 */
  description?: string | null
  /** 图标 */
  icon?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
}

export interface PermissionAddGroupInput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /**
   * 视图
   * @format int64
   */
  viewId?: number | null
  /** 路由命名 */
  name?: string | null
  /** 访问路由地址 */
  path?: string | null
  /** 重定向地址 */
  redirect?: string | null
  /** 权限名称 */
  label?: string | null
  /** 隐藏 */
  hidden?: boolean
  /** 图标 */
  icon?: string | null
  /** 展开 */
  opened?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
}

export interface PermissionAddMenuInput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /**
   * 视图
   * @format int64
   */
  viewId?: number | null
  /** 路由命名 */
  name?: string | null
  /** 路由地址 */
  path?: string | null
  /** 权限名称 */
  label?: string | null
  /** 说明 */
  description?: string | null
  /** 隐藏 */
  hidden?: boolean
  /** 图标 */
  icon?: string | null
  /** 打开新窗口 */
  newWindow?: boolean
  /** 链接外显 */
  external?: boolean
  /** 是否缓存 */
  isKeepAlive?: boolean
  /** 是否固定 */
  isAffix?: boolean
  /** 链接地址 */
  link?: string | null
  /** 是否内嵌窗口 */
  isIframe?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
}

export interface PermissionAssignInput {
  /** @format int64 */
  roleId: number
  permissionIds: number[]
}

/** 权限 */
export interface PermissionEntity {
  /**
   * 主键Id
   * @format int64
   */
  id?: number
  /**
   * 创建者用户Id
   * @format int64
   */
  createdUserId?: number | null
  /**
   * 创建者用户名
   * @maxLength 50
   */
  createdUserName?: string | null
  /**
   * 创建者姓名
   * @maxLength 50
   */
  createdUserRealName?: string | null
  /**
   * 创建时间
   * @format date-time
   */
  createdTime?: string | null
  /**
   * 修改者用户Id
   * @format int64
   */
  modifiedUserId?: number | null
  /**
   * 修改者用户名
   * @maxLength 50
   */
  modifiedUserName?: string | null
  /**
   * 修改者姓名
   * @maxLength 50
   */
  modifiedUserRealName?: string | null
  /**
   * 修改时间
   * @format date-time
   */
  modifiedTime?: string | null
  /** 是否删除 */
  isDeleted?: boolean
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /** 权限名称 */
  label?: string | null
  /** 权限编码 */
  code?: string | null
  /** 权限类型:Group=1,Menu=2,Dot=3 */
  type?: PermissionType
  /**
   * 视图Id
   * @format int64
   */
  viewId?: number | null
  /** 视图管理 */
  view?: ViewEntity
  /** 路由命名 */
  name?: string | null
  /** 路由地址 */
  path?: string | null
  /** 重定向地址 */
  redirect?: string | null
  /** 图标 */
  icon?: string | null
  /** 隐藏 */
  hidden?: boolean
  /** 展开分组 */
  opened?: boolean
  /** 打开新窗口 */
  newWindow?: boolean
  /** 链接外显 */
  external?: boolean
  /** 是否缓存 */
  isKeepAlive?: boolean
  /** 是否固定 */
  isAffix?: boolean
  /** 链接地址 */
  link?: string | null
  /** 是否内嵌窗口 */
  isIframe?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 描述 */
  description?: string | null
  /** 启用 */
  enabled?: boolean
  apis?: ApiEntity[] | null
  childs?: PermissionEntity[] | null
}

export interface PermissionGetApiOutput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /**
   * 接口
   * @format int64
   */
  apiId?: number | null
  /** 权限名称 */
  label?: string | null
  /** 权限编码 */
  code?: string | null
  /** 说明 */
  description?: string | null
  /** 隐藏 */
  hidden?: boolean
  /** 图标 */
  icon?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
  /**
   * 权限Id
   * @format int64
   */
  id: number
}

export interface PermissionGetDotOutput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /** 关联接口 */
  apiIds?: number[] | null
  /** 权限名称 */
  label?: string | null
  /** 权限编码 */
  code?: string | null
  /** 说明 */
  description?: string | null
  /** 图标 */
  icon?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
  /**
   * 权限Id
   * @format int64
   */
  id: number
}

export interface PermissionGetGroupOutput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /**
   * 视图
   * @format int64
   */
  viewId?: number | null
  /** 路由命名 */
  name?: string | null
  /** 访问路由地址 */
  path?: string | null
  /** 重定向地址 */
  redirect?: string | null
  /** 权限名称 */
  label?: string | null
  /** 隐藏 */
  hidden?: boolean
  /** 图标 */
  icon?: string | null
  /** 展开 */
  opened?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
  /**
   * 权限Id
   * @format int64
   */
  id: number
}

export interface PermissionGetMenuOutput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /**
   * 视图
   * @format int64
   */
  viewId?: number | null
  /** 路由命名 */
  name?: string | null
  /** 路由地址 */
  path?: string | null
  /** 权限名称 */
  label?: string | null
  /** 说明 */
  description?: string | null
  /** 隐藏 */
  hidden?: boolean
  /** 图标 */
  icon?: string | null
  /** 打开新窗口 */
  newWindow?: boolean
  /** 链接外显 */
  external?: boolean
  /** 是否缓存 */
  isKeepAlive?: boolean
  /** 是否固定 */
  isAffix?: boolean
  /** 链接地址 */
  link?: string | null
  /** 是否内嵌窗口 */
  isIframe?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
  /**
   * 权限Id
   * @format int64
   */
  id: number
}

export interface PermissionListOutput {
  /**
   * 权限Id
   * @format int64
   */
  id?: number
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /** 权限名称 */
  label?: string | null
  /** 权限类型:Group=1,Menu=2,Dot=3 */
  type?: PermissionType
  /** 路由地址 */
  path?: string | null
  /** 重定向地址 */
  redirect?: string | null
  /** 视图地址 */
  viewPath?: string | null
  /** 链接地址 */
  link?: string | null
  /** 接口路径 */
  apiPaths?: string | null
  /** 图标 */
  icon?: string | null
  /** 展开 */
  opened?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number | null
  /** 描述 */
  description?: string | null
  /** 启用 */
  enabled?: boolean
}

export interface PermissionSaveTenantPermissionsInput {
  /** @format int64 */
  tenantId: number
  permissionIds: number[]
}

/**
 * 权限类型:Group=1,Menu=2,Dot=3
 * @format int32
 */
export type PermissionType = 1 | 2 | 3

export interface PermissionUpdateApiInput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /**
   * 接口
   * @format int64
   */
  apiId?: number | null
  /** 权限名称 */
  label?: string | null
  /** 权限编码 */
  code?: string | null
  /** 说明 */
  description?: string | null
  /** 隐藏 */
  hidden?: boolean
  /** 图标 */
  icon?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
  /**
   * 权限Id
   * @format int64
   */
  id: number
}

export interface PermissionUpdateDotInput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /** 关联接口 */
  apiIds?: number[] | null
  /** 权限名称 */
  label?: string | null
  /** 权限编码 */
  code?: string | null
  /** 说明 */
  description?: string | null
  /** 图标 */
  icon?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
  /**
   * 权限Id
   * @format int64
   */
  id: number
}

export interface PermissionUpdateGroupInput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /**
   * 视图
   * @format int64
   */
  viewId?: number | null
  /** 路由命名 */
  name?: string | null
  /** 访问路由地址 */
  path?: string | null
  /** 重定向地址 */
  redirect?: string | null
  /** 权限名称 */
  label?: string | null
  /** 隐藏 */
  hidden?: boolean
  /** 图标 */
  icon?: string | null
  /** 展开 */
  opened?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
  /**
   * 权限Id
   * @format int64
   */
  id: number
}

export interface PermissionUpdateMenuInput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /**
   * 视图
   * @format int64
   */
  viewId?: number | null
  /** 路由命名 */
  name?: string | null
  /** 路由地址 */
  path?: string | null
  /** 权限名称 */
  label?: string | null
  /** 说明 */
  description?: string | null
  /** 隐藏 */
  hidden?: boolean
  /** 图标 */
  icon?: string | null
  /** 打开新窗口 */
  newWindow?: boolean
  /** 链接外显 */
  external?: boolean
  /** 是否缓存 */
  isKeepAlive?: boolean
  /** 是否固定 */
  isAffix?: boolean
  /** 链接地址 */
  link?: string | null
  /** 是否内嵌窗口 */
  isIframe?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
  /**
   * 权限Id
   * @format int64
   */
  id: number
}

/** 添加 */
export interface PkgAddInput {
  /**
   * 父级Id
   * @format int64
   */
  parentId?: number
  /** 名称 */
  name?: string | null
  /** 编码 */
  code?: string | null
  /** 说明 */
  description?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
}

/** 添加套餐租户列表 */
export interface PkgAddPkgTenantListInput {
  /**
   * 套餐
   * @format int64
   */
  pkgId: number
  /** 租户列表 */
  tenantIds?: number[] | null
}

/** 套餐 */
export interface PkgEntity {
  /**
   * 主键Id
   * @format int64
   */
  id?: number
  /**
   * 创建者用户Id
   * @format int64
   */
  createdUserId?: number | null
  /**
   * 创建者用户名
   * @maxLength 50
   */
  createdUserName?: string | null
  /**
   * 创建者姓名
   * @maxLength 50
   */
  createdUserRealName?: string | null
  /**
   * 创建时间
   * @format date-time
   */
  createdTime?: string | null
  /**
   * 修改者用户Id
   * @format int64
   */
  modifiedUserId?: number | null
  /**
   * 修改者用户名
   * @maxLength 50
   */
  modifiedUserName?: string | null
  /**
   * 修改者姓名
   * @maxLength 50
   */
  modifiedUserRealName?: string | null
  /**
   * 修改时间
   * @format date-time
   */
  modifiedTime?: string | null
  /** 是否删除 */
  isDeleted?: boolean
  /**
   * 父级Id
   * @format int64
   */
  parentId?: number
  /** 子级列表 */
  childs?: PkgEntity[] | null
  /** 名称 */
  name?: string | null
  /** 编码 */
  code?: string | null
  /** 说明 */
  description?: string | null
  /** 启用 */
  enabled?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 租户列表 */
  tenants?: TenantEntity[] | null
  /** 权限列表 */
  permissions?: PermissionEntity[] | null
}

export interface PkgGetListOutput {
  /**
   * 主键
   * @format int64
   */
  id?: number
  /**
   * 父级Id
   * @format int64
   */
  parentId?: number
  /** 名称 */
  name?: string | null
  /** 编码 */
  code?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 描述 */
  description?: string | null
  /** 启用 */
  enabled?: boolean
  /**
   * 创建时间
   * @format date-time
   */
  createdTime?: string | null
}

export interface PkgGetOutput {
  /**
   * 父级Id
   * @format int64
   */
  parentId?: number
  /** 名称 */
  name?: string | null
  /** 编码 */
  code?: string | null
  /** 说明 */
  description?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
  /**
   * 套餐Id
   * @format int64
   */
  id: number
}

export interface PkgGetPageDto {
  /** 名称 */
  name?: string | null
}

export interface PkgGetPageOutput {
  /**
   * 主键
   * @format int64
   */
  id?: number
  /** 名称 */
  name?: string | null
  /** 编码 */
  code?: string | null
  /** 说明 */
  description?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
  /**
   * 创建时间
   * @format date-time
   */
  createdTime?: string | null
}

export interface PkgGetPkgTenantListInput {
  /** 租户名 */
  tenantName?: string | null
  /**
   * 套餐Id
   * @format int64
   */
  pkgId?: number | null
}

export interface PkgGetPkgTenantListOutput {
  /**
   * 主键Id
   * @format int64
   */
  id?: number
  /** 租户名 */
  name?: string | null
  /** 租户编码 */
  code?: string | null
}

export interface PkgSetPkgPermissionsInput {
  /** @format int64 */
  pkgId: number
  permissionIds: number[]
}

/** 修改 */
export interface PkgUpdateInput {
  /**
   * 父级Id
   * @format int64
   */
  parentId?: number
  /** 名称 */
  name?: string | null
  /** 编码 */
  code?: string | null
  /** 说明 */
  description?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
  /**
   * 套餐Id
   * @format int64
   */
  id: number
}

/** 项目配置 */
export interface ProjectConfig {
  /** 名称 */
  name?: string | null
  /** 编码 */
  code?: string | null
  /** 版本 */
  version?: string | null
  /** 描述 */
  description?: string | null
}

/** 添加 */
export interface RegionAddInput {
  /**
   * 上级Id
   * @format int64
   */
  parentId?: number
  /** 名称 */
  name?: string | null
  /** 地区级别:Province=1,City=2,County=3,Town(镇/乡)=4,Vilage(村/社区)=5 */
  level?: RegionLevel
  /** 代码 */
  code?: string | null
  /** 提取Url */
  url?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number | null
  /** 热门 */
  hot?: boolean
  /** 启用 */
  enabled?: boolean
}

export interface RegionGetChildListOutput {
  /**
   * 主键Id
   * @format int64
   */
  id?: number
  /** 名称 */
  name?: string | null
  /** 地区级别:Province=1,City=2,County=3,Town(镇/乡)=4,Vilage(村/社区)=5 */
  level?: RegionLevel
  /** 拼音 */
  pinyin?: string | null
  /** 拼音首字母 */
  pinyinFirst?: string | null
  /** 启用 */
  enabled?: boolean
  /** 热门 */
  hot?: boolean
  /** 叶子节点 */
  leaf?: boolean
}

export interface RegionGetListInput {
  /**
   * 上级Id
   * @format int64
   */
  parentId?: number
  /** 热门 */
  hot?: boolean | null
  /** 启用 */
  enabled?: boolean | null
}

export interface RegionGetOutput {
  /**
   * 上级Id
   * @format int64
   */
  parentId?: number
  /** 名称 */
  name?: string | null
  /** 地区级别:Province=1,City=2,County=3,Town(镇/乡)=4,Vilage(村/社区)=5 */
  level?: RegionLevel
  /** 代码 */
  code?: string | null
  /** 提取Url */
  url?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number | null
  /** 热门 */
  hot?: boolean
  /** 启用 */
  enabled?: boolean
  /**
   * 主键Id
   * @format int64
   */
  id: number
  parentIdList?: number[] | null
}

export interface RegionGetPageInput {
  /**
   * 上级Id
   * @format int64
   */
  parentId?: number | null
  /** 名称 */
  name?: string | null
  /** 地区级别:Province=1,City=2,County=3,Town(镇/乡)=4,Vilage(村/社区)=5 */
  level?: RegionLevel
  /** 热门 */
  hot?: boolean | null
  /** 启用 */
  enabled?: boolean | null
}

export interface RegionGetPageOutput {
  /**
   * 主键Id
   * @format int64
   */
  id?: number
  /**
   * 上级Id
   * @format int64
   */
  parentId?: number
  /** 名称 */
  name?: string | null
  /** 地区级别:Province=1,City=2,County=3,Town(镇/乡)=4,Vilage(村/社区)=5 */
  level?: RegionLevel
  /** 代码 */
  code?: string | null
  /** 拼音 */
  pinyin?: string | null
  /** 拼音首字母 */
  pinyinFirst?: string | null
  /** 城乡分类代码 */
  vilageCode?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number | null
  /** 热门 */
  hot?: boolean
  /** 启用 */
  enabled?: boolean
}

/**
 * 地区级别:Province=1,City=2,County=3,Town(镇/乡)=4,Vilage(村/社区)=5
 * @format int32
 */
export type RegionLevel = 1 | 2 | 3 | 4 | 5

/** 设置启用 */
export interface RegionSetEnableInput {
  /**
   * 地区Id
   * @format int64
   */
  regionId?: number
  /** 是否启用 */
  enabled?: boolean
}

/** 设置热门 */
export interface RegionSetHotInput {
  /**
   * 地区Id
   * @format int64
   */
  regionId?: number
  /** 热门 */
  hot?: boolean
}

/** 修改 */
export interface RegionUpdateInput {
  /**
   * 上级Id
   * @format int64
   */
  parentId?: number
  /** 名称 */
  name?: string | null
  /** 地区级别:Province=1,City=2,County=3,Town(镇/乡)=4,Vilage(村/社区)=5 */
  level?: RegionLevel
  /** 代码 */
  code?: string | null
  /** 提取Url */
  url?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number | null
  /** 热门 */
  hot?: boolean
  /** 启用 */
  enabled?: boolean
  /**
   * 主键Id
   * @format int64
   */
  id: number
}

/** 结果输出 */
export interface ResultOutputApiGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: ApiGetOutput
}

/** 结果输出 */
export interface ResultOutputAuthGetPasswordEncryptKeyOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: AuthGetPasswordEncryptKeyOutput
}

/** 结果输出 */
export interface ResultOutputAuthGetUserInfoOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: AuthGetUserInfoOutput
}

/** 结果输出 */
export interface ResultOutputAuthGetUserPermissionsOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: AuthGetUserPermissionsOutput
}

/** 结果输出 */
export interface ResultOutputAuthUserProfileDto {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 用户个人信息 */
  data?: AuthUserProfileDto
}

/** 结果输出 */
export interface ResultOutputBoolean {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: boolean
}

/** 结果输出 */
export interface ResultOutputCaptchaData {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: CaptchaData
}

/** 结果输出 */
export interface ResultOutputDictGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: DictGetOutput
}

/** 结果输出 */
export interface ResultOutputDictTypeGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: DictTypeGetOutput
}

/** 结果输出 */
export interface ResultOutputDictionaryStringListDictGetListDto {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: Record<string, DictGetListDto[] | null>
}

/** 结果输出 */
export interface ResultOutputDocumentGetContentOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: DocumentGetContentOutput
}

/** 结果输出 */
export interface ResultOutputDocumentGetGroupOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: DocumentGetGroupOutput
}

/** 结果输出 */
export interface ResultOutputDocumentGetMenuOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: DocumentGetMenuOutput
}

/** 结果输出 */
export interface ResultOutputFileEntity {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 文件 */
  data?: FileEntity
}

/** 结果输出 */
export interface ResultOutputIEnumerableObject {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: any[] | null
}

/** 结果输出 */
export interface ResultOutputInt64 {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /**
   * 数据
   * @format int64
   */
  data?: number
}

/** 结果输出 */
export interface ResultOutputListApiGetEnumsOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: ApiGetEnumsOutput[] | null
}

/** 结果输出 */
export interface ResultOutputListApiListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: ApiListOutput[] | null
}

/** 结果输出 */
export interface ResultOutputListAuthUserMenuDto {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: AuthUserMenuDto[] | null
}

/** 结果输出 */
export interface ResultOutputListDocumentListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: DocumentListOutput[] | null
}

/** 结果输出 */
export interface ResultOutputListFileEntity {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: FileEntity[] | null
}

/** 结果输出 */
export interface ResultOutputListInt64 {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: number[] | null
}

/** 结果输出 */
export interface ResultOutputListObject {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: any[] | null
}

/** 结果输出 */
export interface ResultOutputListOrgListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: OrgListOutput[] | null
}

/** 结果输出 */
export interface ResultOutputListPermissionListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: PermissionListOutput[] | null
}

/** 结果输出 */
export interface ResultOutputListPkgGetListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: PkgGetListOutput[] | null
}

/** 结果输出 */
export interface ResultOutputListPkgGetPkgTenantListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: PkgGetPkgTenantListOutput[] | null
}

/** 结果输出 */
export interface ResultOutputListProjectConfig {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: ProjectConfig[] | null
}

/** 结果输出 */
export interface ResultOutputListRegionGetChildListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: RegionGetChildListOutput[] | null
}

/** 结果输出 */
export interface ResultOutputListRoleGetListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: RoleGetListOutput[] | null
}

/** 结果输出 */
export interface ResultOutputListRoleGetRoleUserListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: RoleGetRoleUserListOutput[] | null
}

/** 结果输出 */
export interface ResultOutputListString {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: string[] | null
}

/** 结果输出 */
export interface ResultOutputListViewListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: ViewListOutput[] | null
}

/** 结果输出 */
export interface ResultOutputObject {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: any
}

/** 结果输出 */
export interface ResultOutputOrgGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: OrgGetOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputApiEntity {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputApiEntity
}

/** 结果输出 */
export interface ResultOutputPageOutputDictGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputDictGetPageOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputDictTypeGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputDictTypeGetPageOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputFileGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputFileGetPageOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputLoginLogListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputLoginLogListOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputOprationLogListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputOprationLogListOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputPkgGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputPkgGetPageOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputPkgGetPkgTenantListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputPkgGetPkgTenantListOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputRegionGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputRegionGetPageOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputRoleGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputRoleGetPageOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputTaskListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputTaskListOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputTaskLog {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputTaskLog
}

/** 结果输出 */
export interface ResultOutputPageOutputTenantListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputTenantListOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputUserGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputUserGetPageOutput
}

/** 结果输出 */
export interface ResultOutputPermissionGetApiOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: PermissionGetApiOutput
}

/** 结果输出 */
export interface ResultOutputPermissionGetDotOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: PermissionGetDotOutput
}

/** 结果输出 */
export interface ResultOutputPermissionGetGroupOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: PermissionGetGroupOutput
}

/** 结果输出 */
export interface ResultOutputPermissionGetMenuOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: PermissionGetMenuOutput
}

/** 结果输出 */
export interface ResultOutputPkgGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: PkgGetOutput
}

/** 结果输出 */
export interface ResultOutputRegionGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: RegionGetOutput
}

/** 结果输出 */
export interface ResultOutputRoleGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: RoleGetOutput
}

/** 结果输出 */
export interface ResultOutputString {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: string | null
}

/** 结果输出 */
export interface ResultOutputTaskGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: TaskGetOutput
}

/** 结果输出 */
export interface ResultOutputTenantGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: TenantGetOutput
}

/** 结果输出 */
export interface ResultOutputUserGetBasicOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: UserGetBasicOutput
}

/** 结果输出 */
export interface ResultOutputUserGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: UserGetOutput
}

/** 结果输出 */
export interface ResultOutputUserGetPermissionOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 用户权限 */
  data?: UserGetPermissionOutput
}

/** 结果输出 */
export interface ResultOutputValidateResult {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: ValidateResult
}

/** 结果输出 */
export interface ResultOutputViewGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: ViewGetOutput
}

/** 添加 */
export interface RoleAddInput {
  /**
   * 父级Id
   * @format int64
   */
  parentId?: number
  /** 名称 */
  name?: string | null
  /** 编码 */
  code?: string | null
  /** 角色类型:Group=1,Role=2 */
  type?: RoleType
  /** 数据范围:All=1,DeptWithChild=2,Dept=3,Self=4,Custom=5 */
  dataScope?: DataScope
  /** 指定部门 */
  orgIds?: number[] | null
  /** 部门列表 */
  orgs?: OrgEntity[] | null
  /** 说明 */
  description?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number
}

/** 添加角色用户列表 */
export interface RoleAddRoleUserListInput {
  /**
   * 角色
   * @format int64
   */
  roleId: number
  /** 用户 */
  userIds?: number[] | null
}

/** 角色 */
export interface RoleEntity {
  /**
   * 主键Id
   * @format int64
   */
  id?: number
  /**
   * 创建者用户Id
   * @format int64
   */
  createdUserId?: number | null
  /**
   * 创建者用户名
   * @maxLength 50
   */
  createdUserName?: string | null
  /**
   * 创建者姓名
   * @maxLength 50
   */
  createdUserRealName?: string | null
  /**
   * 创建时间
   * @format date-time
   */
  createdTime?: string | null
  /**
   * 修改者用户Id
   * @format int64
   */
  modifiedUserId?: number | null
  /**
   * 修改者用户名
   * @maxLength 50
   */
  modifiedUserName?: string | null
  /**
   * 修改者姓名
   * @maxLength 50
   */
  modifiedUserRealName?: string | null
  /**
   * 修改时间
   * @format date-time
   */
  modifiedTime?: string | null
  /** 是否删除 */
  isDeleted?: boolean
  /**
   * 租户Id
   * @format int64
   */
  tenantId?: number | null
  /**
   * 父级Id
   * @format int64
   */
  parentId?: number
  /** 子级列表 */
  childs?: RoleEntity[] | null
  /** 名称 */
  name?: string | null
  /** 编码 */
  code?: string | null
  /** 角色类型:Group=1,Role=2 */
  type?: RoleType
  /** 数据范围:All=1,DeptWithChild=2,Dept=3,Self=4,Custom=5 */
  dataScope?: DataScope
  /** 说明 */
  description?: string | null
  /** 隐藏 */
  hidden?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 用户列表 */
  users?: UserEntity[] | null
  /** 部门列表 */
  orgs?: OrgEntity[] | null
  /** 权限列表 */
  permissions?: PermissionEntity[] | null
}

export interface RoleGetListOutput {
  /**
   * 主键
   * @format int64
   */
  id?: number
  /**
   * 父级Id
   * @format int64
   */
  parentId?: number
  /** 名称 */
  name?: string | null
  /** 编码 */
  code?: string | null
  /** 角色类型:Group=1,Role=2 */
  type?: RoleType
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 描述 */
  description?: string | null
}

export interface RoleGetOutput {
  /**
   * 父级Id
   * @format int64
   */
  parentId?: number
  /** 名称 */
  name?: string | null
  /** 编码 */
  code?: string | null
  /** 角色类型:Group=1,Role=2 */
  type?: RoleType
  /** 数据范围:All=1,DeptWithChild=2,Dept=3,Self=4,Custom=5 */
  dataScope?: DataScope
  /** 指定部门 */
  orgIds?: number[] | null
  /** 部门列表 */
  orgs?: OrgEntity[] | null
  /** 说明 */
  description?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /**
   * 角色Id
   * @format int64
   */
  id: number
}

export interface RoleGetPageDto {
  /** 名称 */
  name?: string | null
}

export interface RoleGetPageOutput {
  /**
   * 主键
   * @format int64
   */
  id?: number
  /** 名称 */
  name?: string | null
  /** 编码 */
  code?: string | null
  /** 说明 */
  description?: string | null
  /** 隐藏 */
  hidden?: boolean
  /**
   * 创建时间
   * @format date-time
   */
  createdTime?: string | null
}

export interface RoleGetRoleUserListOutput {
  /**
   * 主键Id
   * @format int64
   */
  id?: number
  /** 姓名 */
  name?: string | null
  /** 手机号 */
  mobile?: string | null
  /** 邮箱 */
  email?: string | null
}

/** 设置数据范围 */
export interface RoleSetDataScopeInput {
  /**
   * 角色Id
   * @format int64
   */
  roleId: number
  /** 数据范围:All=1,DeptWithChild=2,Dept=3,Self=4,Custom=5 */
  dataScope?: DataScope
  /** 指定部门 */
  orgIds?: number[] | null
}

/**
 * 角色类型:Group=1,Role=2
 * @format int32
 */
export type RoleType = 1 | 2

/** 修改 */
export interface RoleUpdateInput {
  /**
   * 父级Id
   * @format int64
   */
  parentId?: number
  /** 名称 */
  name?: string | null
  /** 编码 */
  code?: string | null
  /** 角色类型:Group=1,Role=2 */
  type?: RoleType
  /** 数据范围:All=1,DeptWithChild=2,Dept=3,Self=4,Custom=5 */
  dataScope?: DataScope
  /** 指定部门 */
  orgIds?: number[] | null
  /** 部门列表 */
  orgs?: OrgEntity[] | null
  /** 说明 */
  description?: string | null
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /**
   * 角色Id
   * @format int64
   */
  id: number
}

/** 发送邮件验证码 */
export interface SendEmailCodeInput {
  /**
   * 邮件地址
   * @minLength 1
   */
  email: string
  /** 验证码Id */
  codeId?: string | null
  /**
   * 验证码Id
   * @minLength 1
   */
  captchaId: string
  track: SlideTrack
}

/** 发送短信验证码 */
export interface SendSmsCodeInput {
  /**
   * 手机号
   * @minLength 1
   */
  mobile: string
  /** 验证码Id */
  codeId?: string | null
  /**
   * 验证码Id
   * @minLength 1
   */
  captchaId: string
  track: SlideTrack
}

/**
 * 性别:Unknown(未知)=0,Male(男)=1,Female(女)=2
 * @format int32
 */
export type Sex = 0 | 1 | 2

export interface SlideTrack {
  /** @format int32 */
  backgroundImageWidth?: number
  /** @format int32 */
  backgroundImageHeight?: number
  /** @format int32 */
  sliderImageWidth?: number
  /** @format int32 */
  sliderImageHeight?: number
  /** @format date-time */
  startTime?: string
  /** @format date-time */
  endTime?: string
  tracks?: Track[] | null
  /** @format float */
  percent?: number
}

/** 员工添加 */
export interface StaffAddInput {
  /** 工号 */
  jobNumber?: string | null
  /** 职位 */
  position?: string | null
  /** 性别:Unknown(未知)=0,Male(男)=1,Female(女)=2 */
  sex?: Sex
  /**
   * 入职时间
   * @format date-time
   */
  entryTime?: string | null
  /** 企业微信名片 */
  workWeChatCard?: string | null
  /** 个人简介 */
  introduce?: string | null
}

/** 添加 */
export interface TaskAddInput {
  /** 任务标题 */
  topic?: string | null
  /** 任务参数 */
  body?: string | null
  /**
   * 任务执行多少轮，-1为永久循环
   * @format int32
   */
  round?: number
  /** SEC=1,RunOnDay=11,RunOnWeek=12,RunOnMonth=13,Custom=21 */
  interval?: TaskInterval
  /** 定时参数 60,60,60,120,120,1200,1200 */
  intervalArgument?: string | null
  /** 报警邮件，多个邮件地址则逗号分隔 */
  alarmEmail?: string | null
  /**
   * 失败重试次数
   * @format int32
   */
  failRetryCount?: number | null
  /**
   * 失败重试间隔（秒）
   * @format int32
   */
  failRetryInterval?: number | null
}

export interface TaskGetOutput {
  /** 任务标题 */
  topic?: string | null
  /** 任务参数 */
  body?: string | null
  /**
   * 任务执行多少轮，-1为永久循环
   * @format int32
   */
  round?: number
  /** SEC=1,RunOnDay=11,RunOnWeek=12,RunOnMonth=13,Custom=21 */
  interval?: TaskInterval
  /** 定时参数 60,60,60,120,120,1200,1200 */
  intervalArgument?: string | null
  /** 报警邮件，多个邮件地址则逗号分隔 */
  alarmEmail?: string | null
  /**
   * 失败重试次数
   * @format int32
   */
  failRetryCount?: number | null
  /**
   * 失败重试间隔（秒）
   * @format int32
   */
  failRetryInterval?: number | null
  /**
   * 任务Id
   * @minLength 1
   */
  id: string
}

export interface TaskGetPageInput {
  /** 分组名称 */
  groupName?: string | null
  /** 任务名称 */
  taskName?: string | null
  /** 集群Id */
  clusterId?: string | null
  /** Running=0,Paused=1,Completed=2 */
  taskStatus?: TaskStatus
  /**
   * 创建开始时间
   * @format date-time
   */
  startAddTime?: string | null
  /**
   * 创建结束时间
   * @format date-time
   */
  endAddTime?: string | null
}

/**
 * SEC=1,RunOnDay=11,RunOnWeek=12,RunOnMonth=13,Custom=21
 * @format int32
 */
export type TaskInterval = 1 | 11 | 12 | 13 | 21

export interface TaskListOutput {
  /** 主键 */
  id?: string | null
  /** 任务标题 */
  topic?: string | null
  /** 任务数据 */
  body?: string | null
  /**
   * 任务执行多少轮
   * @format int32
   */
  round?: number
  /** SEC=1,RunOnDay=11,RunOnWeek=12,RunOnMonth=13,Custom=21 */
  interval?: TaskInterval
  /** 定时参数值 */
  intervalArgument?: string | null
  /** Running=0,Paused=1,Completed=2 */
  status?: TaskStatus
  /**
   * 创建时间
   * @format date-time
   */
  createTime?: string
  /**
   * 最后运行时间
   * @format date-time
   */
  lastRunTime?: string
  /**
   * 当前运行到第几轮
   * @format int32
   */
  currentRound?: number
  /**
   * 错次数
   * @format int32
   */
  errorTimes?: number
}

export interface TaskLog {
  taskId?: string | null
  /** @format int32 */
  round?: number
  /** @format int64 */
  elapsedMilliseconds?: number
  success?: boolean
  exception?: string | null
  remark?: string | null
  /** @format date-time */
  createTime?: string
}

export interface TaskLogGetPageDto {
  taskId?: string | null
}

/**
 * Running=0,Paused=1,Completed=2
 * @format int32
 */
export type TaskStatus = 0 | 1 | 2

/** 修改 */
export interface TaskUpdateInput {
  /** 任务标题 */
  topic?: string | null
  /** 任务参数 */
  body?: string | null
  /**
   * 任务执行多少轮，-1为永久循环
   * @format int32
   */
  round?: number
  /** SEC=1,RunOnDay=11,RunOnWeek=12,RunOnMonth=13,Custom=21 */
  interval?: TaskInterval
  /** 定时参数 60,60,60,120,120,1200,1200 */
  intervalArgument?: string | null
  /** 报警邮件，多个邮件地址则逗号分隔 */
  alarmEmail?: string | null
  /**
   * 失败重试次数
   * @format int32
   */
  failRetryCount?: number | null
  /**
   * 失败重试间隔（秒）
   * @format int32
   */
  failRetryInterval?: number | null
  /**
   * 任务Id
   * @minLength 1
   */
  id: string
}

/** 添加 */
export interface TenantAddInput {
  /**
   * 租户Id
   * @format int64
   */
  id?: number
  /**
   * 企业名称
   * @minLength 1
   */
  name: string
  /** 编码 */
  code?: string | null
  /** 套餐Ids */
  pkgIds?: number[] | null
  /** 姓名 */
  realName?: string | null
  /**
   * 账号
   * @minLength 1
   */
  userName: string
  /** 密码 */
  password?: string | null
  /** 手机号码 */
  phone?: string | null
  /** 邮箱地址 */
  email?: string | null
  /** 域名 */
  domain?: string | null
  /** 数据库注册键 */
  dbKey?: string | null
  /** MySql=0,SqlServer=1,PostgreSQL=2,Oracle=3,Sqlite=4,OdbcOracle=5,OdbcSqlServer=6,OdbcMySql=7,OdbcPostgreSQL=8,Odbc=9,OdbcDameng=10,MsAccess=11,Dameng=12,OdbcKingbaseES=13,ShenTong=14,KingbaseES=15,Firebird=16,Custom=17,ClickHouse=18,GBase=19,QuestDb=20,Xugu=21,CustomOracle=22,CustomSqlServer=23,CustomMySql=24,CustomPostgreSQL=25 */
  dbType?: DataType
  /** 连接字符串 */
  connectionString?: string | null
  /** 启用 */
  enabled?: boolean
  /** 说明 */
  description?: string | null
}

/** 租户 */
export interface TenantEntity {
  /**
   * 主键Id
   * @format int64
   */
  id?: number
  /**
   * 创建者用户Id
   * @format int64
   */
  createdUserId?: number | null
  /**
   * 创建者用户名
   * @maxLength 50
   */
  createdUserName?: string | null
  /**
   * 创建者姓名
   * @maxLength 50
   */
  createdUserRealName?: string | null
  /**
   * 创建时间
   * @format date-time
   */
  createdTime?: string | null
  /**
   * 修改者用户Id
   * @format int64
   */
  modifiedUserId?: number | null
  /**
   * 修改者用户名
   * @maxLength 50
   */
  modifiedUserName?: string | null
  /**
   * 修改者姓名
   * @maxLength 50
   */
  modifiedUserRealName?: string | null
  /**
   * 修改时间
   * @format date-time
   */
  modifiedTime?: string | null
  /** 是否删除 */
  isDeleted?: boolean
  /**
   * 授权用户
   * @format int64
   */
  userId?: number
  /** 用户 */
  user?: UserEntity
  /**
   * 授权部门
   * @format int64
   */
  orgId?: number
  /** 组织架构 */
  org?: OrgEntity
  /** 租户类型:Platform=1,Tenant=2 */
  tenantType?: TenantType
  /** 域名 */
  domain?: string | null
  /** 数据库注册键 */
  dbKey?: string | null
  /** MySql=0,SqlServer=1,PostgreSQL=2,Oracle=3,Sqlite=4,OdbcOracle=5,OdbcSqlServer=6,OdbcMySql=7,OdbcPostgreSQL=8,Odbc=9,OdbcDameng=10,MsAccess=11,Dameng=12,OdbcKingbaseES=13,ShenTong=14,KingbaseES=15,Firebird=16,Custom=17,ClickHouse=18,GBase=19,QuestDb=20,Xugu=21,CustomOracle=22,CustomSqlServer=23,CustomMySql=24,CustomPostgreSQL=25 */
  dbType?: DataType
  /** 连接字符串 */
  connectionString?: string | null
  /** 启用 */
  enabled?: boolean
  /** 说明 */
  description?: string | null
  /** 套餐列表 */
  pkgs?: PkgEntity[] | null
}

export interface TenantGetOutput {
  /**
   * 企业名称
   * @minLength 1
   */
  name: string
  /** 编码 */
  code?: string | null
  /** 姓名 */
  realName?: string | null
  /**
   * 账号
   * @minLength 1
   */
  userName: string
  /** 密码 */
  password?: string | null
  /** 手机号码 */
  phone?: string | null
  /** 邮箱地址 */
  email?: string | null
  /** 域名 */
  domain?: string | null
  /** 数据库注册键 */
  dbKey?: string | null
  /** MySql=0,SqlServer=1,PostgreSQL=2,Oracle=3,Sqlite=4,OdbcOracle=5,OdbcSqlServer=6,OdbcMySql=7,OdbcPostgreSQL=8,Odbc=9,OdbcDameng=10,MsAccess=11,Dameng=12,OdbcKingbaseES=13,ShenTong=14,KingbaseES=15,Firebird=16,Custom=17,ClickHouse=18,GBase=19,QuestDb=20,Xugu=21,CustomOracle=22,CustomSqlServer=23,CustomMySql=24,CustomPostgreSQL=25 */
  dbType?: DataType
  /** 连接字符串 */
  connectionString?: string | null
  /** 启用 */
  enabled?: boolean
  /** 说明 */
  description?: string | null
  /**
   * 租户Id
   * @format int64
   */
  id: number
  /** 套餐列表 */
  pkgs?: PkgEntity[] | null
  /** 套餐Ids */
  pkgIds?: number[] | null
}

export interface TenantGetPageDto {
  /** 企业名称 */
  name?: string | null
}

export interface TenantListOutput {
  /**
   * 主键
   * @format int64
   */
  id?: number
  /** 企业名称 */
  name?: string | null
  /** 企业编码 */
  code?: string | null
  pkgs?: PkgEntity[] | null
  /** 套餐 */
  pkgNames?: string[] | null
  /** 姓名 */
  realName?: string | null
  /** 账号 */
  userName?: string | null
  /** 手机号码 */
  phone?: string | null
  /** 邮箱地址 */
  email?: string | null
  /** MySql=0,SqlServer=1,PostgreSQL=2,Oracle=3,Sqlite=4,OdbcOracle=5,OdbcSqlServer=6,OdbcMySql=7,OdbcPostgreSQL=8,Odbc=9,OdbcDameng=10,MsAccess=11,Dameng=12,OdbcKingbaseES=13,ShenTong=14,KingbaseES=15,Firebird=16,Custom=17,ClickHouse=18,GBase=19,QuestDb=20,Xugu=21,CustomOracle=22,CustomSqlServer=23,CustomMySql=24,CustomPostgreSQL=25 */
  dbType?: DataType
  /** 数据库名称 */
  dbTypeName?: string | null
  /** 启用 */
  enabled?: boolean
  /** 说明 */
  description?: string | null
  /**
   * 创建时间
   * @format date-time
   */
  createdTime?: string | null
}

/** 设置启用 */
export interface TenantSetEnableInput {
  /**
   * 租户Id
   * @format int64
   */
  tenantId?: number
  /** 是否启用 */
  enabled?: boolean
}

/**
 * 租户类型:Platform=1,Tenant=2
 * @format int32
 */
export type TenantType = 1 | 2

/** 修改 */
export interface TenantUpdateInput {
  /**
   * 企业名称
   * @minLength 1
   */
  name: string
  /** 编码 */
  code?: string | null
  /** 套餐Ids */
  pkgIds?: number[] | null
  /** 姓名 */
  realName?: string | null
  /**
   * 账号
   * @minLength 1
   */
  userName: string
  /** 密码 */
  password?: string | null
  /** 手机号码 */
  phone?: string | null
  /** 邮箱地址 */
  email?: string | null
  /** 域名 */
  domain?: string | null
  /** 数据库注册键 */
  dbKey?: string | null
  /** MySql=0,SqlServer=1,PostgreSQL=2,Oracle=3,Sqlite=4,OdbcOracle=5,OdbcSqlServer=6,OdbcMySql=7,OdbcPostgreSQL=8,Odbc=9,OdbcDameng=10,MsAccess=11,Dameng=12,OdbcKingbaseES=13,ShenTong=14,KingbaseES=15,Firebird=16,Custom=17,ClickHouse=18,GBase=19,QuestDb=20,Xugu=21,CustomOracle=22,CustomSqlServer=23,CustomMySql=24,CustomPostgreSQL=25 */
  dbType?: DataType
  /** 连接字符串 */
  connectionString?: string | null
  /** 启用 */
  enabled?: boolean
  /** 说明 */
  description?: string | null
  /**
   * 租户Id
   * @format int64
   */
  id: number
}

export interface Track {
  /** @format int32 */
  x?: number
  /** @format int32 */
  y?: number
  /** @format int32 */
  t?: number
}

/** 添加 */
export interface UserAddInput {
  /**
   * 用户Id
   * @format int64
   */
  id?: number
  /**
   * 账号
   * @minLength 1
   */
  userName: string
  /**
   * 姓名
   * @minLength 1
   */
  name: string
  /** 手机号 */
  mobile?: string | null
  /** 邮箱 */
  email?: string | null
  /** 角色Ids */
  roleIds?: number[] | null
  /** 所属部门Ids */
  orgIds?: number[] | null
  /**
   * 主属部门Id
   * @format int64
   */
  orgId?: number
  /**
   * 直属主管Id
   * @format int64
   */
  managerUserId?: number | null
  /** 直属主管姓名 */
  managerUserName?: string | null
  /** 员工添加 */
  staff: StaffAddInput
  /** 密码 */
  password?: string | null
  /** 启用 */
  enabled?: boolean
}

/** 添加会员 */
export interface UserAddMemberInput {
  /**
   * 会员Id
   * @format int64
   */
  id?: number
  /**
   * 账号
   * @minLength 1
   */
  userName: string
  /** 姓名 */
  name?: string | null
  /** 手机号 */
  mobile?: string | null
  /** 邮箱 */
  email?: string | null
  /**
   * 密码
   * @minLength 1
   */
  password: string
  /** 用户状态:WaitChangePasssword=2,WaitActive=3 */
  status?: UserStatus
}

/** 修改密码 */
export interface UserChangePasswordInput {
  /**
   * 旧密码
   * @minLength 1
   */
  oldPassword: string
  /**
   * 新密码
   * @minLength 1
   */
  newPassword: string
  /**
   * 确认新密码
   * @minLength 1
   */
  confirmPassword: string
}

/** 用户 */
export interface UserEntity {
  /**
   * 主键Id
   * @format int64
   */
  id?: number
  /**
   * 创建者用户Id
   * @format int64
   */
  createdUserId?: number | null
  /**
   * 创建者用户名
   * @maxLength 50
   */
  createdUserName?: string | null
  /**
   * 创建者姓名
   * @maxLength 50
   */
  createdUserRealName?: string | null
  /**
   * 创建时间
   * @format date-time
   */
  createdTime?: string | null
  /**
   * 修改者用户Id
   * @format int64
   */
  modifiedUserId?: number | null
  /**
   * 修改者用户名
   * @maxLength 50
   */
  modifiedUserName?: string | null
  /**
   * 修改者姓名
   * @maxLength 50
   */
  modifiedUserRealName?: string | null
  /**
   * 修改时间
   * @format date-time
   */
  modifiedTime?: string | null
  /** 是否删除 */
  isDeleted?: boolean
  /**
   * 租户Id
   * @format int64
   */
  tenantId?: number | null
  /** 租户 */
  tenant?: TenantEntity
  /** 账号 */
  userName?: string | null
  /** 密码 */
  password?: string | null
  /** 密码加密类型:MD5Encrypt32=0,PasswordHasher=1 */
  passwordEncryptType?: PasswordEncryptType
  /** 姓名 */
  name?: string | null
  /** 手机号 */
  mobile?: string | null
  /** 邮箱 */
  email?: string | null
  /**
   * 主属部门Id
   * @format int64
   */
  orgId?: number
  /** 组织架构 */
  org?: OrgEntity
  /**
   * 直属主管Id
   * @format int64
   */
  managerUserId?: number | null
  /** 用户 */
  managerUser?: UserEntity
  /** 昵称 */
  nickName?: string | null
  /** 头像 */
  avatar?: string | null
  /** 用户状态:WaitChangePasssword=2,WaitActive=3 */
  status?: UserStatus
  /** 用户类型:Member=0,DefaultUser=1,TenantAdmin=10,PlatformAdmin=100 */
  type?: UserType
  /** 启用 */
  enabled?: boolean
  /** 角色列表 */
  roles?: RoleEntity[] | null
  /** 部门列表 */
  orgs?: OrgEntity[] | null
  /** 用户员工 */
  staff?: UserStaffEntity
}

export interface UserGetBasicOutput {
  /** 头像 */
  avatar?: string | null
  /** 姓名 */
  name?: string | null
  /** 昵称 */
  nickName?: string | null
  /** 手机号 */
  mobile?: string | null
  /** 邮箱 */
  email?: string | null
}

export interface UserGetOrgDto {
  /** @format int64 */
  id?: number
  name?: string | null
}

export interface UserGetOutput {
  /**
   * 账号
   * @minLength 1
   */
  userName: string
  /**
   * 姓名
   * @minLength 1
   */
  name: string
  /** 手机号 */
  mobile?: string | null
  /** 邮箱 */
  email?: string | null
  /**
   * 主属部门Id
   * @format int64
   */
  orgId?: number
  /**
   * 直属主管Id
   * @format int64
   */
  managerUserId?: number | null
  /** 直属主管姓名 */
  managerUserName?: string | null
  /** 员工添加 */
  staff: StaffAddInput
  /**
   * 主键Id
   * @format int64
   */
  id: number
  /** 角色列表 */
  roles?: UserGetRoleDto[] | null
  /** 部门列表 */
  orgs?: UserGetOrgDto[] | null
  /** 所属部门Ids */
  orgIds?: number[] | null
  /** 角色Ids */
  roleIds?: number[] | null
}

/** 用户分页查询条件 */
export interface UserGetPageDto {
  /**
   * 部门Id
   * @format int64
   */
  orgId?: number | null
}

export interface UserGetPageOutput {
  /**
   * 主键Id
   * @format int64
   */
  id?: number
  /** 账号 */
  userName?: string | null
  /** 姓名 */
  name?: string | null
  /** 手机号 */
  mobile?: string | null
  /** 邮箱 */
  email?: string | null
  /** 用户类型:Member=0,DefaultUser=1,TenantAdmin=10,PlatformAdmin=100 */
  type?: UserType
  roles?: RoleEntity[] | null
  /** 角色 */
  roleNames?: string[] | null
  /** 是否主管 */
  isManager?: boolean
  /** 启用 */
  enabled?: boolean
  /**
   * 创建时间
   * @format date-time
   */
  createdTime?: string | null
}

/** 用户权限 */
export interface UserGetPermissionOutput {
  /** 接口列表 */
  apis?: ApiModel[] | null
  /** 权限点编码列表 */
  codes?: string[] | null
}

export interface UserGetRoleDto {
  /** @format int64 */
  id?: number
  name?: string | null
}

/** 重置密码 */
export interface UserResetPasswordInput {
  /**
   * 主键Id
   * @format int64
   */
  id?: number
  /** 密码 */
  password?: string | null
}

/** 设置启用 */
export interface UserSetEnableInput {
  /**
   * 用户Id
   * @format int64
   */
  userId?: number
  /** 是否启用 */
  enabled?: boolean
}

/** 设置主管 */
export interface UserSetManagerInput {
  /**
   * 用户Id
   * @format int64
   */
  userId?: number
  /**
   * 部门Id
   * @format int64
   */
  orgId?: number
  /** 是否主管 */
  isManager?: boolean
}

/** 用户员工 */
export interface UserStaffEntity {
  /**
   * 主键Id
   * @format int64
   */
  id?: number
  /**
   * 创建者用户Id
   * @format int64
   */
  createdUserId?: number | null
  /**
   * 创建者用户名
   * @maxLength 50
   */
  createdUserName?: string | null
  /**
   * 创建者姓名
   * @maxLength 50
   */
  createdUserRealName?: string | null
  /**
   * 创建时间
   * @format date-time
   */
  createdTime?: string | null
  /**
   * 修改者用户Id
   * @format int64
   */
  modifiedUserId?: number | null
  /**
   * 修改者用户名
   * @maxLength 50
   */
  modifiedUserName?: string | null
  /**
   * 修改者姓名
   * @maxLength 50
   */
  modifiedUserRealName?: string | null
  /**
   * 修改时间
   * @format date-time
   */
  modifiedTime?: string | null
  /** 是否删除 */
  isDeleted?: boolean
  /**
   * 租户Id
   * @format int64
   */
  tenantId?: number | null
  /** 职位 */
  position?: string | null
  /** 工号 */
  jobNumber?: string | null
  /** 性别:Unknown(未知)=0,Male(男)=1,Female(女)=2 */
  sex?: Sex
  /**
   * 入职时间
   * @format date-time
   */
  entryTime?: string | null
  /** 企业微信名片 */
  workWeChatCard?: string | null
  /** 个人简介 */
  introduce?: string | null
}

/**
 * 用户状态:WaitChangePasssword=2,WaitActive=3
 * @format int32
 */
export type UserStatus = 2 | 3

/**
 * 用户类型:Member=0,DefaultUser=1,TenantAdmin=10,PlatformAdmin=100
 * @format int32
 */
export type UserType = 0 | 1 | 10 | 100

/** 更新基本信息 */
export interface UserUpdateBasicInput {
  /**
   * 姓名
   * @minLength 1
   */
  name: string
  /** 昵称 */
  nickName?: string | null
}

/** 修改 */
export interface UserUpdateInput {
  /**
   * 账号
   * @minLength 1
   */
  userName: string
  /**
   * 姓名
   * @minLength 1
   */
  name: string
  /** 手机号 */
  mobile?: string | null
  /** 邮箱 */
  email?: string | null
  /** 角色Ids */
  roleIds?: number[] | null
  /** 所属部门Ids */
  orgIds?: number[] | null
  /**
   * 主属部门Id
   * @format int64
   */
  orgId?: number
  /**
   * 直属主管Id
   * @format int64
   */
  managerUserId?: number | null
  /** 直属主管姓名 */
  managerUserName?: string | null
  /** 员工添加 */
  staff: StaffAddInput
  /**
   * 主键Id
   * @format int64
   */
  id: number
}

/** 修改会员 */
export interface UserUpdateMemberInput {
  /**
   * 账号
   * @minLength 1
   */
  userName: string
  /** 姓名 */
  name?: string | null
  /** 手机号 */
  mobile?: string | null
  /** 邮箱 */
  email?: string | null
  /**
   * 主键Id
   * @format int64
   */
  id: number
}

export interface ValidateResult {
  /** Success=0,ValidateFail=1,Timeout=2 */
  result?: ValidateResultType
  message?: string | null
}

/**
 * Success=0,ValidateFail=1,Timeout=2
 * @format int32
 */
export type ValidateResultType = 0 | 1 | 2

/** 添加 */
export interface ViewAddInput {
  /**
   * 所属节点
   * @format int64
   */
  parentId?: number | null
  /** 视图命名 */
  name?: string | null
  /** 视图名称 */
  label?: string | null
  /** 视图路径 */
  path?: string | null
  /** 说明 */
  description?: string | null
  /** 缓存 */
  cache?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
}

/** 视图管理 */
export interface ViewEntity {
  /**
   * 主键Id
   * @format int64
   */
  id?: number
  /**
   * 创建者用户Id
   * @format int64
   */
  createdUserId?: number | null
  /**
   * 创建者用户名
   * @maxLength 50
   */
  createdUserName?: string | null
  /**
   * 创建者姓名
   * @maxLength 50
   */
  createdUserRealName?: string | null
  /**
   * 创建时间
   * @format date-time
   */
  createdTime?: string | null
  /**
   * 修改者用户Id
   * @format int64
   */
  modifiedUserId?: number | null
  /**
   * 修改者用户名
   * @maxLength 50
   */
  modifiedUserName?: string | null
  /**
   * 修改者姓名
   * @maxLength 50
   */
  modifiedUserRealName?: string | null
  /**
   * 修改时间
   * @format date-time
   */
  modifiedTime?: string | null
  /** 是否删除 */
  isDeleted?: boolean
  /**
   * 所属节点
   * @format int64
   */
  parentId?: number
  /** 视图命名 */
  name?: string | null
  /** 视图名称 */
  label?: string | null
  /** 视图路径 */
  path?: string | null
  /** 说明 */
  description?: string | null
  /** 缓存 */
  cache?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
  childs?: ViewEntity[] | null
}

export interface ViewGetOutput {
  /**
   * 所属节点
   * @format int64
   */
  parentId?: number | null
  /** 视图命名 */
  name?: string | null
  /** 视图名称 */
  label?: string | null
  /** 视图路径 */
  path?: string | null
  /** 说明 */
  description?: string | null
  /** 缓存 */
  cache?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
  /**
   * 视图Id
   * @format int64
   */
  id: number
}

export interface ViewListOutput {
  /**
   * 视图Id
   * @format int64
   */
  id?: number
  /**
   * 视图父级
   * @format int64
   */
  parentId?: number | null
  /** 视图命名 */
  name?: string | null
  /** 视图名称 */
  label?: string | null
  /** 视图路径 */
  path?: string | null
  /** 缓存 */
  cache?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
  /** 说明 */
  description?: string | null
}

export interface ViewSyncDto {
  /** 视图命名 */
  name?: string | null
  /** 地址 */
  path?: string | null
  /** 视图名称 */
  label?: string | null
  /** 说明 */
  description?: string | null
  /** 缓存 */
  cache?: boolean
}

export interface ViewSyncInput {
  views?: ViewSyncDto[] | null
}

/** 修改 */
export interface ViewUpdateInput {
  /**
   * 所属节点
   * @format int64
   */
  parentId?: number | null
  /** 视图命名 */
  name?: string | null
  /** 视图名称 */
  label?: string | null
  /** 视图路径 */
  path?: string | null
  /** 说明 */
  description?: string | null
  /** 缓存 */
  cache?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 启用 */
  enabled?: boolean
  /**
   * 视图Id
   * @format int64
   */
  id: number
}
