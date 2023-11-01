using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.View;

namespace ZhonTai.Admin.Repositories;

public class ViewRepository : AdminRepositoryBase<ViewEntity>, IViewRepository
{
    public ViewRepository(UnitOfWorkManagerCloud muowm) : base(muowm)
    {
    }
}