using FreeSql.DataAnnotations;

namespace ZhonTai.Admin.Domain.LoginLog;

/// <summary>
/// 登录日志
/// </summary>
[Table(Name = "base_login_log", OldName = "ad_login_log")]
public partial class LoginLogEntity : LogAbstract
{
}