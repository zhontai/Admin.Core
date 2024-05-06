using System.Collections.Generic;

namespace ZhonTai.Admin.Services.Region;

public class RegionGetOutput : RegionUpdateInput
{
    public List<long> ParentIdList { get; set; }
}