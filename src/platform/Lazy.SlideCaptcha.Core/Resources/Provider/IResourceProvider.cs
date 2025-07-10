
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.SlideCaptcha.Core.Resources.Provider
{
    public interface IResourceProvider
    {
        List<Resource> Backgrounds();
        List<TemplatePair> Templates();
    }
}
