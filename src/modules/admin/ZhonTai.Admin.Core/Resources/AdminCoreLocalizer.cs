using Microsoft.Extensions.Localization;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Resources;

namespace ZhonTai.Admin.Core.Resources;

/// <summary>
/// Admin核心库国际化
/// </summary>
[InjectSingleton]
public class AdminCoreLocalizer: ModuleLocalizer
{
    public AdminCoreLocalizer(IStringLocalizer<AdminCoreLocalizer> localizer) : base(localizer)
    {
    }
}
