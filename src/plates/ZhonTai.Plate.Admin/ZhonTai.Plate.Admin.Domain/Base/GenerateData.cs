using System;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using ZhonTai.Common.Helpers;
using ZhonTai.Common.Domain.Db;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;

namespace ZhonTai.Plate.Admin.Domain
{
    public abstract class GenerateData
    {
        protected virtual void SaveDataToJsonFile<T>(object data, bool isTenant = false, string path = "InitData/Admin", PropsContractResolver propsContractResolver = null) where T : class, new()
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = propsContractResolver != null ? propsContractResolver : (isTenant ? new CamelCasePropertyNamesContractResolver() : new PropsContractResolver(new List<string> { "TenantId" }));
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;

            var table = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() as TableAttribute;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), $"{path}/{table.Name}{(isTenant ? ".tenant" : "")}.json").ToPath();
            var jsonData = JsonConvert.SerializeObject(data, Formatting.Indented, settings);
            FileHelper.WriteFile(filePath, jsonData);
        }
    }
}
