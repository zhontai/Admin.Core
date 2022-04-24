using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Api;

namespace ZhonTai.Admin.Repositories
{
    public class ApiRepository : RepositoryBase<ApiEntity>, IApiRepository
    {
        public ApiRepository(DbUnitOfWorkManager uowm) : base(uowm)
        {
        }
    }
}