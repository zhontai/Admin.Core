using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.Pkg;

namespace ZhonTai.Admin.Repositories;

public class PkgRepository : AdminRepositoryBase<PkgEntity>, IPkgRepository
{
    public PkgRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }

    /// <summary>
    /// 获得本角色和下级角色Id
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
    /// 获得当前角色和下级角色Id
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<List<long>> GetChildIdListAsync(long[] ids)
    {
        return await Select
        .Where(a => ids.Contains(a.Id))
        .AsTreeCte()
        .ToListAsync(a => a.Id);
    }
}