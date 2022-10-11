using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;
using ZhonTai.Admin.Services.Api.Dto;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Api.Dto;

namespace ZhonTai.Tests.Controllers;

public class ApiControllerTest : BaseControllerTest
{
    public ApiControllerTest() : base()
    {
    }

    [Fact]
    public async Task Get()
    {
        var res = await GetResult<ResultOutput<ApiGetOutput>>("/api/admin/api/get?id=161227167658053");
        Assert.True(res.Success);
    }

    [Fact]
    public async Task GetList()
    {
        var res = await GetResult<ResultOutput<List<ApiListOutput>>>("/api/admin/api/get-list?key=接口管理");
        Assert.True(res.Success);
    }

    [Fact]
    public async Task GetPage()
    {
        await Login();
        var input = new PageInput<ApiGetPageDto>
        {
            CurrentPage = 1,
            PageSize = 20,
            Filter = new ApiGetPageDto
            {
                Label = "接口管理"
            }
        };

        var res = await PostResult($"/api/admin/api/get-page", input);
        Assert.True(res.Success);
    }

    [Fact]
    public async Task Add()
    {
        var input = new ApiAddInput
        {
           Label = "新接口",
           Path = "/api/admin/api/newapi",
           HttpMethods = "post"
        };

        var res = await PostResult($"/api/admin/api/add", input);
        Assert.True(res.Success);
    }

    [Fact]
    public async Task Update()
    {
        var output = await GetResult<ResultOutput<ApiGetOutput>>("/api/admin/api/get?id=161227167658053");
        var res = await PutResult($"/api/admin/api/update", output.Data);
        Assert.True(res.Success);
    }

    [Fact]
    public async Task Delete()
    {
        var res = await DeleteResult($"/api/admin/api/soft-delete?{ToParams(new { id = 191182807191621 })}");
        Assert.True(res.Success);
    }
}