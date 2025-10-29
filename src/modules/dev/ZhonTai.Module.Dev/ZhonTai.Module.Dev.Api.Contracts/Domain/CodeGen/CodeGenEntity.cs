using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using ZhonTai.Admin.Core.Entities;

namespace ZhonTai.Module.Dev.Api.Contracts.Domain.CodeGen;

/// <summary>
/// 代码生成
/// </summary>
[Table(Name = "dev_config")]
public class CodeGenEntity : EntityBase
{
    /// <summary>
    /// 作者姓名
    /// </summary>
    [Column(StringLength = 32)]
    public string AuthorName { get; set; }

    /// <summary>
    /// 是否移除表前缀
    /// </summary>
    public Boolean TablePrefix { get; set; } = true;

    /// <summary>
    /// 生成方式 1 CodeFirst 2 DbFirst
    /// </summary>
    [Column(StringLength = 32)]
    public string? GenerateType { get; set; }

    /// <summary>
    /// 库定位器名
    /// </summary>
    [Column(StringLength = 64)]
    public string DbKey { get; set; }

    /// <summary>
    /// 数据库名(保留字段)
    /// </summary>
    [Column(StringLength = 64)]
    public string? DbName { get; set; }

    /// <summary>
    /// 数据库类型
    /// </summary>
    [Column(StringLength = 64)]
    public string? DbType { get; set; }

    /// <summary>
    /// 数据库表名
    /// </summary>
    [Column(StringLength = 128)]
    public string TableName { get; set; }

    /// <summary>
    /// 命名空间
    /// </summary>
    [Column(StringLength = 128)]
    public string Namespace { get; set; }

    /// <summary>
    /// 实体名称
    /// </summary>
    [Column(StringLength = 128)]
    public string EntityName { get; set; }

    /// <summary>
    /// 业务名
    /// </summary>
    [Column(StringLength = 128)]
    public string BusName { get; set; }

    /// <summary>
    /// Api分区名称
    /// </summary>
    [Column(StringLength = 128)]
    public String? ApiAreaName { get; set; }
    /// <summary>
    /// 基类名称
    /// </summary>
    [Column(StringLength = 32)]
    public String? BaseEntity { get; set; }

    /// <summary>
    /// 菜单编码
    /// </summary>
    [Column(StringLength = 128)]
    public string MenuPid { get; set; } = "/app";

    /// <summary>
    /// 菜单后缀
    /// </summary>
    [Column(StringLength = 128)]
    public string MenuAfterText { get; set; }

    /// <summary>
    /// 后端输出目录
    /// </summary>
    [Column(StringLength = 256)]
    public String BackendOut { get; set; }

    /// <summary>
    /// 前端输出目录
    /// </summary>
    [Column(StringLength = 256)]
    public String? FrontendOut { get; set; }

    /// <summary>
    /// 数据迁移输出目录
    /// </summary>
    [Column(StringLength = 256)]
    public String DbMigrateSqlOut { get; set; }

    /// <summary>
    /// 备注说明
    /// </summary>
    [Column(StringLength = 256)]
    public String? Comment { get; set; }

    /// <summary>
    /// 实体导入的命令空间
    /// </summary>
    [Column(StringLength = 256)]
    public String? Usings { get; set; }

    /// <summary>
    /// 生成Entity实体类
    /// </summary>
    public Boolean GenEntity { get; set; }

    /// <summary>
    /// 生成Repository仓储类
    /// </summary>
    public Boolean GenRepository { get; set; }

    /// <summary>
    /// 生成Service服务类
    /// </summary>
    public Boolean GenService { get; set; }

    /// <summary>
    /// 生成新增服务
    /// </summary>
    public Boolean GenAdd { get; set; } = true;

    /// <summary>
    /// 生成更新服务
    /// </summary>
    public Boolean GenUpdate { get; set; } = true;

    /// <summary>
    /// 新增删除服务
    /// </summary>
    public Boolean GenDelete { get; set; } = true;

    /// <summary>
    /// 生成列表查询服务
    /// </summary>
    public Boolean GenGetList { get; set; }

    /// <summary>
    /// 生成软删除服务
    /// </summary>
    public Boolean GenSoftDelete { get; set; }

    /// <summary>
    /// 生成批量删除服务
    /// </summary>
    public Boolean GenBatchDelete { get; set; }

    /// <summary>
    /// 生成批量软删除服务
    /// </summary>
    public Boolean GenBatchSoftDelete { get; set; }

    /// <summary>
    /// 字段列表
    /// </summary>
    public IEnumerable<CodeGenFieldEntity>? Fields { get; set; }
}