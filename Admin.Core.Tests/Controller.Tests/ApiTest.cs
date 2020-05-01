using Xunit;
using Moq;
using Admin.Core.Model.Admin;
using Admin.Core.Service.Admin.Api;
using Admin.Core.Common.Input;
using Admin.Core.Controllers.Admin;

namespace Admin.Core.Tests.Controller.Tests
{
    public class ApiTest
    {
        Mock<IApiService> mockIApiService = new Mock<IApiService>();

        ApiController _apiController;

        public ApiTest()
        {
            _apiController = new ApiController(mockIApiService.Object);
        }

        [Fact]
        public async void GetList()
        {
            var p = new PageInput<ApiEntity>();
            var res = await _apiController.GetPage(p);
            Assert.True(res.Success);
                
            //var data = res.;
            //Assert.NotNull(data);
        }
    }
}
