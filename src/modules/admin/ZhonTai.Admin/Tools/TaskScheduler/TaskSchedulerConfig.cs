namespace ZhonTai.Admin.Tools.TaskScheduler;

/// <summary>
/// 任务调度配置
/// </summary>
public class TaskSchedulerConfig
{
    public static class Models
    {
        /// <summary>
        /// 告警邮件
        /// </summary>
        public class AlerEmail
        {
            /// <summary>
            /// 是否启用
            /// </summary>
            public bool Enable { get; set; }

            /// <summary>
            /// 邮件地址
            /// </summary>
            public string Adress { get; set; }
        }

        /// <summary>
        /// 进程启动信息
        /// </summary>
        public class ProcessStartInfoModel
        {
            /// <summary>
            /// 应用程序
            /// </summary>
            public string FileName { get; set; }

            /// <summary>
            /// 工作目录
            /// </summary>
            public string WorkingDirectory { get; set; }
        }
    }

    /// <summary>
    /// 进程启动信息
    /// </summary>
    public Models.ProcessStartInfoModel ProcessStartInfo { get; set; }

    /// <summary>
    /// 告警邮件
    /// </summary>
    public Models.AlerEmail AlerEmail { get; set; }
}