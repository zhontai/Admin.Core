using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.Dictionary;

namespace ZhonTai.Admin.Repositories;

public class DictionaryRepository : AppRepositoryBase<DictionaryEntity>, IDictionaryRepository
{
    public DictionaryRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}