using System.Collections.Generic;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Repositories;

namespace ZhonTai.Admin.Domain.Pkg;

public interface IPkgRepository : IRepositoryBase<PkgEntity>
{
    /// <summary>
    /// 获得本套餐和下级套餐Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<List<long>> GetChildIdListAsync(long id);

    /// <summary>
    /// 获得当前套餐和下级套餐Id
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<List<long>> GetChildIdListAsync(long[] ids);
}