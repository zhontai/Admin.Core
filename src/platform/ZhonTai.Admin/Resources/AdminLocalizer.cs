using Microsoft.Extensions.Localization;
using ZhonTai.Admin.Core.Attributes;

namespace ZhonTai.Admin.Resources;

/// <summary>
/// Admin国际化
/// </summary>
[InjectSingleton]
public class AdminLocalizer: ModuleLocalizer
{
    public AdminLocalizer(IStringLocalizer<AdminLocalizer> localizer) : base(localizer)
    {
    }
}
