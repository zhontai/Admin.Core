namespace Admin.Core.Common.Configs
{
    /// <summary>
    /// 应用配置
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// Swagger文档
        /// </summary>
        public bool Swagger { get; set; }

        /// <summary>
        /// Api地址，默认 http://*:8081
        /// </summary>
        public string Urls { get; set; } = "http://*:8081";

        /// <summary>
        /// Aop配置
        /// </summary>
        public AopConfig Aop { get; set; }

        /// <summary>
        /// 日志配置
        /// </summary>
        public LogConfig Log { get; set; }

        /// <summary>
        /// 验证码配置
        /// </summary>
        public VarifyCodeConfig VarifyCode { get; set; }
    }

    /// <summary>
    /// Aop配置
    /// </summary>
    public class AopConfig
    {
        /// <summary>
        /// 事物
        /// </summary>
        public bool Transaction { get; set; }
    }

    /// <summary>
    /// 日志配置
    /// </summary>
    public class LogConfig
    {
        /// <summary>
        /// 操作日志
        /// </summary>
        public bool Operation { get; set; }
    }

    /// <summary>
    /// 验证码配置
    /// </summary>
    public class VarifyCodeConfig
    {
        /// <summary>
        /// 操作日志
        /// </summary>
        public string[] Font { get; set; }
    }
}
