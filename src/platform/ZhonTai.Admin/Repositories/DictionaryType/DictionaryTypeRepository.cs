using ZhonTai.Admin.Core.Db;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.DictionaryType;

namespace ZhonTai.Admin.Repositories;

public class DictionaryTypeRepository : RepositoryCloud<DictionaryTypeEntity>, IDictionaryTypeRepository
{
    public DictionaryTypeRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}