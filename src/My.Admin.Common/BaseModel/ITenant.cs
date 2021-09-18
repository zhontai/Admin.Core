namespace My.Admin.Common.BaseModel
{
    public interface ITenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        long? TenantId { get; set; }
    }
}