using ZhonTai.Plugin.Lazy.SlideCaptcha.Core;
using ZhonTai.Plugin.Lazy.SlideCaptcha.Core.Resources.Handler;
using ZhonTai.Plugin.Lazy.SlideCaptcha.Core.Resources.Provider;
using ZhonTai.Plugin.Lazy.SlideCaptcha.Core.Validator;

namespace Microsoft.Extensions.DependencyInjection;

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
