using Admin.Core.Service.Personnel.Employee.Input;

namespace Admin.Core.Service.Personnel.Employee.Output
{
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
        public string PositionName { get; set; }
    }
}