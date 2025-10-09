using ProtoBuf.Grpc;
using ZhonTai.Admin.Core.GrpcServices;
using ZhonTai.Admin.Core.GrpcServices.Dtos;
using ZhonTai.Admin.Domain.Api;

namespace ZhonTai.Admin.GrpcServices;

/// <summary>
/// 接口Grpc服务
/// </summary>
public class ApiGrpcService : IApiGrpcService
{
    private readonly IApiRepository _apiRepository;

    public ApiGrpcService(IApiRepository apiRepository)
    {
        _apiRepository = apiRepository;
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
}
