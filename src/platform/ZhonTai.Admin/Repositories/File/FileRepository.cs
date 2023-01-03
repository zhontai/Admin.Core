using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Domain.File;

namespace ZhonTai.Admin.Repositories;

public class FileRepository : AdminRepositoryBase<FileEntity>, IFileRepository
{
    public FileRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}