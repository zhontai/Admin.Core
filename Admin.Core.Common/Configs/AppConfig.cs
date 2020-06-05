namespace Admin.Core.Common.Configs
{
    /// <summary>
    /// 应用配置
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// Api地址，默认 http://*:8888
        /// </summary>
        public string Urls { get; set; } = "http://*:8888";

        /// <summary>
        /// Swagger文档
        /// </summary>
        public bool Swagger { get; set; } = false;

        /// <summary>
        /// Aop配置
        /// </summary>
        public AopConfig Aop { get; set; } = new AopConfig();

        /// <summary>
        /// 日志配置
        /// </summary>
        public LogConfig Log { get; set; } = new LogConfig();

        /// <summary>
        /// 限流
        /// </summary>
        public bool RateLimit { get; set; } = false;

        /// <summary>
        /// 验证码配置
        /// </summary>
        public VarifyCodeConfig VarifyCode { get; set; } = new VarifyCodeConfig();
    }

    /// <summary>
    /// Aop配置
    /// </summary>
    public class AopConfig
    {
        /// <summary>
        /// 事物
        /// </summary>
        public bool Transaction { get; set; } = true;
    }

    /// <summary>
    /// 日志配置
    /// </summary>
    public class LogConfig
    {
        /// <summary>
        /// 操作日志
        /// </summary>
        public bool Operation { get; set; } = true;
    }

    /// <summary>
    /// 验证码配置
    /// </summary>
    public class VarifyCodeConfig
    {
        /// <summary>
        /// 启用
        /// </summary>
        public bool Enable { get; set; } = true;
        
        /// <summary>
        /// 操作日志
        /// </summary>
        public string[] Font { get; set; } = { "Times New Roman", "Verdana", "Arial", "Gungsuh", "Impact" };
    }
}
