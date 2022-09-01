using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain;
using ZhonTai.Admin.Services.Employee.Input;
using ZhonTai.Admin.Services.Employee.Output;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.Employee;
using ZhonTai.Admin.Domain.Organization;

namespace ZhonTai.Admin.Services.Employee;

/// <summary>
/// 员工服务
/// </summary>
public class EmployeeService : BaseService, IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IRepositoryBase<EmployeeOrganizationEntity> _employeeOrganizationRepository;

    public EmployeeService(
        IEmployeeRepository userRepository,
        IRepositoryBase<EmployeeOrganizationEntity> employeeOrganizationRepository
    )
    {
        _employeeRepository = userRepository;
        _employeeOrganizationRepository = employeeOrganizationRepository;
    }

    /// <summary>
    /// 查询员工
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ResultOutput<EmployeeGetOutput>> GetAsync(long id)
    {
        var res = new ResultOutput<EmployeeGetOutput>();

        var dto = await _employeeRepository.Select
        .WhereDynamic(id)
        .IncludeMany(a => a.Orgs.Select(b => new OrganizationEntity { Id = b.Id }))
        .ToOneAsync(a => new EmployeeGetOutput
        {
            OrganizationName = a.MainOrg.Name
        });

        return res.Ok(dto);
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<IResultOutput> GetPageAsync(PageInput input)
    {
        var list = await _employeeRepository.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .Count(out var total)
        .OrderByDescending(true, a => a.Id)
        .IncludeMany(a => a.Orgs.Select(b => new OrganizationEntity { Name = b.Name }))
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync(a => new EmployeeListOutput 
        { 
            OrganizationName = a.MainOrg.Name
        });

        var data = new PageOutput<EmployeeListOutput>()
        {
            List = list,
            Total = total
        };

        return ResultOutput.Ok(data);
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Transaction]
    public async Task<IResultOutput> AddAsync(EmployeeAddInput input)
    {
        var entity = Mapper.Map<EmployeeEntity>(input);
        var employeeId = (await _employeeRepository.InsertAsync(entity))?.Id;

        if (!(employeeId > 0))
        {
            return ResultOutput.NotOk();
        }

        //附属部门
        if (input.OrganizationIds != null && input.OrganizationIds.Any())
        {
            var organizations = input.OrganizationIds.Select(organizationId => new EmployeeOrganizationEntity
            { 
                EmployeeId = employeeId.Value, 
                OrganizationId = organizationId 
            });
            await _employeeOrganizationRepository.InsertAsync(organizations);
        }

        return ResultOutput.Ok();
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Transaction]
    public async Task<IResultOutput> UpdateAsync(EmployeeUpdateInput input)
    {
        if (!(input?.Id > 0))
        {
            return ResultOutput.NotOk();
        }

        var employee = await _employeeRepository.GetAsync(input.Id);
        if (!(employee?.Id > 0))
        {
            return ResultOutput.NotOk("用户不存在！");
        }

        Mapper.Map(input, employee);
        await _employeeRepository.UpdateAsync(employee);

        await _employeeOrganizationRepository.DeleteAsync(a => a.EmployeeId == employee.Id);

        //附属部门
        if (input.OrganizationIds != null && input.OrganizationIds.Any())
        {
            var organizations = input.OrganizationIds.Select(organizationId => new EmployeeOrganizationEntity 
            { 
                EmployeeId = employee.Id, 
                OrganizationId = organizationId 
            });
            await _employeeOrganizationRepository.InsertAsync(organizations);
        }

        return ResultOutput.Ok();
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Transaction]
    public async Task<IResultOutput> DeleteAsync(long id)
    {
        //删除员工附属部门
        await _employeeOrganizationRepository.DeleteAsync(a => a.EmployeeId == id);

        //删除员工
        await _employeeRepository.DeleteAsync(m => m.Id == id);

        return ResultOutput.Ok();
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IResultOutput> SoftDeleteAsync(long id)
    {
        await _employeeRepository.SoftDeleteAsync(id);

        return ResultOutput.Ok();
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<IResultOutput> BatchSoftDeleteAsync(long[] ids)
    {
        await _employeeRepository.SoftDeleteAsync(ids);

        return ResultOutput.Ok();
    }
}