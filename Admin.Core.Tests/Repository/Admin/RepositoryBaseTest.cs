using Xunit;
using Admin.Core.Repository;
using Admin.Core.Model.Admin;

namespace Admin.Core.Tests.Service.Repository.Admin
{
    public class RepositoryBaseTest : BaseTest
    {
        private readonly IRepositoryBase<UserEntity, long> _repositoryBase;

        public RepositoryBaseTest()
        {
            _repositoryBase = GetService<IRepositoryBase<UserEntity, long>>();
        }

        [Fact]
        public async void GetAsyncByExpression()
        {
            var id = 1;
            var user = await _repositoryBase.GetAsync(a => a.Id == id);
            Assert.Equal(id, user?.Id);
        }
    }
}
