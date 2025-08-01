namespace ZhonTai.Plugin.Lazy.SlideCaptcha.Core.Resources.Handler;

public interface IResourceHandler
{
    bool CanHandle(string handlerType);
    byte[] Handle(Resource resource);
}
