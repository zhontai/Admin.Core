using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Services.Permission.Dto;

public class PermissionSaveTenantPermissionsInput
{
    [Required(ErrorMessage = "租户不能为空！")]
    public long TenantId { get; set; }

    [Required(ErrorMessage = "权限不能为空！")]
    public List<long> PermissionIds { get; set; }
}