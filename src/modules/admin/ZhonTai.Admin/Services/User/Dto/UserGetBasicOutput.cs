using System;

namespace ZhonTai.Admin.Services.User.Dto;

public class UserGetBasicOutput
{
    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime? LastLoginTime { get; set; }

    /// <summary>
    /// 最后登录IP
    /// </summary>
    public string LastLoginIP { get; set; }

    /// <summary>
    /// 最后登录国家
    /// </summary>
    public string LastLoginCountry { get; set; }

    /// <summary>
    /// 最后登录省份
    /// </summary>
    public string LastLoginProvince { get; set; }

    /// <summary>
    /// 最后登录城市
    /// </summary>
    public string LastLoginCity { get; set; }
}