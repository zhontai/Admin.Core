namespace Admin.Core.Common.Auth
{
    public interface IUser
    {
        /// <summary>
        /// 主键
        /// </summary>
        long Id { get; }
        /// <summary>
        /// 登录日志Id
        /// </summary>
        long LoginLogId { get; }
        /// <summary>
        /// 用户名
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 姓名
        /// </summary>
        string RealName { get; }
        /// <summary>
        /// IP
        /// </summary>
        string IP { get; }
    }
}
