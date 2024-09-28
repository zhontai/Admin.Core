using Microsoft.Extensions.Localization;

namespace ZhonTai.Admin.Resources;

/// <summary>
/// Admin国际化
/// </summary>
public class AdminLocalizer: ModuleLocalizer
{
    public AdminLocalizer(IStringLocalizer<AdminLocalizer> localizer) : base(localizer)
    {
    }
}
