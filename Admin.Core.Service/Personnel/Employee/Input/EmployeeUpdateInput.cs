
namespace Admin.Core.Service.Personnel.Employee.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public class EmployeeUpdateInput: EmployeeAddInput
    {
        /// <summary>
        /// 编号
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
    }
}