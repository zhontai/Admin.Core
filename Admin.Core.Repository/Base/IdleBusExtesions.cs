using Admin.Core.Common.Configs;
using Admin.Core.Common.Consts;

namespace Admin.Core.Repository
{
    public static class IdleBusExtesions
    {
        public static IFreeSql Get(this IdleBus<IFreeSql> ib, long tenantId, AppConfig appConfig)
        {
            var tenantName = AdminConsts.TenantName;
            if (appConfig.TenantType == TenantType.Own)
            {
                tenantName = "tenant_" + tenantId.ToString();
            }
            var freeSql = ib.Get(tenantName);
            return freeSql;
        }
    }
}
