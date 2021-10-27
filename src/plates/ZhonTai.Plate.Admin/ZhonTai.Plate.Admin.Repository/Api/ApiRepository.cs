using ZhonTai.Plate.Admin.Domain;
using ZhonTai.Plate.Admin.Domain.Api;

namespace ZhonTai.Plate.Admin.Repository
{
    public class ApiRepository : RepositoryBase<ApiEntity>, IApiRepository
    {
        public ApiRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}