namespace ZhonTai.DynamicApi;

/// <summary>
/// Specifies the dynamic webapi options for the assembly.
/// </summary>
public class AssemblyDynamicApiOptions
{
    /// <summary>
    /// Routing prefix for all APIs
    /// <para></para>
    /// Default value is null.
    /// </summary>
    public string ApiPrefix { get; }

    /// <summary>
    /// API HTTP Verb.
    /// <para></para>
    /// Default value is null.
    /// </summary>
    public string HttpVerb { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="apiPrefix">Routing prefix for all APIs</param>
    /// <param name="httpVerb">API HTTP Verb.</param>
    public AssemblyDynamicApiOptions(string apiPrefix = null, string httpVerb = null)
    {
        ApiPrefix = apiPrefix;
        HttpVerb = httpVerb;
    }
}