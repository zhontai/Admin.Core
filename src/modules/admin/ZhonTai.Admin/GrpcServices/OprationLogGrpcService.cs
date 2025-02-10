using Mapster;
using ProtoBuf.Grpc;
using ZhonTai.Admin.Core.GrpcServices;
using ZhonTai.Admin.Core.GrpcServices.Dtos;
using ZhonTai.Admin.Core.Protos;
using ZhonTai.Admin.Services.OperationLog;
using ZhonTai.Admin.Services.OperationLog.Dto;

namespace ZhonTai.Admin.GrpcServices;

public class OprationLogGrpcService : IOprationLogGrpcService
{
    private readonly IOperationLogService _operationLogService;

    public OprationLogGrpcService(IOperationLogService operationLogService)
    {
        _operationLogService = operationLogService;
    }

    public async Task<ProtoLong> AddAsync(OperationLogAddGrpcInput input, CallContext context = default)
    {
        return await _operationLogService.AddAsync(input.Adapt<OperationLogAddInput>());
    }
}
