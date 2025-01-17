namespace ZhonTai.Admin.Tools.TaskScheduler;

public class ShellArgs
{
    /// <summary>
    /// 执行应用
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 执行参数
    /// </summary>
    public string Arguments { get; set; }

    /// <summary>
    /// 命令应用工作目录
    /// </summary>
    public string WorkingDirectory { get; set; }
}
