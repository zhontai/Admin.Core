using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Services.Msg.Events;

/// <summary>
/// 邮件单发
/// </summary>
public class EmailSingleSendEvent
{
    public static class Models 
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public class EmailModel
        {
            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 地址
            /// </summary>
            public string Address { get; set; }
        }
    }

    /// <summary>
    /// 发件人
    /// </summary>
    public Models.EmailModel? FromEmail { get; set; }

    /// <summary>
    /// 收件人
    /// </summary>
    [Required(ErrorMessage = "请输入收件人")]
    public Models.EmailModel ToEmail { get; set; }

    /// <summary>
    /// 邮件标题
    /// </summary>
    [Required(ErrorMessage = "请输入邮件标题")]
    public string Subject { get; set; }

    /// <summary>
    /// 邮件内容
    /// </summary>
    [Required(ErrorMessage = "请输入邮件内容")]
    public string Body { get; set; }
}
