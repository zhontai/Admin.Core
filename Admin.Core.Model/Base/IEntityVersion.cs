using FreeSql.DataAnnotations;
using Newtonsoft.Json;

namespace Admin.Core.Model
{
    public interface IEntityVersion
    {
        /// <summary>
        /// 版本
        /// </summary>
        long Version { get; set; }
    }
}
