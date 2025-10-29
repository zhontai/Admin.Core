namespace ZhonTai.Module.Dev.Api.Contracts.Services.CodeGen.Dtos;

public class CodeGenFieldGetOutput
{
    public long Id { get; set; }

    public long CodeGenId { get; set; }

    /// <summary>
    /// 库定位器名
    /// </summary>
    public string DbKey { get; set; }

    /// <summary>
    /// 字段名
    /// </summary>
    public string ColumnName { get; set; } = "";

    /// <summary>
    /// 数据库列名(物理字段名)
    /// </summary>
    public string ColumnRawName { get; set; }

    /// <summary>
    /// .NET数据类型
    /// </summary>
    public string NetType { get; set; } = "string";

    /// <summary>
    /// 数据库中类型（物理类型）
    /// </summary>
    public string DbType { get; set; }

    /// <summary>
    /// 字段描述
    /// </summary>
    public string Comment { get; set; }

    /// <summary>
    /// 默认值
    /// </summary>
    public string DefaultValue { get; set; }
    /// <summary>
    /// 字段标题
    /// </summary>
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
    public long? Length { get; set; }

    /// <summary>
    /// 编辑器
    /// </summary>
    public string Editor { get; set; }

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
    public string IndexMode { get; set; }

    /// <summary>
    /// 唯一键
    /// </summary>
    public bool IsUnique { get; set; }

    /// <summary>
    /// 查询方式
    /// </summary>
    public string QueryType { get; set; }


    /// <summary>
    /// 字典编码
    /// </summary>
    public string DictTypeCode { get; set; }

    /// <summary>
    /// 外联实体名
    /// </summary>
    public string IncludeEntity { get; set; }

    /// <summary>
    /// 外联对应关系 0 1对1 1 1对多
    /// </summary>
    public int IncludeMode { get; set; }

    /// <summary>
    /// 外联实体关联键
    /// </summary>
    public string IncludeEntityKey { get; set; }

    /// <summary>
    /// 显示文本字段
    /// </summary>
    public string DisplayColumn { get; set; }
    /// <summary>
    /// 选中值字段
    /// </summary>
    public string ValueColumn { get; set; }

    /// <summary>
    /// 父级字段
    /// </summary>
    public string PidColumn { get; set; }

    /// <summary>
    /// 作用类型（字典）
    /// </summary>
    public string EffectType { get; set; }

    /// <summary>
    /// 前端规则检测触发时机
    /// </summary>
    public string FrontendRuleTrigger { get; set; }
}