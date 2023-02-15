using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using ZhonTai.DynamicApi.Enums;

namespace ZhonTai.DynamicApi;

public class DynamicApiOptions
{
    public DynamicApiOptions()
    {
        RemoveControllerPostfixes = new List<string>() { "AppService", "ApplicationService", "ApiController", "Controller", "Services", "Service" };
        RemoveActionPostfixes = new List<string>() { "Async" };
        FormBodyBindingIgnoredTypes = new List<Type>() { typeof(IFormFile) };
        DefaultHttpVerb = "POST";
        DefaultApiPrefix = "api";
        AssemblyDynamicApiOptions = new Dictionary<Assembly, AssemblyDynamicApiOptions>();
    }


    /// <summary>
    /// API HTTP Verb.
    /// <para></para>
    /// Default value is "POST".
    /// </summary>
    public string DefaultHttpVerb { get; set; }

    public string DefaultAreaName { get; set; }

    /// <summary>
    /// Routing prefix for all APIs
    /// <para></para>
    /// Default value is "api".
    /// </summary>
    public string DefaultApiPrefix { get; set; }

    /// <summary>
    /// Remove the dynamic API class(Controller) name postfix.
    /// <para></para>
    /// Default value is {"AppService", "ApplicationService"}.
    /// </summary>
    public List<string> RemoveControllerPostfixes { get; set; }

    /// <summary>
    /// Remove the dynamic API class's method(Action) postfix.
    /// <para></para>
    /// Default value is {"Async"}.
    /// </summary>
    public List<string> RemoveActionPostfixes { get; set; }

    /// <summary>
    /// Ignore MVC Form Binding types.
    /// </summary>
    public List<Type> FormBodyBindingIgnoredTypes { get; set; }

    /// <summary>
    /// Naming convention
    /// </summary>
    public NamingConventionEnum NamingConvention { get; set; } = NamingConventionEnum.KebabCase;

    /// <summary>
    /// The method that processing the name of the action.
    /// </summary>
    public Func<string, string> GetRestFulActionName { get; set; }

    /// <summary>
    /// The method that processing the name of the controller.
    /// </summary>
    public Func<string, string> GetRestFulControllerName { get; set; }

    /// <summary>
    /// Specifies the dynamic webapi options for the assembly.
    /// </summary>
    public Dictionary<Assembly, AssemblyDynamicApiOptions> AssemblyDynamicApiOptions { get; }

    public ISelectController SelectController { get; set; } = new DefaultSelectController();
    public IActionRouteFactory ActionRouteFactory { get; set; } = new DefaultActionRouteFactory();

    public bool FormatResult { get; set; } = true;

    public Type FormatResultType { get; set; } = FormatResultContext.FormatResultType;

    /// <summary>
    /// Verify that all configurations are valid
    /// </summary>
    public void Valid()
    {
        if (string.IsNullOrEmpty(DefaultHttpVerb))
        {
            throw new ArgumentException($"{nameof(DefaultHttpVerb)} can not be empty.");
        }

        if (string.IsNullOrEmpty(DefaultAreaName))
        {
            DefaultAreaName = string.Empty;
        }

        if (string.IsNullOrEmpty(DefaultApiPrefix))
        {
            DefaultApiPrefix = string.Empty;
        }

        if (FormBodyBindingIgnoredTypes == null)
        {
            throw new ArgumentException($"{nameof(FormBodyBindingIgnoredTypes)} can not be null.");
        }

        if (RemoveControllerPostfixes == null)
        {
            throw new ArgumentException($"{nameof(RemoveControllerPostfixes)} can not be null.");
        }
    }

    /// <summary>
    /// Add the dynamic webapi options for the assembly.
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="apiPreFix"></param>
    /// <param name="httpVerb"></param>
    public void AddAssemblyOptions(Assembly assembly, string apiPreFix = null, string httpVerb = null)
    {
        if (assembly == null)
        {
            throw new ArgumentException($"{nameof(assembly)} can not be null.");
        }

        this.AssemblyDynamicApiOptions[assembly] = new AssemblyDynamicApiOptions(apiPreFix, httpVerb);
    }

    /// <summary>
    /// Add the dynamic webapi options for the assemblies.
    /// </summary>
    /// <param name="assemblies"></param>
    /// <param name="apiPreFix"></param>
    /// <param name="httpVerb"></param>
    public void AddAssemblyOptions(Assembly[] assemblies, string apiPreFix = null, string httpVerb = null)
    {
        if (assemblies == null)
        {
            throw new ArgumentException($"{nameof(assemblies)} can not be null.");
        }

        foreach (var assembly in assemblies)
        {
            AddAssemblyOptions(assembly, apiPreFix, httpVerb);
        }
    }
}