﻿namespace ZhonTai.Admin.Services.MsgType.Dto;

/// <summary>
/// 查询列表响应
/// </summary>
public class MsgTypeGetListOutput
{
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 父级Id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }
}