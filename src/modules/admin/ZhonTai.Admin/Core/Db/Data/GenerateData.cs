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
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Domain.Tenant;
using System.Threading.Tasks;

namespace ZhonTai.Admin.Core.Db.Data;

public abstract class GenerateData
{
    private readonly string _tenantName = InterfaceHelper.GetPropertyNames<ITenant>().FirstOrDefault()?.ToLower();

    protected virtual void IgnorePropName(JsonTypeInfo ti, bool isTenant)
    {
        foreach (var jsonPropertyInfo in ti.Properties)
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

    /// <summary>
    /// 保存实体数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="db"></param>
    /// <param name="appConfig">应用配置</param>
    /// <param name="outPath">输出路径 InitData/xxx </param>
    /// <returns></returns>
    protected virtual async Task SaveEntityAsync<T>(IFreeSql db, AppConfig appConfig, string outPath) where T : EntityBase, new()
    {
        var modules = await db.Queryable<T>().ToListAsync();
        //是否多租户
        var isTenant = appConfig.Tenant && typeof(T).IsAssignableFrom(typeof(EntityTenant));
        if (isTenant)
        {
            var tenantIds = await db.Queryable<TenantEntity>().ToListAsync(a => a.Id);
            SaveDataToJsonFile<T>(modules.Where(a => (a as EntityTenant).TenantId > 0 && tenantIds.Contains((a as EntityTenant).TenantId.Value)), isTenant, outPath);
        }

        SaveDataToJsonFile<T>(modules, isTenant, outPath);
    }
}
