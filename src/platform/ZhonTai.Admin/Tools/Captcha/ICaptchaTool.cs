using System.Threading.Tasks;

namespace ZhonTai.Admin.Tools.Captcha;

/// <summary>
/// 验证接口
/// </summary>
public interface ICaptchaTool
{
    /// <summary>
    /// 获得验证数据
    /// </summary>
    /// <returns></returns>
    Task<CaptchaOutput> GetAsync(string captchaKey);

    /// <summary>
    /// 检查验证数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> CheckAsync(CaptchaInput input);
}