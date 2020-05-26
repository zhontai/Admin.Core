﻿

using Admin.Core.Common.Cache;

namespace Admin.Core.Common.Configs
{
    /// <summary>
    /// 缓存配置
    /// </summary>
    public class CacheConfig
    {
        /// <summary>
        /// 缓存类型
        /// </summary>
        public CacheType Type { get; set; } = CacheType.Memory;

        /// <summary>
        /// Redis配置
        /// </summary>
        public RedisConfig Redis { get; set; } = new RedisConfig();
    }

    /// <summary>
    /// Redis配置
    /// </summary>
    public class RedisConfig
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; } = "127.0.0.1:6379,password=,defaultDatabase=2";
    }
}
