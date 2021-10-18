using Xunit;
using ZhonTai.Plate.Admin.Service.Api;

namespace ZhonTai.Plate.Admin.Tests.Service
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