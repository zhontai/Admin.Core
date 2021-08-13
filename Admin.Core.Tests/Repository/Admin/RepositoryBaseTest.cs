using Admin.Core.Model.Admin;
using Admin.Core.Repository;
using Xunit;

namespace Admin.Core.Tests.Service.Repository.Admin
{
    public class RepositoryBaseTest : BaseTest
    {
        private readonly IRepositoryBase<ApiEntity> _repositoryBase;

        public RepositoryBaseTest()
        {
            _repositoryBase = GetService<IRepositoryBase<ApiEntity>>();
        }

        [Fact]
        public async void GetAsyncByExpression()
        {
            var id = 161227167658053;
            var user = await _repositoryBase.GetAsync(a => a.Id == id);
            Assert.Equal(id, user?.Id);
        }
    }
}