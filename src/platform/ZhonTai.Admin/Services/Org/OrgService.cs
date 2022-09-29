using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.Org.Input;
using ZhonTai.Admin.Services.Org.Output;
using System.Threading.Tasks;
using ZhonTai.Admin.Domain.Org;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.DynamicApi.Attributes;
using ZhonTai.DynamicApi;

namespace ZhonTai.Admin.Services.Org;

/// <summary>
/// 部门服务
/// </summary>
[DynamicApi(Area = AdminConsts.AreaName)]
public class OrgService : BaseService, IOrgService, IDynamicApi
{
    private readonly IOrgRepository _orgRepository;

    public OrgService(IOrgRepository orgRepository)
    {
        _orgRepository = orgRepository;
    }

    /// <summary>
    /// 查询部门
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IResultOutput> GetAsync(long id)
    {
        var result = await _orgRepository.GetAsync<OrgGetOutput>(id);
        return ResultOutput.Ok(result);
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<IResultOutput> GetListAsync(string key)
    {
        var data = await _orgRepository
            .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
            .OrderBy(a => a.ParentId)
            .OrderBy(a => a.Sort)
            .ToListAsync<OrgListOutput>();

        return ResultOutput.Ok(data);
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<IResultOutput> AddAsync(OrgAddInput input)
    {
        var dictionary = Mapper.Map<OrgEntity>(input);
        var id = (await _orgRepository.InsertAsync(dictionary)).Id;
        return ResultOutput.Result(id > 0);
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<IResultOutput> UpdateAsync(OrgUpdateInput input)
    {
        if (!(input?.Id > 0))
        {
            return ResultOutput.NotOk();
        }

        var entity = await _orgRepository.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            return ResultOutput.NotOk("数据字典不存在！");
        }

        Mapper.Map(input, entity);
        await _orgRepository.UpdateAsync(entity);
        return ResultOutput.Ok();
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IResultOutput> DeleteAsync(long id)
    {
        var result = await _orgRepository.DeleteRecursiveAsync(a => a.Id == id);

        return ResultOutput.Result(result);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IResultOutput> SoftDeleteAsync(long id)
    {
        var result = await _orgRepository.SoftDeleteRecursiveAsync(a => a.Id == id);

        return ResultOutput.Result(result);
    }
}