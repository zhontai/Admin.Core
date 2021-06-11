using Admin.Core.Model.Admin;

namespace Admin.Core.Repository.Admin
{
    public class ApiRepository : RepositoryBase<ApiEntity>, IApiRepository
    {
        public ApiRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}