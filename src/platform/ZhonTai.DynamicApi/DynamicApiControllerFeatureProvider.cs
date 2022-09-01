using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace ZhonTai.DynamicApi;

public class DynamicApiControllerFeatureProvider: ControllerFeatureProvider
{
    private ISelectController _selectController;

    public DynamicApiControllerFeatureProvider(ISelectController selectController)
    {
        _selectController = selectController;
    }

    protected override bool IsController(TypeInfo typeInfo)
    {
        return _selectController.IsController(typeInfo);
    }
}