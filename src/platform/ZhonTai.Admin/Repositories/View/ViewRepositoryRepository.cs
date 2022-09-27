using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.View;

namespace ZhonTai.Admin.Repositories;

public class ViewRepository : RepositoryCloud<ViewEntity>, IViewRepository
{
    public ViewRepository(UnitOfWorkManagerCloud muowm) : base(muowm)
    {
    }
}