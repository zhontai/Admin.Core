﻿using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Services.DictType.Dto;

namespace ZhonTai.Admin.Services.DictType;

/// <summary>
/// 数据字典类型接口
/// </summary>
public partial interface IDictTypeService
{
    Task<DictTypeGetOutput> GetAsync(long id);

    Task<PageOutput<DictTypeGetPageOutput>> GetPageAsync(PageInput<DictTypeGetPageInput> input);

    Task<long> AddAsync(DictTypeAddInput input);

    Task UpdateAsync(DictTypeUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}