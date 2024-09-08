using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Services.Msg.Events;

/// <summary>
/// 邮箱验证码
/// </summary>
public class EmailSendCodeEvent
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
    /// 收件人
    /// </summary>
    [Required(ErrorMessage = "请输入收件人")]
    public Models.EmailModel ToEmail { get; set; }

    /// <summary>
    /// 验证码
    /// </summary>
    public string Code { get; set; }
}
