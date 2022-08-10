using System;

namespace ZhonTai.Admin.Services.Employee.Output
{
    public class EmployeeListOutput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string JobNumber { get; set; }

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

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
    }
}