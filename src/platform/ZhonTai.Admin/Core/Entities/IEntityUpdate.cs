using System;

namespace ZhonTai.Admin.Core.Entities;

public interface IEntityUpdate<TKey> where TKey : struct
{
    long? ModifiedUserId { get; set; }
    string ModifiedUserName { get; set; }
    DateTime? ModifiedTime { get; set; }
}