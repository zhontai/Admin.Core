using Mapster;
using ProtoBuf.Grpc;
using ZhonTai.Admin.Core.GrpcServices;
using ZhonTai.Admin.Core.GrpcServices.Dtos;
using ZhonTai.Admin.Domain.Api;
using ZhonTai.Admin.Services.Api;
using ZhonTai.Admin.Services.Api.Dto;

namespace ZhonTai.Admin.GrpcServices;

/// <summary>
/// 接口Grpc服务
/// </summary>
public class ApiGrpcService : IApiGrpcService
{
    private readonly IApiRepository _apiRepository;
    private readonly IApiService _apiService;
    public ApiGrpcService(IApiRepository apiRepository, IApiService apiService)
    {
        _apiRepository = apiRepository;
        _apiService = apiService;
    }

    /// <summary>
    /// 获取Api列表
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<GrpcOutput<List<ApiGrpcOutput>>> GetApiList(CallContext context = default)
    {
        var data = await _apiRepository.Select.ToListAsync<ApiGrpcOutput>();
        var output = new GrpcOutput<List<ApiGrpcOutput>>()
        {
            Data = data
        };
        return output;
    }


    /// <summary>
    /// 同步Api
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task SyncAsync(ApiSyncGrpcInput input, CallContext context = default)
    {
        await _apiService.SyncAsync(input.Adapt<ApiSyncInput>());
    }
}
