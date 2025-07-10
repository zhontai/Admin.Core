using Lazy.SlideCaptcha.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Lazy.SlideCaptcha.Core.Resources.Handler
{
    public class CachedResourceHandlerManager : IResourceHandlerManager
    {
        private IEnumerable<IResourceHandler> _resourceHandlers;
        private Dictionary<Resource, byte[]> _cache = new Dictionary<Resource, byte[]>();

        public CachedResourceHandlerManager(IEnumerable<IResourceHandler> resourceHandlers)
        {
            _resourceHandlers = resourceHandlers;
        }

        public byte[] Handle(Resource resource)
        {
            if (resource == null) throw new ArgumentNullException(nameof(resource));

            if (_cache.ContainsKey(resource))
            {
                return _cache[resource];
            }

            foreach (var provider in _resourceHandlers)
            {
                if (provider.CanHandle(resource.Type))
                {
                    var bytes = provider.Handle(resource);
                    _cache.Add(resource, bytes);
                    return bytes;
                }
            }

            throw new SlideCaptchaException("没有可用的资源提供者!");
        }
    }
}
