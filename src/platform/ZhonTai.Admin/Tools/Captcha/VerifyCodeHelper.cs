using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;
using System.Text;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Configs;

namespace ZhonTai.Admin.Tools.Captcha;

[SingleInstance]
public class VerifyCodeHelper
{
    private readonly AppConfig _appConfig;

    public VerifyCodeHelper(AppConfig appConfig)
    {
        _appConfig = appConfig;
    }

    private static string GenerateRandom(int length)
    {
        var chars = new StringBuilder();
        //验证码的字符集，去掉了一些容易混淆的字符
        char[] character = { '2', '3', '4', '5', '6', '8', '9', 'a', 'b', 'd', 'e', 'f', 'h', 'k', 'm', 'n', 'r', 'x', 'y', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
        var rnd = new Random();
        //生成验证码字符串
        for (int i = 0; i < length; i++)
        {
            chars.Append(character[rnd.Next(character.Length)]);
        }
        return chars.ToString();
    }

    public byte[] Draw(out string code, int length = 4)
    {
        int width = 110;
        int height = 36;
        int fontSize = 22;

        //颜色列表，用于验证码、噪线、噪点
        Color[] colors = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };
        //字体列表，用于验证码
        string[] fonts = _appConfig.VarifyCode.Fonts;

        var chars = GenerateRandom(length);
        code = chars;

        using var img = new Image<Rgba32>(width, height, Color.White);

        img.Mutate(ctx =>
        {
            var rnd = new Random();
            //画噪线
            for (int i = 0; i < 1; i++)
            {
                int x1 = rnd.Next(width), y1 = rnd.Next(height);
                int x2 = rnd.Next(width), y2 = rnd.Next(height);
                int ctrlx1 = rnd.Next(width / 4, width / 4 * 3), ctrly1 = rnd.Next(5, height - 5);
                int ctrlx2 = rnd.Next(width / 4, width / 4 * 3), ctrly2 = rnd.Next(5, height - 5);
                Color color = colors[rnd.Next(colors.Length)];
                ctx.DrawBeziers(new DrawingOptions
                {
                    GraphicsOptions = new GraphicsOptions
                    {
                        BlendPercentage = 1
                    }
                }, color, 1, new PointF(x1, y1), new PointF(ctrlx1, ctrly1), new PointF(ctrlx2, ctrly2), new PointF(x2, y2));
            }

            //画验证码字符串
            {
                Color color;
                Font font;
                string fontName;
                FontFamily fontFamily;
                for (int i = 0; i < length; i++)
                {
                    fontName = fonts[rnd.Next(fonts.Length)];
                    fontFamily = SystemFonts.Families.Where(a => a.Name == fontName).FirstOrDefault();
                    fontFamily = fontFamily.Name.NotNull() ? fontFamily : SystemFonts.Families.FirstOrDefault();
                    font = new Font(fontFamily, fontSize);
                    color = colors[rnd.Next(colors.Length)];
                    ctx.DrawText(new DrawingOptions
                    {
                        GraphicsOptions = new GraphicsOptions
                        {
                            BlendPercentage = 1
                        }
                    }, chars[i].ToString(), font, color, new PointF((float)i * 24 + 2, 0));
                }
            }
        });

        //将验证码图片写入内存流，并将其以 "image/Png" 格式输出
        using var ms = new MemoryStream();
        img.SaveAsPng(ms);
        return ms.ToArray();
    }

    public string GetBase64String(out string code, int length = 4)
    {
        return Convert.ToBase64String(Draw(out code, length));
    }
}