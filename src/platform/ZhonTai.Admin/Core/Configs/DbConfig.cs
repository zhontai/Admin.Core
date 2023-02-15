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
    /// 同步数据
    /// </summary>
    public bool SyncData { get; set; } = false;

    /// <summary>
    /// 同步更新数据
    /// </summary>
    public bool SysUpdateData { get; set; } = false;

    /// <summary>
    /// 同步数据地址
    /// </summary>
    public string SyncDataPath { get; set; } = "InitData/Admin";

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