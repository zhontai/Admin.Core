
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Lazy.SlideCaptcha.Core.Resources
{
    public interface IResourceManager
    {
        byte[] RandomBackground();
        (byte[], byte[]) RandomTemplate();
    }
}
