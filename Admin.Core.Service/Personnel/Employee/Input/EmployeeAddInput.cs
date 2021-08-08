using Admin.Core.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Admin.Core.Service.Personnel.Employee.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class EmployeeAddInput
    {
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
        /// 附属部门
        /// </summary>
        public long[] OrganizationIds { get; set; }

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
        public DateTime? EntryTime { get; set; }
    }
}