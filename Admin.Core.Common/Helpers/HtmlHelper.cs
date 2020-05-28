using System;
using System.Text; 
using System.Net; 
using System.IO; 
using System.Threading;
using System.Text.RegularExpressions;
using Admin.Core.Common.Attributes;

namespace Admin.Core.Common.Helpers
{
    /// <summary>
    /// Html操作相关类
    /// </summary>
    [SingleInstance]
    public class HtmlHelper
    {
        #region 私有字段
        private readonly string _ContentType = "application/json";
        private readonly string _Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
        private readonly string _UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
        private int _Delay = 1000;
        private int _CurrentTry = 0;
        #endregion

        #region 公有属性
        /// <summary> 
        /// Cookie
        /// </summary> 
        public CookieContainer CookieContainer { get; } = new CookieContainer();

        /// <summary> 
        /// 语言
        /// </summary> 
        public Encoding Encoding { get; set; } = Encoding.GetEncoding("utf-8");

        public int NetworkDelay
        {
            get
            {
                Random r = new Random();
                return r.Next(_Delay, _Delay * 2);
            }
            set
            {
                _Delay = value;
            }
        }

        public int MaxTry { get; set; } = 300;
        #endregion

        #region 获取HTML
        /// <summary>
        /// 获取HTML
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="postData">post 提交的字符串</param>
        /// <param name="isPost">是否是post</param>
        /// <param name="cookieContainer">CookieContainer</param>
        public string GetHtml(string url, string postData, bool isPost, CookieContainer cookieContainer)
        {
            if (string.IsNullOrEmpty(postData)) return GetHtml(url, cookieContainer);
            Thread.Sleep(NetworkDelay);
            _CurrentTry++;
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            try
            {
                byte[] byteRequest = Encoding.Default.GetBytes(postData);
                httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.CookieContainer = cookieContainer;
                httpWebRequest.ContentType = _ContentType;
                httpWebRequest.ServicePoint.ConnectionLimit = MaxTry;
                httpWebRequest.Referer = url;
                httpWebRequest.Accept = _Accept;
                httpWebRequest.UserAgent = _UserAgent;
                httpWebRequest.Method = isPost ? "POST" : "GET";
                httpWebRequest.ContentLength = byteRequest.Length;
                Stream stream = httpWebRequest.GetRequestStream();
                stream.Write(byteRequest, 0, byteRequest.Length);
                stream.Close();
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, Encoding);
                string html = streamReader.ReadToEnd();
                streamReader.Close();
                responseStream.Close();
                _CurrentTry = 0;
                httpWebRequest.Abort();
                httpWebResponse.Close();
                return html;
            }
            catch
            {
                if (_CurrentTry <= MaxTry) GetHtml(url, postData, isPost, cookieContainer);
                _CurrentTry--;
                if (httpWebRequest != null) httpWebRequest.Abort();
                if (httpWebResponse != null) httpWebResponse.Close();
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取HTML
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="cookieContainer">CookieContainer</param>
        public string GetHtml(string url, CookieContainer cookieContainer)
        {
            Thread.Sleep(NetworkDelay);
            _CurrentTry++;
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            try
            {
                httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.CookieContainer = cookieContainer;
                httpWebRequest.ContentType = _ContentType;
                httpWebRequest.ServicePoint.ConnectionLimit = MaxTry;
                httpWebRequest.Referer = url;
                httpWebRequest.Accept = _Accept;
                httpWebRequest.UserAgent = _UserAgent;
                httpWebRequest.Method = "GET";
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, Encoding);
                string html = streamReader.ReadToEnd();
                streamReader.Close();
                responseStream.Close();
                _CurrentTry--;
                httpWebRequest.Abort();
                httpWebResponse.Close();
                return html;
            }
            catch (Exception)
            {
                if (_CurrentTry <= MaxTry) GetHtml(url, cookieContainer);
                _CurrentTry--;
                if (httpWebRequest != null) httpWebRequest.Abort();
                if (httpWebResponse != null) httpWebResponse.Close();
                return string.Empty;
            }
        }
        #endregion

        #region 获取字符流
        //---------------------------------------------------------------------------------------------------------------
        // 示例:
        // System.Net.CookieContainer cookie = new System.Net.CookieContainer(); 
        // Stream s = HttpHelper.GetStream("http://ptlogin2.qq.com/getimage?aid=15000102&0.43878429697395826", cookie);
        // picVerify.Image = Image.FromStream(s);
        //---------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 获取字符流
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="cookieContainer">cookieContainer</param>
        public Stream GetStream(string url, CookieContainer cookieContainer)
        {
            _CurrentTry++;

            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;

            try
            {
                httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.CookieContainer = cookieContainer;
                httpWebRequest.ContentType = _ContentType;
                httpWebRequest.ServicePoint.ConnectionLimit = MaxTry;
                httpWebRequest.Referer = url;
                httpWebRequest.Accept = _Accept;
                httpWebRequest.UserAgent = _UserAgent;
                httpWebRequest.Method = "GET";

                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                _CurrentTry--;
                return responseStream;
            }
            catch (Exception)
            {
                if (_CurrentTry <= MaxTry)
                {
                    GetHtml(url, cookieContainer);
                }

                _CurrentTry--;

                if (httpWebRequest != null)
                {
                    httpWebRequest.Abort();
                } if (httpWebResponse != null)
                {
                    httpWebResponse.Close();
                }
                return null;
            }
        }
        #endregion

        #region 清除HTML标记
        /// <summary>
        /// 清除HTML标记
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns>已经去除后的文字</returns>
        public string NoHTML(string Htmlstring)
        {
            //删除脚本   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

            //删除HTML   
            Regex regex = new Regex("<.+?>", RegexOptions.IgnoreCase);
            Htmlstring = regex.Replace(Htmlstring, "");
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");

            return Htmlstring;
        }
        #endregion

        #region 删除文本中带的HTML标记
        /// <summary>
        /// 删除文本中带的HTML标记
        /// </summary>
        /// <param name="InString">输入要删除带HTML的字符串</param>    
        /// <returns>返回处理过的字符串</returns>
        public string DelHtmlCode(string InString)
        {
            string strTemp = InString;
            while (strTemp.Contains("<"))
            {
                if (!strTemp.Contains(">")) { break; }    //当字符串内不包含">"时退出循环
                int htmlBeginNum = strTemp.IndexOf("<");
                int htmlEndNum = strTemp.IndexOf(">");
                //删除从"<"到">"之间的所有字符串
                strTemp = strTemp.Remove(htmlBeginNum, htmlEndNum - htmlBeginNum + 1);
            }
            strTemp = strTemp.Replace("\n", "");
            strTemp = strTemp.Replace("\r", "");
            strTemp = strTemp.Replace("\n\r", "");
            strTemp = strTemp.Replace("&nbsp;", "");
            strTemp = strTemp.Replace(" ", "");
            strTemp = strTemp.Trim();
            return strTemp;
        }
        #endregion

        #region 匹配页面的链接
        /// <summary>
        /// 获取页面的链接正则
        /// </summary>
        public string GetHref(string HtmlCode)
        {
            string MatchVale = "";
            string Reg = @"(h|H)(r|R)(e|E)(f|F) *= *('|"")?((\w|\\|\/|\.|:|-|_)+)[\S]*";
            foreach (Match m in Regex.Matches(HtmlCode, Reg))
            {
                MatchVale += (m.Value).ToLower().Replace("href=", "").Trim() + "|";
            }
            return MatchVale;
        }
        #endregion

        #region 匹配页面的图片地址
        /// <summary>
        /// 匹配页面的图片地址
        /// </summary>
        /// <param name="HtmlCode"></param>
        /// <param name="imgHttp">要补充的http://路径信息</param>
        /// <returns></returns>
        public string GetImgSrc(string HtmlCode, string imgHttp)
        {
            string MatchVale = "";
            string Reg = @"<img.+?>";
            foreach (Match m in Regex.Matches(HtmlCode.ToLower(), Reg))
            {
                MatchVale += GetImg((m.Value).ToLower().Trim(), imgHttp) + "|";
            }

            return MatchVale;
        }

        /// <summary>
        /// 匹配<img src="" />中的图片路径实际链接
        /// </summary>
        /// <param name="ImgString"><img src="" />字符串</param>
        /// <param name="imgHttp"></param>
        /// <returns></returns>
        public string GetImg(string ImgString, string imgHttp)
        {
            string MatchVale = "";
            string Reg = @"src=.+\.(bmp|jpg|gif|png|)";
            foreach (Match m in Regex.Matches(ImgString.ToLower(), Reg))
            {
                MatchVale += (m.Value).ToLower().Trim().Replace("src=", "");
            }
            if (MatchVale.IndexOf(".net") != -1 || MatchVale.IndexOf(".com") != -1 || MatchVale.IndexOf(".org") != -1 || MatchVale.IndexOf(".cn") != -1 || MatchVale.IndexOf(".cc") != -1 || MatchVale.IndexOf(".info") != -1 || MatchVale.IndexOf(".biz") != -1 || MatchVale.IndexOf(".tv") != -1)
                return (MatchVale);
            else
                return (imgHttp + MatchVale);
        }
        #endregion

        #region 抓取远程页面内容
        /// <summary>
        /// 以GET方式抓取远程页面内容
        /// </summary>
        public string Get_Http(string tUrl)
        {
            string strResult;
            try
            {
                HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(tUrl);
                hwr.Timeout = 19600;
                HttpWebResponse hwrs = (HttpWebResponse)hwr.GetResponse();
                Stream myStream = hwrs.GetResponseStream();
                StreamReader sr = new StreamReader(myStream, Encoding.Default);
                StringBuilder sb = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    sb.Append(sr.ReadLine() + "\r\n");
                }
                strResult = sb.ToString();
                hwrs.Close();
            }
            catch (Exception ee)
            {
                strResult = ee.Message;
            }
            return strResult;
        }

        /// <summary>
        /// 以POST方式抓取远程页面内容
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">参数列表</param>
        /// <param name="encodeType"></param>
        /// <returns></returns>
        public string Post_Http(string url, string postData, string encodeType)
        {
            string strResult;
            try
            {
                Encoding encoding = Encoding.GetEncoding(encodeType);
                byte[] POST = encoding.GetBytes(postData);
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.Method = "POST";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ContentLength = POST.Length;
                Stream newStream = myRequest.GetRequestStream();
                newStream.Write(POST, 0, POST.Length); //设置POST
                newStream.Close();
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.Default);
                strResult = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                strResult = ex.Message;
            }
            return strResult;
        }
        #endregion

        #region 压缩HTML输出
        /// <summary>
        /// 压缩HTML输出
        /// </summary>
        public string ZipHtml(string Html)
        {
            Html = Regex.Replace(Html, @">\s+?<", "><");//去除HTML中的空白字符
            Html = Regex.Replace(Html, @"\r\n\s*", "");
            Html = Regex.Replace(Html, @"<body([\s|\S]*?)>([\s|\S]*?)</body>", @"<body$1>$2</body>", RegexOptions.IgnoreCase);
            return Html;
        }
        #endregion

        #region 过滤指定HTML标签
        /// <summary>
        /// 过滤指定HTML标签
        /// </summary>
        /// <param name="s_TextStr">要过滤的字符</param>
        /// <param name="html_Str">a img p div</param>
        public string DelHtml(string s_TextStr, string html_Str)
        {
            string rStr = "";
            if (!string.IsNullOrEmpty(s_TextStr))
            {
                rStr = Regex.Replace(s_TextStr, "<" + html_Str + "[^>]*>", "", RegexOptions.IgnoreCase);
                rStr = Regex.Replace(rStr, "</" + html_Str + ">", "", RegexOptions.IgnoreCase);
            }
            return rStr;
        }
        #endregion
    }
}