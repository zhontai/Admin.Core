using ZhonTai.Plate.Admin.Domain;
using ZhonTai.Plate.Admin.Domain.Role;

namespace ZhonTai.Plate.Admin.Repository
{
    public class RoleRepository : RepositoryBase<RoleEntity>, IRoleRepository
    {
        public RoleRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}