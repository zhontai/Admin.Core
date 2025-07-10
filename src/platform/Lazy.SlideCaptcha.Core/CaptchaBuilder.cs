using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.SlideCaptcha.Core
{
    public class CaptchaBuilder
    {
        public IServiceCollection Services { get; set; }

        public CaptchaBuilder(IServiceCollection serviceCollection)
        {
            Services = serviceCollection;
        }
    }
}
