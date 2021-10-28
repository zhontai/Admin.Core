namespace ZhonTai.Tools.Captcha
{
    public class PointDto
    {
        /// <summary>
        /// x坐标
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// y坐标
        /// </summary>
        public int Y { get; set; }

        public PointDto(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
