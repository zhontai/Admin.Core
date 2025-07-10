using Lazy.SlideCaptcha.Core.Exceptions;
using Lazy.SlideCaptcha.Core.Resources.Handler;
using Lazy.SlideCaptcha.Core.Resources.Provider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Lazy.SlideCaptcha.Core.Resources
{
    public class DefaultResourceManager : IResourceManager
    {
        private readonly Random _random = new Random();
        private readonly IResourceHandlerManager _resourceProviderManager;
        private readonly List<Resource> _backgrounds = new List<Resource>();
        private readonly List<TemplatePair> _templates = new List<TemplatePair>();

        public DefaultResourceManager(IEnumerable<IResourceProvider> resourceProviders, IResourceHandlerManager resourceProviderManager)
        {
            _resourceProviderManager = resourceProviderManager;

            foreach (var provider in resourceProviders)
            {
                _backgrounds.AddRange(provider.Backgrounds());
                _templates.AddRange(provider.Templates());
            }
        }

        public byte[] RandomBackground()
        {
            if (_backgrounds.Count == 0) throw new SlideCaptchaException("背景图不能为空");

            var background = _backgrounds[_random.Next(_backgrounds.Count)];
            return _resourceProviderManager.Handle(background);
        }

        public (byte[], byte[]) RandomTemplate()
        {
            if (_templates.Count == 0) throw new SlideCaptchaException("模板不能为空");

            var template = _templates[_random.Next(_templates.Count)];
            var hole = _resourceProviderManager.Handle(template.Hole);
            var slider = _resourceProviderManager.Handle(template.Slider);
            return (slider, hole);
        }
    }
}
