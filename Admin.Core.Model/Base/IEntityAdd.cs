using System;

namespace Admin.Core.Model
{
    public interface IEntityAdd
    {
        long? CreatedUserId { get; set; }
        string CreatedUserName { get; set; }
        DateTime? CreatedTime { get; set; }
    }
}
