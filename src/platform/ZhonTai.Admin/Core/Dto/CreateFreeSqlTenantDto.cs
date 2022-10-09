using FreeSql;

namespace ZhonTai.Admin.Core.Dto;

public class CreateFreeSqlTenantDto
{
    /// <summary>
    /// 数据库注册键
    /// </summary>
    public string DbKey { get; set; }

    /// <summary>
    /// 数据库
    /// </summary>
    public DataType? DbType { get; set; }

    /// <summary>
    /// 连接字符串
    /// </summary>
    public string ConnectionString { get; set; }
}
