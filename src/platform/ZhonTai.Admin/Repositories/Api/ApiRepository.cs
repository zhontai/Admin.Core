using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Api;

namespace ZhonTai.Admin.Repositories;

public class ApiRepository : RepositoryCloud<ApiEntity>, IApiRepository
{
    public ApiRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}