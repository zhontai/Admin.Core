using Lazy.SlideCaptcha.Core;
using Lazy.SlideCaptcha.Core.Resources.Handler;
using Lazy.SlideCaptcha.Core.Resources.Provider;
using Lazy.SlideCaptcha.Core.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CaptchaBuilderExtensions
    {
        public static CaptchaBuilder AddResourceProvider<TProvider>(this CaptchaBuilder builder) where TProvider : class, IResourceProvider
        {
            builder.Services.AddSingleton<IResourceProvider, TProvider>();
            return builder;
        }

        public static CaptchaBuilder AddResourceHandler<THandler>(this CaptchaBuilder builder) where THandler : class, IResourceHandler
        {
            builder.Services.AddSingleton<IResourceHandler, THandler>();
            return builder;
        }

        public static CaptchaBuilder ReplaceValidator<TValidator>(this CaptchaBuilder builder) where TValidator : class, IValidator
        {
            builder.Services.Replace<IValidator, TValidator>();
            return builder;
        }

        public static CaptchaBuilder DisableDefaultTemplates(this CaptchaBuilder builder)
        {
            var serviceDescriptor = builder.Services.FirstOrDefault(e => e.ImplementationType == typeof(EmbeddedResourceProvider));
            if (serviceDescriptor != null)
            {
                builder.Services.Remove(serviceDescriptor);
            }

            return builder;
        }
    }
}
