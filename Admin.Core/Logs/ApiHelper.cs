using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Admin.Core.Common.Helpers;
using Admin.Core.Db;
using Admin.Core.Common.Attributes;


namespace Admin.Core.Logs
{
    /// <summary>
    /// Api帮助类
    /// </summary>
    [SingleInstance]
    public class ApiHelper
    {
        private List<ApiHelperDto> _apis;
        private static readonly object _lockObject = new object();

        public List<ApiHelperDto> GetApis()
        {
            if (_apis != null && _apis.Any())
                    return _apis;

            lock (_lockObject)
            {
                if (_apis != null && _apis.Any())
                    return _apis;

                _apis = new List<ApiHelperDto>();
                var filePath = Path.Combine(AppContext.BaseDirectory, "Db/Data/data.json").ToPath();
                var jsonData = FileHelper.ReadFile(filePath);
                var apis = JsonConvert.DeserializeObject<Data>(jsonData).Apis;
                foreach (var api in apis)
                {
                    var parentLabel = apis.FirstOrDefault(a => a.Id == api.ParentId)?.Label;

                    _apis.Add(new ApiHelperDto
                    {
                        Label = parentLabel.NotNull() ? $"{parentLabel} / {api.Label}" : api.Label,
                        Path = api.Path?.ToLower().Trim('/')
                    });
                }

                return _apis;
            }
        }
    }

    public class ApiHelperDto
    {
        /// <summary>
        /// 接口名称
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 接口地址
        /// </summary>
        public string Path { get; set; }
    }
}
