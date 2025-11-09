using System.Collections.Generic;

namespace ZhonTai.Module.Dev.Api.Contracts.Services.CodeGen.Dtos;

public class CodeGenGetOutput
{
    public long Id { get; set; }
    /// <summary>
    /// 作者姓名
    /// </summary>
    public string AuthorName { get; set; }

    /// <summary>
    /// 是否移除表前缀
    /// </summary>
    public bool TablePrefix { get; set; } = true;

    /// <summary>
    /// 生成方式
    /// </summary>
    public string GenerateType { get; set; }

    /// <summary>
    /// 库定位器名
    /// </summary>
    public string DbKey { get; set; }

    ///// <summary>
    ///// 数据库名(保留字段)
    ///// </summary>
    //public string? DbName { get; set; }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public string DbType { get; set; }

    /// <summary>
    /// 数据库表名
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// 命名空间
    /// </summary>
    public string Namespace { get; set; }

    /// <summary>
    /// 实体名称
    /// </summary>
    public string EntityName { get; set; }

    /// <summary>
    /// 业务名
    /// </summary>
    public string BusName { get; set; }

    /// <summary>
    /// Api分区名称
    /// </summary>
    public string ApiAreaName { get; set; }
    /// <summary>
    /// 基类名称
    /// </summary>
    public string BaseEntity { get; set; }

    /// <summary>
    /// 父菜单
    /// </summary>
    public string MenuPid { get; set; }

    /// <summary>
    /// 菜单后缀
    /// </summary>
    public string MenuAfterText { get; set; }

    /// <summary>
    /// 后端输出目录
    /// </summary>
    public string BackendOut { get; set; }

    /// <summary>
    /// 前端输出目录
    /// </summary>
    public string FrontendOut { get; set; }

    /// <summary>
    /// 数据库迁移目录
    /// </summary>
    public string DbMigrateSqlOut { get; set; }

    /// <summary>
    /// 备注说明
    /// </summary>
    public string Comment { get; set; }

    /// <summary>
    /// 实体导入的命令空间
    /// </summary>
    public string Usings { get; set; }

    /// <summary>
    /// 生成Entity实体类
    /// </summary>
    public bool GenEntity { get; set; }

    /// <summary>
    /// 生成Repository仓储类
    /// </summary>
    public bool GenRepository { get; set; }

    /// <summary>
    /// 生成Service服务类
    /// </summary>
    public bool GenService { get; set; }

    /// <summary>
    /// 生成查询单条记录
    /// </summary>
    public bool GenGet { get; set; } = true;

    /// <summary>
    /// 生成分页查询
    /// </summary>
    public bool GenGetPage { get; set; } = true;

    /// <summary>
    /// 生成列表查询服务
    /// </summary>
    public bool GenGetList { get; set; }

    /// <summary>
    /// 生成新增服务
    /// </summary>
    public bool GenAdd { get; set; } = true;

    /// <summary>
    /// 生成更新服务
    /// </summary>
    public bool GenUpdate { get; set; } = true;

    /// <summary>
    /// 新增删除服务
    /// </summary>
    public bool GenDelete { get; set; }

    /// <summary>
    /// 生成软删除服务
    /// </summary>
    public bool GenSoftDelete { get; set; } = true;

    /// <summary>
    /// 生成批量删除服务
    /// </summary>
    public bool GenBatchDelete { get; set; }

    /// <summary>
    /// 生成批量软删除服务
    /// </summary>
    public bool GenBatchSoftDelete { get; set; }

    /// <summary>
    /// 字段列表
    /// </summary>
    public IEnumerable<CodeGenFieldGetOutput> Fields { get; set; }
}