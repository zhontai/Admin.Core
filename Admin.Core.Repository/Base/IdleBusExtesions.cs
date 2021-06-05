using Admin.Core.Common.Configs;
using Admin.Core.Common.Consts;

namespace Admin.Core.Repository
{
    public static class IdleBusExtesions
    {
        public static IFreeSql GetTenant(this IdleBus<IFreeSql> ib, long? tenantId, AppConfig appConfig)
        {
            var tenantName = AdminConsts.TenantName;
            //需要查询租户数据库类型
            //if (appConfig.TenantDbType == TenantDbType.Own)
            //{
            //    tenantName = "tenant_" + tenantId?.ToString();
            //}
            var freeSql = ib.Get(tenantName);
            return freeSql;
        }
    }
}
