using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using ZhonTai.Tools.DynamicApi.Attributes;
using ZhonTai.Tools.DynamicApi.Helpers;

namespace ZhonTai.Tools.DynamicApi
{
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
}