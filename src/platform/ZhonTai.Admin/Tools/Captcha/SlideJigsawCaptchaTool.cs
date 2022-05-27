using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.PixelFormats;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Tools.Cache;

/*
Linux下Ubuntu如果报Gdip错误，需要按照以下步骤操作
1. apt-get install libgdiplus
2. cd /usr/lib
3. ln -s libgdiplus.so gdiplus.dll
*/
namespace ZhonTai.Admin.Tools.Captcha
{
    /// <summary>
    /// 滑块拼图验证
    /// </summary>
    [SingleInstance]
    public class SlideJigsawCaptchaTool : ICaptchaTool
    {
        private readonly ICacheTool _cache;

        public SlideJigsawCaptchaTool(ICacheTool cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// 随机范围内数字
        /// </summary>
        /// <param name="startNum"></param>
        /// <param name="endNum"></param>
        /// <returns></returns>
		private static int GetRandomInt(int startNum, int endNum)
        {
            return (endNum > startNum ? new Random().Next(endNum - startNum) : 0) + startNum;
        }

        /// <summary>
        /// 随机生成拼图坐标
        /// </summary>
        /// <param name="originalWidth"></param>
        /// <param name="originalHeight"></param>
        /// <param name="templateWidth"></param>
        /// <param name="templateHeight"></param>
        /// <returns></returns>
        private static PointModel GeneratePoint(int originalWidth, int originalHeight, int templateWidth, int templateHeight)
        {
            var random = new Random();
            int widthDifference = originalWidth - templateWidth;
            int heightDifference = originalHeight - templateHeight;
            int x;
            if (widthDifference <= 0)
            {
                x = 5;
            }
            else
            {
                x = random.Next(originalWidth - templateWidth - 100) + 100;
            }

            int y;
            if (heightDifference <= 0)
            {
                y = 5;
            }
            else
            {
                y = random.Next(originalHeight - templateHeight - 5) + 5;
            }

            return new PointModel(x, y);
        }

        /// <summary>
        /// 随机生成干扰图坐标
        /// </summary>
        /// <param name="originalWidth"></param>
        /// <param name="originalHeight"></param>
        /// <param name="templateWidth"></param>
        /// <param name="templateHeight"></param>
        /// <param name="blockX"></param>
        /// <param name="blockY"></param>
        /// <returns></returns>
        private static PointModel GenerateInterferencePoint(int originalWidth, int originalHeight, int templateWidth, int templateHeight, int blockX, int blockY)
        {
            int x;
            if (originalWidth - blockX - 5 > templateWidth * 2)
            {
                //在原扣图右边插入干扰图
                x = GetRandomInt(blockX + templateWidth + 5, originalWidth - templateWidth);
            }
            else
            {
                //在原扣图左边插入干扰图
                x = GetRandomInt(100, blockX - templateWidth - 5);
            }

            int y;
            if (originalHeight - blockY - 5 > templateHeight * 2)
            {
                //在原扣图下边插入干扰图
                y = GetRandomInt(blockY + templateHeight + 5, originalHeight - templateHeight);
            }
            else
            {
                //在原扣图上边插入干扰图
                y = GetRandomInt(5, blockY - templateHeight - 5);
            }

            return new PointModel(x, y);
        }

        private static ComplexPolygon CalcBlockShape(Image<Rgba32> holeTemplateImage)
        {
            int temp = 0;
            var pathList = new List<IPath>();
            holeTemplateImage.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < holeTemplateImage.Height; y++)
                {
                    var rowSpan = accessor.GetRowSpan(y);
                    for (int x = 0; x < rowSpan.Length; x++)
                    {
                        ref Rgba32 pixel = ref rowSpan[x];
                        if (pixel.A != 0)
                        {
                            if (temp == 0)
                            {
                                temp = x;
                            }
                        }
                        else
                        {
                            if (temp != 0)
                            {
                                pathList.Add(new RectangularPolygon(temp, y, x - temp, 1));
                                temp = 0;
                            }
                        }
                    }
                }
            });

            return new ComplexPolygon(new PathCollection(pathList));
        }

        /// <summary>
        /// 获得验证数据
        /// </summary>
        /// <returns>JObject</returns>
        public async Task<CaptchaOutput> GetAsync(string captchaKey)
        {
            //获取网络图片
            //var client = new HttpClient();
            //var stream = await client.GetStreamAsync("https://picsum.photos/310/155");
            //client.Dispose();

            //底图
            using var baseImage = await Image.LoadAsync<Rgba32>($@"{Directory.GetCurrentDirectory()}\wwwroot\captcha\jigsaw\{new Random().Next(1, 4)}.jpg".ToPath());
            //模板图
            using var templateImage = await Image.LoadAsync<Rgba32>($@"{Directory.GetCurrentDirectory()}\wwwroot\captcha\jigsaw\templates\{new Random().Next(1, 7)}.png".ToPath());

            int baseWidth = baseImage.Width;
            int baseHeight = baseImage.Height;
            int templateWidth = templateImage.Width;
            int templateHeight = templateImage.Height;

            //拼图
            using var blockImage = new Image<Rgba32>(templateWidth, templateHeight);
            //滑块拼图
            using var sliderBlockImage = new Image<Rgba32>(templateWidth, baseHeight);

            //随机生成拼图坐标
            PointModel blockPoint = GeneratePoint(baseWidth, baseHeight, templateWidth, templateHeight);

            //根据模板图计算轮廓
            var blockShape = CalcBlockShape(templateImage);
            //生成拼图
            blockImage.Mutate(x =>
            {
                x.Clip(blockShape, p => p.DrawImage(baseImage, new Point(-blockPoint.X, -blockPoint.Y), 1));
            });
            //叠加拼图
            //blockImage.Mutate(x => x.DrawImage(templateImage, new Point(0, 0), 1));

            //生成滑块拼图
            sliderBlockImage.Mutate(x => x.DrawImage(blockImage, new Point(0, blockPoint.Y), 1));

            //生成拼图底图
            baseImage.Mutate(x => x.DrawImage(templateImage, new Point(blockPoint.X, blockPoint.Y), (float)0.5));

            //生成干扰图坐标
            PointModel interferencePoint = GenerateInterferencePoint(baseWidth, baseHeight, templateWidth, templateHeight, blockPoint.X, blockPoint.Y);

            //生成干扰图底图
            baseImage.Mutate(x => x.DrawImage(templateImage, new Point(interferencePoint.X, interferencePoint.Y), (float)0.5));

            var token = Guid.NewGuid().ToString();
            var captchaData = new CaptchaOutput
            {
                Token = token,
                Data = new SlideJigsawCaptchaDto()
                {
                    BaseImage = baseImage.ToBase64String(PngFormat.Instance),
                    BlockImage = sliderBlockImage.ToBase64String(PngFormat.Instance)
                }
            };

            var key = string.Format(captchaKey, token);
            await _cache.SetAsync(key, blockPoint.X);

            return captchaData;
        }

        /// <summary>
        /// 检查验证数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> CheckAsync(CaptchaInput input)
        {
            if (input == null || input.Data.IsNull())
            {
                return false;
            }
            var key = string.Format(input.CaptchaKey, input.Token);
            if (await _cache.ExistsAsync(key))
            {
                try
                {
                    var point = JsonConvert.DeserializeObject<PointModel>(input.Data);
                    var x = await _cache.GetAsync<int>(key);
                    if (Math.Abs(x - point.X) < 5)
                    {
                        if (input.DeleteCache)
                        {
                            await _cache.DelAsync(key);
                        }
                        return true;
                    }
                    else
                    {
                        await _cache.DelAsync(key);
                        return false;
                    }
                }
                catch
                {
                    await _cache.DelAsync(key);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
