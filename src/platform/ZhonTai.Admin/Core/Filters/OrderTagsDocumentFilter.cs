using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Concurrent;
using System.Linq;
using ZhonTai.DynamicApi.Attributes;

namespace ZhonTai.Admin.Core.Filters;

/// <summary>
/// 接口排序文档过滤器
/// </summary>
public class OrderTagsDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var orderTagList = new ConcurrentDictionary<string, int>();
        foreach (var apiDescription in context.ApiDescriptions)
        {
            var order = 0;
            var actionDescriptor = apiDescription.ActionDescriptor as ControllerActionDescriptor;
            var objOrderAttribute = actionDescriptor.EndpointMetadata.FirstOrDefault(x => x is OrderAttribute);
            if (objOrderAttribute != null)
            {
                var orderAttribute = objOrderAttribute as OrderAttribute;
                order = orderAttribute.Value;
            }
            orderTagList.TryAdd(actionDescriptor.ControllerName, order);
        }

        swaggerDoc.Tags = swaggerDoc.Tags
                                    .OrderBy(u => orderTagList.TryGetValue(u.Name, out int order) ? order : 0)
                                    .ThenBy(u => u.Name)
                                    .ToArray();
    }
}