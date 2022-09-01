using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Role;

namespace ZhonTai.Admin.Repositories;

public class RoleRepository : RepositoryBase<RoleEntity>, IRoleRepository
{
    public RoleRepository(DbUnitOfWorkManager uowm) : base(uowm)
    {
    }
}