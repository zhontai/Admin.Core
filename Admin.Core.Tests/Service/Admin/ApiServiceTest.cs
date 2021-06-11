using Admin.Core.Service.Admin.Api;
using Xunit;

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
            var res = await _apiService.GetAsync(161227168079941);
            Assert.True(res.Success);
        }
    }
}