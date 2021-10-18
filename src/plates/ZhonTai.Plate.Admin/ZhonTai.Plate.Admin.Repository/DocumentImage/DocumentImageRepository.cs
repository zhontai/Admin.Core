using ZhonTai.Plate.Admin.Domain.DocumentImage;

namespace ZhonTai.Plate.Admin.Repository
{
    public class DocumentImageRepository : RepositoryBase<DocumentImageEntity>, IDocumentImageRepository
    {
        public DocumentImageRepository(MyUnitOfWorkManager muowm) : base(muowm)
        {
        }
    }
}