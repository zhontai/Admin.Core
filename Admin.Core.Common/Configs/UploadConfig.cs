namespace Admin.Core.Common.Configs
{
    /// <summary>
    /// 上传配置
    /// </summary>
    public class UploadConfig
    {
        public AvatarConfig Avatar { get; set; }
        public AvatarConfig Document { get; set; }
    }

    /// <summary>
    /// 头像配置
    /// </summary>
    public class AvatarConfig
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 请求路径
        /// </summary>
        public string RequestPath { get; set; }

        /// <summary>
        /// 路径格式
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// 路径日期格式
        /// </summary>
        public string DateTimeFormat { get; set; }

        /// <summary>
        /// 图片大小 1M = 1024 * 1024
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 图片格式
        /// </summary>
        public string[] ContentType { get; set; }
    }

    /// <summary>
    /// 文档配置
    /// </summary>
    public class DocumentConfig
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 请求路径
        /// </summary>
        public string RequestPath { get; set; }

        /// <summary>
        /// 路径格式
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 路径日期格式
        /// </summary>
        public string DateTimeFormat { get; set; }

        /// <summary>
        /// 文件大小 10M = 10 * 1024 * 1024
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 文件格式
        /// </summary>
        public string[] ContentType { get; set; }
    }
}
