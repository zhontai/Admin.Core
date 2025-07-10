using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.SlideCaptcha.Core.Resources.Provider
{
    public class OptionsResourceProvider : IResourceProvider
    {
        private readonly List<Resource> _backgrounds = new List<Resource>();
        private readonly List<TemplatePair> _templates = new List<TemplatePair>();

        public OptionsResourceProvider(IOptionsMonitor<CaptchaOptions> optionAccessor)
        {
            var options = optionAccessor.CurrentValue;

            if (options.Backgrounds != null)
            {
                _backgrounds.AddRange(options.Backgrounds);
            }

            if (options.Templates != null)
            {
                _templates.AddRange(options.Templates);
            }
        }

        public List<Resource> Backgrounds()
        {
            return _backgrounds; // ?克隆
        }

        public List<TemplatePair> Templates()
        {
            return _templates;
        }
    }
}
