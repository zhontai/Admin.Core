using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Domain.OperationLog;

namespace ZhonTai.Admin.Core.Extensions;

/// <summary>
/// FreeSql扩展
/// </summary>
public static class FreeSqlExtensions
{
    /// <summary>
    /// 使用日志数据库
    /// </summary>
    /// <param name="that"></param>
    /// <param name="dbConfig"></param>
    public static void UseLogDb(this IFreeSql that, DbConfig dbConfig)
    {
        if (dbConfig.Key == DbKeys.LogDb)
        {
            if (dbConfig.Type != FreeSql.DataType.QuestDb)
            {
                that.CodeFirst.Entity<OperationLogEntity>(a =>
                {
                    a.HasIndex(a => new { a.ApiPath, a.CreatedTime }).HasName("idx_{tablename}_01");
                });
            }
        }
    }
}