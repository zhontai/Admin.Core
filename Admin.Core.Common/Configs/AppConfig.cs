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

        public AopConfig Aop { get; set; }
    }

    public class AopConfig
    {
        /// <summary>
        /// 事物
        /// </summary>
        public bool Transaction { get; set; }
    }
}
