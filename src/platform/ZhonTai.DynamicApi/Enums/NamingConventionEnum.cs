namespace ZhonTai.DynamicApi.Enums;

public enum NamingConventionEnum
{
    /// <summary>
    /// camelCase
    /// </summary>
    CamelCase,
    /// <summary>
    /// PascalCase
    /// </summary>
    PascalCase,
    /// <summary>
    /// snake_case
    /// </summary>
    SnakeCase,
    /// <summary>
    /// kebab-case
    /// </summary>
    KebabCase,
    /// <summary>
    /// extension.case
    /// </summary>
    ExtensionCase,
    /// <summary>
    /// Customize with GetRestFulControllerName and GetRestFulActionName method 
    /// </summary>
    Custom
}
