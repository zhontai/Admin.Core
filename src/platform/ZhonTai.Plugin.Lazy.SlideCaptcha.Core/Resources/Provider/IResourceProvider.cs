﻿
using System;
using System.Collections.Generic;
using System.Text;

namespace ZhonTai.Plugin.Lazy.SlideCaptcha.Core.Resources.Provider
{
    public interface IResourceProvider
    {
        List<Resource> Backgrounds();
        List<TemplatePair> Templates();
    }
}
