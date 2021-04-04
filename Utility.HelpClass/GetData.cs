using System;
using System.Web;
using System.Data;
using System.Configuration;


namespace Utility.HelpClass
{
    /// <summary>
    /// .net中取相关数据的操作类
    /// 2013.6.7
    /// code by jeven_xiao(详细方法操作见方法注释)
    /// </summary>
    public class GetData
    {
        /// <summary>
        /// 获取 web.config 的 AppSetting值

        /// </summary>
        public static string GetConfig(string key)
        {
            string result = "";
            if (ConfigurationManager.AppSettings[key] != null)
                result = ConfigurationManager.AppSettings[key].ToString();
            return result;
        }

        /// <summary>
        /// 获取request参数(get post)
        /// </summary>
        public static string GetRequest(string key)
        {
            return System.Web.HttpContext.Current.Request[key].To_String();
        }

        /// <summary>
        /// 获取request参数(post)
        /// </summary>
        public static string GetRequestPost(string key)
        {
            return System.Web.HttpContext.Current.Request.Form[key].To_String();
        }

        /// <summary>
        /// 获取session中保存的字符串

        /// </summary>
        public static string GetSessionStr(string key)
        {
            return System.Web.HttpContext.Current.Session[key].To_String();
        }
        /// <summary>
        /// 获取session中保存的对象
        /// </summary>
        public static object GetSessionObj(string key)
        {
            object a = new object();
            if (System.Web.HttpContext.Current.Session[key] != null)
            {
                a = System.Web.HttpContext.Current.Session[key];
            }
            else
            {
                a = null;
            }
            return a;
        }
        /// <summary>
        /// 获取XML文档中简单的一条数据

        /// </summary>
        /// <param name="FilePath">XML文档路径</param>
        /// <param name="DataTable">表名</param>
        /// <param name="key">字段名</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public static string GetXmlData(string FilePath, string DataTableName, string key, string where)
        {
            string result = "";
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(System.Web.HttpContext.Current.Server.MapPath(FilePath));
                DataTable dt = ds.Tables[DataTableName];
                if (dt.Select(where).Length > 0)
                {
                    result = dt.Select(where)[0][key].ToString();
                }
            }
            catch
            {
                result = "";
            }
            return result;
        }
        /// <summary>
        /// 获取当前页的URL
        /// </summary>
        /// <returns></returns>
        public static string GetPageUrl()
        {
            string rewurl = "";
            try
            {
                rewurl = System.Web.HttpContext.Current.Request.RawUrl;
            }
            catch
            {
                rewurl = "";
            }
            return rewurl;
        }

        /// <summary>
        /// 获取随机数

        /// </summary>
        /// <returns></returns>
        private static char[] strcon = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        public static string GetRandom(int strLength) {
            Random rd = new Random();
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
            for (int i = 0; i < strLength; i++)
            {
                newRandom.Append(strcon[rd.Next(62)]);
            }
            return newRandom.ToString().ToUpper();
        }

        #region COOKIE操作

        #region 设置Cookie值

        /// <summary>
        /// 写临时Cookie值

        /// </summary>
        /// <param name="key">Cookie名称</param>
        /// <param name="value">Cookie值</param>
        public static void setCookie(string key, string value)
        {
            setCookie(key, value, DateTime.MinValue);
        }

        /// <summary>
        /// 写Cookie值

        /// </summary>
        /// <param name="key">Cookie名称</param>
        /// <param name="value">Cookie值</param>
        /// <param name="dt">过期时间</param>
        /// <param name="domain">Cookie作用域</param>
        public static void setCookie(string key, string value, DateTime dt, string domain="")
        {
            System.Web.HttpCookie cookie = new System.Web.HttpCookie(key, HttpUtility.UrlEncode(value));
            if (dt != DateTime.MinValue)
                cookie.Expires = dt;

            //if (domain == "")
            //    domain = GetData.GetConfig("Domain");

            //if (domain != "")
            //    cookie.Domain = domain;

            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
           

        }
        #endregion

        #region 清除Cookie值

        /// <summary>
        /// 清除Cookie值

        /// </summary>
        /// <param name="key">Cookie名称</param>
        /// <param name="domain">Cookie作用域</param>
        public static void ClearCookie(string key, string domain="")
        {
            setCookie(key, "", DateTime.Now.AddHours(-2), domain);
        }

        #endregion

        #region 获取Cookie值

        /// <summary>
        /// 获取Cookie值

        /// </summary>
        /// <param name="key">Cookie名称</param>
        /// <param name="domain">Cookie域</param>
        /// <returns></returns>
        public static string getCookieStr(string key, string domain="")
        {
            string result = "";
            System.Web.HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[key];
            if (cookie != null)
            {
                //if (domain == "")
                //    domain = GetData.GetConfig("Domain");
                //if (domain != "")
                //    cookie.Domain = domain;
                result = HttpUtility.UrlDecode(cookie.Value.ToString());
            }
            return result;
        }
        #endregion

        #endregion

        #region 获取查询时间段


        /// <summary>
        /// 获取查询时间段

        /// </summary>
        /// <param name="t">时间字符串</param>
        /// <returns>[0]:起始日期；[1]截止日期</returns>
        public static string[] GetDateArea(string t)
        {
            string[] result = new string[2];
            if (t == "")
            {
                result[0] = DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToString("yyyy-MM-dd");
                result[1] = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                try
                {
                    t = DateTime.Parse(t).ToString("yyyy-MM-dd");
                    result[0] = t;
                    result[1] = t;
                }
                catch
                {
                    result[0] = DateTime.Now.AddDays(-DateTime.Now.Day).ToString("yyyy-MM-dd");
                    result[1] = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            return result;
        }

        /// <summary>
        /// 获取查询时间段

        /// </summary>
        /// <param name="t1">起始时间字符串</param>
        /// <param name="t2">截止时间字符串</param>
        /// <returns>[0]:起始日期；[1]截止日期</returns>
        public static string[] GetDateArea(string t1, string t2)
        {
            string[] result = new string[2];

            if (t1 == "")
                result[0] = DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToString("yyyy-MM-dd");
            else
            {
                try
                {
                    result[0] = DateTime.Parse(t1).ToString("yyyy-MM-dd");
                }
                catch
                {
                    result[0] = DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToString("yyyy-MM-dd");
                }
            }
            if (t2 == "")
                result[1] = DateTime.Now.ToString("yyyy-MM-dd");
            else
            {
                try
                {
                    result[1] = DateTime.Parse(t2).ToString("yyyy-MM-dd");
                }
                catch
                {
                    result[1] = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            return result;
        }

        #endregion

        #region 根据尺寸获取图片的完整路径

        /// <summary>
        /// 根据尺寸获取图片的完整路径

        /// </summary>
        /// <param name="sPath">原始图完整路径(路径+包含扩展名的文件名)</param>
        /// <param name="iWidth">宽度</param>
        /// <param name="iHeight">高度</param>
        /// <returns></returns>
        public static string GetImgSizeName(string sPath, int iWidth, int iHeight)
        {
            string ext = sPath.Substring(sPath.LastIndexOf("."));
            return sPath + "_" + iWidth.ToString() + "x" + iHeight.ToString() + ext;
        }
        #endregion

        /// <summary>
        /// 根据当前时间获取时间区间描述
        /// </summary>
        /// <returns>时间区间描述</returns>
        public static string GetDateTimeRange(string dt)
        {
            string result = "";
            try
            {
                DateTime dt1 = Convert.ToDateTime(dt);
                DateTime dt2 = DateTime.Now;
                System.TimeSpan st = dt2.Subtract(dt1);
                int d = st.Days; //天

                int h = st.Hours;//小时
                int m = st.Minutes;//分钟
                int s = st.Seconds;//秒

                if (d > 30)
                {
                    result = (d / 30).ToString() + "个月前";
                }
                else if (d > 0 && d <= 30)
                {
                    result = d.ToString() + "天前";
                }
                else if (h > 0 && d == 0)
                {
                    result = h.ToString() + "小时前";
                }
                else if (m > 0 && h == 0)
                {
                    result = m.ToString() + "分钟前";

                }
                else if (s > 0 && m == 0)
                {
                    result = s.ToString() + "秒钟前";
                }
                else
                {
                    result = "1秒钟前";
                }
            }
            catch
            {
                result = "Date_Error";
            }
            return result;

        }
        /// <summary>
        /// 获取客户端真实IP,非代理地址
        /// </summary>
        /// <returns></returns>
        public static string GetUserIP()
        {
            string userIP = string.Empty;
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] == null)
            {
                userIP = System.Web.HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                userIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            return userIP;
        }


        /// <summary>
        /// 获得Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static HttpCookie GetCookie(string cookieName)
        {
            HttpRequest request = HttpContext.Current.Request;
            if (request != null)
                return request.Cookies[cookieName];
            return null;
        }


        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="cookie"></param>
        public static void AddCookie(HttpCookie cookie)
        {
            HttpResponse response = HttpContext.Current.Response;
            if (response != null)
            {
                //指定客户端脚本是否可以访问[默认为false]
                cookie.HttpOnly = true;
                //指定统一的Path，比便能通存通取
                cookie.Path = "/";
                //设置跨域,这样在其它二级域名下就都可以访问到了
                //cookie.Domain = "chinesecoo.com";
                response.AppendCookie(cookie);
            }
        }


        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expires"></param>
        public static void SetCookie(string cookieName, string key, string value, DateTime? expires)
        {
            HttpResponse response = HttpContext.Current.Response;
            if (response != null)
            {
                HttpCookie cookie = response.Cookies[cookieName];
                if (cookie != null)
                {
                    if (!string.IsNullOrEmpty(key) && cookie.HasKeys)
                        cookie.Values.Set(key, value);
                    else
                        if (!string.IsNullOrEmpty(value))
                            cookie.Value = value;
                    if (expires != null)
                        cookie.Expires = expires.Value;
                    response.SetCookie(cookie);
                }
            }

        }

        /// <summary>
        /// 设置Cookie子键的值

        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetCookie(string cookieName, string key, string value)
        {
            SetCookie(cookieName, key, value, null);
        }






    }
}
