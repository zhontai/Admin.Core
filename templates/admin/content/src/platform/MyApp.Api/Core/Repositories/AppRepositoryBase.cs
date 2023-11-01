using MyApp.Api.Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;

namespace MyApp.Api.Core.Repositories;

public class AppRepositoryBase<TEntity> : RepositoryBase<TEntity> where TEntity : class
{
    public AppRepositoryBase(UnitOfWorkManagerCloud uowm) : base(DbKeys.AppDb, uowm)
    {

    }
}