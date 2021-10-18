using FreeSql.DataAnnotations;

namespace ZhonTai.Plate.Admin.Domain.LoginLog
{
    /// <summary>
    /// 操作日志
    /// </summary>
	[Table(Name = "ad_login_log")]
    public class LoginLogEntity : LogAbstract
    {
    }
}