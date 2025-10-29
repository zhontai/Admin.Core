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

export type ActionResult = object

export interface BaseDataGetOutput {
  databases?: DatabaseGetOutput[] | null
  authorName?: string | null
  apiAreaName?: string | null
  namespace?: string | null
  backendOut?: string | null
  frontendOut?: string | null
  dbMigrateSqlOut?: string | null
  usings?: string | null
  menuAfterText?: string | null
}

export interface CodeGenFieldGetOutput {
  /** @format int64 */
  id?: number
  /** @format int64 */
  codeGenId?: number
  /** 库定位器名 */
  dbKey?: string | null
  /** 字段名 */
  columnName?: string | null
  /** 数据库列名(物理字段名) */
  columnRawName?: string | null
  /** .NET数据类型 */
  netType?: string | null
  /** 数据库中类型（物理类型） */
  dbType?: string | null
  /** 字段描述 */
  comment?: string | null
  /** 默认值 */
  defaultValue?: string | null
  /** 字段标题 */
  title?: string | null
  /** 主键 */
  isPrimary?: boolean
  /** 可空 */
  isNullable?: boolean
  /**
   * 长度
   * @format int64
   */
  length?: number | null
  /** 编辑器 */
  editor?: string | null
  /**
   * 同步表结构时的列排序
   * @format int32
   */
  position?: number
  /** 是否通用字段 */
  whetherCommon?: boolean
  /** 列表是否缩进（字典） */
  whetherRetract?: boolean
  /** 是否是查询条件 */
  whetherQuery?: boolean
  /** 增 */
  whetherAdd?: boolean
  /** 改 */
  whetherUpdate?: boolean
  /** 分布显示 */
  whetherTable?: boolean
  /** 列表 */
  whetherList?: boolean
  /** 索引方式 */
  indexMode?: string | null
  /** 唯一键 */
  isUnique?: boolean
  /** 查询方式 */
  queryType?: string | null
  /** 字典编码 */
  dictTypeCode?: string | null
  /** 外联实体名 */
  includeEntity?: string | null
  /**
   * 外联对应关系 0 1对1 1 1对多
   * @format int32
   */
  includeMode?: number
  /** 外联实体关联键 */
  includeEntityKey?: string | null
  /** 显示文本字段 */
  displayColumn?: string | null
  /** 选中值字段 */
  valueColumn?: string | null
  /** 父级字段 */
  pidColumn?: string | null
  /** 作用类型（字典） */
  effectType?: string | null
  /** 前端规则检测触发时机 */
  frontendRuleTrigger?: string | null
}

export interface CodeGenGetOutput {
  /** @format int64 */
  id?: number
  /** 作者姓名 */
  authorName?: string | null
  /** 是否移除表前缀 */
  tablePrefix?: boolean
  /** 生成方式 */
  generateType?: string | null
  /** 库定位器名 */
  dbKey?: string | null
  /** 数据库类型 */
  dbType?: string | null
  /** 数据库表名 */
  tableName?: string | null
  /** 命名空间 */
  namespace?: string | null
  /** 实体名称 */
  entityName?: string | null
  /** 业务名 */
  busName?: string | null
  /** Api分区名称 */
  apiAreaName?: string | null
  /** 基类名称 */
  baseEntity?: string | null
  /** 父菜单 */
  menuPid?: string | null
  /** 菜单后缀 */
  menuAfterText?: string | null
  /** 后端输出目录 */
  backendOut?: string | null
  /** 前端输出目录 */
  frontendOut?: string | null
  /** 数据库迁移目录 */
  dbMigrateSqlOut?: string | null
  /** 备注说明 */
  comment?: string | null
  /** 实体导入的命令空间 */
  usings?: string | null
  /** 生成Entity实体类 */
  genEntity?: boolean
  /** 生成Repository仓储类 */
  genRepository?: boolean
  /** 生成Service服务类 */
  genService?: boolean
  /** 生成新增服务 */
  genAdd?: boolean
  /** 生成更新服务 */
  genUpdate?: boolean
  /** 新增删除服务 */
  genDelete?: boolean
  /** 生成列表查询服务 */
  genGetList?: boolean
  /** 生成软删除服务 */
  genSoftDelete?: boolean
  /** 生成批量删除服务 */
  genBatchDelete?: boolean
  /** 生成批量软删除服务 */
  genBatchSoftDelete?: boolean
  /** 字段列表 */
  fields?: CodeGenFieldGetOutput[] | null
}

export interface CodeGenUpdateInput {
  /** @format int64 */
  id?: number
  /** 作者姓名 */
  authorName?: string | null
  /** 是否移除表前缀 */
  tablePrefix?: boolean
  /** 生成方式 */
  generateType?: string | null
  /** 库定位器名 */
  dbKey?: string | null
  /** 数据库类型 */
  dbType?: string | null
  /** 数据库表名 */
  tableName?: string | null
  /** 命名空间 */
  namespace?: string | null
  /** 实体名称 */
  entityName?: string | null
  /** 业务名 */
  busName?: string | null
  /** Api分区名称 */
  apiAreaName?: string | null
  /** 基类名称 */
  baseEntity?: string | null
  /** 父菜单 */
  menuPid?: string | null
  /** 菜单后缀 */
  menuAfterText?: string | null
  /** 后端输出目录 */
  backendOut?: string | null
  /** 前端输出目录 */
  frontendOut?: string | null
  /** 数据库迁移目录 */
  dbMigrateSqlOut?: string | null
  /** 备注说明 */
  comment?: string | null
  /** 实体导入的命令空间 */
  usings?: string | null
  /** 生成Entity实体类 */
  genEntity?: boolean
  /** 生成Repository仓储类 */
  genRepository?: boolean
  /** 生成Service服务类 */
  genService?: boolean
  /** 生成新增服务 */
  genAdd?: boolean
  /** 生成更新服务 */
  genUpdate?: boolean
  /** 新增删除服务 */
  genDelete?: boolean
  /** 生成列表查询服务 */
  genGetList?: boolean
  /** 生成软删除服务 */
  genSoftDelete?: boolean
  /** 生成批量删除服务 */
  genBatchDelete?: boolean
  /** 生成批量软删除服务 */
  genBatchSoftDelete?: boolean
  /** 字段列表 */
  fields?: CodeGenFieldGetOutput[] | null
}

export interface DatabaseGetOutput {
  dbKey?: string | null
  type?: string | null
}

/** 模板组新增输入 */
export interface DevGroupAddInput {
  /**
   * 模板组名称
   * @minLength 1
   */
  name: string
  /** 备注 */
  remark?: string | null
}

/** 模板组列表查询条件输入 */
export interface DevGroupGetListInput {
  /** 模板组名称 */
  name?: string | null
  /**
   * 模板Id
   * @format int64
   */
  id?: number | null
}

/** 模板组列表查询结果输出 */
export interface DevGroupGetListOutput {
  /** @format int64 */
  id?: number
  /** @format date-time */
  createdTime?: string
  createdUserName?: string | null
  modifiedUserName?: string | null
  /** @format date-time */
  modifiedTime?: string | null
  /** 模板组名称 */
  name?: string | null
  /** 备注 */
  remark?: string | null
}

/** 模板组查询结果输出 */
export interface DevGroupGetOutput {
  /** @format int64 */
  id?: number
  /** 模板组名称 */
  name?: string | null
  /** 备注 */
  remark?: string | null
}

/** 模板组分页查询条件输入 */
export interface DevGroupGetPageInput {
  /** 模板组名称 */
  name?: string | null
  /**
   * 模板Id
   * @format int64
   */
  id?: number | null
}

/** 模板组分页查询结果输出 */
export interface DevGroupGetPageOutput {
  /** @format int64 */
  id?: number
  /** @format date-time */
  createdTime?: string
  createdUserName?: string | null
  modifiedUserName?: string | null
  /** @format date-time */
  modifiedTime?: string | null
  /** 模板组名称 */
  name?: string | null
  /** 备注 */
  remark?: string | null
}

/** 模板组更新数据输入 */
export interface DevGroupUpdateInput {
  /** @format int64 */
  id: number
  /**
   * 模板组名称
   * @minLength 1
   */
  name: string
  /** 备注 */
  remark?: string | null
}

/** 项目新增输入 */
export interface DevProjectAddInput {
  /**
   * 使用模板组
   * @format int64
   */
  groupId?: number
  /**
   * 项目名称
   * @minLength 1
   */
  name: string
  /**
   * 项目编码
   * @minLength 1
   */
  code: string
  /** 是否启用 */
  isEnable: boolean
  /** 备注 */
  remark?: string | null
}

/** 项目生成新增输入 */
export interface DevProjectGenAddInput {
  /**
   * 所属项目
   * @format int64
   */
  projectId: number
  /**
   * 模板组
   * @minLength 1
   */
  groupIds: string
  /** 页面提交的模板组数组 */
  groupIds_Values?: string[] | null
}

export interface DevProjectGenGenerateInput {
  /**
   * 项目Id
   * @format int64
   */
  projectId?: number
  /** 模型Ids */
  modelIds?: number[] | null
  /** 分组Ids */
  groupIds?: number[] | null
  /** 模板Ids */
  templateIds?: number[] | null
  /** 是否是预览 */
  isPreview?: boolean
}

export interface DevProjectGenGenerateOutput {
  /**
   * 模板id
   * @format int64
   */
  templateId?: number
  /** 输出路径 */
  path?: string | null
  /** 生成内容 */
  content?: string | null
}

/** 项目生成列表查询条件输入 */
export interface DevProjectGenGetListInput {
  /**
   * 所属项目
   * @format int64
   */
  projectId?: number | null
}

/** 项目生成列表查询结果输出 */
export interface DevProjectGenGetListOutput {
  /** @format int64 */
  id?: number
  /** @format date-time */
  createdTime?: string
  createdUserName?: string | null
  modifiedUserName?: string | null
  /** @format date-time */
  modifiedTime?: string | null
  /**
   * 所属项目
   * @format int64
   */
  projectId?: number
  /** 所属项目显示文本 */
  projectId_Text?: string | null
  /** 模板组 */
  groupIds?: string | null
  /** 模板组显示文本 */
  groupIds_Texts?: string[] | null
  /** 页面使用的模板组数组 */
  groupIds_Values?: string[] | null
}

/** 项目生成查询结果输出 */
export interface DevProjectGenGetOutput {
  /** @format int64 */
  id?: number
  /**
   * 所属项目
   * @format int64
   */
  projectId?: number
  /** 所属项目显示文本 */
  projectId_Text?: string | null
  /** 模板组 */
  groupIds?: string | null
  /** 模板组显示文本 */
  groupIds_Texts?: string[] | null
  /** 页面使用的模板组数组 */
  groupIds_Values?: string[] | null
}

/** 项目生成分页查询条件输入 */
export interface DevProjectGenGetPageInput {
  /**
   * 所属项目
   * @format int64
   */
  projectId?: number | null
}

/** 项目生成分页查询结果输出 */
export interface DevProjectGenGetPageOutput {
  /** @format int64 */
  id?: number
  /** @format date-time */
  createdTime?: string
  createdUserName?: string | null
  modifiedUserName?: string | null
  /** @format date-time */
  modifiedTime?: string | null
  /**
   * 所属项目
   * @format int64
   */
  projectId?: number
  /** 所属项目显示文本 */
  projectId_Text?: string | null
  /** 模板组 */
  groupIds?: string | null
  /** 模板组显示文本 */
  groupIds_Texts?: string[] | null
  /** 页面使用的模板组数组 */
  groupIds_Values?: string[] | null
}

/** 项目生成预览 */
export interface DevProjectGenPreviewMenuInput {
  /**
   * 项目Id
   * @format int64
   */
  projectId?: number
  /** 模板组 */
  groupIds?: number[] | null
  /** 模板状态 */
  templateStatus?: boolean | null
}

export interface DevProjectGenPreviewMenuOutput {
  /**
   * 分组ID
   * @format int64
   */
  groupId?: number
  /** 分组名 */
  groupName?: string | null
  /** 模板列表 */
  templateList?: DevProjectGenPreviewTemplateOutput[] | null
}

export interface DevProjectGenPreviewTemplateOutput {
  /**
   * 模板组Id
   * @format int64
   */
  groupId?: number
  /**
   * 模板Id
   * @format int64
   */
  templateId?: number
  /** 模板名称 */
  templateName?: string | null
  /** 模板生成路径 */
  tempaltePath?: string | null
  /** 是否启用 */
  isEnable?: boolean
}

/** 项目生成更新数据输入 */
export interface DevProjectGenUpdateInput {
  /** @format int64 */
  id: number
  /**
   * 所属项目
   * @format int64
   */
  projectId: number
  /**
   * 模板组
   * @minLength 1
   */
  groupIds: string
  /** 页面提交的模板组数组 */
  groupIds_Values?: string[] | null
}

/** 项目列表查询条件输入 */
export interface DevProjectGetListInput {
  /** 项目名称 */
  name?: string | null
  /** 项目编码 */
  code?: string | null
}

/** 项目列表查询结果输出 */
export interface DevProjectGetListOutput {
  /** @format int64 */
  id?: number
  /** @format date-time */
  createdTime?: string
  createdUserName?: string | null
  modifiedUserName?: string | null
  /** @format date-time */
  modifiedTime?: string | null
  /** 项目名称 */
  name?: string | null
  /** 项目编码 */
  code?: string | null
  /** 是否启用 */
  isEnable?: boolean
  /**
   * 使用模板组
   * @format int64
   */
  groupId?: number
  /** 使用模板组显示文本 */
  groupId_Text?: string | null
  /** 备注 */
  remark?: string | null
}

/** 项目查询结果输出 */
export interface DevProjectGetOutput {
  /** @format int64 */
  id?: number
  /** 项目名称 */
  name?: string | null
  /** 项目编码 */
  code?: string | null
  /** 是否启用 */
  isEnable?: boolean
  /**
   * 使用模板组
   * @format int64
   */
  groupId?: number
  /** 使用模板组显示文本 */
  groupId_Text?: string | null
  /** 备注 */
  remark?: string | null
}

/** 项目分页查询条件输入 */
export interface DevProjectGetPageInput {
  /** 项目名称 */
  name?: string | null
  /** 项目编码 */
  code?: string | null
}

/** 项目分页查询结果输出 */
export interface DevProjectGetPageOutput {
  /** @format int64 */
  id?: number
  /** @format date-time */
  createdTime?: string
  createdUserName?: string | null
  modifiedUserName?: string | null
  /** @format date-time */
  modifiedTime?: string | null
  /** 项目名称 */
  name?: string | null
  /** 项目编码 */
  code?: string | null
  /** 是否启用 */
  isEnable?: boolean
  /**
   * 使用模板组
   * @format int64
   */
  groupId?: number
  /** 使用模板组显示文本 */
  groupId_Text?: string | null
  /** 备注 */
  remark?: string | null
}

/** 项目模型新增输入 */
export interface DevProjectModelAddInput {
  /**
   * 所属项目
   * @format int64
   */
  projectId?: number
  /**
   * 模型名称
   * @minLength 1
   */
  name: string
  /**
   * 模型编码
   * @minLength 1
   */
  code: string
  /** 是否启用 */
  isEnable: boolean
  /** 备注 */
  remark?: string | null
}

/** 项目模型字段新增输入 */
export interface DevProjectModelFieldAddInput {
  /**
   * 所属模型
   * @format int64
   */
  modelId?: number | null
  /**
   * 字段名称
   * @minLength 1
   */
  name: string
  /**
   * 字段编码
   * @minLength 1
   */
  code: string
  /** 字段类型 */
  dataType?: string | null
  /** 是否必填 */
  isRequired?: boolean | null
  /**
   * 最大长度
   * @format int32
   */
  maxLength?: number | null
  /**
   * 最小长度
   * @format int32
   */
  minLength?: number | null
  /**
   * 字段顺序
   * @format int32
   */
  sort: number
  /** 字段描述 */
  description?: string | null
  /**
   * 字段属性
   * @minLength 1
   */
  properties: string
}

/** 项目模型字段列表查询条件输入 */
export interface DevProjectModelFieldGetListInput {
  /**
   * 所属模型
   * @format int64
   */
  modelId?: number | null
  /** 字段名称 */
  name?: string | null
}

/** 项目模型字段列表查询结果输出 */
export interface DevProjectModelFieldGetListOutput {
  /** @format int64 */
  id?: number
  /** @format date-time */
  createdTime?: string
  createdUserName?: string | null
  modifiedUserName?: string | null
  /** @format date-time */
  modifiedTime?: string | null
  /**
   * 所属模型
   * @format int64
   */
  modelId?: number | null
  /** 所属模型显示文本 */
  modelId_Text?: string | null
  /** 字段名称 */
  name?: string | null
  /** 字段编码 */
  code?: string | null
  /** 字段类型 */
  dataType?: string | null
  /** 字段类型名称 */
  dataTypeDictName?: string | null
  /** 是否必填 */
  isRequired?: boolean | null
  /**
   * 最大长度
   * @format int32
   */
  maxLength?: number | null
  /**
   * 最小长度
   * @format int32
   */
  minLength?: number | null
  /**
   * 字段顺序
   * @format int32
   */
  sort?: number
  /** 字段描述 */
  description?: string | null
  /** 字段属性 */
  properties?: string | null
  /** 字段属性名称 */
  propertiesDictName?: string | null
}

/** 项目模型字段查询结果输出 */
export interface DevProjectModelFieldGetOutput {
  /** @format int64 */
  id?: number
  /**
   * 所属模型
   * @format int64
   */
  modelId?: number | null
  /** 所属模型显示文本 */
  modelId_Text?: string | null
  /** 字段名称 */
  name?: string | null
  /** 字段编码 */
  code?: string | null
  /** 字段类型 */
  dataType?: string | null
  /** 是否必填 */
  isRequired?: boolean | null
  /**
   * 最大长度
   * @format int32
   */
  maxLength?: number | null
  /**
   * 最小长度
   * @format int32
   */
  minLength?: number | null
  /**
   * 字段顺序
   * @format int32
   */
  sort?: number
  /** 字段描述 */
  description?: string | null
  /** 字段属性 */
  properties?: string | null
}

/** 项目模型字段分页查询条件输入 */
export interface DevProjectModelFieldGetPageInput {
  /**
   * 所属模型
   * @format int64
   */
  modelId?: number | null
  /** 字段名称 */
  name?: string | null
}

/** 项目模型字段分页查询结果输出 */
export interface DevProjectModelFieldGetPageOutput {
  /** @format int64 */
  id?: number
  /** @format date-time */
  createdTime?: string
  createdUserName?: string | null
  modifiedUserName?: string | null
  /** @format date-time */
  modifiedTime?: string | null
  /**
   * 所属模型
   * @format int64
   */
  modelId?: number | null
  /** 所属模型显示文本 */
  modelId_Text?: string | null
  /** 字段名称 */
  name?: string | null
  /** 字段编码 */
  code?: string | null
  /** 字段类型 */
  dataType?: string | null
  /** 字段类型名称 */
  dataTypeDictName?: string | null
  /** 是否必填 */
  isRequired?: boolean | null
  /**
   * 最大长度
   * @format int32
   */
  maxLength?: number | null
  /**
   * 最小长度
   * @format int32
   */
  minLength?: number | null
  /**
   * 字段顺序
   * @format int32
   */
  sort?: number
  /** 字段描述 */
  description?: string | null
  /** 字段属性 */
  properties?: string | null
  /** 字段属性名称 */
  propertiesDictName?: string | null
}

/** 项目模型字段更新数据输入 */
export interface DevProjectModelFieldUpdateInput {
  /** @format int64 */
  id: number
  /**
   * 所属模型
   * @format int64
   */
  modelId?: number | null
  /**
   * 字段名称
   * @minLength 1
   */
  name: string
  /**
   * 字段编码
   * @minLength 1
   */
  code: string
  /** 字段类型 */
  dataType?: string | null
  /** 是否必填 */
  isRequired?: boolean | null
  /**
   * 最大长度
   * @format int32
   */
  maxLength?: number | null
  /**
   * 最小长度
   * @format int32
   */
  minLength?: number | null
  /**
   * 字段顺序
   * @format int32
   */
  sort: number
  /** 字段描述 */
  description?: string | null
  /**
   * 字段属性
   * @minLength 1
   */
  properties: string
}

/** 项目模型列表查询条件输入 */
export interface DevProjectModelGetListInput {
  /**
   * 所属项目
   * @format int64
   */
  projectId?: number | null
  /** 模型名称 */
  name?: string | null
  /** 模型编码 */
  code?: string | null
}

/** 项目模型列表查询结果输出 */
export interface DevProjectModelGetListOutput {
  /** @format int64 */
  id?: number
  /** @format date-time */
  createdTime?: string
  createdUserName?: string | null
  modifiedUserName?: string | null
  /** @format date-time */
  modifiedTime?: string | null
  /**
   * 所属项目
   * @format int64
   */
  projectId?: number
  /** 所属项目显示文本 */
  projectId_Text?: string | null
  /** 模型名称 */
  name?: string | null
  /** 模型编码 */
  code?: string | null
  /** 是否启用 */
  isEnable?: boolean
  /** 备注 */
  remark?: string | null
}

/** 项目模型查询结果输出 */
export interface DevProjectModelGetOutput {
  /** @format int64 */
  id?: number
  /**
   * 所属项目
   * @format int64
   */
  projectId?: number
  /** 所属项目显示文本 */
  projectId_Text?: string | null
  /** 模型名称 */
  name?: string | null
  /** 模型编码 */
  code?: string | null
  /** 是否启用 */
  isEnable?: boolean
  /** 备注 */
  remark?: string | null
}

/** 项目模型分页查询条件输入 */
export interface DevProjectModelGetPageInput {
  /**
   * 所属项目
   * @format int64
   */
  projectId?: number | null
  /** 模型名称 */
  name?: string | null
  /** 模型编码 */
  code?: string | null
}

/** 项目模型分页查询结果输出 */
export interface DevProjectModelGetPageOutput {
  /** @format int64 */
  id?: number
  /** @format date-time */
  createdTime?: string
  createdUserName?: string | null
  modifiedUserName?: string | null
  /** @format date-time */
  modifiedTime?: string | null
  /**
   * 所属项目
   * @format int64
   */
  projectId?: number
  /** 所属项目显示文本 */
  projectId_Text?: string | null
  /** 模型名称 */
  name?: string | null
  /** 模型编码 */
  code?: string | null
  /** 是否启用 */
  isEnable?: boolean
  /** 备注 */
  remark?: string | null
}

/** 项目模型更新数据输入 */
export interface DevProjectModelUpdateInput {
  /** @format int64 */
  id: number
  /**
   * 所属项目
   * @format int64
   */
  projectId: number
  /**
   * 模型名称
   * @minLength 1
   */
  name: string
  /**
   * 模型编码
   * @minLength 1
   */
  code: string
  /** 是否启用 */
  isEnable: boolean
  /** 备注 */
  remark?: string | null
}

/** 项目更新数据输入 */
export interface DevProjectUpdateInput {
  /** @format int64 */
  id: number
  /**
   * 项目名称
   * @minLength 1
   */
  name: string
  /**
   * 项目编码
   * @minLength 1
   */
  code: string
  /** 是否启用 */
  isEnable: boolean
  /**
   * 使用模板组
   * @format int64
   */
  groupId: number
  /** 备注 */
  remark?: string | null
}

/** 模板新增输入 */
export interface DevTemplateAddInput {
  /**
   * 模板名称
   * @minLength 1
   */
  name: string
  /**
   * 模板分组
   * @format int64
   */
  groupId?: number
  /** 生成路径 */
  outTo?: string | null
  /** 是否启用 */
  isEnable: boolean
  /**
   * 模板内容
   * @minLength 1
   */
  content: string
}

/** 模板列表查询条件输入 */
export interface DevTemplateGetListInput {
  /** 模板名称 */
  name?: string | null
  /**
   * 模板分组
   * @format int64
   */
  groupId?: number | null
}

/** 模板列表查询结果输出 */
export interface DevTemplateGetListOutput {
  /** @format int64 */
  id?: number
  /** @format date-time */
  createdTime?: string
  createdUserName?: string | null
  modifiedUserName?: string | null
  /** @format date-time */
  modifiedTime?: string | null
  /** 模板名称 */
  name?: string | null
  /**
   * 模板分组
   * @format int64
   */
  groupId?: number
  /** 模板分组显示文本 */
  groupId_Text?: string | null
  /** 生成路径 */
  outTo?: string | null
  /** 是否启用 */
  isEnable?: boolean
  /** 模板内容 */
  content?: string | null
}

/** 模板查询结果输出 */
export interface DevTemplateGetOutput {
  /** @format int64 */
  id?: number
  /** 模板名称 */
  name?: string | null
  /**
   * 模板分组
   * @format int64
   */
  groupId?: number
  /** 模板分组显示文本 */
  groupId_Text?: string | null
  /** 生成路径 */
  outTo?: string | null
  /** 是否启用 */
  isEnable?: boolean
  /** 模板内容 */
  content?: string | null
}

/** 模板分页查询条件输入 */
export interface DevTemplateGetPageInput {
  /** 模板名称 */
  name?: string | null
  /**
   * 模板分组
   * @format int64
   */
  groupId?: number | null
}

/** 模板分页查询结果输出 */
export interface DevTemplateGetPageOutput {
  /** @format int64 */
  id?: number
  /** @format date-time */
  createdTime?: string
  createdUserName?: string | null
  modifiedUserName?: string | null
  /** @format date-time */
  modifiedTime?: string | null
  /** 模板名称 */
  name?: string | null
  /**
   * 模板分组
   * @format int64
   */
  groupId?: number
  /** 模板分组显示文本 */
  groupId_Text?: string | null
  /** 生成路径 */
  outTo?: string | null
  /** 是否启用 */
  isEnable?: boolean
  /** 模板内容 */
  content?: string | null
}

/** 模板更新数据输入 */
export interface DevTemplateUpdateInput {
  /** @format int64 */
  id: number
  /**
   * 模板名称
   * @minLength 1
   */
  name: string
  /**
   * 模板分组
   * @format int64
   */
  groupId: number
  /** 生成路径 */
  outTo?: string | null
  /** 是否启用 */
  isEnable: boolean
  /**
   * 模板内容
   * @minLength 1
   */
  content: string
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

/** 分页信息输入 */
export interface PageInputDevGroupGetPageInput {
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
  /** 模板组分页查询条件输入 */
  filter?: DevGroupGetPageInput
}

/** 分页信息输入 */
export interface PageInputDevProjectGenGetPageInput {
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
  /** 项目生成分页查询条件输入 */
  filter?: DevProjectGenGetPageInput
}

/** 分页信息输入 */
export interface PageInputDevProjectGetPageInput {
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
  /** 项目分页查询条件输入 */
  filter?: DevProjectGetPageInput
}

/** 分页信息输入 */
export interface PageInputDevProjectModelFieldGetPageInput {
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
  /** 项目模型字段分页查询条件输入 */
  filter?: DevProjectModelFieldGetPageInput
}

/** 分页信息输入 */
export interface PageInputDevProjectModelGetPageInput {
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
  /** 项目模型分页查询条件输入 */
  filter?: DevProjectModelGetPageInput
}

/** 分页信息输入 */
export interface PageInputDevTemplateGetPageInput {
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
  /** 模板分页查询条件输入 */
  filter?: DevTemplateGetPageInput
}

/** 分页信息输出 */
export interface PageOutputDevGroupGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: DevGroupGetPageOutput[] | null
}

/** 分页信息输出 */
export interface PageOutputDevProjectGenGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: DevProjectGenGetPageOutput[] | null
}

/** 分页信息输出 */
export interface PageOutputDevProjectGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: DevProjectGetPageOutput[] | null
}

/** 分页信息输出 */
export interface PageOutputDevProjectModelFieldGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: DevProjectModelFieldGetPageOutput[] | null
}

/** 分页信息输出 */
export interface PageOutputDevProjectModelGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: DevProjectModelGetPageOutput[] | null
}

/** 分页信息输出 */
export interface PageOutputDevTemplateGetPageOutput {
  /**
   * 数据总数
   * @format int64
   */
  total?: number
  /** 数据 */
  list?: DevTemplateGetPageOutput[] | null
}

/** 结果输出 */
export interface ResultOutputActionResult {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: ActionResult
}

/** 结果输出 */
export interface ResultOutputBaseDataGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: BaseDataGetOutput
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
export interface ResultOutputCodeGenGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  data?: CodeGenGetOutput
}

/** 结果输出 */
export interface ResultOutputDevGroupGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 模板组查询结果输出 */
  data?: DevGroupGetOutput
}

/** 结果输出 */
export interface ResultOutputDevProjectGenGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 项目生成查询结果输出 */
  data?: DevProjectGenGetOutput
}

/** 结果输出 */
export interface ResultOutputDevProjectGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 项目查询结果输出 */
  data?: DevProjectGetOutput
}

/** 结果输出 */
export interface ResultOutputDevProjectModelFieldGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 项目模型字段查询结果输出 */
  data?: DevProjectModelFieldGetOutput
}

/** 结果输出 */
export interface ResultOutputDevProjectModelGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 项目模型查询结果输出 */
  data?: DevProjectModelGetOutput
}

/** 结果输出 */
export interface ResultOutputDevTemplateGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 模板查询结果输出 */
  data?: DevTemplateGetOutput
}

/** 结果输出 */
export interface ResultOutputIEnumerableCodeGenGetOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: CodeGenGetOutput[] | null
}

/** 结果输出 */
export interface ResultOutputIEnumerableDevGroupGetListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: DevGroupGetListOutput[] | null
}

/** 结果输出 */
export interface ResultOutputIEnumerableDevProjectGenGetListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: DevProjectGenGetListOutput[] | null
}

/** 结果输出 */
export interface ResultOutputIEnumerableDevProjectGenPreviewMenuOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: DevProjectGenPreviewMenuOutput[] | null
}

/** 结果输出 */
export interface ResultOutputIEnumerableDevProjectGetListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: DevProjectGetListOutput[] | null
}

/** 结果输出 */
export interface ResultOutputIEnumerableDevProjectModelFieldGetListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: DevProjectModelFieldGetListOutput[] | null
}

/** 结果输出 */
export interface ResultOutputIEnumerableDevProjectModelGetListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: DevProjectModelGetListOutput[] | null
}

/** 结果输出 */
export interface ResultOutputIEnumerableDevTemplateGetListOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: DevTemplateGetListOutput[] | null
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
export interface ResultOutputListDevProjectGenGenerateOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 数据 */
  data?: DevProjectGenGenerateOutput[] | null
}

/** 结果输出 */
export interface ResultOutputPageOutputDevGroupGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputDevGroupGetPageOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputDevProjectGenGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputDevProjectGenGetPageOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputDevProjectGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputDevProjectGetPageOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputDevProjectModelFieldGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputDevProjectModelFieldGetPageOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputDevProjectModelGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputDevProjectModelGetPageOutput
}

/** 结果输出 */
export interface ResultOutputPageOutputDevTemplateGetPageOutput {
  /** 是否成功标记 */
  success?: boolean
  /** 编码 */
  code?: string | null
  /** 消息 */
  msg?: string | null
  /** 分页信息输出 */
  data?: PageOutputDevTemplateGetPageOutput
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
