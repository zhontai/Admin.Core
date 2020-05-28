using Xunit;
using Admin.Core.Service.Admin.Api;

namespace Admin.Core.Tests.Service.Admin
{
    public class ApiServiceTest : BaseTest
    {
        private readonly IApiService _apiService;

        public ApiServiceTest()
        {
            _apiService = GetService<IApiService>();
        }

        [Fact]
        public async void GetAsync()
        {
            var res = await _apiService.GetAsync(1);
            Assert.True(res.Success);
        }
    }
}
