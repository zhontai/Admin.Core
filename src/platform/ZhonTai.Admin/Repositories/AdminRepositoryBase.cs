using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;

namespace ZhonTai.Admin.Repositories
{
    /// <summary>
    /// 权限库基础仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class AdminRepositoryBase<TEntity> : RepositoryBase<TEntity> where TEntity : class
    {
        public AdminRepositoryBase(UnitOfWorkManagerCloud uowm) : base(DbKeys.AppDb, uowm) 
        {
            
        }
    }
}