
using DataType = FreeSql.DataType;

namespace Admin.Core.Common.Configs
{
    /// <summary>
    /// 多数据库
    /// </summary>
    public class MultiDb
    {
        /// <summary>
        /// 数据库命名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataType Type { get; set; }

        /// <summary>
        /// 数据库字符串
        /// </summary>
        public string ConnectionString { get; set; }
    }

    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DbConfig
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataType Type { get; set; } = DataType.Sqlite;

        /// <summary>
        /// 数据库字符串
        /// </summary>
        public string ConnectionString { get; set; } = "Data Source=|DataDirectory|\\admindb.db; Pooling=true;Min Pool Size=1";

        /// <summary>
        /// 空闲时间(分)
        /// </summary>
        public int IdleTime { get; set; } = 10;

        /// <summary>
        /// 生成数据
        /// </summary>
        public bool GenerateData { get; set; } = false;

        /// <summary>
        /// 同步结构
        /// </summary>
        public bool SyncStructure { get; set; } = true;

        /// <summary>
        /// 同步数据
        /// </summary>
        public bool SyncData { get; set; } = true;

        /// <summary>
        /// 建库
        /// </summary>
        public bool CreateDb { get; set; } = true;

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
        public MultiDb[] Dbs { get; set; }
    }
}
