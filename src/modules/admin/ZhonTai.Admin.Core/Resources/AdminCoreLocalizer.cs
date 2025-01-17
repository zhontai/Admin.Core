using Microsoft.Extensions.Localization;
using ZhonTai.Admin.Core.Attributes;

namespace ZhonTai.Admin.Resources;

/// <summary>
/// Admin国际化
/// </summary>
[InjectSingleton]
public class AdminCoreLocalizer: ModuleLocalizer
{
    public AdminCoreLocalizer(IStringLocalizer<AdminCoreLocalizer> localizer) : base(localizer)
    {
    }
}
