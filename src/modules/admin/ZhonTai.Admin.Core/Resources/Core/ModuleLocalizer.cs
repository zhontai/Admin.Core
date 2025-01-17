using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace ZhonTai.Admin.Resources;

public abstract class ModuleLocalizer : IModuleLocalizer
{
    private readonly IStringLocalizer _localizer;

    protected ModuleLocalizer(IStringLocalizer localizer)
    {
        _localizer = localizer;
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        return _localizer.GetAllStrings(includeParentCultures);
    }

    public LocalizedString this[string name] => _localizer[name];

    public LocalizedString this[string name, params object[] arguments] => _localizer[name, arguments];
}