using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.Organization.Input;
using ZhonTai.Admin.Services.Organization.Output;
using System.Threading.Tasks;
using ZhonTai.Admin.Services;
using ZhonTai.Admin.Domain.Organization;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;

namespace ZhonTai.Admin.Services.Organization;

/// <summary>
/// 部门服务
/// </summary>
public class OrganizationService : BaseService, IOrganizationService
{
    private readonly IOrganizationRepository _organizationRepository;

    public OrganizationService(IOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
    }

    /// <summary>
    /// 查询部门
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IResultOutput> GetAsync(long id)
    {
        var result = await _organizationRepository.GetAsync<OrganizationGetOutput>(id);
        return ResultOutput.Ok(result);
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<IResultOutput> GetListAsync(string key)
    {
        var data = await _organizationRepository
            .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
            .OrderBy(a => a.ParentId)
            .OrderBy(a => a.Sort)
            .ToListAsync<OrganizationListOutput>();

        return ResultOutput.Ok(data);
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<IResultOutput> AddAsync(OrganizationAddInput input)
    {
        var dictionary = Mapper.Map<OrganizationEntity>(input);
        var id = (await _organizationRepository.InsertAsync(dictionary)).Id;
        return ResultOutput.Result(id > 0);
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<IResultOutput> UpdateAsync(OrganizationUpdateInput input)
    {
        if (!(input?.Id > 0))
        {
            return ResultOutput.NotOk();
        }

        var entity = await _organizationRepository.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            return ResultOutput.NotOk("数据字典不存在！");
        }

        Mapper.Map(input, entity);
        await _organizationRepository.UpdateAsync(entity);
        return ResultOutput.Ok();
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IResultOutput> DeleteAsync(long id)
    {
        var result = await _organizationRepository.DeleteRecursiveAsync(a => a.Id == id);

        return ResultOutput.Result(result);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IResultOutput> SoftDeleteAsync(long id)
    {
        var result = await _organizationRepository.SoftDeleteRecursiveAsync(a => a.Id == id);

        return ResultOutput.Result(result);
    }
}