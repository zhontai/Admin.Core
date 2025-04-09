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
   * @maxLength 60
   */
  createdUserName?: string | null
  /**
   * 创建者姓名
   * @maxLength 60
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
   * @maxLength 60
   */
  modifiedUserName?: string | null
  /**
   * 修改者姓名
   * @maxLength 60
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
  /** 启用接口日志 */
  enabledLog?: boolean
  /** 启用请求参数 */
  enabledParams?: boolean
  /** 启用响应结果 */
  enabledResult?: boolean
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

/** 接口列表 */
export interface ApiGetListOutput {
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
  /** 启用操作日志 */
  enabledLog?: boolean
  /** 启用请求参数 */
  enabledParams?: boolean
  /** 启用响应结果 */
  enabledResult?: boolean
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

/** 查询分页 */
export interface ApiGetPageInput {
  /** 接口名称 */
  label?: string | null
}

/** 接口 */
export interface ApiModel {
  /** 请求方法 */
  httpMethods?: string | null
  /** 请求地址 */
  path?: string | null
}

/** 设置启用请求日志 */
export interface ApiSetEnableLogInput {
  /**
   * 接口Id
   * @format int64
   */
  apiId?: number
  /** 是否启用请求参数 */
  enabledLog?: boolean
}

/** 设置启用请求参数 */
export interface ApiSetEnableParamsInput {
  /**
   * 接口Id
   * @format int64
   */
  apiId?: number
  /** 是否启用请求参数 */
  enabledParams?: boolean
}

/** 设置启用请求参数 */
export interface ApiSetEnableResultInput {
  /**
   * 接口Id
   * @format int64
   */
  apiId?: number
  /** 是否启用响应结果 */
  enabledResult?: boolean
}

/** 接口同步 */
export interface ApiSyncInput {
  /** 接口同步列表 */
  apis?: ApiSyncModel[] | null
}

/** 接口同步模型 */
export interface ApiSyncModel {
  /** 接口名称 */
  label?: string | null
  /** 接口地址 */
  path?: string | null
  /** 父级路径 */
  parentPath?: string | null
  /** 接口提交方法 */
  httpMethods?: string | null
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

/** 查询密钥 */
export interface AuthGetPasswordEncryptKeyOutput {
  /** 缓存键 */
  key?: string | null
  /** 密码加密密钥 */
  encryptKey?: string | null
  /** 密码加密向量 */
  iv?: string | null
}

/** 用户信息 */
export interface AuthGetUserInfoOutput {
  /** 用户个人信息 */
  user?: AuthUserProfileOutput
  /** 用户菜单列表 */
  menus?: AuthUserMenuOutput[] | null
  /** 用户权限列表 */
  permissions?: string[] | null
}

/** 用户权限 */
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

/** 用户菜单 */
export interface AuthUserMenuOutput {
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
export interface AuthUserProfileOutput {
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
 * MySql=0,SqlServer=1,PostgreSQL=2,Oracle=3,Sqlite=4,OdbcOracle=5,OdbcSqlServer=6,OdbcMySql=7,OdbcPostgreSQL=8,Odbc=9,MsAccess=11,Dameng=12,ShenTong=14,KingbaseES=15,Firebird=16,Custom=17,ClickHouse=18,GBase=19,QuestDb=20,Xugu=21,CustomOracle=22,CustomSqlServer=23,CustomMySql=24,CustomPostgreSQL=25,DuckDB=26,TDengine=27
 * @format int32
 */
export type DataType = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 11 | 12 | 14 | 15 | 16 | 17 | 18 | 19 | 20 | 21 | 22 | 23 | 24 | 25 | 26 | 27

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
  /** 说明 */
  description?: string | null
  /** 启用 */
  enabled?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number | null
}

/** 字典列表 */
export interface DictGetListOutput {
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

/** 字典 */
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
  /** 说明 */
  description?: string | null
  /** 启用 */
  enabled?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number | null
  /**
   * 主键Id
   * @format int64
   */
  id: number
}

/** 字典分页请求 */
export interface DictGetPageInput {
  /**
   * 字典类型Id
   * @format int64
   */
  dictTypeId?: number
  /** 字典名称 */
  name?: string | null
}

/** 字典分页响应 */
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

/** 字典类型 */
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

/** 字典类型 */
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

/** 字典类型分页请求 */
export interface DictTypeGetPageInput {
  /** 字典名称 */
  name?: string | null
}

/** 字典类型分页响应 */
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
  /** 说明 */
  description?: string | null
  /** 启用 */
  enabled?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number | null
  /**
   * 主键Id
   * @format int64
   */
  id: number
}

/** 添加分组 */
export interface DocAddGroupInput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /** 文档类型:Group=1,Markdown=2 */
  type?: DocType
  /** 名称 */
  label?: string | null
  /** 命名 */
  name?: string | null
  /** 打开 */
  opened?: boolean | null
}

/** 添加图片 */
export interface DocAddImageInput {
  /**
   * 用户Id
   * @format int64
   */
  documentId?: number
  /** 请求路径 */
  url?: string | null
}

/** 添加菜单 */
export interface DocAddMenuInput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /** 文档类型:Group=1,Markdown=2 */
  type?: DocType
  /** 命名 */
  name?: string | null
  /** 名称 */
  label?: string | null
  /** 说明 */
  description?: string | null
}

/** 文档内容 */
export interface DocGetContentOutput {
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

/** 文档分组 */
export interface DocGetGroupOutput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /** 文档类型:Group=1,Markdown=2 */
  type?: DocType
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

/** 文档菜单 */
export interface DocGetMenuOutput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /** 文档类型:Group=1,Markdown=2 */
  type?: DocType
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

/** 文档列表 */
export interface DocListOutput {
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
  type?: DocType
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
export type DocType = 1 | 2

/** 更新文档内容 */
export interface DocUpdateContentInput {
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

/** 更新分组 */
export interface DocUpdateGroupInput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /** 文档类型:Group=1,Markdown=2 */
  type?: DocType
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

/** 更新菜单 */
export interface DocUpdateMenuInput {
  /**
   * 父级节点
   * @format int64
   */
  parentId?: number
  /** 文档类型:Group=1,Markdown=2 */
  type?: DocType
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

/** 导出信息输入 */
export interface ExportInput {
  dynamicFilter?: DynamicFilterInfo
  /** 排序列表 */
  sortList?: SortInput[] | null
  /** 文件名 */
  fileName?: string | null
}

/** 删除 */
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
   * @maxLength 60
   */
  createdUserName?: string | null
  /**
   * 创建者姓名
   * @maxLength 60
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
   * @maxLength 60
   */
  modifiedUserName?: string | null
  /**
   * 修改者姓名
   * @maxLength 60
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

/** 文件分页请求 */
export interface FileGetPageInput {
  /** 文件名 */
  fileName?: string | null
}

/** 文件分页响应 */
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

/** 导入信息输出 */
export interface ImportOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /**
   * 新增数
   * @format int64
   */
  insertCount?: number
  /**
   * 更新数
   * @format int64
   */
  updateCount?: number
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
  /** 国家 */
  country?: string | null
  /** 省份 */
  province?: string | null
  /** 城市 */
  city?: string | null
  /** 网络服务商 */
  isp?: string | null
  /**
   * 耗时（毫秒）
   * @format int64
   */
  elapsedMilliseconds?: number
  /** 操作状态 */
  status?: boolean | null
  /** 操作消息 */
  msg?: string | null
  /**
   * 创建者用户Id
   * @format int64
   */
  createdUserId?: number | null
  /** 创建者用户名 */
  createdUserName?: string | null
  /** 创建者姓名 */
  createdUserRealName?: string | null
}

/** 分页请求 */
export interface LoginLogGetPageInput {
  /** 创建者 */
  createdUserName?: string | null
  /** 操作状态 */
  status?: boolean | null
  /** IP */
  ip?: string | null
  /**
   * 创建开始时间
   * @format date-time
   */
  addStartTime?: string | null
  /**
   * 创建结束时间
   * @format date-time
   */
  addEndTime?: string | null
}

/** 分页响应 */
export interface LoginLogGetPageOutput {
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
  /** 国家 */
  country?: string | null
  /** 省份 */
  province?: string | null
  /** 城市 */
  city?: string | null
  /** 网络服务商 */
  isp?: string | null
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

/** 添加 */
export interface MsgAddInput {
  /** 标题 */
  title?: string | null
  /** 内容 */
  content?: string | null
  /**
   * 类型Id
   * @format int64
   */
  typeId?: number
  /** 类型名称 */
  typeName?: string | null
  /** 消息状态:Draft=1,Published=2,Scheduled=3,Revoked=4,Archived=5 */
  status?: MsgStatusEnum
}

/** 添加消息用户列表 */
export interface MsgAddMsgUserListInput {
  /**
   * 消息
   * @format int64
   */
  msgId: number
  /** 用户 */
  userIds?: number[] | null
}

/** 消息用户列表 */
export interface MsgGetMsgUserListOutput {
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
  /** 是否已读 */
  isRead?: boolean
  /**
   * 已读时间
   * @format date-time
   */
  readTime?: string | null
}

/** 消息 */
export interface MsgGetOutput {
  /** 标题 */
  title?: string | null
  /** 内容 */
  content?: string | null
  /**
   * 类型Id
   * @format int64
   */
  typeId?: number
  /** 类型名称 */
  typeName?: string | null
  /** 消息状态:Draft=1,Published=2,Scheduled=3,Revoked=4,Archived=5 */
  status?: MsgStatusEnum
  /**
   * 消息Id
   * @format int64
   */
  id: number
}

/** 消息分页请求 */
export interface MsgGetPageInput {
  /** 标题 */
  title?: string | null
}

/** 消息分页响应 */
export interface MsgGetPageOutput {
  /**
   * 消息Id
   * @format int64
   */
  id?: number
  /** 标题 */
  title?: string | null
  /**
   * 类型Id
   * @format int64
   */
  typeId?: number
  /** 类型名称 */
  typeName?: string | null
  /** 消息状态:Draft=1,Published=2,Scheduled=3,Revoked=4,Archived=5 */
  status?: MsgStatusEnum
  /**
   * 创建时间
   * @format date-time
   */
  createdTime?: string | null
}

/**
 * 消息状态:Draft=1,Published=2,Scheduled=3,Revoked=4,Archived=5
 * @format int32
 */
export type MsgStatusEnum = 1 | 2 | 3 | 4 | 5

/** 添加 */
export interface MsgTypeAddInput {
  /**
   * 父级Id
   * @format int64
   */
  parentId?: number
  /** 名称 */
  name?: string | null
  /** 编码 */
  code?: string | null
  /** 启用 */
  enabled?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 说明 */
  description?: string | null
}

/** 查询列表响应 */
export interface MsgTypeGetListOutput {
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

/** 消息类型 */
export interface MsgTypeGetOutput {
  /**
   * 父级Id
   * @format int64
   */
  parentId?: number
  /** 名称 */
  name?: string | null
  /** 编码 */
  code?: string | null
  /** 启用 */
  enabled?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 说明 */
  description?: string | null
  /**
   * 消息分类Id
   * @format int64
   */
  id: number
}

/** 修改 */
export interface MsgTypeUpdateInput {
  /**
   * 父级Id
   * @format int64
   */
  parentId?: number
  /** 名称 */
  name?: string | null
  /** 编码 */
  code?: string | null
  /** 启用 */
  enabled?: boolean
  /**
   * 排序
   * @format int32
   */
  sort?: number
  /** 说明 */
  description?: string | null
  /**
   * 消息分类Id
   * @format int64
   */
  id: number
}

/** 修改 */
export interface MsgUpdateInput {
  /** 标题 */
  title?: string | null
  /** 内容 */
  content?: string | null
  /**
   * 类型Id
   * @format int64
   */
  typeId?: number
  /** 类型名称 */
  typeName?: string | null
  /** 消息状态:Draft=1,Published=2,Scheduled=3,Revoked=4,Archived=5 */
  status?: MsgStatusEnum
  /**
   * 消息Id
   * @format int64
   */
  id: number
}

/**
 * Invalid=0,Minio=1,Aliyun=2,QCloud=3,Qiniu=4,HuaweiCloud=5,BaiduCloud=6,Ctyun=7
 * @format int32
 */
export type OSSProvider = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7

/** 添加 */
export interface OperationLogAddInput {
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
  /** 国家 */
  country?: string | null
  /** 省份 */
  province?: string | null
  /** 城市 */
  city?: string | null
  /** 网络服务商 */
  isp?: string | null
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
  /**
   * 状态码
   * @format int32
   */
  statusCode?: number | null
  /** 操作结果 */
  result?: string | null
}

/** 查询分页请求 */
export interface OperationLogGetPageInput {
  /** 创建者 */
  createdUserName?: string | null
  /** 操作状态 */
  status?: boolean | null
  /** 操作接口 */
  api?: string | null
  /** IP */
  ip?: string | null
  /**
   * 创建开始时间
   * @format date-time
   */
  addStartTime?: string | null
  /**
   * 创建结束时间
   * @format date-time
   */
  addEndTime?: string | null
}

/** 查询分页响应 */
export interface OperationLogGetPageOutput {
  /**
   * 编号
   * @format int64
   */
  id?: number
  /** 昵称 */
  nickName?: string | null
  /** 接口名称 */
  apiLabel?: string | null
  /** 接口地址 */
  apiPath?: string | null
  /** 接口提交方法 */
  apiMethod?: string | null
  /** IP */
  ip?: string | null
  /** 国家 */
  country?: string | null
  /** 省份 */
  province?: string | null
  /** 城市 */
  city?: string | null
  /** 网络服务商 */
  isp?: string | null
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
  /**
   * 状态码
   * @format int32
   */
  statusCode?: number | null
  /** 操作消息 */
  msg?: string | null
  /** 请求参数 */
  params?: string | null
  /** 响应结果 */
  result?: string | null
  /** 创建者 */
  createdUserName?: string | null
  /** 创建者姓名 */
  createdUserRealName?: string | null
  /**
   * 创建时间
   * @format date-time
   */
  createdTime?: string | null
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
   * @maxLength 60
   */
  createdUserName?: string | null
  /**
   * 创建者姓名
   * @maxLength 60
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
   * @maxLength 60
   */
  modifiedUserName?: string | null
  /**
   * 修改者姓名
   * @maxLength 60
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

/** 部门 */
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

/** 部门列表 */
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
export interface PageInputApiGetPageInput {
  dynamicFilter?: DynamicFilterInfo
  /** 排序列表 */
  sortList?: SortInput[] | null
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
  /** 查询分页 */
  filter?: ApiGetPageInput
}

/** 分页信息输入 */
export interface PageInputDictGetPageInput {
  dynamicFilter?: DynamicFilterInfo
  /** 排序列表 */
  sortList?: SortInput[] | null
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
  /** 字典分页请求 */
  filter?: DictGetPageInput
}

/** 分页信息输入 */
export interface PageInputDictTypeGetPageInput {
  dynamicFilter?: DynamicFilterInfo
  /** 排序列表 */
  sortList?: SortInput[] | null
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
  /** 字典类型分页请求 */
  filter?: DictTypeGetPageInput
}

/** 分页信息输入 */
export interface PageInputFileGetPageInput {
  dynamicFilter?: DynamicFilterInfo
  /** 排序列表 */
  sortList?: SortInput[] | null
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
  /** 文件分页请求 */
  filter?: FileGetPageInput
}

/** 分页信息输入 */
export interface PageInputLoginLogGetPageInput {
  dynamicFilter?: DynamicFilterInfo
  /** 排序列表 */
  sortList?: SortInput[] | null
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
  /** 分页请求 */
  filter?: LoginLogGetPageInput
}

/** 分页信息输入 */
export interface PageInputMsgGetPageInput {
  dynamicFilter?: DynamicFilterInfo
  /** 排序列表 */
  sortList?: SortInput[] | null
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
  /** 消息分页请求 */
  filter?: MsgGetPageInput
}

/** 分页信息输入 */
export interface PageInputOperationLogGetPageInput {
  dynamicFilter?: DynamicFilterInfo
  /** 排序列表 */
  sortList?: SortInput[] | null
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
  /** 查询分页请求 */
  filter?: OperationLogGetPageInput
}

/** 分页信息输入 */
export interface PageInputPkgGetPageInput {
  dynamicFilter?: DynamicFilterInfo
  /** 排序列表 */
  sortList?: SortInput[] | null
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
  /** 套餐分页请求 */
  filter?: PkgGetPageInput
}

/** 分页信息输入 */
export interface PageInputPkgGetPkgTenantListInput {
  dynamicFilter?: DynamicFilterInfo
  /** 排序列表 */
  sortList?: SortInput[] | null
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
  /** 套餐租户列表请求 */
  filter?: PkgGetPkgTenantListInput
}

/** 分页信息输入 */
export interface PageInputPrintTemplateGetPageInput {
  dynamicFilter?: DynamicFilterInfo
  /** 排序列表 */
  sortList?: SortInput[] | null
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
  /** 分页请求 */
  filter?: PrintTemplateGetPageInput
}

/** 分页信息输入 */
export interface PageInputRegionGetPageInput {
  dynamicFilter?: DynamicFilterInfo
  /** 排序列表 */
  sortList?: SortInput[] | null
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
  /** 地区分页请求 */
  filter?: RegionGetPageInput
}

/** 分页信息输入 */
export interface PageInputRoleGetPageInput {
  dynamicFilter?: DynamicFilterInfo
  /** 排序列表 */
  sortList?: SortInput[] | null
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
  /** 角色分页请求 */
  filter?: RoleGetPageInput
}

/** 分页信息输入 */
export interface PageInputSiteMsgGetPageInput {
  dynamicFilter?: DynamicFilterInfo
  /** 排序列表 */
  sortList?: SortInput[] | null
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
  /** 站点消息分页请求 */
  filter?: SiteMsgGetPageInput
}

/** 分页信息输入 */
export interface PageInputTaskGetPageInput {
  dynamicFilter?: DynamicFilterInfo
  /** 排序列表 */
  sortList?: SortInput[] | null
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
  /** 任务分页请求 */
  filter?: TaskGetPageInput
}

/** 分页信息输入 */
export interface PageInputTaskLogGetPageInput {
  dynamicFilter?: DynamicFilterInfo
  /** 排序列表 */
  sortList?: SortInput[] | null
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
  /** 任务日志分页请求 */
  filter?: TaskLogGetPageInput
}

/** 分页信息输入 */
export interface PageInputTenantGetPageInput {
  dynamicFilter?: DynamicFilterInfo
  /** 排序列表 */
  sortList?: SortInput[] | null
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
  /** 租户分页请求 */
  filter?: TenantGetPageInput
}

/** 分页信息输入 */
export interface PageInputUserGetPageInput {
  dynamicFilter?: DynamicFilterInfo
  /** 排序列表 */
  sortList?: SortInput[] | null
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
  /** 用户分页查询条件 */
  filter?: UserGetPageInput
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
export interface PageOutputLoginLogGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: LoginLogGetPageOutput[] | null
}

/** 分页信息输出 */
export interface PageOutputMsgGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: MsgGetPageOutput[] | null
}

/** 分页信息输出 */
export interface PageOutputOperationLogGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: OperationLogGetPageOutput[] | null
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
export interface PageOutputPrintTemplateGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: PrintTemplateGetPageOutput[] | null
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
export interface PageOutputSiteMsgGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: SiteMsgGetPageOutput[] | null
}

/** 分页信息输出 */
export interface PageOutputTaskGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: TaskGetPageOutput[] | null
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
export interface PageOutputTenantGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: TenantGetPageOutput[] | null
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

/** 添加接口 */
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

/** 添加权限点 */
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

/** 条件分组 */
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

/** 添加菜单 */
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

/** 权限分配 */
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
   * @maxLength 60
   */
  createdUserName?: string | null
  /**
   * 创建者姓名
   * @maxLength 60
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
   * @maxLength 60
   */
  modifiedUserName?: string | null
  /**
   * 修改者姓名
   * @maxLength 60
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
  /** 平台 */
  platform?: string | null
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
  /** 是否系统菜单 */
  isSystem?: boolean
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

/** 权限点 */
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

/** 权限分组 */
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

/** 权限菜单 */
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

/** 权限列表 */
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

/** 保存租户权限 */
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

/** 修改权限点 */
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

/** 修改权限分组 */
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

/** 修改权限菜单 */
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
   * @maxLength 60
   */
  createdUserName?: string | null
  /**
   * 创建者姓名
   * @maxLength 60
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
   * @maxLength 60
   */
  modifiedUserName?: string | null
  /**
   * 修改者姓名
   * @maxLength 60
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

/** 套餐列表响应 */
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

/** 套餐 */
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

/** 套餐分页请求 */
export interface PkgGetPageInput {
  /** 名称 */
  name?: string | null
}

/** 套餐分页响应 */
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

/** 套餐租户列表请求 */
export interface PkgGetPkgTenantListInput {
  /** 租户名 */
  tenantName?: string | null
  /**
   * 套餐Id
   * @format int64
   */
  pkgId?: number | null
}

/** 套餐租户列表响应 */
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

/** 设置套餐权限 */
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

/** 添加 */
export interface PrintTemplateAddInput {
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
}

/** 表单响应 */
export interface PrintTemplateGetOutput {
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
  /**
   * 打印模板Id
   * @format int64
   */
  id: number
  /**
   * 版本
   * @format int64
   */
  version?: number
}

/** 分页请求 */
export interface PrintTemplateGetPageInput {
  /** 名称 */
  name?: string | null
  /** 编码 */
  code?: string | null
}

/** 分页响应 */
export interface PrintTemplateGetPageOutput {
  /**
   * 打印模板Id
   * @format int64
   */
  id?: number
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
  /**
   * 版本
   * @format int64
   */
  version?: number
}

/** 修改模板响应 */
export interface PrintTemplateGetUpdateTemplateOutput {
  /**
   * 打印模板Id
   * @format int64
   */
  id: number
  /** 模板 */
  template?: string | null
  /** 打印数据 */
  printData?: string | null
  /**
   * 版本
   * @format int64
   */
  version?: number
}

/** 设置启用 */
export interface PrintTemplateSetEnableInput {
  /**
   * 打印模板Id
   * @format int64
   */
  printTemplateId?: number
  /** 是否启用 */
  enabled?: boolean
}

/** 修改 */
export interface PrintTemplateUpdateInput {
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
  /**
   * 打印模板Id
   * @format int64
   */
  id: number
  /**
   * 版本
   * @format int64
   */
  version?: number
}

/** 修改模板 */
export interface PrintTemplateUpdateTemplateInput {
  /**
   * 打印模板Id
   * @format int64
   */
  id: number
  /** 模板 */
  template?: string | null
  /** 打印数据 */
  printData?: string | null
  /**
   * 版本
   * @format int64
   */
  version?: number
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
  /** 简称 */
  shortName?: string | null
  /** 地区级别:Province=1,City=2,County=3,Town(镇/乡)=4,Vilage(村/社区)=5 */
  level?: RegionLevel
  /** 代码 */
  code?: string | null
  /** 驻地 */
  capital?: string | null
  /**
   * 人口（万人）
   * @format int32
   */
  population?: number | null
  /**
   * 面积（平方千米）
   * @format int32
   */
  area?: number | null
  /** 区号 */
  areaCode?: string | null
  /** 邮编 */
  zipCode?: string | null
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

/** 下级列表 */
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

/** 地区列表请求 */
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

/** 地区 */
export interface RegionGetOutput {
  /**
   * 上级Id
   * @format int64
   */
  parentId?: number
  /** 名称 */
  name?: string | null
  /** 简称 */
  shortName?: string | null
  /** 地区级别:Province=1,City=2,County=3,Town(镇/乡)=4,Vilage(村/社区)=5 */
  level?: RegionLevel
  /** 代码 */
  code?: string | null
  /** 驻地 */
  capital?: string | null
  /**
   * 人口（万人）
   * @format int32
   */
  population?: number | null
  /**
   * 面积（平方千米）
   * @format int32
   */
  area?: number | null
  /** 区号 */
  areaCode?: string | null
  /** 邮编 */
  zipCode?: string | null
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
  /** 上级Id列表 */
  parentIdList?: number[] | null
}

/** 地区分页请求 */
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

/** 地区分页响应 */
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
  /** 简称 */
  shortName?: string | null
  /** 地区级别:Province=1,City=2,County=3,Town(镇/乡)=4,Vilage(村/社区)=5 */
  level?: RegionLevel
  /** 代码 */
  code?: string | null
  /** 拼音 */
  pinyin?: string | null
  /** 拼音首字母 */
  pinyinFirst?: string | null
  /** 驻地 */
  capital?: string | null
  /**
   * 人口（万人）
   * @format int32
   */
  population?: number | null
  /**
   * 面积（平方千米）
   * @format int32
   */
  area?: number | null
  /** 区号 */
  areaCode?: string | null
  /** 邮编 */
  zipCode?: string | null
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
  /** 简称 */
  shortName?: string | null
  /** 地区级别:Province=1,City=2,County=3,Town(镇/乡)=4,Vilage(村/社区)=5 */
  level?: RegionLevel
  /** 代码 */
  code?: string | null
  /** 驻地 */
  capital?: string | null
  /**
   * 人口（万人）
   * @format int32
   */
  population?: number | null
  /**
   * 面积（平方千米）
   * @format int32
   */
  area?: number | null
  /** 区号 */
  areaCode?: string | null
  /** 邮编 */
  zipCode?: string | null
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
  /** 接口 */
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
  /** 查询密钥 */
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
  /** 用户信息 */
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
  /** 用户权限 */
  data?: AuthGetUserPermissionsOutput
}

/** 结果输出 */
export interface ResultOutputAuthUserProfileOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 用户个人信息 */
  data?: AuthUserProfileOutput
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
  /** 字典 */
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
  /** 字典类型 */
  data?: DictTypeGetOutput
}

/** 结果输出 */
export interface ResultOutputDictionaryStringListDictGetListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: Record<string, DictGetListOutput[] | null>
}

/** 结果输出 */
export interface ResultOutputDocGetContentOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 文档内容 */
  data?: DocGetContentOutput
}

/** 结果输出 */
export interface ResultOutputDocGetGroupOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 文档分组 */
  data?: DocGetGroupOutput
}

/** 结果输出 */
export interface ResultOutputDocGetMenuOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 文档菜单 */
  data?: DocGetMenuOutput
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
export interface ResultOutputImportOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 导入信息输出 */
  data?: ImportOutput
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
export interface ResultOutputListApiGetListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: ApiGetListOutput[] | null
}

/** 结果输出 */
export interface ResultOutputListAuthUserMenuOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: AuthUserMenuOutput[] | null
}

/** 结果输出 */
export interface ResultOutputListDocListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: DocListOutput[] | null
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
export interface ResultOutputListMsgGetMsgUserListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: MsgGetMsgUserListOutput[] | null
}

/** 结果输出 */
export interface ResultOutputListMsgTypeGetListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: MsgTypeGetListOutput[] | null
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
export interface ResultOutputMsgGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 消息 */
  data?: MsgGetOutput
}

/** 结果输出 */
export interface ResultOutputMsgTypeGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 消息类型 */
  data?: MsgTypeGetOutput
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
  /** 部门 */
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
export interface ResultOutputPageOutputLoginLogGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputLoginLogGetPageOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputMsgGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputMsgGetPageOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputOperationLogGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputOperationLogGetPageOutput
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
export interface ResultOutputPageOutputPrintTemplateGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputPrintTemplateGetPageOutput
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
export interface ResultOutputPageOutputSiteMsgGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputSiteMsgGetPageOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputTaskGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputTaskGetPageOutput
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
export interface ResultOutputPageOutputTenantGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputTenantGetPageOutput
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
export interface ResultOutputPermissionGetDotOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 权限点 */
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
  /** 权限分组 */
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
  /** 权限菜单 */
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
  /** 套餐 */
  data?: PkgGetOutput
}

/** 结果输出 */
export interface ResultOutputPrintTemplateGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 表单响应 */
  data?: PrintTemplateGetOutput
}

/** 结果输出 */
export interface ResultOutputPrintTemplateGetUpdateTemplateOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 修改模板响应 */
  data?: PrintTemplateGetUpdateTemplateOutput
}

/** 结果输出 */
export interface ResultOutputRegionGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 地区 */
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
  /** 角色 */
  data?: RoleGetOutput
}

/** 结果输出 */
export interface ResultOutputSiteMsgGetContentOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 消息内容 */
  data?: SiteMsgGetContentOutput
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
  /** 任务 */
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
export interface ResultOutputTokenInfo {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 令牌信息 */
  data?: TokenInfo
}

/** 结果输出 */
export interface ResultOutputUserGetBasicOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 用户基本信息 */
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
  /** 用户 */
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
  /** 视图 */
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
   * @maxLength 60
   */
  createdUserName?: string | null
  /**
   * 创建者姓名
   * @maxLength 60
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
   * @maxLength 60
   */
  modifiedUserName?: string | null
  /**
   * 修改者姓名
   * @maxLength 60
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

/** 角色列表响应 */
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

/** 角色 */
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

/** 角色分页请求 */
export interface RoleGetPageInput {
  /** 名称 */
  name?: string | null
}

/** 角色分页响应 */
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

/** 角色用户列表响应 */
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

/** 发送邮箱验证码 */
export interface SendEmailCodeInput {
  /**
   * 邮箱地址
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

/** 消息内容 */
export interface SiteMsgGetContentOutput {
  /**
   * 消息Id
   * @format int64
   */
  msgId?: number
  /** 标题 */
  title?: string | null
  /** 类型名称 */
  typeName?: string | null
  /** 内容 */
  content?: string | null
  /**
   * 接收时间
   * @format date-time
   */
  receivedTime?: string | null
  /** 是否已读 */
  isRead?: boolean | null
}

/** 站点消息分页请求 */
export interface SiteMsgGetPageInput {
  /** 是否已读 */
  isRead?: boolean | null
  /**
   * 分类Id
   * @format int64
   */
  typeId?: number | null
  /** 标题 */
  title?: string | null
}

/** 站点消息分页响应 */
export interface SiteMsgGetPageOutput {
  /**
   * 唯一Id
   * @format int64
   */
  id?: number
  /**
   * 消息Id
   * @format int64
   */
  msgId?: number
  /** 标题 */
  title?: string | null
  /**
   * 类型Id
   * @format int64
   */
  typeId?: number
  /** 类型名称 */
  typeName?: string | null
  /** 是否已读 */
  isRead?: boolean | null
  /**
   * 接收时间
   * @format date-time
   */
  receivedTime?: string | null
}

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

/** 排序 */
export interface SortInput {
  /** 属性名称 */
  propName?: string | null
  /** 排序方式:Asc=0,Desc=1 */
  order?: SortOrder
  /** 是否升序 */
  isAscending?: boolean | null
}

/**
 * 排序方式:Asc=0,Desc=1
 * @format int32
 */
export type SortOrder = 0 | 1

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

/** 任务 */
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

/** 任务分页请求 */
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

/** 任务分页响应 */
export interface TaskGetPageOutput {
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

/**
 * SEC=1,RunOnDay=11,RunOnWeek=12,RunOnMonth=13,Custom=21
 * @format int32
 */
export type TaskInterval = 1 | 11 | 12 | 13 | 21

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

/** 任务日志分页请求 */
export interface TaskLogGetPageInput {
  /** 任务Id */
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
  /** MySql=0,SqlServer=1,PostgreSQL=2,Oracle=3,Sqlite=4,OdbcOracle=5,OdbcSqlServer=6,OdbcMySql=7,OdbcPostgreSQL=8,Odbc=9,MsAccess=11,Dameng=12,ShenTong=14,KingbaseES=15,Firebird=16,Custom=17,ClickHouse=18,GBase=19,QuestDb=20,Xugu=21,CustomOracle=22,CustomSqlServer=23,CustomMySql=24,CustomPostgreSQL=25,DuckDB=26,TDengine=27 */
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
   * @maxLength 60
   */
  createdUserName?: string | null
  /**
   * 创建者姓名
   * @maxLength 60
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
   * @maxLength 60
   */
  modifiedUserName?: string | null
  /**
   * 修改者姓名
   * @maxLength 60
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
  /** MySql=0,SqlServer=1,PostgreSQL=2,Oracle=3,Sqlite=4,OdbcOracle=5,OdbcSqlServer=6,OdbcMySql=7,OdbcPostgreSQL=8,Odbc=9,MsAccess=11,Dameng=12,ShenTong=14,KingbaseES=15,Firebird=16,Custom=17,ClickHouse=18,GBase=19,QuestDb=20,Xugu=21,CustomOracle=22,CustomSqlServer=23,CustomMySql=24,CustomPostgreSQL=25,DuckDB=26,TDengine=27 */
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
  /** MySql=0,SqlServer=1,PostgreSQL=2,Oracle=3,Sqlite=4,OdbcOracle=5,OdbcSqlServer=6,OdbcMySql=7,OdbcPostgreSQL=8,Odbc=9,MsAccess=11,Dameng=12,ShenTong=14,KingbaseES=15,Firebird=16,Custom=17,ClickHouse=18,GBase=19,QuestDb=20,Xugu=21,CustomOracle=22,CustomSqlServer=23,CustomMySql=24,CustomPostgreSQL=25,DuckDB=26,TDengine=27 */
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
  /** 套餐Id列表 */
  pkgIds?: number[] | null
}

/** 租户分页请求 */
export interface TenantGetPageInput {
  /** 企业名称 */
  name?: string | null
}

/** 租户分页响应 */
export interface TenantGetPageOutput {
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
  /** MySql=0,SqlServer=1,PostgreSQL=2,Oracle=3,Sqlite=4,OdbcOracle=5,OdbcSqlServer=6,OdbcMySql=7,OdbcPostgreSQL=8,Odbc=9,MsAccess=11,Dameng=12,ShenTong=14,KingbaseES=15,Firebird=16,Custom=17,ClickHouse=18,GBase=19,QuestDb=20,Xugu=21,CustomOracle=22,CustomSqlServer=23,CustomMySql=24,CustomPostgreSQL=25,DuckDB=26,TDengine=27 */
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
  /** MySql=0,SqlServer=1,PostgreSQL=2,Oracle=3,Sqlite=4,OdbcOracle=5,OdbcSqlServer=6,OdbcMySql=7,OdbcPostgreSQL=8,Odbc=9,MsAccess=11,Dameng=12,ShenTong=14,KingbaseES=15,Firebird=16,Custom=17,ClickHouse=18,GBase=19,QuestDb=20,Xugu=21,CustomOracle=22,CustomSqlServer=23,CustomMySql=24,CustomPostgreSQL=25,DuckDB=26,TDengine=27 */
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

/** 令牌信息 */
export interface TokenInfo {
  /** 访问令牌 */
  accessToken?: string | null
  /**
   * 访问令牌（冗余属性，兼容旧版本）
   * @deprecated
   */
  token?: string | null
  /**
   * 访问令牌的过期时间
   * @format date-time
   */
  accessTokenExpiresAt?: string
  /**
   * 访问令牌的生命周期（以秒为单位）
   * @format int32
   */
  accessTokenLifeTime?: number
  /** 刷新令牌 */
  refreshToken?: string | null
  /**
   * 刷新令牌的过期时间
   * @format date-time
   */
  refreshTokenExpiresAt?: string
  /**
   * 刷新令牌的生命周期（以秒为单位）
   * @format int32
   */
  refreshTokenLifeTime?: number
  /**
   * 创建令牌信息时间戳
   * @format int64
   */
  timestamp?: number
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
  /**
   * 直属主管Id
   * @format int64
   */
  managerUserId?: number | null
  /** 直属主管姓名 */
  managerUserName?: string | null
  /** 员工添加 */
  staff: StaffAddInput
  /** 所属部门Ids */
  orgIds?: number[] | null
  /**
   * 主属部门Id
   * @format int64
   */
  orgId?: number
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

/** 批量设置部门 */
export interface UserBatchSetOrgInput {
  /** 用户Id列表 */
  userIds?: number[] | null
  /** 所属部门Ids */
  orgIds?: number[] | null
  /**
   * 主属部门Id
   * @format int64
   */
  orgId?: number
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
   * @maxLength 60
   */
  createdUserName?: string | null
  /**
   * 创建者姓名
   * @maxLength 60
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
   * @maxLength 60
   */
  modifiedUserName?: string | null
  /**
   * 修改者姓名
   * @maxLength 60
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
  /**
   * 最后登录时间
   * @format date-time
   */
  lastLoginTime?: string | null
  /** 最后登录IP */
  lastLoginIP?: string | null
  /** 最后登录国家 */
  lastLoginCountry?: string | null
  /** 最后登录省份 */
  lastLoginProvince?: string | null
  /** 最后登录城市 */
  lastLoginCity?: string | null
  /** 启用 */
  enabled?: boolean
  /** 角色列表 */
  roles?: RoleEntity[] | null
  /** 部门列表 */
  orgs?: OrgEntity[] | null
  /** 用户员工 */
  staff?: UserStaffEntity
}

/** 用户基本信息 */
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
  /**
   * 最后登录时间
   * @format date-time
   */
  lastLoginTime?: string | null
  /** 最后登录IP */
  lastLoginIP?: string | null
  /** 最后登录国家 */
  lastLoginCountry?: string | null
  /** 最后登录省份 */
  lastLoginProvince?: string | null
  /** 最后登录城市 */
  lastLoginCity?: string | null
}

/** 用户 */
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
  roles?: UserGetRoleModel[] | null
  /** 角色Id列表 */
  roleIds?: number[] | null
}

/** 用户分页查询条件 */
export interface UserGetPageInput {
  /**
   * 部门Id
   * @format int64
   */
  orgId?: number | null
}

/** 用户分页查询响应 */
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
  /** 在线 */
  online?: boolean
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

/** 用户角色 */
export interface UserGetRoleModel {
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
   * @maxLength 60
   */
  createdUserName?: string | null
  /**
   * 创建者姓名
   * @maxLength 60
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
   * @maxLength 60
   */
  modifiedUserName?: string | null
  /**
   * 修改者姓名
   * @maxLength 60
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
  /** 平台 */
  platform?: string | null
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
  sort?: number | null
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
   * @maxLength 60
   */
  createdUserName?: string | null
  /**
   * 创建者姓名
   * @maxLength 60
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
   * @maxLength 60
   */
  modifiedUserName?: string | null
  /**
   * 修改者姓名
   * @maxLength 60
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
  /** 平台 */
  platform?: string | null
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

/** 视图查询 */
export interface ViewGetListInput {
  /** 平台 */
  platform?: string | null
  /** 视图命名 */
  name?: string | null
  /** 视图名称 */
  label?: string | null
  /** 视图路径 */
  path?: string | null
}

/** 视图 */
export interface ViewGetOutput {
  /**
   * 所属节点
   * @format int64
   */
  parentId?: number | null
  /** 平台 */
  platform?: string | null
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
  sort?: number | null
  /** 启用 */
  enabled?: boolean
  /**
   * 视图Id
   * @format int64
   */
  id: number
}

/** 视图列表 */
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
  /** 平台 */
  platform?: string | null
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

/** 视图同步 */
export interface ViewSyncInput {
  /** 视图列表 */
  views?: ViewSyncModel[] | null
}

/** 视图同步模型 */
export interface ViewSyncModel {
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

/** 修改 */
export interface ViewUpdateInput {
  /**
   * 所属节点
   * @format int64
   */
  parentId?: number | null
  /** 平台 */
  platform?: string | null
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
  sort?: number | null
  /** 启用 */
  enabled?: boolean
  /**
   * 视图Id
   * @format int64
   */
  id: number
}

/** WebSocket请求 */
export interface WebSocketPreConnectInput {
  /**
   * WebSocketId
   * @format int64
   */
  websocketId?: number | null
}
