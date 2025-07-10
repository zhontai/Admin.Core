using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.SlideCaptcha.Core.Validator
{
    public class BasicValidator : BaseValidator, IValidator
    {
        public override bool ValidateCore(SlideTrack slideTrack, CaptchaValidateData captchaValidateData)
        {
            slideTrack.CheckTracks();

            // 进行行为轨迹检测
            var startSlidingTime = slideTrack.StartTime.ToFileTimeUtc();
            long endSlidingTime = slideTrack.EndTime.ToFileTimeUtc();
            var bgImageWidth = slideTrack.BackgroundImageWidth;
            var trackList = slideTrack.Tracks;

            // check1: 滑动时间如果小于300毫秒 返回false
            if (startSlidingTime + 300 > endSlidingTime)
            {
                return false;
            }

            // check2:轨迹数据要是少于10，或者大于背景宽度的五倍 返回false
            if (trackList.Count < 10 || trackList.Count > bgImageWidth * 5)
            {
                return false;
            }

            // check3:x轴和y轴应该是从0开始的，要是一开始x轴和y轴乱跑，返回false
            var firstTrack = trackList[0];
            if (firstTrack.X > 10 || firstTrack.X < -10 || firstTrack.Y > 10 || firstTrack.Y < -10)
            {
                return false;
            }

            // check4: 如果y轴是相同的，必然是机器操作，直接返回false （暂时去掉，容易失败）
            // check5：x轴或者y轴直接的区间跳跃过大的话返回 false （暂时去掉，容易失败）
            // check6: 如果x轴超过图片宽度的频率过高，返回false
            int check4 = 1;
            int check6 = 0;
            for (int i = 1; i < trackList.Count; i++)
            {
                var track = trackList[i];
                // check4
                if (firstTrack.Y == track.Y) check4++;

                // check7
                if (track.X >= bgImageWidth) check6++;

                // check5
                var preTrack = trackList[i - 1];
                if ((track.X - preTrack.X) > 50 || (track.Y - preTrack.Y) > 50) return false; // 快速来回拖动可能导致这里验证不通过
            }
            if (check4 == trackList.Count || check6 > 200)
            {
                return false;
            }

            return true;

            //// check7: x轴应该是由快到慢的， 要是速率一致，返回false
            //int splitPos = (int)(trackList.Count * 0.7);
            //var splitPostTrack = trackList[splitPos - 1];
            //int posTime = splitPostTrack.T;
            //float startAvgPosTime = posTime / (float)splitPos;

            //var lastTrack = trackList[trackList.Count - 1];
            //float endAvgPosTime = (lastTrack.T - posTime) / (float)(trackList.Count - splitPos);

            //return endAvgPosTime > startAvgPosTime;
        }
    }
}
