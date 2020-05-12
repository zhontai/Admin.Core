﻿using System;

namespace Admin.Core.Common.BaseModel
{
    public interface IEntityUpdate<TKey> where TKey : struct
    {
        TKey? ModifiedUserId { get; set; }
        string ModifiedUserName { get; set; }
        DateTime? ModifiedTime { get; set; }
    }
}
