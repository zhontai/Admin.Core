using FreeSql.DataAnnotations;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Consts;

namespace ZhonTai.Admin.Domain.LoginLog;

/// <summary>
/// 登录日志
/// </summary>
[Database(DbNames.Log)]
[Table(Name = DbConsts.TableNamePrefix + "login_log")]
public partial class LoginLogEntity : LogAbstract
{
}