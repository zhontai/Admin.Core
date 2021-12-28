using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using ZhonTai.Tools.DynamicApi.Attributes;
using ZhonTai.Tools.DynamicApi.Helpers;

namespace ZhonTai.Tools.DynamicApi
{
    public class DynamicApiConvention : IApplicationModelConvention
    {
        private readonly ISelectController _selectController;
        private readonly IActionRouteFactory _actionRouteFactory;

        public DynamicApiConvention(ISelectController selectController, IActionRouteFactory actionRouteFactory)
        {
            _selectController = selectController;
            _actionRouteFactory = actionRouteFactory;
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var type = controller.ControllerType.AsType();
                var DynamicApiAttr = ReflectionHelper.GetSingleAttributeOrDefaultByFullSearch<DynamicApiAttribute>(type.GetTypeInfo());

                if (!(_selectController is DefaultSelectController) && _selectController.IsController(type))
                {
                    controller.ControllerName = controller.ControllerName.RemovePostFix(AppConsts.ControllerPostfixes.ToArray());
                    if (AppConsts.PascalToKebabCase)
                    {
                        controller.ControllerName = PascalToKebabCase(controller.ControllerName);
                    }
                    else
                    {
                        controller.ControllerName = GetRestFulControllerName(controller.ControllerName);
                    }
                    
                    ConfigureDynamicApi(controller, DynamicApiAttr);
                }
                else
                {
                    if (typeof(IDynamicApi).GetTypeInfo().IsAssignableFrom(type))
                    {
                        controller.ControllerName = controller.ControllerName.RemovePostFix(AppConsts.ControllerPostfixes.ToArray());
                        if (AppConsts.PascalToKebabCase)
                        {
                            controller.ControllerName = PascalToKebabCase(controller.ControllerName);
                        }
                        else
                        {
                            controller.ControllerName = GetRestFulControllerName(controller.ControllerName);
                        }
                        ConfigureArea(controller, DynamicApiAttr);
                        ConfigureDynamicApi(controller, DynamicApiAttr);
                    }
                    else
                    {
                        if (DynamicApiAttr != null)
                        {
                            ConfigureArea(controller, DynamicApiAttr);
                            ConfigureDynamicApi(controller, DynamicApiAttr);
                        }
                    }
                }
            }
        }

        public string PascalToKebabCase(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            return Regex.Replace(
                value,
                "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
                "-$1",
                RegexOptions.Compiled)
                .Trim()
                .ToLower();
        }

        private void ConfigureArea(ControllerModel controller, DynamicApiAttribute attr)
        {
            if (!controller.RouteValues.ContainsKey("area"))
            {
                if (attr == null)
                {
                    throw new ArgumentException(nameof(attr));
                }

                if (!string.IsNullOrEmpty(attr.Area))
                {
                    controller.RouteValues["area"] = attr.Area;
                }
                else if (!string.IsNullOrEmpty(AppConsts.DefaultAreaName))
                {
                    controller.RouteValues["area"] = AppConsts.DefaultAreaName;
                }
            }

        }

        private void ConfigureDynamicApi(ControllerModel controller, DynamicApiAttribute controllerAttr)
        {
            ConfigureApiExplorer(controller);
            ConfigureSelector(controller, controllerAttr);
            ConfigureParameters(controller);
        }


        private void ConfigureParameters(ControllerModel controller)
        {
            foreach (var action in controller.Actions)
            {
                if (!CheckNoMapMethod(action))
                    foreach (var para in action.Parameters)
                    {
                        if (para.BindingInfo != null)
                        {
                            continue;
                        }

                        if (!TypeHelper.IsPrimitiveExtendedIncludingNullable(para.ParameterInfo.ParameterType))
                        {
                            if (CanUseFormBodyBinding(action, para))
                            {
                                para.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromBodyAttribute() });
                            }
                        }
                    }
            }
        }


        private bool CanUseFormBodyBinding(ActionModel action, ParameterModel parameter)
        {
            if (AppConsts.FormBodyBindingIgnoredTypes.Any(t => t.IsAssignableFrom(parameter.ParameterInfo.ParameterType)))
            {
                return false;
            }

            foreach (var selector in action.Selectors)
            {
                if (selector.ActionConstraints == null)
                {
                    continue;
                }

                foreach (var actionConstraint in selector.ActionConstraints)
                {

                    var httpMethodActionConstraint = actionConstraint as HttpMethodActionConstraint;
                    if (httpMethodActionConstraint == null)
                    {
                        continue;
                    }

                    if (httpMethodActionConstraint.HttpMethods.All(hm => hm.IsIn("GET", "DELETE", "TRACE", "HEAD")))
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        #region ConfigureApiExplorer

        private void ConfigureApiExplorer(ControllerModel controller)
        {
            if (controller.ApiExplorer.GroupName.IsNullOrEmpty())
            {
                controller.ApiExplorer.GroupName = controller.ControllerName;
            }

            if (controller.ApiExplorer.IsVisible == null)
            {
                controller.ApiExplorer.IsVisible = true;
            }

            foreach (var action in controller.Actions)
            {
                if (!CheckNoMapMethod(action))
                    ConfigureApiExplorer(action);
            }
        }

        private void ConfigureApiExplorer(ActionModel action)
        {
            if (action.ApiExplorer.IsVisible == null)
            {
                action.ApiExplorer.IsVisible = true;
            }
        }

        #endregion
        /// <summary>
        /// //不映射指定的方法
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private bool CheckNoMapMethod(ActionModel action)
        {
            bool isExist = false;
            var noMapMethod = ReflectionHelper.GetSingleAttributeOrDefault<NonDynamicMethodAttribute>(action.ActionMethod);

            if (noMapMethod != null)
            {
                action.ApiExplorer.IsVisible = false;//对应的Api不映射
                isExist = true;
            }

            return isExist;
        }
        private void ConfigureSelector(ControllerModel controller, DynamicApiAttribute controllerAttr)
        {

            if (controller.Selectors.Any(selector => selector.AttributeRouteModel != null))
            {
                return;
            }

            var areaName = string.Empty;

            if (controllerAttr != null)
            {
                areaName = controllerAttr.Area;
            }

            foreach (var action in controller.Actions)
            {
                if (!CheckNoMapMethod(action))
                    ConfigureSelector(areaName, controller.ControllerName, action);
            }
        }

        private void ConfigureSelector(string areaName, string controllerName, ActionModel action)
        {

            var nonAttr = ReflectionHelper.GetSingleAttributeOrDefault<NonDynamicApiAttribute>(action.ActionMethod);

            if (nonAttr != null)
            {
                return;
            }

            if (action.Selectors.IsNullOrEmpty() || action.Selectors.Any(a => a.ActionConstraints.IsNullOrEmpty()))
            {
                if (!CheckNoMapMethod(action))
                    AddAppServiceSelector(areaName, controllerName, action);
            }
            else
            {
                NormalizeSelectorRoutes(areaName, controllerName, action);
            }
        }

        private void AddAppServiceSelector(string areaName, string controllerName, ActionModel action)
        {

            var verb = GetHttpVerb(action);
            if (AppConsts.PascalToKebabCase)
            {
                action.ActionName = PascalToKebabCase(action.ActionName);
            }
            else
            {
                action.ActionName = GetRestFulActionName(action.ActionName);
            }

            var appServiceSelectorModel = action.Selectors[0];

            if (appServiceSelectorModel.AttributeRouteModel == null)
            {
                appServiceSelectorModel.AttributeRouteModel = CreateActionRouteModel(areaName, controllerName, action);
            }

            if (!appServiceSelectorModel.ActionConstraints.Any())
            {
                appServiceSelectorModel.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { verb }));
                switch (verb)
                {
                    case "GET":
                        appServiceSelectorModel.EndpointMetadata.Add(new HttpGetAttribute());
                        break;
                    case "POST":
                        appServiceSelectorModel.EndpointMetadata.Add(new HttpPostAttribute());
                        break;
                    case "PUT":
                        appServiceSelectorModel.EndpointMetadata.Add(new HttpPutAttribute());
                        break;
                    case "DELETE":
                        appServiceSelectorModel.EndpointMetadata.Add(new HttpDeleteAttribute());
                        break;
                    default:
                        throw new Exception($"Unsupported http verb: {verb}.");
                }
            }


        }



        /// <summary>
        /// Processing action name
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        private static string GetRestFulActionName(string actionName)
        {
            // custom process action name
            var appConstsActionName = AppConsts.GetRestFulActionName?.Invoke(actionName);
            if (appConstsActionName != null)
            {
                return appConstsActionName;
            }

            // default process action name.

            // Remove Postfix
            actionName = actionName.RemovePostFix(AppConsts.ActionPostfixes.ToArray());

            // Remove Prefix
            var verbKey = actionName.GetPascalOrCamelCaseFirstWord().ToLower();
            if (AppConsts.HttpVerbs.ContainsKey(verbKey))
            {
                if (actionName.Length == verbKey.Length)
                {
                    return "";
                }
                else
                {
                    return actionName.Substring(verbKey.Length);
                }
            }
            else
            {
                return actionName;
            }
        }

        private static string GetRestFulControllerName(string controllerName)
        {
            // custom process action name
            var appConstsControllerName = AppConsts.GetRestFulControllerName?.Invoke(controllerName);
            if (appConstsControllerName != null)
            {
                return appConstsControllerName;
            }
            else
            {
                return controllerName;
            }
        }

        private void NormalizeSelectorRoutes(string areaName, string controllerName, ActionModel action)
        {
            if (AppConsts.PascalToKebabCase)
            {
                action.ActionName = PascalToKebabCase(action.ActionName);
            }
            else
            {
                action.ActionName = GetRestFulActionName(action.ActionName);
            }
            
            foreach (var selector in action.Selectors)
            {
                selector.AttributeRouteModel = selector.AttributeRouteModel == null ?
                     CreateActionRouteModel(areaName, controllerName, action) :
                     AttributeRouteModel.CombineAttributeRouteModel(CreateActionRouteModel(areaName, controllerName, action), selector.AttributeRouteModel);
            }
        }

        private static string GetHttpVerb(ActionModel action)
        {
            var getValueSuccess = AppConsts.AssemblyDynamicApiOptions
                .TryGetValue(action.Controller.ControllerType.Assembly, out AssemblyDynamicApiOptions assemblyDynamicApiOptions);
            if (getValueSuccess && !string.IsNullOrWhiteSpace(assemblyDynamicApiOptions?.HttpVerb))
            {
                return assemblyDynamicApiOptions.HttpVerb;
            }


            var verbKey = action.ActionName.GetPascalOrCamelCaseFirstWord().ToLower();

            var verb = AppConsts.HttpVerbs.ContainsKey(verbKey) ? AppConsts.HttpVerbs[verbKey] : AppConsts.DefaultHttpVerb;
            return verb;
        }

        private AttributeRouteModel CreateActionRouteModel(string areaName, string controllerName, ActionModel action)
        {
            var route =  _actionRouteFactory.CreateActionRouteModel(areaName, controllerName, action);

            return new AttributeRouteModel(new RouteAttribute(route));
        }
    }
}