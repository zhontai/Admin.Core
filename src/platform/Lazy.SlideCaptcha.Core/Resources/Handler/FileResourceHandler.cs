using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Lazy.SlideCaptcha.Core.Resources.Handler
{
    public class FileResourceHandler : IResourceHandler
    {
        public const string TYPE = "file";

        public bool CanHandle(string handlerType)
        {
            return handlerType == TYPE;
        }

        public byte[] Handle(Resource resource)
        {
            if (resource == null) throw new ArgumentNullException(nameof(resource));
            return File.ReadAllBytes(resource.Data);
        }
    }
}
