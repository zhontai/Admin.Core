using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Admin.Core.Common.Attributes;
using Admin.Core.Common.Configs;

namespace Admin.Core.Common.Helpers
{
    [SingleInstance]
    public class VerifyCodeHelper
    {
        private readonly AppConfig _appConfig;
        public VerifyCodeHelper(AppConfig appConfig)
        {
            _appConfig = appConfig;
        }

        private string GenerateRandom(int length)
        {
            var chars = new StringBuilder();
            //验证码的字符集，去掉了一些容易混淆的字符 
            char[] character = { '2', '3', '4', '5', '6', '8', '9', 'a', 'b', 'd', 'e', 'f', 'h', 'k', 'm', 'n', 'r', 'x', 'y', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
            Random rnd = new Random();
            //生成验证码字符串 
            for (int i = 0; i < length; i++)
            {
                chars.Append(character[rnd.Next(character.Length)]);
            }
            return chars.ToString();
        }

        public byte[] Draw(out string code, int length = 4)
        {
            int codeW = 110;
            int codeH = 36;
            int fontSize = 22;
           
            //颜色列表，用于验证码、噪线、噪点 
            Color[] color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };
            //字体列表，用于验证码 
            string[] fonts = _appConfig.VarifyCode.Fonts;

            code = GenerateRandom(length);

            //创建画布
            using (Bitmap bmp = new Bitmap(codeW, codeH))
            using (Graphics g = Graphics.FromImage(bmp))
            using (MemoryStream ms = new MemoryStream())
            {
                g.Clear(Color.White);
                Random rnd = new Random();
                //画噪线 
                for (int i = 0; i < 1; i++)
                {
                    int x1 = rnd.Next(codeW);
                    int y1 = rnd.Next(codeH);
                    int x2 = rnd.Next(codeW);
                    int y2 = rnd.Next(codeH);
                    Color clr = color[rnd.Next(color.Length)];
                    g.DrawLine(new Pen(clr), x1, y1, x2, y2);
                }

                //画验证码字符串 
                {
                    string fnt;
                    Font ft;
                    Color clr;
                    for (int i = 0; i < code.Length; i++)
                    {
                        fnt = fonts[rnd.Next(fonts.Length)];
                        ft = new Font(fnt, fontSize);
                        clr = color[rnd.Next(color.Length)];
                        g.DrawString(code[i].ToString(), ft, new SolidBrush(clr), (float)i * 24 + 2, (float)0);
                    }
                }

                //将验证码图片写入内存流，并将其以 "image/Png" 格式输出
                bmp.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public string GetBase64String(out string code, int length = 4)
        {
            return Convert.ToBase64String(Draw(out code, length));
        }
    }
}
