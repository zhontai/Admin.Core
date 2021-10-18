using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ZhonTai.HttpApi;

namespace ZhonTai.Plate.Admin.Host
{
    public class Startup : BaseStartup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env) : base(configuration, env)
        {
        }
    }
}