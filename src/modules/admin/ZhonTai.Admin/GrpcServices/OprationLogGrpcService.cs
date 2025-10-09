using Mapster;
using ProtoBuf.Grpc;
using ZhonTai.Admin.Core.GrpcServices;
using ZhonTai.Admin.Core.GrpcServices.Dtos;
using ZhonTai.Admin.Core.Protos;
using ZhonTai.Admin.Services.Api.Dto;
using ZhonTai.Admin.Services.OperationLog;

namespace ZhonTai.Admin.GrpcServices;

/// <summary>
/// 操作日志Grpc服务
/// </summary>
public class OprationLogGrpcService : IOprationLogGrpcService
{
    private readonly IOperationLogService _operationLogService;

    public OprationLogGrpcService(IOperationLogService operationLogService)
    {
        _operationLogService = operationLogService;
    }

    /// <summary>
    /// 添加操作日志
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<ProtoLong> AddAsync(OperationLogAddGrpcInput input, CallContext context = default)
    {
        return await _operationLogService.AddAsync(input.Adapt<OperationLogAddInput>());
    }
}
