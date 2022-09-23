
using ZhonTai.Admin.Domain.Employee.Input;

namespace ZhonTai.Admin.Domain.Employee.Output;

public class EmployeeGetOutput : EmployeeUpdateInput
{
    /// <summary>
    /// 主属部门
    /// </summary>
    public string OrganizationName { get; set; }

    /// <summary>
    /// 附属部门
    /// </summary>
    public string[] OrganizationNames { get; set; }

    /// <summary>
    /// 职位
    /// </summary>
    public string Position { get; set; }
}