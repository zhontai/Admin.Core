


namespace Admin.Core.Repository
{
    public static class IdleBusExtesions
    {
        public static IFreeSql Get(this IdleBus<IFreeSql> ib, long tenantId)
        {
            var freeSql = ib.Get("tenant_" + tenantId.ToString());
            return freeSql;
        }
    }
}
