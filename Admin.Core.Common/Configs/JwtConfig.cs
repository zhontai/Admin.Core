
using DataType = FreeSql.DataType;

namespace Admin.Core.Common.Configs
{
    /// <summary>
    /// Jwt配置
    /// </summary>
    public class JwtConfig
    {
        /// <summary>
        /// 发行者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 订阅者
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string SecurityKey { get; set; }

        /// <summary>
        /// 有效期(分钟)
        /// </summary>
        public int Expires { get; set; }
    }
}
