using Microsoft.AspNetCore.Mvc;
using AngleSharp;
using ZhonTai.Admin.Core.Consts;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Region;
using ZhonTai.Admin.Repositories;
using ZhonTai.Admin.Resources;
using ZhonTai.Common.Extensions;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using Yitter.IdGenerator;
using ToolGood.Words.Pinyin;
using Flurl.Http;
using System.Text;
using AngleSharp.Dom;
using System.Text.RegularExpressions;
using ZhonTai.Common.Helpers;

namespace ZhonTai.Admin.Services.Region;

/// <summary>
/// 地区服务
/// </summary>
[Order(220)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class RegionService : BaseService, IDynamicApi
{
    private readonly AdminRepositoryBase<RegionEntity> _regionRep;
    private readonly AdminLocalizer _adminLocalizer;

    public RegionService(AdminRepositoryBase<RegionEntity> regionRep, AdminLocalizer adminLocalizer)
    {
        _regionRep = regionRep;
        _adminLocalizer = adminLocalizer;
    }

    [NonAction]
    public async Task<List<long>> GetParentIdListAsync(long id)
    {
        var regionRep = _regionRep;
        using var _ = regionRep.DataFilter.DisableAll();

        return await regionRep.Select
        .Where(a => a.Id == id)
        .AsTreeCte(up: true)
        .ToListAsync(a => a.Id);
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<RegionGetOutput> GetAsync(long id)
    {
        var regionRep = _regionRep;
        using var _ = regionRep.DataFilter.DisableAll();

        var ouput =  await regionRep.Select
        .WhereDynamic(id)
        .ToOneAsync<RegionGetOutput>();

        ouput.ParentIdList = await GetParentIdListAsync(ouput.ParentId);

        return ouput;
    }

    /// <summary>
    /// 查询下级列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<RegionGetChildListOutput>> GetChildListAsync(RegionGetListInput input)
    {
        var regionRep = _regionRep;
        using var _ = regionRep.DataFilter.DisableAll();

        return await regionRep.Select
        .Where(a => a.ParentId == input.ParentId)
        .WhereIf(input.Hot.HasValue, a => a.Hot == input.Hot.Value)
        .WhereIf(input.Enabled.HasValue, a => a.Enabled == input.Enabled.Value)
        .OrderBy(a => a.Level)
        .OrderByDescending(a => a.Hot)
        .OrderBy(a => new { a.Sort, a.Code })
        .ToListAsync<RegionGetChildListOutput>();
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<RegionGetPageOutput>> GetPageAsync(PageInput<RegionGetPageInput> input)
    {
        var regionRep = _regionRep;
        using var _ = regionRep.DataFilter.DisableAll();

        var filter = input.Filter;

        var list = await regionRep.Select
        .WhereIf(filter != null && filter.Name.NotNull(), a => a.Name.Contains(filter.Name))
        .WhereIf(filter != null && filter.ParentId.HasValue, a => a.ParentId == filter.ParentId.Value)
        .WhereIf(filter != null && filter.Level.HasValue, a => a.Level == filter.Level.Value)
        .WhereIf(filter != null && filter.Hot.HasValue, a => a.Hot == filter.Hot.Value)
        .WhereIf(filter != null && filter.Enabled.HasValue, a => a.Enabled == filter.Enabled.Value)
        .Count(out var total)
        .Page(input.CurrentPage, input.PageSize)
        .OrderBy(a => a.Level)
        .OrderByDescending(a => a.Hot)
        .OrderBy(a => new { a.Sort, a.Code })
        .ToListAsync<RegionGetPageOutput>();

        var data = new PageOutput<RegionGetPageOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(RegionAddInput input)
    {
        var regionRep = _regionRep;
        using var _ = regionRep.DataFilter.DisableAll();

        if (await regionRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["此地区名已存在"]);
        }

        if (input.Code.NotNull() && await regionRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["此地区代码已存在"]);
        }

        var entity = Mapper.Map<RegionEntity>(input);
        entity.Pinyin = WordsHelper.GetPinyin(entity.Name);
        entity.PinyinFirst = WordsHelper.GetFirstPinyin(entity.Name);

        await regionRep.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(RegionUpdateInput input)
    {
        var regionRep = _regionRep;
        using var _ = regionRep.DataFilter.DisableAll();

        var entity = await regionRep.GetAsync(input.Id);
        if (entity == null)
        {
            throw ResultOutput.Exception(_adminLocalizer["地区不存在"]);
        }

        if (await regionRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Name == input.Name))
        {
            throw ResultOutput.Exception(_adminLocalizer["此地区名已存在"]);
        }

        if (input.Code.NotNull() && await regionRep.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Code == input.Code))
        {
            throw ResultOutput.Exception(_adminLocalizer["此地区代码已存在"]);
        }

        Mapper.Map(input, entity);
        await regionRep.UpdateAsync(entity);
    }

    [NonAction]
    public async Task<List<long>> GetChildIdListAsync(long id)
    {
        var regionRep = _regionRep;
        using var _ = regionRep.DataFilter.DisableAll();

        return await regionRep.Select
        .Where(a => a.Id == id)
        .AsTreeCte()
        .ToListAsync(a => a.Id);
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id)
    {
        var regionRep = _regionRep;
        using var _ = regionRep.DataFilter.DisableAll();

        var idList = await GetChildIdListAsync(id);

        await regionRep.DeleteAsync(a => idList.Contains(a.Id));
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task SoftDeleteAsync(long id)
    {
        var regionRep = _regionRep;
        using var _ = regionRep.DataFilter.DisableAll();

        var idList = await GetChildIdListAsync(id);

        await regionRep.SoftDeleteAsync(a => idList.Contains(a.Id));
    }

    /// <summary>
    /// 设置启用
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SetEnableAsync(RegionSetEnableInput input)
    {
        var regionRep = _regionRep;
        using var _ = regionRep.DataFilter.DisableAll();

        var entity = await regionRep.GetAsync(input.RegionId);
        entity.Enabled = input.Enabled;
        await regionRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 设置热门
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task SetHotAsync(RegionSetHotInput input)
    {
        var regionRep = _regionRep;
        using var _ = regionRep.DataFilter.DisableAll();

        var entity = await regionRep.GetAsync(input.RegionId);
        entity.Hot = input.Hot;
        await regionRep.UpdateAsync(entity);
    }

    /// <summary>
    /// 获得省份列表
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static List<RegionInfo> GetProvinceList(string html)
    {
        var regex = new Regex(@"var\s+json\s*=\s*(\[.*?\])\s*;",
                            RegexOptions.Singleline | RegexOptions.IgnoreCase);

        var match = regex.Match(html);
        if (!match.Success)
            throw new InvalidOperationException("未找到json数据");

        var jsonString = match.Groups[1].Value;

        return JsonHelper.Deserialize<List<RegionInfo>>(jsonString) ?? [];
    }

    /// <summary>
    /// 同步数据
    /// </summary>
    /// <param name="regionLevel">地区级别</param>
    /// <returns></returns>
    public async Task SyncDataAsync(RegionLevel regionLevel = RegionLevel.City)
    {
        // 支持GBK编码
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var xzqhHtml = await "http://xzqh.mca.gov.cn/map".GetStringAsync();
        var provinceList = GetProvinceList(xzqhHtml);

        var regionRep = _regionRep;
        using var _ = regionRep.DataFilter.DisableAll();

        foreach (var province in provinceList)
        {
            var regionList = new List<RegionEntity>();

            //转换省份数据
            var shengJi = province.ParseShengJi();
            var provinceRegion = new RegionEntity()
            {
                Id = YitIdHelper.NextId(),
                ParentId = 0,
                Name = shengJi.Name,
                ShortName = shengJi.ShortName,
                Code = province.QuHuaDaiMa,
                AreaCode = province.QuHao,
                Level = RegionLevel.Province,
                Pinyin = WordsHelper.GetPinyin(shengJi.Name),
                PinyinFirst = WordsHelper.GetFirstPinyin(shengJi.Name),
            };
            regionList.Add(provinceRegion);

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            // 使用GBK编码
            var gbk = Encoding.GetEncoding("GBK");
            var gbkBytes = gbk.GetBytes(province.ShengJi);
            var shengJiEncoded = string.Join("", gbkBytes.Select(b => $"%{b:X2}"));
            var document = await context.OpenAsync($"http://xzqh.mca.gov.cn/defaultQuery?shengji={shengJiEncoded}&diji=-1&xianji=-1");

            var tableElement = document.QuerySelector("table.info_table");

            //获取城市数据
            var cityList = tableElement.QuerySelectorAll("tr[flag].shi_nub");
            foreach (var city in cityList)
            {
                var cityTdList = city.QuerySelectorAll("td");
                var cityId = YitIdHelper.NextId();
                var cityName = cityTdList[0].QuerySelector(".name_text").Text().Trim();
                var population = cityTdList[2].TextContent.Trim();
                var area = cityTdList[3].TextContent.Trim();
                var cityCode = cityTdList[4].TextContent.Trim();
                var cityRegion = new RegionEntity()
                {
                    Id = cityId,
                    ParentId = provinceRegion.Id,
                    Name = cityName,
                    Capital = cityTdList[1].TextContent.Trim(),
                    Population = population.NotNull() ? population.ToInt() : null,
                    Area = area.NotNull() ? area.ToInt() : null,
                    Code = cityCode.NotNull() ? cityCode : cityId.ToString(),
                    AreaCode = cityTdList[5].TextContent.Trim(),
                    ZipCode = cityTdList[6].TextContent.Trim(),
                    Level = RegionLevel.City,
                    Pinyin = WordsHelper.GetPinyin(cityName),
                    PinyinFirst = WordsHelper.GetFirstPinyin(cityName),
                };
                regionList.Add(cityRegion);

                //获取县/区数据
                var countyList = tableElement.QuerySelectorAll($"tr[parent=\"{cityRegion.Name}\"]");
                foreach (var county in countyList)
                {
                    var countyTdList = county.QuerySelectorAll("td");
                    var countyId = YitIdHelper.NextId();
                    var countyName = countyTdList[0].TextContent;
                    var countyPopulation = countyTdList[2].TextContent.Trim();
                    var countyArea = countyTdList[3].TextContent.Trim();
                    var countyCode = countyTdList[4].TextContent.Trim();
                    var countyRegion = new RegionEntity()
                    {
                        Id = countyId,
                        ParentId = cityRegion.Id,
                        Name = countyName,
                        Capital = countyTdList[1].TextContent.Trim(),
                        Population = countyPopulation.NotNull() ? countyPopulation.ToInt() : null,
                        Area = countyArea.NotNull() ? countyArea.ToInt() : null,
                        Code = countyCode.NotNull() ? countyCode : countyId.ToString(),
                        AreaCode = countyTdList[5].TextContent.Trim(),
                        ZipCode = countyTdList[6].TextContent.Trim(),
                        Level = RegionLevel.County,
                        Pinyin = WordsHelper.GetPinyin(countyName),
                        PinyinFirst = WordsHelper.GetFirstPinyin(countyName),
                    };
                    regionList.Add(countyRegion);
                }

                var codeList = regionList.Select(a => a.Code).ToList();
                var existsDataList = await regionRep.Where(a => codeList.Contains(a.Code)).ToListAsync();
                var existsDataCodeList = existsDataList.Select(a => a.Code).ToList();
                var insertDataList = regionList.Where(a => !existsDataCodeList.Contains(a.Code));

                if (insertDataList.Any())
                {
                    await regionRep.InsertAsync(insertDataList);
                }

                if (existsDataList.Any())
                {
                    foreach (var item in existsDataList)
                    {
                        var updateItem = regionList.Where(a => a.Code == item.Code).First();
                        item.Name = updateItem.Name;
                        item.Capital = updateItem.Capital;
                        item.Population = updateItem.Population;
                        item.Area = updateItem.Area;
                        item.Code = updateItem.Code;
                        item.AreaCode = updateItem.AreaCode;
                        item.ZipCode = updateItem.ZipCode;
                        item.Pinyin = updateItem.Pinyin;
                        item.PinyinFirst = updateItem.PinyinFirst;
                    }

                    await regionRep.UpdateAsync(existsDataList);
                }
            }
        }
    }
}