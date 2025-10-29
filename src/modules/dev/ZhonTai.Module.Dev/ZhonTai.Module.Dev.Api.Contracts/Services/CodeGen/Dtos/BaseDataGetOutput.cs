using System.Collections.Generic;

namespace ZhonTai.Module.Dev.Api.Contracts.Services.CodeGen.Dtos;

public class BaseDataGetOutput
{
    public IEnumerable<DatabaseGetOutput> Databases { get; set; }
    public string AuthorName { get; set; } = "SirHQ";
    public string ApiAreaName { get; set; } = "";
    public string Namespace { get; set; } = "";
    public string BackendOut { get; set; } = "";
    public string FrontendOut { get; set; } = "";
    public string DbMigrateSqlOut { get; set; } = "";
    public string Usings { get; set; } = "";
    public string MenuAfterText { get; set; } = ""; 
}