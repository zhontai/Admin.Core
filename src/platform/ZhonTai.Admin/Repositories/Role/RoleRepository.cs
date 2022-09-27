using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Role;

namespace ZhonTai.Admin.Repositories;

public class RoleRepository : RepositoryCloud<RoleEntity>, IRoleRepository
{
    public RoleRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}