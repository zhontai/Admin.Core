using System;
using ZhonTai.Admin.Domain.UserStaff;

namespace ZhonTai.Admin.Domain.User;

/// <summary>
/// 员工添加
/// </summary>
public class StaffAddInput
{
    /// <summary>
    /// 工号
    /// </summary>
    public string JobNumber { get; set; }

    /// <summary>
    /// 职位
    /// </summary>
    public string Position { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public Sex? Sex { get; set; }

    /// <summary>
    /// 入职时间
    /// </summary>
    public DateTime? EntryTime { get; set; }

    /// <summary>
    /// 个人简介
    /// </summary>
    public string Introduce { get; set; }
}