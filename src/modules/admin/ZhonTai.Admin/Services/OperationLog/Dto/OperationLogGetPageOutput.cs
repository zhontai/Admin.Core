using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ZhonTai.Admin.Services.OperationLog.Dto;

public class OperationLogGetPageOutput
{
    /// <summary>
    /// 编号
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 接口名称
    /// </summary>
    public string ApiLabel { get; set; }

    /// <summary>
    /// 接口地址
    /// </summary>
    public string ApiPath { get; set; }

    /// <summary>
    /// 接口提交方法
    /// </summary>
    public string ApiMethod { get; set; }

    /// <summary>
    /// IP
    /// </summary>
    public string IP { get; set; }

    /// <summary>
    /// 国家
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// 省份
    /// </summary>
    public string Province { get; set; }

    /// <summary>
    /// 城市
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// 网络服务商
    /// </summary>
    public string Isp { get; init; }

    /// <summary>
    /// 浏览器
    /// </summary>
    public string Browser { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    public string Os { get; set; }

    /// <summary>
    /// 设备
    /// </summary>
    public string Device { get; set; }

    /// <summary>
    /// 耗时（毫秒）
    /// </summary>
    public long ElapsedMilliseconds { get; set; }

    /// <summary>
    /// 操作状态
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// 状态码
    /// </summary>
    public int? StatusCode { get; set; }

    /// <summary>
    /// 操作消息
    /// </summary>
    public string Msg { get; set; }

    /// <summary>
    /// 请求参数
    /// </summary>
    public string Params { get; set; }

    /// <summary>
    /// 响应结果
    /// </summary>
    public string Result { get; set; }


    /// <summary>
    /// 创建者
    /// </summary>
    public string CreatedUserName { get; set; }

    /// <summary>
    /// 创建者姓名
    /// </summary>
    public string CreatedUserRealName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedTime { get; set; }
}