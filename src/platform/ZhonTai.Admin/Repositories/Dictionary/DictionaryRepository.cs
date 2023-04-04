using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.Dict;

namespace ZhonTai.Admin.Repositories;

public class DictionaryRepository : AdminRepositoryBase<DictEntity>, IDictRepository
{
    public DictionaryRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}