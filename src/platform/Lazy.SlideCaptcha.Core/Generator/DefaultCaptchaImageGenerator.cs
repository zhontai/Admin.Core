using System;
using System.Collections.Generic;
using System.Linq;
using Lazy.SlideCaptcha.Core.Resources;
using Microsoft.Extensions.Options;
using SkiaSharp;

namespace Lazy.SlideCaptcha.Core.Generator
{
    public class DefaultCaptchaImageGenerator : ICaptchaImageGenerator
    {
        private IResourceManager _resourceManager;
        private Random _random = new Random();
        private CaptchaOptions _options;

        public DefaultCaptchaImageGenerator(IResourceManager resourceManager, IOptionsSnapshot<CaptchaOptions> options)
        {
            _resourceManager = resourceManager;
            _options = options.Value;
        }

        /// <summary>
        /// 计算凹槽轮廓
        /// 原理： 一行一行扫描，每行不透明小方块连接形成数个小长方形
        /// </summary>
        /// <param name="holeTemplateImage"></param>
        /// <returns></returns>
        private static SKPath CalcHoleShape(SKBitmap holeTemplateImage)
        {
            int temp = 0;
            var path = new SKPath();
            for (int y = 0; y < holeTemplateImage.Height; y++)
            {
                for (int x = 0; x < holeTemplateImage.Width; x++)
                {
                    var pixel = holeTemplateImage.GetPixel(x, y);
                    if (pixel.Alpha != 0)
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
                            path.AddRect(new SKRect(temp, y, x, y + 1));
                            temp = 0;
                        }
                    }
                }
            }
            return path;
        }

        public CaptchaImageData Generate()
        {
            var background = _resourceManager.RandomBackground();
            (var silder, var hole) = _resourceManager.RandomTemplate();

            using var backgroundImage = SKBitmap.FromImage(SKImage.FromEncodedData(background));
            using var sliderTemplateImage = SKBitmap.FromImage(SKImage.FromEncodedData(silder));
            using var holeTemplateImage = SKBitmap.FromImage(SKImage.FromEncodedData(hole));
            using var holeMattingImage = new SKBitmap(sliderTemplateImage.Width, sliderTemplateImage.Height);
            using var sliderBarImage = new SKBitmap(sliderTemplateImage.Width, backgroundImage.Height);

            // 凹槽位置
            int randomX = _random.Next(holeTemplateImage.Width + 5, backgroundImage.Width - holeTemplateImage.Width - 10);
            int randomY = _random.Next(5, backgroundImage.Height - holeTemplateImage.Height - 5);

            // 根据透明度计算凹槽图轮廓形状(形状由不透明区域形成)
            var holeShape = CalcHoleShape(holeTemplateImage);
            // 生成凹槽抠图
            using var slideCanvas = new SKCanvas(holeMattingImage);
            slideCanvas.ClipPath(holeShape, SKClipOperation.Intersect, true);
            slideCanvas.DrawBitmap(backgroundImage, -randomX, -randomY);
            // 叠加拖块模板
            slideCanvas.DrawBitmap(sliderTemplateImage, 0, 0);
            // 绘制拖块条
            var slideBarCanvas = new SKCanvas(sliderBarImage);
            slideBarCanvas.DrawBitmap(holeMattingImage, 0, randomY);

            // 生成背景
            using var bgCanvas = new SKCanvas(backgroundImage);
            bgCanvas.DrawBitmap(holeTemplateImage, randomX, randomY);
            // 绘制干扰块
            DrawInterferences(backgroundImage, holeTemplateImage, randomX, randomY, _options.InterferenceCount);

            return new CaptchaImageData
            {
                X = randomX,
                Y = randomY,
                BackgroundImageWidth = backgroundImage.Width,
                BackgroundImageHeight = backgroundImage.Height,
                BackgroundImageBase64 = ToBase64(backgroundImage),
                SliderImageWidth = holeMattingImage.Width,
                SliderImageHeight = holeMattingImage.Height,
                SliderImageBase64 = ToBase64(sliderBarImage)
            };
        }

        /// <summary>
        /// 生成干扰块
        /// 要求: 所有缺块不能重叠, 
        /// </summary>
        /// <param name="backgroundImage"></param>
        /// <param name="holeTemplateImage"></param>
        /// <param name="holeX"></param>
        /// <param name="holeY"></param>
        /// <param name="count"></param>
        private void DrawInterferences(SKBitmap backgroundImage, SKBitmap holeTemplateImage, int holeX, int holeY, int count)
        {
            if (count <= 0) return;

            var holdeWidth = holeTemplateImage.Width;
            var holdeHeight = holeTemplateImage.Height;
            var minX = holdeWidth + 5;
            var maxX = backgroundImage.Width - holdeWidth - 10;
            var minY = 5;
            var maxY = backgroundImage.Height - holdeHeight - 5;

            using var canvas = new SKCanvas(backgroundImage);

            var excludeRegions = new List<SKRect>
            {
                new SKRect(holeX, holeY, holeX + holdeWidth, holeY + holdeHeight)
            };
            for (var i = 0; i < count; i++)
            {
                (var x, var y) = GenerateInterferencePosition(minX, maxX, minY, maxY, holeX, holeY, holdeWidth, holdeHeight, excludeRegions);
                if (x == 0) continue;

                canvas.DrawBitmap(holeTemplateImage, x, y);
                excludeRegions.Add(new SKRect(x, y, x + holdeWidth, y + holdeHeight));
            }
        }

        /// <summary>
        /// 生成干扰块位置
        /// </summary>
        /// <param name="minX">限定范围最小x</param>
        /// <param name="maxX">限定范围最大x</param>
        /// <param name="minY">限定范围最小y</param>
        /// <param name="maxY">限定范围最大y</param>
        /// <param name="holeWidth">缺块X</param>
        /// <param name="holeHeight">缺块Y</param>
        /// <param name="holeWidth">缺块宽度</param>
        /// <param name="holeHeight">缺块高度</param>
        /// <param name="excludeRegions">排除的范围</param>
        /// <returns></returns>
        private (int x, int y) GenerateInterferencePosition(int minX, int maxX, int minY, int maxY, int holeX, int holeY, int holeWidth, int holeHeight, List<SKRect> excludeRegions)
        {
            var random = new Random();

            for (var i = 0; i < 100; i++)
            {
                var x = random.Next(minX, maxX);
                var y = random.Next(minY, maxY);

                // 要与答案缺块的Y有一定间距
                if (Math.Abs(y - holeY) < holeHeight / 2) continue;

                var region = new SKRect(x, y, x + holeWidth, y + holeHeight);
                var isIntersect = excludeRegions.Any(x => x.IntersectsWith(region));
                if (isIntersect) continue;

                return (x, y);
            }

            // 代表未找到
            return (0, 0);
        }

        private static string ToBase64(SKBitmap bitmap)
        {
            var bytes = bitmap.Encode(SKEncodedImageFormat.Png, 100).ToArray();
            return "data:image/png;base64," + Convert.ToBase64String(bytes);
        }
    }
}
