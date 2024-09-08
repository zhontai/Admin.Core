using FreeSql.DataAnnotations;

namespace ZhonTai.Admin.Domain.LoginLog;

/// <summary>
/// 登录日志
/// </summary>
[Table(Name = "ad_login_log")]
public partial class LoginLogEntity : LogAbstract
{
}