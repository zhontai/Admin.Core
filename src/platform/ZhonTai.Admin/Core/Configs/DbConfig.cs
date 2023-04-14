using ZhonTai.Admin.Core.Consts;
using DataType = FreeSql.DataType;

namespace ZhonTai.Admin.Core.Configs;

/// <summary>
/// 数据库配置
/// </summary>
public class DbConfig
{
    /// <summary>
    /// 数据库注册键
    /// </summary>
    public string Key { get; set; } = DbKeys.AppDb;

    /// <summary>
    /// 程序集名称
    /// </summary>
    public string[] AssemblyNames { get; set; }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public DataType Type { get; set; } = DataType.Sqlite;

    /// <summary>
    /// 数据库字符串
    /// </summary>
    public string ConnectionString { get; set; } = "Data Source=|DataDirectory|\\admindb.db; Pooling=true;Min Pool Size=1";

    /// <summary>
    /// 指定程序集
    /// </summary>
    public string ProviderType { get; set; }

    /// <summary>
    /// 生成数据
    /// </summary>
    public bool GenerateData { get; set; } = false;

    /// <summary>
    /// 同步结构
    /// </summary>
    public bool SyncStructure { get; set; } = false;

    /// <summary>
    /// 同步结构脚本
    /// </summary>
    public bool SyncStructureSql { get; set; } = false;

    /// <summary>
    /// 同步数据
    /// </summary>
    public bool SyncData { get; set; } = false;

    /// <summary>
    /// 同步数据监听Curd操作
    /// </summary>
    public bool SyncDataCurd { get; set; } = false;

    /// <summary>
    /// 同步更新数据
    /// </summary>
    public bool SysUpdateData { get; set; } = false;

    /// <summary>
    /// 同步数据地址
    /// </summary>
    public string SyncDataPath { get; set; } = "InitData/Admin";

    /// <summary>
    /// 同步数据包含表列表
    /// </summary>
    public string[] SyncDataIncludeTables { get; set; }

    /// <summary>
    /// 同步数据排除表列表
    /// </summary>
    public string[] SyncDataExcludeTables { get; set; }

    /// <summary>
    /// 同步数据操作用户
    /// </summary>
    public SyncDataUser SyncDataUser { get; set; } = new SyncDataUser { Id = 161223411986501, UserName = "admin", TenantId = 161223412138053 };

    /// <summary>
    /// 建库
    /// </summary>
    public bool CreateDb { get; set; } = false;

    /// <summary>
    /// 建库连接字符串
    /// </summary>
    public string CreateDbConnectionString { get; set; }

    /// <summary>
    /// 建库脚本
    /// </summary>
    public string CreateDbSql { get; set; }

    /// <summary>
    /// 监听所有操作
    /// </summary>
    public bool MonitorCommand { get; set; } = false;

    /// <summary>
    /// 监听Curd操作
    /// </summary>
    public bool Curd { get; set; } = false;

    /// <summary>
    /// 空闲时间（分），默认10分钟，设置idleTime=0则不自动回收
    /// </summary>
    public int? IdleTime { get; set; } = 10;

    /// <summary>
    /// 多数据库
    /// </summary>
    public DbConfig[] Dbs { get; set; }

    /// <summary>
    /// 读写分离从库列表
    /// </summary>
    public SlaveDb[] SlaveList { get; set; }
}

/// <summary>
/// 读写分离从库
/// </summary>
public class SlaveDb
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public int Weight { get; set; } = 1;

    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public string ConnectionString { get; set; }
}

/// <summary>
/// 同步数据操作用户
/// </summary>
public class SyncDataUser
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public long TenantId { get; set; }
}