using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Services.Tenant.Dto;

/// <summary>
/// 添加
/// </summary>
public class TenantAddInput
{
    /// <summary>
    /// 企业名称
    /// </summary>
    [Required(ErrorMessage = "请输入企业名称")]
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    [Required(ErrorMessage = "请输入编码")]
    public string Code { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [Required(ErrorMessage = "请输入姓名")]
    public string RealName { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    [Required(ErrorMessage = "请输入手机号码")]
    public string Phone { get; set; }

    /// <summary>
    /// 邮箱地址
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 数据库注册键
    /// </summary>
    public string DbKey { get; set; }

    /// <summary>
    /// 数据库
    /// </summary>
    public FreeSql.DataType? DbType { get; set; }

    /// <summary>
    /// 连接字符串
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
	public bool Enabled { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string Description { get; set; }
}