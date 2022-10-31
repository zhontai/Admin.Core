using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.Dictionary;

namespace ZhonTai.Admin.Repositories;

public class DictionaryRepository : AdminRepositoryBase<DictionaryEntity>, IDictionaryRepository
{
    public DictionaryRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}