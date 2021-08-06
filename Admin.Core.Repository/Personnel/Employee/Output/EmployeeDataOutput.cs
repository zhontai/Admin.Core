using Admin.Core.Model.Enums;
using System;

namespace Admin.Core.Repository.Personnel.Output
{
    /// <summary>
    /// 员工数据导出
    /// </summary>
    public partial class EmployeeDataOutput
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public long? TenantId { get; set; }

        /// <summary>
        /// 员工Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 主属部门Id
        /// </summary>
        public long OrganizationId { get; set; }

        /// <summary>
        /// 主管Id
        /// </summary>
        public long? PrimaryEmployeeId { get; set; }

        /// <summary>
        /// 职位Id
        /// </summary>
        public long PositionId { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime HireDate { get; set; }

    }
}