using My.Admin.Domain.Admin;

namespace My.Admin.Repository.Admin
{
    public class ApiRepository : RepositoryBase<ApiEntity>, IApiRepository
    {
        public ApiRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}