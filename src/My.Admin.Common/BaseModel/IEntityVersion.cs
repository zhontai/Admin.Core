namespace My.Admin.Common.BaseModel
{
    public interface IEntityVersion
    {
        /// <summary>
        /// 版本
        /// </summary>
        long Version { get; set; }
    }
}