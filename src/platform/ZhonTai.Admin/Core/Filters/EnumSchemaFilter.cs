using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using System.Reflection;
using ZhonTai.Common.Extensions;

namespace ZhonTai.Admin.Core.Filters;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        //CommonUtils.GetProperyCommentBySummary
        var type = context.Type;
        if (type.IsEnum)
        {
            var items = Enum.GetValues(type).Cast<Enum>()
            .Where(m => !m.ToString().Equals("Null")).Select(x =>
            $"{x.ToDescription()}={((int)type.InvokeMember(x.ToString(), BindingFlags.GetField, null, null, null))}").ToList();

            if (items?.Count > 0)
            {
                string description = string.Join(",", items);
                schema.Extensions.Add("extensions", new OpenApiObject
                {
                    ["description"] = new OpenApiString(description)
                });
                schema.Description = string.IsNullOrEmpty(schema.Description) ? description : $"{schema.Description}:{description}";
            }
        }
    }
}