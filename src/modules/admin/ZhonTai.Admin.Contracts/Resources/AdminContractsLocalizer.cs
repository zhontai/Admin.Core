using Microsoft.Extensions.Localization;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Resources;

namespace ZhonTai.Admin.Contracts.Resources;

/// <summary>
/// Admin契约库国际化
/// </summary>
[InjectSingleton]
public class AdminContractsLocalizer : ModuleLocalizer
{
    public AdminContractsLocalizer(IStringLocalizer<AdminContractsLocalizer> localizer) : base(localizer)
    {
    }
}
