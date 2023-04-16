using System.Linq;
using System.IO;
using System.Text.Json;
using FreeSql.DataAnnotations;
using ZhonTai.Common.Helpers;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using ZhonTai.Admin.Core.Attributes;
using System.Text.Json.Serialization.Metadata;
using ZhonTai.Admin.Core.Entities;

namespace ZhonTai.Admin.Core.Db.Data;

public abstract class GenerateData
{
    private readonly string _tenantName = InterfaceHelper.GetPropertyNames<ITenant>().FirstOrDefault()?.ToLower();

    protected virtual void IgnorePropName(JsonTypeInfo ti, bool isTenant)
    {
        foreach(var jsonPropertyInfo in ti.Properties)
        {
            jsonPropertyInfo.ShouldSerialize = (obj, _) =>
            {
                if (jsonPropertyInfo.Name.ToLower() == _tenantName && EntityHelper.IsImplementInterface(ti.Type, typeof(ITenant)))
                {
                    return isTenant;
                }

                return !jsonPropertyInfo.AttributeProvider.IsDefined(typeof(NotGenAttribute), false);
            };
        }
    }

    protected virtual void SaveDataToJsonFile<T>(object data, bool isTenant = false, string path = "InitData/Admin") where T : class, new()
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create(new TextEncoderSettings(UnicodeRanges.All)),
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers = { (JsonTypeInfo ti) => IgnorePropName(ti, isTenant) }
            }
        };

        var table = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() as TableAttribute;
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), $"{path}/{table.Name}{(isTenant ? ".tenant" : "")}.json").ToPath();

        var jsonData = JsonSerializer.Serialize(data, jsonSerializerOptions);

        FileHelper.WriteFile(filePath, jsonData);
    }
}
