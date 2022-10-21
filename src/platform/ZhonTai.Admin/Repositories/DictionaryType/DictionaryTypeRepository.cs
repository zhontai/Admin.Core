using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.DictionaryType;

namespace ZhonTai.Admin.Repositories;

public class DictionaryTypeRepository : AdminRepositoryBase<DictionaryTypeEntity>, IDictionaryTypeRepository
{
    public DictionaryTypeRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}