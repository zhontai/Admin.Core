@{
    var gen = Model as ZhonTai.Module.Dev.Api.Contracts.Domain.CodeGen.CodeGenEntity;
    if (gen == null) return;
    var entityNamePC = gen.EntityName.NamingPascalCase();
}
using @(gen.Namespace).Domain.@(entityNamePC);
using @(gen.Namespace).Core.Consts;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;

namespace @(gen.Namespace).Api.Repositories.@(entityNamePC);

public class @(entityNamePC)Repository : RepositoryBase<@(entityNamePC)Entity>, I@(entityNamePC)Repository
{
    public @(entityNamePC)Repository(UnitOfWorkManagerCloud uowm) : base(DbKeys.AppDb, uowm)
    {
    }
}