using Lazy.SlideCaptcha.Core.Resources.Handler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Lazy.SlideCaptcha.Core.Resources.Provider
{
    public class EmbeddedResourceProvider : IResourceProvider
    {
        private const int DEFAULT_TEMPLATE_COUNT = 5;

        public List<Resource> Backgrounds()
        {
            return new List<Resource>();
        }

        public List<TemplatePair> Templates()
        {
            var templatePairs = new List<TemplatePair>();

            var assembly = Assembly.GetExecutingAssembly();
            var names = assembly.GetManifestResourceNames().ToList() ;

            for (var i = 0; i < DEFAULT_TEMPLATE_COUNT; i++)
            {
                var sliderResourceName = $"Lazy.SlideCaptcha.Core.templates._{i + 1}.slider.png";
                var holeResourceName = $"Lazy.SlideCaptcha.Core.templates._{i + 1}.hole.png";

                var sliderResource = new Resource(EmbeddedResourceHandler.TYPE, sliderResourceName);
                var holeResource = new Resource(EmbeddedResourceHandler.TYPE, holeResourceName);
                templatePairs.Add(TemplatePair.Create(sliderResource, holeResource));
            }

            return templatePairs;
        }
    }
}
