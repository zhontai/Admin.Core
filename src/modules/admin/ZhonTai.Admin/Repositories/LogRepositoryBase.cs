using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;

namespace ZhonTai.Admin.Repositories;

/// <summary>
/// 日志库基础仓储
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class LogRepositoryBase<TEntity> : RepositoryBase<TEntity> where TEntity : class
{
    public LogRepositoryBase(UnitOfWorkManagerCloud uowm) : base(DbKeys.LogDb, uowm) 
    {
        
    }
}