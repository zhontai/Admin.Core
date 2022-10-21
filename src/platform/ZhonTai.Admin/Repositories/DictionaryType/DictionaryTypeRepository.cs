using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.DictionaryType;

namespace ZhonTai.Admin.Repositories;

public class DictionaryTypeRepository : AppRepositoryBase<DictionaryTypeEntity>, IDictionaryTypeRepository
{
    public DictionaryTypeRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}