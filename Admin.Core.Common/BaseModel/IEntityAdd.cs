using System;

namespace Admin.Core.Common.BaseModel
{
    public interface IEntityAdd<TKey> where TKey: struct
    {
        long? CreatedUserId { get; set; }
        string CreatedUserName { get; set; }
        DateTime? CreatedTime { get; set; }
    }
}
