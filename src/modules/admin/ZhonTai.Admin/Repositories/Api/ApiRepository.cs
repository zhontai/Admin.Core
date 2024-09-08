using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.Api;

namespace ZhonTai.Admin.Repositories;

public class ApiRepository : AdminRepositoryBase<ApiEntity>, IApiRepository
{
    public ApiRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}