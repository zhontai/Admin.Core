using System.Text.Json.Serialization;
using ZhonTai.Admin.Domain.UserStaff;

namespace ZhonTai.Admin.Services.Auth.Dto;

/// <summary>
/// 用户个人信息
/// </summary>
public class AuthUserProfileOutput
{
    /// <summary>
    /// 账号
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public Sex? Sex { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// 企业
    /// </summary>
    public string CorpName { get; set; }

    /// <summary>
    /// 职位
    /// </summary>
    public string Position { get; set; }

    /// <summary>
    /// 主属部门
    /// </summary>
    public string DeptName { get; set; }

    /// <summary>
    /// 企业微信名片
    /// </summary>
    public string WorkWeChatCard { get; set; }

    /// <summary>
    /// 水印文案
    /// </summary>
    public string WatermarkText { get; set; }
}