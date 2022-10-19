using System.Collections.Generic;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain;
using ZhonTai.Admin.Domain.Org;

namespace ZhonTai.Admin.Repositories;

public class OrgRepository : RepositoryBase<OrgEntity>, IOrgRepository
{
    private readonly IRepositoryBase<UserOrgEntity> _userOrgRepository;
    public OrgRepository(UnitOfWorkManagerCloud uowm, IRepositoryBase<UserOrgEntity> userOrgRepository) : base(DbKeys.AdminDb, uowm)
    {
        _userOrgRepository = userOrgRepository;
    }

    /// <summary>
    /// 获得本部门和下级部门Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<List<long>> GetChildIdListAsync(long id)
    {
        return await Select
        .Where(a => a.Id == id)
        .AsTreeCte()
        .ToListAsync(a => a.Id);
    }

    /// <summary>
    /// 本部门下是否有员工
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> HasUser(long id)
    {
        return await _userOrgRepository.Select.Where(a => a.OrgId == id).AnyAsync();
    }

    /// <summary>
    /// 部门列表下是否有员工
    /// </summary>
    /// <param name="idList"></param>
    /// <returns></returns>
    public async Task<bool> HasUser(List<long> idList)
    {
        return await _userOrgRepository.Select.Where(a => idList.Contains(a.OrgId)).AnyAsync();
    }
}