namespace ZhonTai.Plugin.Lazy.SlideCaptcha.Core.Resources;

public interface IResourceManager
{
    byte[] RandomBackground();
    (byte[], byte[]) RandomTemplate();
}
