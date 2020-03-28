using FreeSql.DataAnnotations;

namespace Admin.Core.Model.Admin
{
    /// <summary>
    /// 操作日志
    /// </summary>
	[Table(Name = "ad_login_log")]
    public class LoginLogEntity : LogAbstract
    {
    }
}
