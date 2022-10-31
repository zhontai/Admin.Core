using System.Collections.Generic;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.UserOrg;

namespace ZhonTai.Admin.Repositories;

public class UserOrgRepository : AdminRepositoryBase<UserOrgEntity>, IUserOrgRepository
{
    public UserOrgRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
        
    }

    /// <summary>
    /// 本部门下是否有员工
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> HasUser(long id)
    {
        return await Select.Where(a => a.OrgId == id).AnyAsync();
    }

    /// <summary>
    /// 部门列表下是否有员工
    /// </summary>
    /// <param name="idList"></param>
    /// <returns></returns>
    public async Task<bool> HasUser(List<long> idList)
    {
        return await Select.Where(a => idList.Contains(a.OrgId)).AnyAsync();
    }
}