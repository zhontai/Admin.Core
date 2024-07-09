using System.Collections.Generic;

namespace ZhonTai.Admin.Services.Auth.Dto;

public class AuthGetUserPermissionsOutput
{
    /// <summary>
    /// 用户权限列表
    /// </summary>
    public List<string> Permissions { get; set; }
}