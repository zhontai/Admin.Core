using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.DictType;

namespace ZhonTai.Admin.Repositories;

public class DictionaryTypeRepository : AdminRepositoryBase<DictTypeEntity>, IDictTypeRepository
{
    public DictionaryTypeRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}