using System;

namespace Admin.Core.Model
{
    public interface IEntityUpdate
    {
        long? ModifiedUserId { get; set; }
        string ModifiedUserName { get; set; }
        DateTime? ModifiedTime { get; set; }
    }
}
