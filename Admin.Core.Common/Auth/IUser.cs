namespace Admin.Core.Common.Auth
{
    /// <summary>
    /// 用户信息接口
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// 主键
        /// </summary>
        long Id { get; }

        /// <summary>
        /// 用户名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 姓名
        /// </summary>
        string RealName { get; }
    }
}
