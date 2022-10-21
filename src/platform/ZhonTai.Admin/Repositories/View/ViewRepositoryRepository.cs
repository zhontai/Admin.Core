using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.View;

namespace ZhonTai.Admin.Repositories;

public class ViewRepository : AppRepositoryBase<ViewEntity>, IViewRepository
{
    public ViewRepository(UnitOfWorkManagerCloud muowm) : base(muowm)
    {
    }
}