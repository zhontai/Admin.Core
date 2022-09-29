
using ZhonTai.Admin.Domain.Staff.Input;

namespace ZhonTai.Admin.Domain.Staff.Output;

public class StaffGetOutput : StaffUpdateInput
{
    /// <summary>
    /// 主属部门
    /// </summary>
    public string OrgName { get; set; }

    /// <summary>
    /// 附属部门
    /// </summary>
    public string[] OrgNames { get; set; }
}