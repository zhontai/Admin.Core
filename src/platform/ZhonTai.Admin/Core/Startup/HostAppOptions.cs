using System;
namespace ZhonTai.Admin.Core.Startup
{
    /// <summary>
    /// HostApp配置
    /// </summary>
    public class HostAppOptions
    {
        /// <summary>
        /// 注入前置服务
        /// </summary>
        public Action<HostAppContext> ConfigurePreServices { get; set; }

        /// <summary>
        /// 注入服务
        /// </summary>
        public Action<HostAppContext> ConfigureServices { get; set; }

        /// <summary>
        /// 注入后置服务
        /// </summary>
        public Action<HostAppContext> ConfigurePostServices { get; set; }
    }
}
