using System.Collections.Generic;
using ZhonTai.Module.Dev.Api.Contracts.Services.DevProject.Dtos;
using ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectModel.Dtos;
using ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectModelField.Dtos;

namespace ZhonTai.Module.Dev.Api.Contracts.Services.DevProjectGen.Dtos;

public class DevProjectRazorRenderModel
{
    /// <summary>
    /// 项目信息
    /// </summary>
    public DevProjectGetOutput Project { get; set; }
    /// <summary>
    /// 模型信息
    /// </summary>
    public DevProjectModelGetOutput Model { get; set; }
    /// <summary>
    /// 字段信息
    /// </summary>
    public List<DevProjectModelFieldGetOutput> Fields { get; set; }

}
