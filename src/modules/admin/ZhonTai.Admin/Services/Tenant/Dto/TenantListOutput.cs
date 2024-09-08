using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ZhonTai.Admin.Domain.Pkg;

namespace ZhonTai.Admin.Services.Tenant.Dto;

public class TenantListOutput
{
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 企业名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 企业编码
    /// </summary>
    public string Code { get; set; }

    [JsonIgnore]
    public ICollection<PkgEntity> Pkgs { get; set; }

    /// <summary>
    /// 套餐
    /// </summary>
    public string[] PkgNames { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string RealName { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 邮箱地址
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 数据库
    /// </summary>
    [JsonIgnore]
    public FreeSql.DataType? DbType { get; set; }

    /// <summary>
    /// 数据库名称
    /// </summary>
    public string DbTypeName => DbType?.ToDescriptionOrString();

    /// <summary>
    /// 启用
    /// </summary>
	public bool Enabled { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedTime { get; set; }
}