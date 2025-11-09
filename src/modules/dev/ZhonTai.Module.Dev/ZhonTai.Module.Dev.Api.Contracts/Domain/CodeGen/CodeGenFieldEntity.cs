using FreeSql.DataAnnotations;
using System;
using ZhonTai.Admin.Core.Entities;

namespace ZhonTai.Module.Dev.Api.Contracts.Domain.CodeGen;

/// <summary>
/// 代码生成字段
/// </summary>
[Table(Name = "dev_code_gen_field")]
public partial class CodeGenFieldEntity : EntityBase
{
    /// <summary>
    /// 代码生成ID
    /// </summary>
    public long CodeGenId { get; set; }

    /// <summary>
    /// 库定位器名
    /// </summary>
    [Column(StringLength = 64)]
    public string DbKey { get; set; }

    /// <summary>
    /// 字段名
    /// </summary>
    [Column(StringLength = 128)]
    public string ColumnName { get; set; } = "";

    /// <summary>
    /// 数据库列名(物理字段名)
    /// </summary>
    [Column(StringLength = 128)]
    public string? ColumnRawName { get; set; }

    /// <summary>
    /// .NET数据类型
    /// </summary>
    [Column(StringLength = 64)]
    public string NetType { get; set; } = "string";
    
    /// <summary>
    /// 数据类型
    /// </summary>
    [Column(StringLength = 64)]
    public string? DataType { get; set; }

    /// <summary>
    /// 字段描述
    /// </summary>
    [Column(StringLength = 256)]
    public string? Comment { get; set; }

    /// <summary>
    /// 默认值
    /// </summary>
    [Column(StringLength = 64)]
    public string? DefaultValue { get; set; }
    /// <summary>
    /// 字段标题
    /// </summary>
    [Column(StringLength = 64)]
    public string Title { get; set; } = "";

    /// <summary>
    /// 主键
    /// </summary>
    public bool IsPrimary { get; set; }

    /// <summary>
    /// 可空
    /// </summary>
    public bool IsNullable { get; set; }

    /// <summary>
    /// 长度
    /// </summary>
    public string? Length { get; set; }

    /// <summary>
    /// 编辑器
    /// </summary>
    [Column(StringLength = 32)]
    public string Editor { get; set; } = "el-input";

    /// <summary>
    /// 同步表结构时的列排序
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// 是否通用字段
    /// </summary>
    public bool WhetherCommon { get; set; }

    /// <summary>
    /// 列表是否缩进（字典）
    /// </summary>
    [Column(StringLength = 8)]
    public bool WhetherRetract { get; set; }

    /// <summary>
    /// 是否是查询条件
    /// </summary>
    public bool WhetherQuery { get; set; }

    /// <summary>
    /// 增
    /// </summary>
    public bool WhetherAdd { get; set; }

    /// <summary>
    /// 改
    /// </summary>
    public bool WhetherUpdate { get; set; }

    /// <summary>
    /// 分布显示
    /// </summary>
    public bool WhetherTable { get; set; }

    /// <summary>
    /// 列表
    /// </summary>
    public bool WhetherList { get; set; }

    /// <summary>
    /// 索引方式
    /// </summary>
    public string? IndexMode { get; set; }

    /// <summary>
    /// 唯一键
    /// </summary>
    public bool IsUnique { get; set; }

    /// <summary>
    /// 查询方式
    /// </summary>
    [Column(StringLength = 16)]
    public string? QueryType { get; set; }

    /// <summary>
    /// 字典编码
    /// </summary>
    [Column(StringLength = 64)]
    public string? DictTypeCode { get; set; }

    /// <summary>
    /// 外联实体名
    /// </summary>
    [Column(StringLength = 64)]
    public string? IncludeEntity { get; set; }

    /// <summary>
    /// 外联对应关系 0 1对1 1 1对多
    /// </summary>
    public int IncludeMode { get; set; }

    /// <summary>
    /// 外联实体关联键
    /// </summary>
    [Column(StringLength = 64)]
    public string? IncludeEntityKey { get; set; }

    /// <summary>
    /// 显示文本字段
    /// </summary>
    [Column(StringLength = 512)]
    public string? DisplayColumn { get; set; }

    /// <summary>
    /// 选中值字段
    /// </summary>
    [Column(StringLength = 256)]
    public string? ValueColumn { get; set; }

    /// <summary>
    /// 父级字段
    /// </summary>
    [Column(StringLength = 64)]
    public string? PidColumn { get; set; }

    /// <summary>
    /// 作用类型（字典）
    /// </summary>
    [Column(StringLength = 64)]
    public string? EffectType { get; set; }

    /// <summary>
    /// 前端规则检测触发时机
    /// </summary>
    [Column(StringLength = 64)]
    public string? FrontendRuleTrigger { get; set; }
}
